﻿
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using System.Drawing;
    using TriangleNet.Rendering.Util;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ColorManager
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Gets or sets the brush used for points.
        /// </summary>
        public Color Point { get; set; }

        /// <summary>
        /// Gets or sets the brush used for steiner points.
        /// </summary>
        public Color SteinerPoint { get; set; }

        /// <summary>
        /// Gets or sets the pen used for mesh edges.
        /// </summary>
        public Color Line { get; set; }

        /// <summary>
        /// Gets or sets the pen used for mesh segments.
        /// </summary>
        public Color Segment { get; set; }

        /// <summary>
        /// Gets or sets the pen used for Voronoi edges.
        /// </summary>
        public Color VoronoiLine { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets a dictionary which maps region ids (or partition indices) to a color.
        /// </summary>
        public Dictionary<uint, Color> ColorDictionary { get; set; }

        /// <summary>
        /// Gets or sets a color map used for function plotting.
        /// </summary>
        public ColorMap ColorMap { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="ColorManager"/> class with default (dark) color scheme.
        /// </summary>
        public static ColorManager Default()
        {
            var colors = new ColorManager
            {
                Background = Color.FromArgb(0, 0, 0),
                Point = Color.Green,
                SteinerPoint = Color.Peru,
                Line = Color.FromArgb(30, 30, 30),
                Segment = Color.DarkBlue,
                VoronoiLine = Color.FromArgb(40, 50, 60)
            };

            return colors;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Dictionary<uint, Color> CreateColorDictionary(int length)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var keys = new uint[length];

            for (uint i = 0; i < length; i++)
            {
                keys[i] = i;
            }

            return CreateColorDictionary(keys);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Dictionary<uint, Color> CreateColorDictionary(IEnumerable<uint> keys)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            ColorDictionary = new Dictionary<uint, Color>();

            int i = 0, n = regionColors.Length;

            foreach (var key in keys)
            {
                ColorDictionary.Add(key, regionColors[i]);

                i = (i + 1) % n;
            }

            return ColorDictionary;
        }

        // Change or add as many colors as you like...
        private static Color[] regionColors = {
            Color.FromArgb(127,   0, 255,   0),
            Color.FromArgb(127, 255,   0,   0),
            Color.FromArgb(127,   0,   0, 255),
            Color.FromArgb(127,   0, 255, 255),
            Color.FromArgb(127, 255, 255,   0),
            Color.FromArgb(127, 255,   0, 255),
            Color.FromArgb(127, 127,   0, 255),
            Color.FromArgb(127,   0, 127, 255)
        };
    }
}
