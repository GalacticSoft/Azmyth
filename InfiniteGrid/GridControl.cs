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

namespace InfiniteGrid
{
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

        public QuadTree<Item> m_quadTree;

        public GridControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.Blue;

            m_cellHeight = 16.0f;
            m_cellWidth = 16.0f;
            m_translateX = 0.0f;
            m_translateY = 0.0f;
            m_scale = 1.0f;
            m_rotateAngle = 0.0f;

            m_translateStep = 10.0f;
            m_scaleStep = 1.1f;
            m_rotateStep = 15.0f;
            m_scaleMin = (float)Math.Pow(m_scaleStep, -15.0f);
            m_scaleMax = (float)Math.Pow(m_scaleStep, 15.0f);

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

            e.Graphics.FillRectangle(Brushes.Blue, e.Graphics.VisibleClipBounds);
            if (m_quadTree != null)
            {
                RectangleF viewport = m_viewport;

                //viewport.Inflate(3, 3);
                //List<Item> items = m_quadTree.Query(viewport);

                Azmyth.Math.PerlinNoise noise = new Azmyth.Math.PerlinNoise(1, 0.05f, 1, 1, 500);

                double height = 0;
                for (float x = viewport.X; x <= viewport.Width; x++)
                {
                    for (float y = viewport.Y; y <= viewport.Height; y++)
                    {
                        height = Math.Round(noise.GetHeight(x, y), 2);
                        if (height <= .03 && height > 0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), new RectangleF(x * m_cellWidth, y * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        else if (height > .03)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.Brown), new RectangleF(x * m_cellWidth, y * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                        else
                        {
                            //e.Graphics.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(x * m_cellWidth, y * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                    }
                }

                //foreach (Item i in items)
                //{
                //    e.Graphics.FillRectangle(new SolidBrush(i.Color), new RectangleF(i.Rectangle.X * m_cellWidth, i.Rectangle.Y * m_cellHeight, m_cellWidth, m_cellHeight));
                // }
            }

            UpdateCellLocations(e.Graphics);
            PaintSelectedCell(e.Graphics);
            PaintHighlightedCell(e.Graphics);
            PaintGrid(e.Graphics);
            


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

            m_highlightedCellLocation.X = (int)Math.Floor(transformedMouseLocation.X / m_cellWidth);
            m_highlightedCellLocation.Y = (int)Math.Floor(transformedMouseLocation.Y / m_cellHeight);

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
           // for (int col = startCol; col <= endCol; col++)
           // {
          //      float x = col * m_cellWidth;
          //      g.DrawLine(m_gridPen, x, bounds.Top, x, bounds.Bottom);
          //  }

            int startRow = (int)(bounds.Top / m_cellHeight);
            int endRow = (int)(bounds.Bottom / m_cellHeight);
            //for (int row = startRow; row <= endRow; row++)
            //{
           //     float y = row * m_cellHeight;
           //     g.DrawLine(m_gridPen, bounds.Left, y, bounds.Right, y);
            //}

            m_debug["bounds"] = bounds;

            Matrix viewportMatrix = g.Transform.Clone();
            Point[] points = new Point[] { new Point(0, 0) };
            viewportMatrix.Invert();
            viewportMatrix.TransformPoints(points);
            Point transformedOrigin = points[0];

            int viewportX = (int)Math.Floor(transformedOrigin.X / m_cellWidth);
            int viewportY = (int)Math.Floor(transformedOrigin.Y / m_cellHeight);

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
            this.BackColor = System.Drawing.Color.Blue;
            this.ResumeLayout(false);

        }
    }
}