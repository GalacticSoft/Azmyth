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
    public delegate void CellEvent(object sender, CellEventArgs e);
    public enum Tool
    {
        Selection,
        Drawing,
        Erase
    }

    public enum ToolShape
    {
        Circle,
        Square,
        Point,
        Line,
    }

    public class GridControl : Control
    {

        public Tool Tool
        {
            get;
            set;
        }

        public ToolShape ToolShape
        {
            get;
            set;
        }

        public int ToolSize
        {
            get;
            set;
        }

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

        private System.Drawing.Rectangle m_viewport;
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

            Tool = Tool.Drawing;
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
                m_debug["m_cellWidth"] = m_cellWidth;
                m_debug["m_cellHeight"] = m_cellHeight;
                m_debug["m_translateY"] = m_translateY;
                m_debug["m_translateX"] = m_translateX;
                m_debug["m_rotateAngle"] = m_rotateAngle;
                m_debug["m_scale"] = m_scale;

                PaintDebug(e.Graphics);
            }

            PaintBorder(e.Graphics);
        }

        private void PaintCells(Graphics g)
        {
            float colorPercent;
            int rgb, cellX, cellY, totalCells;

            Room room = null;
            Color cellColor = Color.White;

            cellX = (int)m_viewport.Left;
            cellY = (int)m_viewport.Top;
            totalCells = (int)((m_viewport.Width + 1) * (m_viewport.Height + 1));

            
            if (m_world != null)
            {
                for (int index = 0; index < totalCells; index++)
                {
                    room = m_world.GetRoom(cellX, cellY);

                    colorPercent = room.Height / m_world.TerrainHeight;
                    rgb = (int)Math.Max(Math.Min(255 * Math.Abs(colorPercent), 255), 0);

                    if (m_heightMap)
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
                        switch (room.m_terrain)
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
                            case TerrainTypes.Mountain:
                                cellColor = ControlPaint.Light(Color.SaddleBrown, colorPercent);
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
                            case TerrainTypes.River:
                                cellColor = ControlPaint.Light(Color.Blue, colorPercent);
                                break;
                            case TerrainTypes.Ice:
                                cellColor = Color.Cyan;
                                break;
                            case TerrainTypes.Black:
                                cellColor = Color.White;
                                break;
                        }
                    }

                    g.FillRectangle(new SolidBrush(cellColor), new RectangleF(cellX * m_cellWidth, cellY * m_cellHeight, m_cellWidth, m_cellHeight));

                    PaintSelection(g, room, cellColor);
                    //viewport.Inflate(300, 300);
                    //List<Item> items = m_quadTree.Query(viewport);

                    //foreach (Item i in items)
                    //{
                    //     e.Graphics.FillRectangle(new SolidBrush(i.Color), new RectangleF(i.Rectangle.X * m_cellWidth, i.Rectangle.Y * m_cellHeight, m_cellWidth, m_cellHeight));
                    //}

                    cellX++;

                    if (cellX > m_viewport.Right)
                    {
                        cellY++;
                        cellX = (int)m_viewport.Left;
                    }
                }
            }
        }

        private void PaintSelection(Graphics g, Room room, Color cellColor)
        {
            Rectangle rect;
            Point selection = m_selectedCellLocation;
            int selectionWidth = (int)(m_selectionSize / (m_cellWidth * m_scale));
            int selectionHeight = (int)(m_selectionSize / (m_cellHeight * m_scale));
            switch (m_selectionShape)
            {
                case ToolShape.Square:
                    selection.Offset((selectionWidth) / -2, (selectionHeight) / -2);
                    rect = new Rectangle(selection, new Size(selectionWidth + 1, selectionHeight + 1));

                    if (rect.IntersectsWith(new Rectangle((int)room.GridX, (int)room.GridY, 1, 1)))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255 - cellColor.R, 255 - cellColor.G, 255 - cellColor.B)), new RectangleF(room.GridX * m_cellWidth, room.GridY * m_cellHeight, m_cellWidth, m_cellHeight));
                    }
                    break;
                case ToolShape.Circle:
                    using (GraphicsPath myPath = new GraphicsPath())
                    {
                        selection.Offset(((selectionWidth + 2) / -2), ((selectionHeight + 2) / -2));
                        rect = new Rectangle(selection, new Size(selectionWidth  +1, selectionHeight + 1));

                        myPath.AddEllipse(rect);

                        if (myPath.IsVisible(new Point((int)room.GridX, (int)room.GridY)))
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255 - cellColor.R, 255 - cellColor.G, 255 - cellColor.B)), new RectangleF(room.GridX * m_cellWidth, room.GridY * m_cellHeight, m_cellWidth, m_cellHeight));
                        }
                    }
                    break;
                case ToolShape.Point:
                    rect = new Rectangle(selection, new Size(1, 1));

                    if (rect.IntersectsWith(new Rectangle((int)room.GridX, (int)room.GridY, 1, 1)))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255 - cellColor.R, 255 - cellColor.G, 255 - cellColor.B)), new RectangleF(room.GridX * m_cellWidth, room.GridY * m_cellHeight, m_cellWidth, m_cellHeight));

                    }
                    break;
            }
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
            switch (Tool)
            {
                case Tool.Selection:
                {
                    g.FillRectangle(m_highlightedCellBrush,
                        m_highlightedCellLocation.X * m_cellWidth,
                        m_highlightedCellLocation.Y * m_cellHeight,
                        m_cellWidth,
                        m_cellHeight);
                    break;
                }
                case Tool.Drawing:
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
                    break;
                }  
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
            m_debug["e.Delta"] = e.Delta;

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
            this.Enter += new System.EventHandler(this.GridControl_Enter);
            this.Leave += new System.EventHandler(this.GridControl_Leave);
            this.MouseCaptureChanged += new System.EventHandler(this.GridControl_MouseCaptureChanged);
            this.MouseEnter += new System.EventHandler(this.GridControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.GridControl_MouseLeave);
            this.ResumeLayout(false);

        }

        private void GridControl_MouseLeave(object sender, EventArgs e)
        {

        }

        private void GridControl_Leave(object sender, EventArgs e)
        {

        }

        private void GridControl_Enter(object sender, EventArgs e)
        {

        }

        private void GridControl_MouseEnter(object sender, EventArgs e)
        {

        }

        private void GridControl_MouseCaptureChanged(object sender, EventArgs e)
        {
           
        }
    }

    public class CellEventArgs : EventArgs
    {
        public CellEventArgs(System.Drawing.Rectangle cells)
        {
 
            Cells = cells;
        }

        public System.Drawing.Rectangle Cells { get; set; }
    }
}