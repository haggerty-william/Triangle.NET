﻿// -----------------------------------------------------------------------
// <copyright file="DcelMesh.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// DCEL mesh.
    /// </summary>
    public class DcelMesh
    {
        /// <summary>List of vertices.</summary>
        protected List<Vertex> vertices;
        
        /// <summary>List of half-edges.</summary>
        protected List<HalfEdge> edges;

        /// <summary>List of faces.</summary>
        protected List<Face> faces;

        /// <summary>
        /// Initializes a new instance of the <see cref="DcelMesh" /> class.
        /// </summary>
        public DcelMesh()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DcelMesh" /> class.
        /// </summary>
        /// <param name="initialize">If false, lists will not be initialized.</param>
        protected DcelMesh(bool initialize)
        {
            if (initialize)
            {
                vertices = [];
                edges = [];
                faces = [];
            }
        }

        /// <summary>
        /// Gets the vertices of the Voronoi diagram.
        /// </summary>
        public List<Vertex> Vertices => vertices;

        /// <summary>
        /// Gets the list of half-edges specify the Voronoi diagram topology.
        /// </summary>
        public List<HalfEdge> HalfEdges => edges;

        /// <summary>
        /// Gets the faces of the Voronoi diagram.
        /// </summary>
        public List<Face> Faces => faces;

        /// <summary>
        /// Gets the collection of edges of the Voronoi diagram.
        /// </summary>
        public IEnumerable<IEdge> Edges => EnumerateEdges();

        /// <summary>
        /// Check if the DCEL is consistent.
        /// </summary>
        /// <param name="closed">If true, faces are assumed to be closed (i.e. all edges must have
        /// a valid next pointer).</param>
        /// <param name="depth">Maximum edge count of faces (default = 0 means skip check).</param>
        /// <returns></returns>
        /// <remarks>
        /// The <paramref name="depth"/> value relates to the maximum degree of a vertex in the
        /// triangulation. For quality meshes, the maximum degree is usually low, but for meshes
        /// build from PSLGs without quality constraints applied, either provide a larger value
        /// or disable the check by setting <paramref name="depth"/> to 0 (default).
        /// </remarks>
        public virtual bool IsConsistent(bool closed = true, int depth = 0)
        {
            // Check vertices for null pointers.
            foreach (var vertex in vertices)
            {
                if (vertex.id < 0)
                {
                    continue;
                }

                if (vertex.leaving == null)
                {
                    return false;
                }

                if (vertex.Leaving.Origin.id != vertex.id)
                {
                    return false;
                }
            }

            // Check faces for null pointers.
            foreach (var face in faces)
            {
                if (face.ID < 0)
                {
                    continue;
                }

                if (face.edge == null)
                {
                    return false;
                }

                if (face.id != face.edge.face.id)
                {
                    return false;
                }
            }

            // Check half-edges for null pointers.
            foreach (var edge in edges)
            {
                if (edge.id < 0)
                {
                    continue;
                }

                if (edge.twin == null)
                {
                    return false;
                }

                if (edge.origin == null)
                {
                    return false;
                }

                if (edge.face == null)
                {
                    return false;
                }

                if (closed && edge.next == null)
                {
                    return false;
                }
            }

            // Check half-edges (topology).
            foreach (var edge in edges)
            {
                if (edge.id < 0)
                {
                    continue;
                }

                var twin = edge.twin;
                var next = edge.next;

                if (edge.id != twin.twin.id)
                {
                    return false;
                }

                if (closed)
                {
                    if (next.origin.id != twin.origin.id)
                    {
                        return false;
                    }

                    if (next.twin.next.origin.id != edge.twin.origin.id)
                    {
                        return false;
                    }
                }
            }

            if (closed && depth > 0)
            {
                // Check if faces are closed.
                foreach (var face in faces)
                {
                    if (face.id < 0)
                    {
                        continue;
                    }

                    var edge = face.edge;
                    var next = edge.next;

                    int id = edge.id;
                    int k = 0;

                    while (next.id != id && k < depth)
                    {
                        next = next.next;
                        k++;
                    }

                    if (next.id != id)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Search for half-edge without twin and add a twin. Connect twins to form connected
        /// boundary contours.
        /// </summary>
        /// <remarks>
        /// This method assumes that all faces are closed (i.e. no edge.next pointers are null).
        /// </remarks>
        public void ResolveBoundaryEdges()
        {
            // Maps vertices to leaving boundary edge.
            var map = new Dictionary<int, HalfEdge>();

            // TODO: parallel?
            foreach (var edge in edges)
            {
                if (edge.twin == null)
                {
                    var twin = edge.twin = new HalfEdge(edge.next.origin, Face.Empty);
                    twin.twin = edge;

                    map.Add(twin.origin.id, twin);
                }
            }

            int j = edges.Count;

            foreach (var edge in map.Values)
            {
                edge.id = j++;
                edge.next = map[edge.twin.origin.id];

                edges.Add(edge);
            }
        }

        /// <summary>
        /// Enumerates all edges of the DCEL.
        /// </summary>
        /// <remarks>
        /// This method assumes that each half-edge has a twin (i.e. NOT null).
        /// </remarks>
        protected virtual IEnumerable<IEdge> EnumerateEdges()
        {
            var edges = new List<IEdge>(this.edges.Count / 2);

            foreach (var edge in this.edges)
            {
                var twin = edge.twin;

                // Report edge only once.
                if (edge.id < twin.id)
                {
                    edges.Add(new Edge(edge.origin.id, twin.origin.id));
                }
            }

            return edges;
        }
    }
}
