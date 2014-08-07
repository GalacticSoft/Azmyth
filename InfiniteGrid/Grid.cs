using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace InfiniteGrid
{
    public delegate void CellEvent(object sender, CellEventArgs e);

    //TODO: Add Zoom feature.
    //TODO: Fix Pointer tracking when selecting.
    //TODO: Fix selection code there seems to be a lower limit of -200 in both x and y and an abritrary upper limit.
    //TODO: offset the dottted line of the grid so it doesn't appear to slide across. Maybe add emphasis on corners.
    //TODO: When filling a 200, 200 grid the bottom right cell is (199, 200), should be (199, 199) for 0 based grids.
    public partial class Grid : UserControl
    {
        private int m_deltaX = 0;
        private int m_deltaY = 0;
        private int m_offsetX = 0;
        private int m_offsetY = 0;
        private int m_cellOffsetX = 0;
        private int m_cellOffsetY = 0;
        private int m_cellSize = 32;

        private Rectangle m_selection;
        private Point m_lastMousePosition;
        private Point m_lastSelectMousePosition;

        private int m_selectedX = 0;
        private int m_selectedY = 0;
        private int m_selectDeltaX;
        private int m_selectDeltaY;
        private int m_selectionOffsetX;
        private int m_selectionOffsetY;
        private int m_selectionStartX;
        private int m_selectionStartY;

        private int m_mouseOffsetX = 0;
        private int m_mouseOffsetY = 0;
        private int m_mouseX = 0;
        private int m_mouseY = 0;

        private bool m_dragging = false;
        private bool m_selecting = false;

        public event CellEvent HoverChanged;
        public event CellEvent ViewportChanged;
        public event CellEvent ViewportChanging;
        public event CellEvent SelectionChanged;

        private QuadTree<Item> m_quadTree;

        [Browsable(false)]
        public Rectangle Selection
        {
            get
            {
                int selectedX = GetCellX(m_selection.Location);
                int selectedY = GetCellY(m_selection.Location);
                int selectedWidth = m_selection.Width / m_cellSize;
                int selectedHeight = m_selection.Height / m_cellSize;

                return new Rectangle(selectedX, selectedY, selectedWidth, selectedHeight);
            }
        }

        [Browsable(false)]
        public Rectangle Viewport
        {
            get
            {
                return GetViewPortRectangle();
            }
        }

        [Browsable(true), Category("Appearance")]
        public bool ShowOrigin { get; set; }

        [Browsable(true), Category("Appearance")]
        public bool ShowGrid { get; set; }

        [Browsable(true), Category("Appearance")]
        public bool ShowSelection { get; set; }

        [Browsable(true), Category("Appearance")]
        public Color SelectionColor { get; set; }
        
        [Browsable(true), Category("Appearance")]
        public Color OriginColor { get; set; }
        
        [Browsable(true), Category("Appearance")]
        public Color GridColor { get; set; }


        public QuadTree<Item> QuadTree
        {
            get { return m_quadTree; }
            set
            {
                m_quadTree = value;
                Invalidate();
            }
        }
        public Grid()
        {
            GridColor = Color.Black;
            OriginColor = Color.Black;
            SelectionColor = Color.Red;

            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void Grid_Paint(object sender, PaintEventArgs e)
        {
            int width = Width;
            int height = Height;

            int rows = (height / m_cellSize) + 1;
            int cols = (width / m_cellSize) + 1;

            Graphics graphics = e.Graphics;
            Rectangle selection = Selection;
            Rectangle client = ClientRectangle;

            client.Inflate(200, 200);
            if (m_quadTree != null)
            {
                if (m_quadTree.Count > 0)
                {
                    Rectangle rect = Viewport;

                    rect.Inflate(5, 5);
                    
                    foreach (Item i in m_quadTree.Query(rect))
                    {
                        graphics.FillRectangle(new SolidBrush(i.Color), (i.Rectangle.X * m_cellSize) + m_offsetX, (i.Rectangle.Y * m_cellSize) + m_offsetY, m_cellSize, m_cellSize);
                    }
                }
            }

            if (ShowGrid)
            {
                using (Pen gridPen = new Pen(GridColor, 1))
                {
                    //TODO: calcuate the dots based on offset.
                    float[] dashValues = { 1, 1 };
                    gridPen.DashPattern = dashValues;

                    for (int index = 0; index <= rows; index++)
                    {
                        int yPos = (index * m_cellSize) + (m_offsetY % m_cellSize);

                        graphics.DrawLine(gridPen, new Point(0, yPos), new Point(width, yPos));
                    }

                    for (int index = 0; index <= cols; index++)
                    {
                        int xPos = (index * m_cellSize) + (m_offsetX % m_cellSize);

                        graphics.DrawLine(gridPen, new Point(xPos, 0), new Point(xPos, width));
                    }
                }
            }

            if (ShowOrigin)
            {
                if (client.Contains(new Point(m_offsetX, m_offsetY)))
                {
                    using (Pen m_originPen = new Pen(OriginColor, 3))
                    {
                        graphics.DrawString("(0, 0) (" + m_cellOffsetX + ", " + m_cellOffsetY + ")",
                                new Font("Arial", 20), new SolidBrush(OriginColor),
                                new Point(m_offsetX + m_cellSize, m_offsetY + m_cellSize));

                        graphics.DrawRectangle(m_originPen, new Rectangle(new Point(m_offsetX, m_offsetY), new Size(m_cellSize, m_cellSize)));
                    }
                }
            }

            if (client.Contains(selection))
            {
                using (Pen m_selectionPen = new Pen(new SolidBrush(SelectionColor), 3))
                {
                    if (ShowSelection)
                    {
                        graphics.DrawString("(" + selection.X + "," + selection.Y + ") (" + selection.Width + ", " + selection.Height + ")",
                                new Font("Arial", 20), new SolidBrush(SelectionColor),
                                new Point(0, this.Height - 40));
                    }

                    graphics.DrawRectangle(m_selectionPen, m_selection);
                }
            }
        }

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_dragging)
            {
                m_dragging = false;

                Invalidate();

                if(ViewportChanged != null)
                {
                    ViewportChanged(this, new CellEventArgs(Viewport));
                }
            }
            if (m_selecting)
            {
                m_selecting = false;

                if (m_selection.X == m_selectionStartX && m_selection.Width % m_cellSize != 0)
                {
                    var nextWholeCellSnapX = m_cellSize - (m_selection.Width % m_cellSize);
                    m_selection.Width += nextWholeCellSnapX;
                }
                else if (e.Location.X < m_selectionStartX)
                {
                    var nextWholeCellSnapX = m_selection.X % m_cellSize;
                    nextWholeCellSnapX += (m_cellSize - GetPartialCellSizeX());
                    nextWholeCellSnapX %= m_cellSize;
                    m_selection.X -= nextWholeCellSnapX;
                    m_selection.Width += nextWholeCellSnapX;
                }

                if (m_selection.Y == m_selectionStartY && m_selection.Height % m_cellSize != 0)
                {
                    var nextWholeCellSnapY = m_cellSize - (m_selection.Height % m_cellSize);
                    m_selection.Height += nextWholeCellSnapY;
                }
                else if (e.Location.Y < m_selectionStartY)
                {
                    var nextWholeCellSnapY = m_selection.Y % m_cellSize;
                    nextWholeCellSnapY += (m_cellSize - GetPartialCellSizeY());
                    nextWholeCellSnapY %= m_cellSize;
                    m_selection.Y -= nextWholeCellSnapY;
                    m_selection.Height += nextWholeCellSnapY;
                }

                Invalidate();

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, new CellEventArgs(Selection));
                }
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_dragging)
            {
                Point p = e.Location;

                m_deltaX = p.X - m_lastMousePosition.X;
                m_deltaY = p.Y - m_lastMousePosition.Y;

                m_offsetX += m_deltaX;
                m_offsetY += m_deltaY;

                m_selection.X += m_deltaX;
                m_selection.Y += m_deltaY;

                // I changed this from ceiling to Floor.  It made more sense to me to have this represent the # of
                // whole cells offset from the viewport and not to include a partial cell in this.  It shouldn't be too
                // hard to flip this in the other direction, if you wanted tho.
                m_cellOffsetX = (int)System.Math.Floor((double)m_offsetX / (double)m_cellSize);
                m_cellOffsetY = (int)System.Math.Floor((double)m_offsetY / (double)m_cellSize);

                m_lastMousePosition = (e.Location);

                Invalidate();

                if (ViewportChanging != null)
                {
                    ViewportChanging(this, new CellEventArgs(Viewport));
                }
            }
            if (m_selecting)
            {
                Point p = e.Location;
                // TODO: Improved pointer tracking
                // The pointer tracking seems to be off by the distance between  
                // the original click and the distance from the cells origin

                m_selectDeltaX = p.X - m_lastSelectMousePosition.X;
                m_selectDeltaY = p.Y - m_lastSelectMousePosition.Y;

                m_selectionOffsetX += m_selectDeltaX;
                m_selectionOffsetY += m_selectDeltaY;

                m_selection.Height = System.Math.Abs(m_cellSize + m_selectionOffsetY + m_selectDeltaY);
                m_selection.Width = System.Math.Abs(m_cellSize + m_selectionOffsetX + m_selectDeltaX);

                m_selection.X = System.Math.Min(m_selectionStartX, m_selectionStartX + m_cellSize + m_selectionOffsetX + m_selectDeltaX);
                m_selection.Y = System.Math.Min(m_selectionStartY, m_selectionStartY + m_cellSize + m_selectionOffsetY + m_selectDeltaY);

                m_lastSelectMousePosition = e.Location;

                Invalidate();
            }

            if (HoverChanged != null)
            {
                HoverChanged(this, new CellEventArgs(new Rectangle(GetCellX(e.Location), GetCellY(e.Location), 1, 1)));
            }
        }

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_dragging = true;
                m_lastMousePosition = (e.Location);
            }
            else if (e.Button == MouseButtons.Left)
            {
                m_selecting = true;
                m_lastSelectMousePosition = e.Location;
                m_selectDeltaX = 0;
                m_selectionOffsetX = 0;
                m_selectDeltaY = 0;
                m_selectionOffsetY = 0;

                m_selectedX = GetCellX(e.Location);
                m_selectedY = GetCellY(e.Location);

                var cellX = (int)System.Math.Floor((((double)m_mouseOffsetX) / ((double)m_cellSize)));
                var cellY = (int)System.Math.Floor((((double)m_mouseOffsetY) / ((double)m_cellSize)));

                m_selectionStartX = cellX * m_cellSize + GetPartialCellSizeX();
                m_selectionStartY = cellY * m_cellSize + GetPartialCellSizeY();

                m_selection = new Rectangle(m_selectionStartX, m_selectionStartY, m_cellSize, m_cellSize);

                Invalidate();
            }
        }

        private int GetPartialCellSizeX()
        {
            return (m_offsetX + m_cellSize) % m_cellSize;
        }

        private int GetPartialCellSizeY()
        {
            return (m_offsetY + m_cellSize) % m_cellSize;
        }

        private int GetCellX(Point p)
        {
            int x = 0;

            m_mouseX = p.X;

            // We add a cell to the offset so that when the viewport and the grid are perfectly aligned the modulo
            // returns 0 instead of 16.  I substract this out from p.X to get the mouse position as it lies in the
            // entirely visible grid.  
            m_mouseOffsetX = p.X - GetPartialCellSizeX();

            // Just doing a simple division works for m_mouseOffsetX > 0. But when a partially visible cell is selected
            // m_mouseOffsetX will be negative and -10 / 16 = 0, not -1.
            int cellX = (int)Math.Floor((((double)m_mouseOffsetX) / ((double)m_cellSize)));

            x = m_cellOffsetX - cellX;

            return x * -1;
        }

        private int GetCellY(Point p)
        {
            int y = 0;

            m_mouseY = p.Y;

            // We add a cell to the offset so that when the viewport and the grid are perfectly aligned the modulo
            // returns 0 instead of 16.  I substract this out from p.Y to get the mouse position as it lies in the
            // entirely visible grid.  
            m_mouseOffsetY = p.Y - GetPartialCellSizeY();

            // Just doing a simple division works for m_mouseOffsetY > 0. But when a partially visible cell is selected
            // m_mouseOffsetY will be negative and -10 / 16 = 0, not -1.
            int cellY = (int)Math.Floor((((double)m_mouseOffsetY) / ((double)m_cellSize)));

            y = m_cellOffsetY - cellY;

            return y * -1;
        }

        private Rectangle GetViewPortRectangle()
        {
            return new Rectangle(GetViewPortOrigin(), GetViewPortSize());
        }

        private Point GetViewPortOrigin()
        {
            return new Point(GetViewPortX(), GetViewPortY());
        }

        private int GetViewPortX()
        {
            return GetCellX(new Point(0, 0));
        }
        private int GetViewPortY()
        {
            return GetCellY(new Point(0, 0));
        }

        private Size GetViewPortSize()
        {
            return new Size(Width / m_cellSize, Height / m_cellSize);
        }

        private void Grid_Resize(object sender, EventArgs e)
        {
            if (ViewportChanged != null)
            {
                ViewportChanged(this, new CellEventArgs(Viewport));
            }
        }

        public void MoveToOrigin()
        {
            m_offsetX = 0;
            m_offsetY = 0;
            m_cellOffsetX = 0;
            m_cellOffsetY = 0;

            m_selection = new Rectangle(0, 0, m_cellSize, m_cellSize);

            m_selectionOffsetX = 0;
            m_selectionOffsetY = 0;
            m_selectionStartX = 0;
            m_selectionStartY = 0;

            Invalidate();
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
