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
//using Azmyth.Math;

namespace Azmyth.Editor
{
        public partial class Grid : UserControl
        {
            

            private int m_deltaX = 0;
            private int m_deltaY = 0;
            private int m_offsetX = 0;
            private int m_offsetY = 0;
            private int m_cellOffsetX = 0;
            private int m_cellOffsetY = 0;
            private int m_cellSize = 16;
            private System.Drawing.Point m_origin = new System.Drawing.Point(0, 0);

            private Rectangle selection;
            private System.Drawing.Point m_lastMousePosition;
            private System.Drawing.Point m_lastSelectMousePosition;
            
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
            private bool m_showGrid = true;
            private bool m_invertScale = false;
            private bool m_multiSelect = false;

            Pen m_gridPen = null;
            Pen m_selectionPen = null;
            Dictionary<Point, Color> m_cells = new Dictionary<Point, Color>();
            Rectangle bounds = new Rectangle(0, 0, 0, 0);

            public int CellSize
            {
                get
                {
                    return m_cellSize;
                }
                set
                {
                    m_cellSize = value;
                    selection.Width = 0;
                    selection.Height = 0;
                    selection.X = 0;
                    selection.Y = 0;
                    this.Invalidate();
                }
            }

            public event Action<Rectangle> SelectionMade;

            public bool ShowGrid
            {
                get { return m_showGrid; }
                set { m_showGrid = value; Invalidate(); }
            }

            public bool InvertScale
            {
                get { return m_invertScale; }
                set { m_invertScale = value; Invalidate(); }
            }

            public bool MultiSelect
            {
                get { return m_multiSelect; }
                set { m_multiSelect = value; }
            }

            public void SetBounds(int x, int y)
            {
                bounds = new Rectangle(0, 0, x, y);
            }

            public void SetCell(int x, int y, Color color)
            {
                Point p = new Point(x, y);

                if (m_cells.ContainsKey(p))
                {
                    m_cells[p] = color;
                }
                else
                {
                    m_cells.Add(p, color);
                }

                Invalidate();
            }

            public Dictionary<Point, Color> GetCells()
            {
                return m_cells;
            }

            public void ClearCells()
            {
                m_cells.Clear();
                this.Invalidate();
            }
            public Grid()
            {
                InitializeComponent();

                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);

                //need to move this into on paint and calcuate the dots based on offset.
                float[] dashValues = { 1, 8};
                m_gridPen = new Pen(Color.Gray, 1);
                //m_gridPen.DashPattern = dashValues;

                m_selectionPen = new Pen(new SolidBrush(Color.Red), 3);
            }

