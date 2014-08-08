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
using Stoatly.Util;

namespace InfiniteGrid
{
    public delegate void CellEvent(object sender, CellEventArgs e);

    //TODO: Add Zoom feature.
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

        private Rectangle m_dipSelection;
        private Point m_lastMousePosition;
        private Point m_lastSelectMousePosition;

        private int m_selectedX = 0;
        private int m_selectedY = 0;
        private int m_selectDeltaX;
        private int m_selectDeltaY;
        private int m_dipSelectionStartX;
        private int m_dipSelectionStartY;

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

		/// <summary>
		/// Returns the selection rect in cell units
		/// </summary>
        [Browsable(false)]
        public Rectangle CellSelection
        {
            get
            {
                int selectedX = GetCellX(m_dipSelection.Location);
                int selectedY = GetCellY(m_dipSelection.Location);
                int selectedWidth = m_dipSelection.Width / m_cellSize;
                int selectedHeight = m_dipSelection.Height / m_cellSize;

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
			Rectangle client = ClientRectangle;

			// in cells? 
			Rectangle view = Viewport;

			view.Inflate(4, 4);
			client.Inflate(200, 200);

			if (m_quadTree != null)
			{
				foreach (Item i in m_quadTree.Query(view))
				{
					graphics.FillRectangle(new SolidBrush(i.Color), (i.Rectangle.X * m_cellSize) + m_offsetX, (i.Rectangle.Y * m_cellSize) + m_offsetY, m_cellSize, m_cellSize);
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

			if (client.Contains(m_dipSelection))
			{
				using (Pen m_selectionPen = new Pen(new SolidBrush(SelectionColor), 3))
				{
					graphics.DrawRectangle(m_selectionPen, m_dipSelection);
				}
			}

			if (ShowSelection)
			{
				graphics.DrawString("(" + CellSelection.X + "," + CellSelection.Y + ") (" + CellSelection.Width + ", " + CellSelection.Height + ")",
						new Font("Arial", 20), new SolidBrush(SelectionColor),
						new Point(0, this.Height - 40));
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

                if (m_dipSelection.X == m_dipSelectionStartX && m_dipSelection.Width % m_cellSize != 0)
                {
                    var nextWholeCellSnapX = m_cellSize - (m_dipSelection.Width % m_cellSize);
                    m_dipSelection.Width += nextWholeCellSnapX;
                }
                else if (e.Location.X < m_dipSelectionStartX)
                {
                    var nextWholeCellSnapX = m_dipSelection.X % m_cellSize;
                    nextWholeCellSnapX += (m_cellSize - GetPartialCellWidth());
                    nextWholeCellSnapX %= m_cellSize;
                    m_dipSelection.X -= nextWholeCellSnapX;
                    m_dipSelection.Width += nextWholeCellSnapX;
                }

                if (m_dipSelection.Y == m_dipSelectionStartY && m_dipSelection.Height % m_cellSize != 0)
                {
                    var nextWholeCellSnapY = m_cellSize - (m_dipSelection.Height % m_cellSize);
                    m_dipSelection.Height += nextWholeCellSnapY;
                }
                else if (e.Location.Y < m_dipSelectionStartY)
                {
                    var nextWholeCellSnapY = m_dipSelection.Y % m_cellSize;
                    nextWholeCellSnapY += (m_cellSize - GetPartialCellHeight());
                    nextWholeCellSnapY %= m_cellSize;
                    m_dipSelection.Y -= nextWholeCellSnapY;
                    m_dipSelection.Height += nextWholeCellSnapY;
                }

                Invalidate();

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, new CellEventArgs(CellSelection));
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

                m_dipSelection.X += m_deltaX;
                m_dipSelection.Y += m_deltaY;

                // I changed this from ceiling to Floor.  It made more sense to me to have this represent the # of
                // whole cells offset from the viewport and not to include a partial cell in this.  It shouldn't be too
                // hard to flip this in the other direction, if you wanted tho.
                m_cellOffsetX = m_offsetX / m_cellSize;
                m_cellOffsetY = m_offsetY / m_cellSize;

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

                m_selectDeltaX = p.X - m_lastSelectMousePosition.X;
				m_selectDeltaY = p.Y - m_lastSelectMousePosition.Y;

				m_dipSelection.Height = Math.Abs(p.Y - m_dipSelectionStartY);
				m_dipSelection.Width = Math.Abs(p.X - m_dipSelectionStartX);

				m_dipSelection.X = Math.Min(m_dipSelectionStartX, p.X);
				m_dipSelection.Y = Math.Min(m_dipSelectionStartY, p.Y);

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
                m_selectDeltaY = 0;

                m_selectedX = GetCellX(e.Location);
                m_selectedY = GetCellY(e.Location);

                var cellX = (int)System.Math.Floor((((double)m_mouseOffsetX) / ((double)m_cellSize)));
                var cellY = (int)System.Math.Floor((((double)m_mouseOffsetY) / ((double)m_cellSize)));

                m_dipSelectionStartX = cellX * m_cellSize + GetPartialCellWidth();
                m_dipSelectionStartY = cellY * m_cellSize + GetPartialCellHeight();

                m_dipSelection = new Rectangle(m_dipSelectionStartX, m_dipSelectionStartY, m_cellSize, m_cellSize);

                Invalidate();
            }
        }

		/// <summary>
		/// Returns the width in dips of the cell that is only partially dragged into the viewport
		/// </summary>
		/// <returns>the size in dips</returns>
        private int GetPartialCellWidth()
        {
            return m_offsetX % m_cellSize; 
        }

		/// <summary>
		/// Returns the height in dips of the cell that is only partially dragged into the viewport
		/// </summary>
		/// <returns>the size in dips</returns>
        private int GetPartialCellHeight()
        {
            return m_offsetY % m_cellSize;
        }

        public int GetCellX(Point p)
        {
            int x = 0;

            m_mouseX = p.X;

            // We add a cell to the offset so that when the viewport and the grid are perfectly aligned the modulo
            // returns 0 instead of 16.  I substract this out from p.X to get the mouse position as it lies in the
            // entirely visible grid.  
            m_mouseOffsetX = p.X - GetPartialCellWidth();

            // Just doing a simple division works for m_mouseOffsetX > 0. But when a partially visible cell is selected
            // m_mouseOffsetX will be negative and -10 / 16 = 0, not -1.
            int cellX = m_mouseOffsetX / m_cellSize;

            x = m_cellOffsetX - cellX;

            return x * -1;
        }

        public int GetCellY(Point p)
        {
            int y = 0;

            m_mouseY = p.Y;

            m_mouseOffsetY = p.Y - GetPartialCellHeight();

            // Just doing a simple division works for m_mouseOffsetY > 0. But when a partially visible cell is selected
            // m_mouseOffsetY will be negative and -10 / 16 = 0, not -1.
            int cellY = m_mouseOffsetY / m_cellSize;

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

            m_dipSelection = new Rectangle(0, 0, m_cellSize, m_cellSize);

            m_dipSelectionStartX = 0;
            m_dipSelectionStartY = 0;

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
