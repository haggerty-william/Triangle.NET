﻿// -----------------------------------------------------------------------
// <copyright file="Enums.cs">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    /// <summary>
    /// The type of the mesh vertex.
    /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum VertexType { InputVertex, SegmentVertex, FreeVertex, DeadVertex, UndeadVertex };
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Node renumbering algorithms.
    /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum NodeNumbering { None, Linear, CuthillMcKee };
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Labels that signify the result of point location.
    /// </summary>
    /// <remarks>The result of a search indicates that the point falls in the 
    /// interior of a triangle, on an edge, on a vertex, or outside the mesh.
    /// </remarks>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum LocateResult { InTriangle, OnEdge, OnVertex, Outside };
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Labels that signify the result of vertex insertion.
    /// </summary>
    /// <remarks>The result indicates that the vertex was inserted with complete 
    /// success, was inserted but encroaches upon a subsegment, was not inserted 
    /// because it lies on a segment, or was not inserted because another vertex 
    /// occupies the same location.
    /// </remarks>
    enum InsertVertexResult { Successful, Encroaching, Violating, Duplicate };

    /// <summary>
    /// Labels that signify the result of direction finding.
    /// </summary>
    /// <remarks>The result indicates that a segment connecting the two query 
    /// points falls within the direction triangle, along the left edge of the 
    /// direction triangle, or along the right edge of the direction triangle.
    /// </remarks>
    enum FindDirectionResult { Within, Leftcollinear, Rightcollinear };
}
