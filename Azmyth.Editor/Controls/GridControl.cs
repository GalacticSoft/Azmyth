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
using Azmyth.Procedural;
using Azmyth;
using Azmyth.Assets;
namespace Azmyth.Editor
{
    public enum ToolShape
    {
        Circle,
        Square,
        Point,
        Line,
    }

    public class GridControl : Control
    {


        private ToolShape m_selectionShape;
        private int m_selectionSize;

        public World m_world;

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

        private Pen m_gridPen;
        private Brush m_selectedCellBrush;
        private Brush m_highlightedCellBrush;
        
        private bool m_showDebug;
        private Font m_debugFont;
        private Brush m_debugBackgroundBrush;
        private Brush m_debugForegroundBrush;
        private Brush m_originBrush;
        private SortedDictionary<string, object> m_debug;
        private GraphicsPath m_graphicsPath = new GraphicsPath();

        private System.Drawing.Rectangle m_viewport;

        private bool m_heightMap;
        private bool m_tempMap;

        [Browsable(true), Category("Selection Tool")]
        public ToolShape ToolShape
        {
            get;
            set;
        }

        [Browsable(true), Category("Selection Tool")]
        public int ToolSize
        {
            get;
            set;
        }

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

        [Browsable(true), Category("Appearance")]
        public bool TempMap
        {
            get
            {
                return m_tempMap;
            }
            set
            {
                m_tempMap = value;
                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance")]
        public bool ShowDebug
        {
            get
            {
                return m_showDebug;
            }
            set
            {
                m_showDebug = value;
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

            ToolShape = ToolShape.Point;
            ToolSize = 40;

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
            PaintCells(e.Graphics);
            PaintHighlightedCell(e.Graphics);

            if (m_showDebug)
            {
                PaintDebug(e.Graphics);
            }

            PaintBorder(e.Graphics);
        }


        private void PaintCells(Graphics g)
        {
            float colorPercent;
            int rgb, cellX, cellY, totalCells;

            TerrainTile room = null;
            Color cellColor = Color.White;

            Dictionary<PointF, string> cityNames = new Dictionary<PointF, string>();

            cellX = (int)m_viewport.Left;
            cellY = (int)m_viewport.Top;
            totalCells = (int)((m_viewport.Width + 1) * (m_viewport.Height + 1));
            
            if (m_world != null)
            {
                for (int index = 0; index < totalCells; index++)
                {
                    room = m_world.LoadTile(cellX, cellY);

                    colorPercent = room.Height / m_world.TerrainHeight;
                    rgb = (int)Math.Max(Math.Min(255 * Math.Abs(colorPercent), 255), 0);

                    if(m_tempMap)
                    {
                        colorPercent = (float)(room.m_temp / 50);
                        rgb = (int)Math.Max(Math.Min(255 * Math.Abs(colorPercent), 255), 0);
                        cellColor = Color.FromArgb(255, rgb, 0, 255-rgb);
                    }
                    else if (m_heightMap)
                    {
                        if (room.Height > m_world.CoastLine)
                        {
                            cellColor = Color.FromArgb(255, rgb, rgb, rgb);
                        }
                        else
                        {
                            cellColor = Color.FromArgb(255, 0, 0, 255 - rgb);
                        }
                    }
                    else
                    {
                        switch (room.Terrain)
                        {
                            case TerrainTypes.Ocean:
                                cellColor = Color.FromArgb(255, 0, 0, 255 - rgb);
                                break;
                            case TerrainTypes.Dirt:
                                cellColor = ControlPaint.Light(Color.SaddleBrown, colorPercent);
                                break;
                            case TerrainTypes.Sand:
                                cellColor = ControlPaint.Light(Color.BurlyWood, colorPercent);
                                break;
                            case TerrainTypes.Grass:
                                RandomNoise rn = new RandomNoise(1);

                                double val = rn.GetValue(cellX, cellY);

                                if (val < .33)
                                    cellColor = Color.Green;
                                else if (val < .66)
                                    cellColor = Color.ForestGreen;
                                else
                                    cellColor = Color.DarkGreen;
                                
                                break;
                            case TerrainTypes.Stone:
                                cellColor = Color.SlateGray;
                                break;
                            case TerrainTypes.Snow:
                                cellColor = ControlPaint.Light(Color.LightGray, colorPercent);
                                break;
                            case TerrainTypes.Lava:
                                cellColor = Color.Red;
                                break;
                            case TerrainTypes.Ice:
                                cellColor = Color.Cyan;
                                break;
                            case TerrainTypes.None:
                                cellColor = Color.Black;
                                break;
                            case TerrainTypes.Road:
                                cellColor = Color.Yellow;

                                //City city = m_world.GetCity(cellX, cellY);

                                cityNames.Add(new PointF((cellX * m_cellWidth) + m_cellWidth, (cellY * m_cellHeight) + m_cellHeight), room.Name);
                                break;
                        }
                    }

                    if(IsSelected(room))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255 - cellColor.R, 255 - cellColor.G, 255 - cellColor.B)), new RectangleF(room.X * m_cellWidth, room.Y * m_cellHeight, m_cellWidth, m_cellHeight));        
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(cellColor), new RectangleF((cellX * m_cellWidth) + 1, cellY * m_cellHeight, m_cellWidth, m_cellHeight));
                    }

                    if(m_world.GetTerrainType(cellX -1, cellY) == TerrainTypes.Ocean && room.Terrain != TerrainTypes.Ocean)
                    {
                        g.DrawLine(new Pen(Color.White, 2), 
                            new PointF((cellX * m_cellWidth), (cellY * m_cellHeight)), 
                            new PointF((cellX * m_cellWidth), ((cellY * m_cellHeight) + m_cellHeight)));
                    }

                    if (m_world.GetTerrainType(cellX + 1, cellY) == TerrainTypes.Ocean && room.Terrain != TerrainTypes.Ocean)
                    {
                        g.DrawLine(new Pen(Color.White, 2), 
                            new PointF((cellX * m_cellWidth + m_cellWidth), (cellY * m_cellHeight)), 
                            new PointF(((cellX * m_cellWidth) + m_cellWidth), ((cellY * m_cellHeight) + m_cellHeight)));
                    }

                    if (m_world.GetTerrainType(cellX, cellY - 1) == TerrainTypes.Ocean && room.Terrain != TerrainTypes.Ocean)
                    {
                        g.DrawLine(new Pen(Color.White, 2), 
                            new PointF((cellX * m_cellWidth), (cellY * m_cellHeight)), 
                            new PointF((cellX * m_cellWidth) + m_cellWidth , (cellY * m_cellHeight)));
                    }

                    if (m_world.GetTerrainType(cellX, cellY + 1) == TerrainTypes.Ocean && room.Terrain != TerrainTypes.Ocean)
                    {
                        g.DrawLine(new Pen(Color.White, 2), 
                            new PointF((cellX * m_cellWidth), ((cellY * m_cellHeight) + m_cellHeight)-1), 
                            new PointF((cellX * m_cellWidth) + m_cellWidth, ((cellY * m_cellHeight) + m_cellHeight)-1));
                    }

                    cellX++;

                    if (cellX > m_viewport.Right)
                    {
                        cellY++;
                        cellX = (int)m_viewport.Left;
                    }
                }