            private void Grid_Paint(object sender, PaintEventArgs e)
            {
                int width = Width;
                int height = Height;

                int rows = height / m_cellSize;
                int cols = width / m_cellSize;

                Graphics graphics = e.Graphics;
                Rectangle client = ClientRectangle;
                client.Inflate(200, 200);

                foreach (Point p in m_cells.Keys)
                {
                    if (client.Contains(new System.Drawing.Point(p.X, p.Y)))
                    {
                        int r, g, b;

                        r = m_cells[p].R;
                        g = m_cells[p].G;
                        b = m_cells[p].B;

                        if(m_invertScale)
                        {
                            r = 255 - r;
                            b = 255 - b;
                            g = 255 - g;
                        }
                       
                        Color cellColor = Color.FromArgb(r, g, b);
                        graphics.FillRectangle(new SolidBrush(cellColor), new Rectangle(m_offsetX + (p.X * m_cellSize), m_offsetY + (p.Y * m_cellSize), m_cellSize, m_cellSize));

                    }
                }

                if(m_showGrid)
                { 
                    for (int index = 0; index <= rows+1; index++)
                    {
                        int yPos = (index * m_cellSize) + (m_offsetY % m_cellSize);

                        graphics.DrawLine(m_gridPen, new System.Drawing.Point(0, yPos), new System.Drawing.Point(width, yPos));
                    }

                    for (int index = 0; index <= cols; index++)
                    {
                        int xPos = (index * m_cellSize) + (m_offsetX % m_cellSize);

                        graphics.DrawLine(m_gridPen, new System.Drawing.Point(xPos, 0), new System.Drawing.Point(xPos, width));
                    }
                }

                graphics.DrawRectangle(new Pen(Brushes.Red, 3), m_offsetX, m_offsetY, m_cellSize * bounds.Width, m_cellSize * bounds.Height);

                if (client.Contains(new System.Drawing.Point(m_offsetX, m_offsetY)))
                {
                    graphics.DrawString("(0, 0) (" + m_cellOffsetX + ", " + m_cellOffsetY + ")",
                            new Font("Arial", 20), Brushes.Red,
                            new Point(m_offsetX, m_offsetY));

                    graphics.DrawEllipse(Pens.Red, new Rectangle(new Point(m_offsetX - 4, m_offsetY - 4), new Size(8, 8)));
                }

                if (client.Contains(new System.Drawing.Point(m_offsetX + (bounds.Width * m_cellSize), m_offsetY + (bounds.Height * m_cellSize))))
                {
                    graphics.DrawString("(" + bounds.Width + ", " + bounds.Height + ")",
                            new Font("Arial", 20), Brushes.Red,
                            new Point(m_offsetX + (bounds.Width * m_cellSize), m_offsetY + (bounds.Height * m_cellSize)));

                    graphics.DrawEllipse(Pens.Red, new Rectangle(new Point((m_offsetX + (bounds.Width * m_cellSize)) - 4, (m_offsetY + (bounds.Height * m_cellSize)) - 4), new Size(8, 8)));
                }

                int selectedSizeX = selection.Width / m_cellSize;
                int selectedSizeY = selection.Height / m_cellSize;

                graphics.DrawString("(" + m_selectedX + "," + m_selectedY + ") (" + selectedSizeX + ", " + selectedSizeY + ")",
                        new Font("Arial", 20), Brushes.Red,
                        new Point(0, this.Height - 35));


                graphics.DrawRectangle(m_selectionPen, selection);
            }

            private void Grid_MouseUp(object sender, MouseEventArgs e)
            {
                if (m_dragging)
                {
                    m_dragging = false;
                    Invalidate();
                }
                if (m_selecting)
                {
                    m_selecting = false;

                    if (selection.X == m_selectionStartX && selection.Width % m_cellSize != 0)
                    {
                        var nextWholeCellSnapX = m_cellSize - (selection.Width % m_cellSize);
                        selection.Width += nextWholeCellSnapX;
                    }
                    else if (e.Location.X < m_selectionStartX)
                    {
                        var nextWholeCellSnapX = selection.X % m_cellSize;
                        nextWholeCellSnapX += (m_cellSize - GetPartialCellSizeX());
                        nextWholeCellSnapX %= m_cellSize;
                        selection.X -= nextWholeCellSnapX;
                        selection.Width += nextWholeCellSnapX;
                    }

                    if (selection.Y == m_selectionStartY && selection.Height % m_cellSize != 0)
                    {
                        var nextWholeCellSnapY = m_cellSize - (selection.Height % m_cellSize);
                        selection.Height += nextWholeCellSnapY;
                    }
                    else if (e.Location.Y < m_selectionStartY)
                    {
                        var nextWholeCellSnapY = selection.Y % m_cellSize;
                        nextWholeCellSnapY += (m_cellSize - GetPartialCellSizeY());
                        nextWholeCellSnapY %= m_cellSize;
                        selection.Y -= nextWholeCellSnapY;
                        selection.Height += nextWholeCellSnapY;
                    }
                   
                       

                    Invalidate();
                }
            }

