
namespace TriangleNet.Rendering.Text
{
    using System;
    using System.Drawing;
    using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class EpsDocument : IDisposable
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        // Constant to convert from millimeters to PostScript units (1/72th inch).
        private const double UNITS_PER_MM = 72.0 / 25.4;

        private FormattingStreamWriter _w;
        private PageSize _size;

        /// <summary>
        /// Gets or sets the document name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default point size (default = 1).
        /// </summary>
        public int DefaultPointSize { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public EpsDocument(string filename, PageSize pageSize)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : this(File.Create(filename), pageSize)
        {
            Name = Path.GetFileName(filename);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public EpsDocument(Stream stream, PageSize pageSize)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w = new FormattingStreamWriter(stream)
            {
                NewLine = "\n"
            };

            _size = pageSize;

            DefaultPointSize = 1;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void AddComment(string comment, int line = 1)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            for (int i = 0; i < line; i++)
            {
                _w.WriteLine("%");
            }

            var t = comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );

            for (int i = 0; i < t.Length; i++)
            {
                _w.WriteLine("% " + t[i]);
            }

            for (int i = 0; i < line; i++)
            {
                _w.WriteLine("%");
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void DrawPoint(Point p)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("{0} {1} P", p.X, p.Y);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void DrawLine(Point p1, Point p2)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("{0} {1} {2} {3} L", p1.X, p1.Y, p2.X, p2.Y);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void DrawRectangle(Rectangle rect)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("newpath");
            _w.WriteLine("  {0}  {1}  moveto", rect.X, rect.Y);
            _w.WriteLine("  {0}  {1}  lineto", rect.Right, rect.Y);
            _w.WriteLine("  {0}  {1}  lineto", rect.Right, rect.Bottom);
            _w.WriteLine("  {0}  {1}  lineto", rect.X, rect.Bottom);
            _w.WriteLine("  {0}  {1}  lineto", rect.X, rect.Y);
            _w.WriteLine("stroke");

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetClip(Rectangle rect)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("newpath");
            _w.WriteLine("  {0}  {1}  moveto", rect.X, rect.Y);
            _w.WriteLine("  {0}  {1}  lineto", rect.Right, rect.Y);
            _w.WriteLine("  {0}  {1}  lineto", rect.Right, rect.Bottom);
            _w.WriteLine("  {0}  {1}  lineto", rect.X, rect.Bottom);
            _w.WriteLine("  {0}  {1}  lineto", rect.X, rect.Y);
            _w.WriteLine("clip newpath");
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetColor(Color color)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("{0:0.###} {1:0.###} {2:0.###} setrgbcolor",
                ((float)color.R) / 255f,
                ((float)color.G) / 255f,
                ((float)color.B) / 255f);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetStroke(float width)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _w.WriteLine("{0:0.###} setlinewidth", width);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetStroke(float width, Color color)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            SetColor(color);
            SetStroke(width);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void WriteHeader()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var x = _size.X; // * UNITS_PER_MM
            var y = _size.Y;
            var right = _size.Right;
            var bottom = _size.Bottom;

            // Write document header.

            _w.WriteLine("%!PS-Adobe-3.0 EPSF-3.0");
            _w.WriteLine("%%Creator: Triangle.NET");
            _w.WriteLine("%%Title: {0}", Name);
            _w.WriteLine("%%Pages: 1");
            _w.WriteLine("%%BoundingBox:  {0}  {1}  {2}  {3}", (int)x, (int)y, (int)right, (int)bottom);
            _w.WriteLine("%%HiResBoundingBox: {0:0.#####}  {1:0.#####}  {2:0.#####}  {3:0.#####}", x, y, right, bottom);
            _w.WriteLine("%%Document-Fonts: Times-Roman");
            _w.WriteLine("%%LanguageLevel: 3");
            _w.WriteLine("%%EndComments");
            _w.WriteLine("%%Page: 1 1");
            _w.WriteLine("save");
            
            // Define points.
            _w.WriteLine("% Define points.");
            _w.WriteLine("/P {");
            _w.WriteLine("2 dict begin");
            _w.WriteLine("/y exch def");
            _w.WriteLine("/x exch def");
            _w.WriteLine("gsave");
            _w.WriteLine("newpath x y {0} 0 360 arc fill", DefaultPointSize);
            _w.WriteLine("grestore");
            _w.WriteLine("end");
            _w.WriteLine("} def");

            // Define lines.
            _w.WriteLine("% Define lines.");
            _w.WriteLine("/L {");
            _w.WriteLine("2 dict begin");
            _w.WriteLine("/y2 exch def");
            _w.WriteLine("/x2 exch def");
            _w.WriteLine("/y1 exch def");
            _w.WriteLine("/x1 exch def");
            _w.WriteLine("gsave");
            _w.WriteLine("newpath x1 y1 moveto x2 y2 lineto stroke");
            _w.WriteLine("grestore");
            _w.WriteLine("end");
            _w.WriteLine("} def");
        }

        private void Close()
        {
            _w.WriteLine("%");
            _w.WriteLine("restore  showpage");
            _w.WriteLine("%%Trailer");
            _w.WriteLine("%%EOF");
        }

        #region IDisposable implementation

        // Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Dispose()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual void Dispose(bool disposing)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (disposed)
                return;

            Close();

            if (disposing)
            {
                _w.Dispose();
                _w = null;
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~EpsDocument()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Dispose(false);
        }

        #endregion
    }
}