                Font f = new Font(FontFamily.GenericMonospace, 11 * 1 / m_scale, FontStyle.Bold);

                foreach (PointF p in cityNames.Keys)
                {
                    g.DrawString(cityNames[p], f, new SolidBrush(Color.Black), new PointF(p.X, p.Y));
                }
            }
        }

        public bool IsSelected(int x, int y)
        {
            Rectangle rect;
            bool isSelected = false;
            Point selection = m_selectedCellLocation;
            int selectionWidth = (int)(m_selectionSize / (m_cellWidth * m_scale));
            int selectionHeight = (int)(m_selectionSize / (m_cellHeight * m_scale));

            switch (m_selectionShape)
            {
                case ToolShape.Square:
                    selection.Offset(((selectionWidth + 1) / -2), ((selectionHeight + 1) / -2));
                    rect = new Rectangle(selection, new Size(selectionWidth + 1, selectionHeight + 1));

                    if (rect.IntersectsWith(new Rectangle(x, y, 1, 1)))
                    {
                        isSelected = true;
                    }
                    break;
                case ToolShape.Circle:
                    selection.Offset(((selectionWidth + 2) / -2), ((selectionHeight + 2) / -2));
                    rect = new Rectangle(selection, new Size(selectionWidth + 1, selectionHeight + 1));
                    
                    m_graphicsPath.Reset();
                    m_graphicsPath.AddEllipse(rect);

                    if (m_graphicsPath.IsVisible(new Point(x, y)))
                    {
                        isSelected = true;    
                    }
                    break;
                case ToolShape.Point:
                    rect = new Rectangle(selection, new Size(1, 1));

                    if (rect.IntersectsWith(new Rectangle(x, y, 1, 1)))
                    {
                        isSelected = true;    
                    }
                    break;
            }

            return isSelected;
        }

        public bool IsSelected(Point p)
        {
            return IsSelected(p.X, p.Y);
        }

        public bool IsSelected(TerrainTile room)
        {
            return IsSelected(room.X, room.Y);
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
        }

        private void PaintBorder(Graphics g)
        {
            g.ResetTransform();

            g.DrawRectangle(Pens.Black, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width-1, ClientRectangle.Height-1);
        }

        private void PaintHighlightedCell(Graphics g)
        {
            Point mouse = m_mouseLocation;

            switch (ToolShape)
            {
                case ToolShape.Circle:
                    g.ResetTransform();
                    mouse.Offset(ToolSize / -2, ToolSize / -2);
                    g.DrawEllipse(new Pen(Color.Black, 3), new Rectangle(mouse, new Size(ToolSize, ToolSize)));
                    break;
                case ToolShape.Point:
                    g.DrawRectangle(new Pen(Color.Black, 3), m_highlightedCellLocation.X * m_cellWidth,
                        m_highlightedCellLocation.Y * m_cellHeight, m_cellWidth, m_cellHeight);
                    break;
                case ToolShape.Square:
                    g.ResetTransform();
                    mouse.Offset(ToolSize / -2, ToolSize / -2);
                    g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(mouse, new Size(ToolSize, ToolSize)));
                    break;
            }
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

            Matrix viewportMatrix = g.Transform.Clone();
            Point[] points = new Point[] { new Point(0, 0) };
            viewportMatrix.Invert();
            viewportMatrix.TransformPoints(points);
            Point transformedOrigin = points[0];

            int viewportX = (int)System.Math.Floor(transformedOrigin.X / m_cellWidth);
            int viewportY = (int)System.Math.Floor(transformedOrigin.Y / m_cellHeight);

            m_viewport = new Rectangle(viewportX, viewportY, (int)(bounds.Width / m_cellWidth) + 1, (int)(bounds.Height / m_cellHeight) + 1);
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

        public event EventHandler<Point> CellHover;

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
            m_selectionShape = ToolShape;
            m_selectionSize = ToolSize;

            m_mouseDownLocation = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                m_isLeftMouseDown = true;
                Invalidate();


                if (CellHover != null)
                {
                    CellHover(this, m_highlightedCellLocation);
                }
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
            int detents = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                ToolSize += detents * 2;
                Invalidate();
            }
            else
            {
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
}