            private void Grid_MouseMove(object sender, MouseEventArgs e)
            {
                Point p = e.Location;

                if (m_dragging)
                {

                    m_deltaX = p.X - m_lastMousePosition.X;
                    m_deltaY = p.Y - m_lastMousePosition.Y;

                    m_offsetX += m_deltaX;
                    m_offsetY += m_deltaY;

                    selection.X += m_deltaX;
                    selection.Y += m_deltaY;

                    // I changed this from ceiling to Floor.  It made more sense to me to have this represent the # of
                    // whole cells offset from the viewport and not to include a partial cell in this.  It shouldn't be too
                    // hard to flip this in the other direction, if you wanted tho.
                    m_cellOffsetX = m_offsetX / m_cellSize;
                    m_cellOffsetY = (int)System.Math.Floor((double)m_offsetY / (double)m_cellSize);

                    m_lastMousePosition = (e.Location);

                    Invalidate();
                }
                
                if (m_selecting && m_multiSelect)
                {
                    // TODO: Improved pointer tracking

                    m_selectDeltaX = p.X - m_lastSelectMousePosition.X;
                    m_selectDeltaY = p.Y - m_lastSelectMousePosition.Y;

                    m_selectionOffsetX += m_selectDeltaX;
                    m_selectionOffsetY += m_selectDeltaY;

                    selection.Height = System.Math.Abs(m_cellSize + m_selectionOffsetY + m_selectDeltaY);
                    selection.Width = System.Math.Abs(m_cellSize + m_selectionOffsetX + m_selectDeltaX);
                    selection.X = System.Math.Min(m_selectionStartX, m_selectionStartX + m_cellSize + m_selectionOffsetX + m_selectDeltaX);
                    selection.Y = System.Math.Min(m_selectionStartY, m_selectionStartY + m_cellSize + m_selectionOffsetY + m_selectDeltaY);

                    m_lastSelectMousePosition = e.Location;

                    Invalidate();
                }
                
                if(m_selecting && !m_multiSelect)
                {
                    m_lastMousePosition = (e.Location);
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

                    selection = new Rectangle(m_selectionStartX, m_selectionStartY, m_cellSize, m_cellSize);

                    if (SelectionMade != null)
                    {
                        SelectionMade(new Rectangle(m_selectedX, m_selectedY, 1, 1));
                    }

                    Invalidate();
                }
            }

            private void Grid_MouseDown(object sender, MouseEventArgs e)
            { 
                m_lastMousePosition = (e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    m_dragging = true;
                   
                }
                
                if (e.Button == MouseButtons.Left)
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

                    selection = new Rectangle(m_selectionStartX, m_selectionStartY, m_cellSize, m_cellSize);

                    if (SelectionMade != null)
                    {
                        SelectionMade(new Rectangle(m_selectedX, m_selectedY, 1, 1));
                    }

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

            private int GetCellX(System.Drawing.Point p)
            {
                int x = 0;

                m_mouseX = p.X;

                // We add a cell to the offset so that when the viewport and the grid are perfectly aligned the modulo
                // returns 0 instead of 16.  I substract this out from p.X to get the mouse position as it lies in the
                // entirely visible grid.  
                m_mouseOffsetX = p.X - GetPartialCellSizeX();

                // Just doing a simple division works for m_mouseOffsetX > 0. But when a partially visible cell is selected
                // m_mouseOffsetX will be negative and -10 / 16 = 0, not -1.
                int cellX = (int)System.Math.Floor((((double)m_mouseOffsetX) / ((double)m_cellSize)));

                x = m_cellOffsetX - cellX;

                return x * -1;
            }

            private int GetCellY(System.Drawing.Point p)
            {
                int y = 0;

                m_mouseY = p.Y;
                m_mouseOffsetY = p.Y - GetPartialCellSizeY();

                int cellY = (int)System.Math.Floor((((double)m_mouseOffsetY) / ((double)m_cellSize)));

                y = m_cellOffsetY - cellY;

                return y * -1;
            }

            public Rectangle GetViewPortRectangle()
            {
                return new System.Drawing.Rectangle(GetViewPortOrigin(), GetViewPortSize());
            }

            public System.Drawing.Point GetViewPortOrigin()
            {
                return new System.Drawing.Point(GetViewPortX(), GetViewPortY());
            }

            public int GetViewPortX()
            {
                return GetCellX(new System.Drawing.Point(0, 0));
            }
            public int GetViewPortY()
            {
                return GetCellY(new System.Drawing.Point(0, 0));
            }

            public System.Drawing.Size GetViewPortSize()
            {
                return new System.Drawing.Size(Width / m_cellSize, Height / m_cellSize);
            }
        }
    }
