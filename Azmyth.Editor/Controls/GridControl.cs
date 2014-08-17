using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Azmyth.Editor
{
    public delegate void CellEvent(object sender, CellEventArgs e);

    public class GridControl : Control
    {
        private float m_cellHeight;
        private float m_cellWidth;

        private float m_translateX;
        private float m_translateY;
        private float m_scale;
        private float m_rotateAngle;

        private float m_translateStep;
        private float m_scaleStep;
        private float m_rotateStep;
        private float m_scaleMin;
        private float m_scaleMax;

        private Point m_mouseLocation;
        private Point m_previousMouseLocation;
        private Point m_mouseDownLocation;
        private bool m_isLeftMouseDown;
        private bool m_isRightMouseDown;

        private Point m_selectedCellLocation;
        private Point m_highlightedCellLocation;

        private Brush m_selectedCellBrush;
        private Brush m_highlightedCellBrush;
        private Pen m_gridPen;

        private SortedDictionary<string, object> m_debug;
        private Font m_debugFont;
        private Brush m_debugBackgroundBrush;
        private Brush m_debugForegroundBrush;
        private Brush m_originBrush;

        private Rectangle m_viewport;

        public event CellEvent ViewportChanged;

        private bool m_heightMap;

        [Browsable(true), Category("Appearance")]
        public bool HeightMap
        {
            get
            {
                return m_heightMap;
            }
            set
            {
                m_heightMap = value;
                Invalidate();
            }
        }

        private double m_amplitude = 1.00f;
        private double m_persistance = 1.00f;
        private double m_frequency = 0.05f;
        private int m_octaves = 1;
        private int m_seed = 500;

        public double Persistance
        {
            get
            {
                return m_persistance;
            }
            set
            {
                m_persistance = value;
                Invalidate();
            }
        }

        public double Amplitude
        {
            get
            {
                return m_amplitude;
            }
            set
            {
                m_amplitude = value;
                Invalidate();
            }
        }

        public double Frequency
        {
            get
            {
                return m_frequency;
            }
            set
            {
                m_frequency = value;
                Invalidate();
            }
        }

        public int Octaves
        {
            get
            {
                return m_octaves;
            }
            set
            {
                m_octaves = value;
                Invalidate();
            }
        }
        public int Seed
        {
            get
            {
                return m_seed;
            }
            set
            {
                m_seed = value;
                Invalidate();
            }
        }
        public GridControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.White;

            m_cellHeight = 16.0f;
            m_cellWidth = 16.0f;
            m_translateX = 0.0f;
            m_translateY = 0.0f;
            m_scale = 1.0f;
            m_rotateAngle = 0.0f;

            m_translateStep = 10.0f;
            m_scaleStep = 1.1f;
            m_rotateStep = 15.0f;
            m_scaleMin = (float)System.Math.Pow(m_scaleStep, -15.0f);
            m_scaleMax = (float)System.Math.Pow(m_scaleStep, 15.0f);

            m_mouseLocation = new Point(0, 0);
            m_highlightedCellLocation = new Point(0, 0);
            m_selectedCellLocation = new Point(0, 0);

            m_selectedCellBrush = new SolidBrush(Color.FromArgb(192, Color.Blue));
            m_highlightedCellBrush = new SolidBrush(Color.FromArgb(128, Color.SkyBlue));
            m_gridPen = new Pen(Color.LightGray, 1);

            m_viewport = new Rectangle(0, 0, (int)(ClientRectangle.Right / m_cellWidth) + 1, (int)(ClientRectangle.Bottom / m_cellHeight) + 1);

            m_debug = new SortedDictionary<string, object>();
            m_debugFont = new Font(FontFamily.GenericMonospace, 9);
            m_debugBackgroundBrush = new SolidBrush(Color.FromArgb(192, Color.White));
            m_debugForegroundBrush = new SolidBrush(Color.FromArgb(192, Color.Black));
            m_originBrush = new SolidBrush(Color.FromArgb(128, Color.Orange));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Matrix m = e.Graphics.Transform.Clone();
            m.Translate(m_translateX, m_translateY);
            m.Scale(m_scale, m_scale);
            m.Rotate(m_rotateAngle);
            e.Graphics.Transform = m;
            UpdateCellLocations(e.Graphics);

            PaintGrid(e.Graphics);

                RectangleF viewport = m_viewport;

                //viewport.Inflate(300, 300);
                //List<Item> items = m_quadTree.Query(viewport);
                Azmyth.Math.PerlinNoise noise = new Azmyth.Math.PerlinNoise(m_persistance, m_frequency, m_amplitude, m_octaves, m_seed);
                int cellX, cellY, totalCells;

                cellX = (int)viewport.Left;
                cellY = (int)viewport.Top;

                totalCells = (int)((viewport.Width + 1) * (viewport.Height + 1));

                for (int index = 0; index < totalCells; index++)
                {
                    double height = System.Math.Round(noise.GetHeight(cellX, cellY), 2);

                    if (m_heightMap)
                    {
                        if (height > 0)
                        {
                            var val = 255 * height > 255 ? 255 : 255 * height;
                            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, (int)val, (int)val, (int)val)), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        //else if(height == 0)
                       // {
                        //    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 0, 0)), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        ////
                        
                        //}
                        else
                        {
                            var val = 255 * System.Math.Abs(height) > 255 ? 255 : 255 * System.Math.Abs(height);
                            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, (int)val, (int)val, 255)), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        
                        }
                    }
                    else
                    {
                        if(height <= 0.04 && height > 0.03)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.CornflowerBlue), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        
                        }
                        else if (height < .07 && height > 0.04)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        else if (height > .06)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Brown), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        else if(height < -0.07)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                    }
                    
                    cellX++;

                    if (cellX > viewport.Right)
                    {
                        cellY++;
                        cellX = (int)viewport.Left;
                    }
                }

               //foreach (Item i in items)
               //{
               //     e.Graphics.FillRectangle(new SolidBrush(i.Color), new RectangleF(i.Rectangle.X * m_cellWidth, i.Rectangle.Y * m_cellHeight, m_cellWidth, m_cellHeight));
               //}


            PaintSelectedCell(e.Graphics);
            PaintHighlightedCell(e.Graphics);


            m_debug["m_cellWidth"] = m_cellWidth;
            m_debug["m_cellHeight"] = m_cellHeight;
            m_debug["m_translateY"] = m_translateY;
            m_debug["m_translateX"] = m_translateX;
            m_debug["m_rotateAngle"] = m_rotateAngle;
            m_debug["m_scale"] = m_scale;
            m_debug["matrix"] = String.Join(", ", m.Elements);

            

            //PaintDebug(e.Graphics);

            PaintBorder(e.Graphics);
        }

        private void UpdateCellLocations(Graphics g)
        {
            Matrix mouseMatrix = g.Transform.Clone();
            Point[] points = new Point[] { m_mouseLocation };
            mouseMatrix.Invert();
            mouseMatrix.TransformPoints(points);
            Point transformedMouseLocation = points[0];

            m_highlightedCellLocation.X = (int)System.Math.Floor(transformedMouseLocation.X / m_cellWidth);
            m_highlightedCellLocation.Y = (int)System.Math.Floor(transformedMouseLocation.Y / m_cellHeight);

            if (m_isLeftMouseDown)
            {
                m_selectedCellLocation = m_highlightedCellLocation;
            }

            m_debug["m_highlightedCellLocation.X"] = m_highlightedCellLocation.X;
            m_debug["m_highlightedCellLocation.Y"] = m_highlightedCellLocation.Y;
            m_debug["m_selectedCellLocation.X"] = m_selectedCellLocation.X;
            m_debug["m_selectedCellLocation.Y"] = m_selectedCellLocation.Y;
            m_debug["m_mouseLocation.X"] = m_mouseLocation.X;
            m_debug["m_mouseLocation.Y"] = m_mouseLocation.Y;
            m_debug["mouseMatrix"] = String.Join(", ", mouseMatrix.Elements);
            m_debug["transformedMouseLocation.X"] = transformedMouseLocation.X;
            m_debug["transformedMouseLocation.Y"] = transformedMouseLocation.Y;
        }

        private void PaintBorder(Graphics g)
        {
            g.ResetTransform();

            g.DrawRectangle(Pens.Black, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width-1, ClientRectangle.Height-1);
        }

        private void PaintHighlightedCell(Graphics g)
        {
            g.FillRectangle(m_highlightedCellBrush,
                m_highlightedCellLocation.X * m_cellWidth,
                m_highlightedCellLocation.Y * m_cellHeight,
                m_cellWidth,
                m_cellHeight);
        }

        private void PaintSelectedCell(Graphics g)
        {
            g.FillRectangle(m_selectedCellBrush,
                m_selectedCellLocation.X * m_cellWidth,
                m_selectedCellLocation.Y * m_cellHeight,
                m_cellWidth,
                m_cellHeight);
        }

        private void PaintGrid(Graphics g)
        {
            RectangleF bounds = g.VisibleClipBounds;

            int startCol = (int)(bounds.Left / m_cellWidth);
            int endCol = (int)(bounds.Right / m_cellWidth);
            for (int col = startCol; col <= endCol; col++)
            {
                float x = col * m_cellWidth;
                g.DrawLine(m_gridPen, x, bounds.Top, x, bounds.Bottom);
            }

            int startRow = (int)(bounds.Top / m_cellHeight);
            int endRow = (int)(bounds.Bottom / m_cellHeight);
            for (int row = startRow; row <= endRow; row++)
            {
                float y = row * m_cellHeight;
                g.DrawLine(m_gridPen, bounds.Left, y, bounds.Right, y);
            }

            m_debug["bounds"] = bounds;

            Matrix viewportMatrix = g.Transform.Clone();
            Point[] points = new Point[] { new Point(0, 0) };
            viewportMatrix.Invert();
            viewportMatrix.TransformPoints(points);
            Point transformedOrigin = points[0];

            int viewportX = (int)System.Math.Floor(transformedOrigin.X / m_cellWidth);
            int viewportY = (int)System.Math.Floor(transformedOrigin.Y / m_cellHeight);

            m_viewport = new Rectangle(viewportX, viewportY, (int)(bounds.Width / m_cellWidth) + 1, (int)(bounds.Height / m_cellHeight) + 1);
            m_debug["cellBounds"] = m_viewport;
        }

        private void PaintDebug(Graphics g)
        {
            g.FillRectangle(m_originBrush, 0, 0, m_cellWidth, m_cellHeight);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> item in m_debug)
            {
                string value = item.Value == null ? "<null>" : item.Value.ToString();
                sb.AppendLine(String.Format("{0} = {1}", item.Key, value));
            }
            string s = sb.ToString();

            Matrix savedMatrix = g.Transform.Clone();
            g.ResetTransform();

            SizeF stringSize = g.MeasureString(s, m_debugFont);
            RectangleF backgroundRect = new RectangleF(1, 1, stringSize.Width, stringSize.Height);
            g.FillRectangle(m_debugBackgroundBrush, backgroundRect);
            g.DrawString(s, m_debugFont, m_debugForegroundBrush, backgroundRect);

            g.Transform = savedMatrix;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_mouseLocation = e.Location;

            if (m_isRightMouseDown)
            {
                int dX = m_mouseLocation.X - m_previousMouseLocation.X;
                int dY = m_mouseLocation.Y - m_previousMouseLocation.Y;

                m_translateX += dX;
                m_translateY += dY;

                if(ViewportChanged != null)
                {
                    ViewportChanged(this, new CellEventArgs(m_viewport));
                }
            }

            m_previousMouseLocation = e.Location;

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_mouseDownLocation = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                m_isLeftMouseDown = true;
                Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                m_isRightMouseDown = true;
                Invalidate();
            }

            this.Focus();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_isLeftMouseDown = false;
                Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                m_isRightMouseDown = false;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            m_debug["e.Delta"] = e.Delta;

            int detents = e.Delta / SystemInformation.MouseWheelScrollDelta;
            if (detents < 0)
            {
                m_scale /= m_scaleStep;
                if (m_scale < m_scaleMin) m_scale = m_scaleMin;
                Invalidate();
            }
            else if (detents > 0)
            {
                m_scale *= m_scaleStep;
                if (m_scale > m_scaleMax) m_scale = m_scaleMax;
                Invalidate();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: m_translateY -= m_translateStep; break;
                case Keys.A: m_translateX -= m_translateStep; break;
                case Keys.S: m_translateY += m_translateStep; break;
                case Keys.D: m_translateX += m_translateStep; break;
                case Keys.Q: m_rotateAngle -= m_rotateStep; break;
                case Keys.E: m_rotateAngle += m_rotateStep; break;
                case Keys.Subtract:
                    m_scale /= m_scaleStep;
                    if (m_scale < m_scaleMin) m_scale = m_scaleMin;
                    break;
                case Keys.Add:
                    m_scale *= m_scaleStep;
                    if (m_scale > m_scaleMax) m_scale = m_scaleMax;
                    break;
                default: return;
            }
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GridControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);

        }
    }

    public class CellEventArgs : EventArgs
    {
        public CellEventArgs(Rectangle cells)
        {
            Cells = cells;
        }

        public Rectangle Cells { get; set; }
    }
}