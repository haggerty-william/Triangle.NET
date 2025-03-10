﻿// -----------------------------------------------------------------------
// <copyright file="VoronoiBase.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi
{
    using System;
    using System.Collections.Generic;

    using TriangleNet.Topology;
    using TriangleNet.Geometry;
    using TriangleNet.Topology.DCEL;

    using Vertex = TriangleNet.Topology.DCEL.Vertex;

    /// <summary>
    /// The Voronoi diagram is the dual of a point set triangulation.
    /// </summary>
    public abstract class VoronoiBase : DcelMesh
    {
        /// <summary>Predicates</summary>
        protected IPredicates predicates;

        /// <summary>Voronoi factory</summary>
        protected IVoronoiFactory factory;

        /// <summary>
        /// List of infinite half-edges, i.e. half-edges that start at circumcenters
        /// of triangles on the domain boundary.
        /// </summary>
        protected List<HalfEdge> rays;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiBase" /> class.
        /// </summary>
        /// <param name="mesh">Triangle mesh.</param>
        /// <param name="factory">Voronoi object factory.</param>
        /// <param name="predicates">Geometric predicates implementation.</param>
        /// <param name="generate">If set to true, the constructor will call the Generate
        /// method, which builds the Voronoi diagram.</param>
        protected VoronoiBase(Mesh mesh, IVoronoiFactory factory, IPredicates predicates,
            bool generate) : base(false)
        {
            this.factory = factory ?? new DefaultVoronoiFactory();
            this.predicates = predicates;

            edges = [];
            rays = [];

            if (generate)
            {
                Generate(mesh);
            }
        }

        /// <summary>
        /// Generate the Voronoi diagram from given triangle mesh.
        /// </summary>
        /// <param name="mesh"></param>
        protected void Generate(Mesh mesh)
        {
            edges.Clear();
            rays.Clear();

            // Undead vertices cannot be Voronoi cell generators.
            int count = mesh.vertices.Count - mesh.undeads;

            // Allocate space for Voronoi diagram.
            var vertices = new Vertex[mesh.triangles.Count + mesh.hullsize];
            var faces = new Face[count];

            factory.Initialize(vertices.Length, 2 * mesh.NumberOfEdges, faces.Length);

            // Compute triangles circumcenters.
            var map = ComputeVertices(mesh, vertices);

            // Ensure linear numbering of vertices (excluding undeads).
            int vid = 0;

            // Create all Voronoi faces, skipping undead vertices.
            foreach (var vertex in mesh.vertices.Values)
            {
                if (vertex.type == VertexType.UndeadVertex)
                {
                    vertex.id = count++;
                }
                else
                {
                    vertex.id = vid++;
                    faces[vertex.id] = factory.CreateFace(vertex);
                }
            }

            ComputeEdges(mesh, vertices, faces, map);

            // At this point all edges are computed, but the (edge.next) pointers aren't set.
            ConnectEdges(map);

            base.vertices = new List<Vertex>(vertices);
            base.faces = new List<Face>(faces);
        }

        /// <summary>
        /// Compute the Voronoi vertices (the circumcenters of the triangles).
        /// </summary>
        /// <returns>An empty map, which will map all vertices to a list of leaving edges.</returns>
        /// <remarks>
        /// This method will also change triangle ids (to ensure linear numbering of triangles).
        /// </remarks>
        protected List<HalfEdge>[] ComputeVertices(Mesh mesh, Vertex[] vertices)
        {
            Otri tri = default;
            double xi = 0, eta = 0;
            Vertex vertex;
            Point pt;
            int id, i = 0;

            // Maps all vertices to a list of leaving edges.
            var map = new List<HalfEdge>[mesh.triangles.Count];

            // Compute triangle circumcenters
            foreach (var t in mesh.triangles)
            {
                t.id = id = i++;
                tri.tri = t;

                pt = predicates.FindCircumcenter(tri.Org(), tri.Dest(), tri.Apex(), ref xi, ref eta);

                vertex = factory.CreateVertex(pt.x, pt.y);
                vertex.id = id;
                vertex.label = t.label;

                vertices[id] = vertex;
                map[id] = [];
            }

            return map;
        }

        /// <summary>
        /// Compute the edges of the Voronoi diagram.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="vertices"></param>
        /// <param name="faces"></param>
        /// <param name="map">Empty vertex map.</param>
        protected void ComputeEdges(Mesh mesh, Vertex[] vertices, Face[] faces, List<HalfEdge>[] map)
        {
            Otri tri, neighbor = default;
            TriangleNet.Geometry.Vertex org, dest;

            double px, py;
            int id, nid, count = mesh.triangles.Count;

            Face face, neighborFace;
            HalfEdge edge, twin;
            Vertex vertex, end;

            // Count infinite edges (vertex id for their endpoints).
            int j = 0;

            // Count half-edges (edge ids).
            int k = 0;

            // To loop over the set of edges, loop over all triangles, and look at the
            // three edges of each triangle.  If there isn't another triangle adjacent
            // to the edge, operate on the edge. If there is another adjacent triangle,
            // operate on the edge only if the current triangle has a smaller id than
            // its neighbor. This way, each edge is considered only once.
            foreach (var t in mesh.triangles)
            {
                id = t.id;

                tri.tri = t;

                for (int i = 0; i < 3; i++)
                {
                    tri.orient = i;
                    tri.Sym(ref neighbor);

                    nid = neighbor.tri.id;

                    if (id < nid || nid < 0)
                    {
                        // Get the endpoints of the current triangle edge.
                        org = tri.Org();
                        dest = tri.Dest();

                        face = faces[org.id];
                        neighborFace = faces[dest.id];

                        vertex = vertices[id];

                        // For each edge in the triangle mesh, there's a corresponding edge
                        // in the Voronoi diagram, i.e. two half-edges will be created.
                        if (nid < 0)
                        {
                            // Unbounded edge, direction perpendicular to the boundary edge,
                            // pointing outwards.
                            px = dest.y - org.y;
                            py = org.x - dest.x;

                            end = factory.CreateVertex(vertex.x + px, vertex.y + py);
                            end.id = count + j++;

                            vertices[end.id] = end;

                            edge = factory.CreateHalfEdge(end, face);
                            twin = factory.CreateHalfEdge(vertex, neighborFace);

                            // Make (face.edge) always point to an edge that starts at an infinite
                            // vertex. This will allow traversing of unbounded faces.
                            face.edge = edge;
                            face.bounded = false;

                            map[id].Add(twin);

                            rays.Add(twin);
                        }
                        else
                        {
                            end = vertices[nid];

                            // Create half-edges.
                            edge = factory.CreateHalfEdge(end, face);
                            twin = factory.CreateHalfEdge(vertex, neighborFace);

                            // Add to vertex map.
                            map[nid].Add(edge);
                            map[id].Add(twin);
                        }

                        vertex.leaving = twin;
                        end.leaving = edge;

                        edge.twin = twin;
                        twin.twin = edge;

                        edge.id = k++;
                        twin.id = k++;

                        edges.Add(edge);
                        edges.Add(twin);
                    }
                }
            }
        }

        /// <summary>
        /// Connect all edges of the Voronoi diagram.
        /// </summary>
        /// <param name="map">Maps all vertices to a list of leaving edges.</param>
        protected virtual void ConnectEdges(List<HalfEdge>[] map)
        {
            int length = map.Length;

            // For each half-edge, find its successor in the connected face.
            foreach (var edge in edges)
            {
                var face = edge.face.generator.id;

                // The id of the dest vertex of current edge.
                int id = edge.twin.origin.id;

                // The edge origin can also be an infinite vertex. Sort them out
                // by checking the id.
                if (id < length)
                {
                    // Look for the edge that is connected to the current face. Each
                    // Voronoi vertex has degree 3, so this loop is actually O(1).
                    foreach (var next in map[id])
                    {
                        if (next.face.generator.id == face)
                        {
                            edge.next = next;
                            break;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<IEdge> EnumerateEdges()
        {
            var edges = new List<IEdge>(this.edges.Count / 2);

            foreach (var edge in this.edges)
            {
                var twin = edge.twin;

                // Report edge only once.
                if (twin == null)
                {
                    edges.Add(new Edge(edge.origin.id, edge.next.origin.id));
                }
                else if (edge.id < twin.id)
                {
                    edges.Add(new Edge(edge.origin.id, twin.origin.id));
                }
            }

            return edges;
        }
    }
}
