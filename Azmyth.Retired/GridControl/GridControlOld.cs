using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
using Azmyth.Math;
using System.Threading.Tasks;

namespace Azmyth.Editor
{
    public delegate void CellClickEvent(object sender, CellClickEventArgs e);
    public delegate void CellSelectEvent(object sender, CellSelectedEventArgs e);
   
    public partial class GridControlOld : Control
    {
        #region Variables

        private int _cellWidth;
        private int _cellHeight;

        private bool _hotTrack;
        private bool _selectEmpty;

        private VectorGrid _grid;
        private VectorCell _selectedCell;
        private VectorCell _hoveredCell;

        private Dictionary<Vector, Image> _cellImages;
        private Dictionary<Vector, Color> _cellColors;

        private Bitmap buf;

        private event Action<Bitmap, Graphics> completePaint;

        #endregion

        #region Events

        public event CellClickEvent CellClick;
        public event CellClickEvent CellRightClick;
        public event CellClickEvent CellDoubleClick;

        public event CellSelectEvent HoveredCellChanged;
        public event CellSelectEvent SelectedCellChanged;

        #endregion

        #region Methods

        [Browsable(true)]
        [Category("Appearance")]
        public Color GridColor { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public Color FullColor { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public Color HoverColor { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public Color BorderColor { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public bool ShowGrid { get; set; }

        [Browsable(true)]
        [DefaultValue(16)]
        [Category("Layout")]
        public int CellWidth
        {
            get
            {
                return _cellWidth - 3;
            }
            set
            {
                _cellWidth = value + 3;

                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(16)]
        [Category("Layout")]
        public int CellHeight
        {
            get
            {
                return _cellHeight - 3;
            }
            set
            {
                _cellHeight = value + 3;

                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool HotTrack
        {
            get 
            { 
                return _hotTrack; 
            }
            set 
            { 
                _hotTrack = value; 
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool SelectEmpty
        {
            get 
            { 
                return _selectEmpty; 
            }
            set 
            {
                _selectEmpty = value; 
            }
        }

        [Browsable(false)]
        public VectorGrid Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;

                Invalidate();
            }
        }

        [Browsable(false)]
        public int GridWidth
        {
            get 
            { 
                return (CellWidth * MaxX) + 1; 
            }
        }

        [Browsable(false)]
        public int GridHeight
        {
            get 
            { 
                return (CellHeight * MaxY) + 1; 
            }
        }

        [Browsable(false)]
        public Vector MaxVector
        {
            get 
            { 
                return new Vector(_grid.MaxX, _grid.MaxY, 1); 
            }
        }

        [Browsable(false)]
        public int MaxX
        {
            get
            {
                return _grid.MaxX;
            }
        }

        [Browsable(false)]
        public int MaxY
        {
            get
            {
                return _grid.MaxY;
            }
        }

        public VectorCell SelectedCell
        {
            get 
            { 
                return _selectedCell; 
            }
            set 
            { 
                SelectCell(value); 
            }
        }

        #endregion

        public GridControlOld()
        {
            Grid = new VectorGrid(1, 1, 1);

            _selectedCell = (VectorCell)_grid[0, 0];

            _cellWidth = 19;
            _cellHeight = 19;

            _hotTrack = true;
            _selectEmpty = true;

            //GridColor = Color.Gray;
            //FullColor = Color.Black;
            //BorderColor = Color.Black;
            //HoverColor = Color.LightSteelBlue;
            //SelectedColor = Color.Indigo;
            ShowGrid = true;

            _cellImages = new Dictionary<Vector, Image>();
            _cellColors = new Dictionary<Vector, Color>();

            this.completePaint += new Action<Bitmap, Graphics>(GridControl_completePaint);
            InitializeComponent();

            this.DoubleBuffered = true;
            
           // SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);         
        }

        void GridControl_completePaint(Bitmap bmp, Graphics g)
        {
        }

        public void CreateGrid(int SizeX, int SizeY)
        {
            Grid = new VectorGrid(SizeX, SizeY, 1);
        }

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            this.Width = (MaxX * _cellWidth) + 1;
            this.Height = (MaxY * _cellHeight) + 1;
            
            base.OnPaint(e);
            
            DrawFullCells(e.Graphics, e);

            if (ShowGrid)
            {
                DrawGrid(e.Graphics);
            }

            SelectCell(_selectedCell);

            DrawBorder(e.Graphics);

            // now we draw the image into the screen
            //ManagedBackBuffer.Render(e.Graphics);
        }

        #endregion

        #region Public Functions

        public void SetCellColor(Vector vector, Color color, bool drawCell = false)
        {
            //if (_grid.GetCell(vector.X, vector.Y) != null)
            {
                if (_cellColors.ContainsKey(vector))
                {
                    _cellColors[vector] = color;
                }
                else
                {
                    _cellColors.Add(vector, color);
                }

                if (drawCell)
                {
                    DrawCell(vector);
                }
            }
        }

        public void SetCellImage(Vector vector, Image image)
        {
            if (_grid.GetCell(vector.X, vector.Y) != null)
            {
                if (_cellImages.ContainsKey(vector))
                {
                    _cellImages[vector] = image;
                }
                else
                {
                    _cellImages.Add(vector, image);
                }

                DrawCell(vector);
            }
        }

        public void ClearCellImage(Vector vector)
        {
            if (_cellImages.ContainsKey(vector))
            {
                _cellImages.Remove(vector);

                DrawCell(vector);
            }
        }

        public void ClearCellColor(Vector vector)
        {
            if (_cellColors.ContainsKey(vector))
            {
                _cellColors.Remove(vector);

                DrawCell(vector);
            }
        }

        public void ClearCellImages()
        {
            _cellImages.Clear();

            Invalidate();
        }

        public void ClearCellColors()
        {
            _cellColors.Clear();

            Invalidate();
        }

        public void ClearCells()
        {
            _cellColors = new Dictionary<Vector,Color>();
            _cellImages = new Dictionary<Vector,Image>();

            //Invalidate();
        }

        public void SelectCell(Vector vector)
        {
            SelectCell(vector.X, vector.Y);
        }

        public void SelectCell(VectorCell cell)
        {
            SelectCell(cell.X, cell.Y);
        }

        public void SelectCell(int x, int y)
        {
            if((x >= 0 && y >= 0) && (x < MaxX && y < MaxY))
            {
                VectorCell oldCell = _selectedCell;

                _selectedCell = (VectorCell)_grid[x, y];

                DrawCell(oldCell);

                DrawCell(_selectedCell);

                if (SelectedCellChanged != null)
                {
                    CellSelectedEventArgs e = new CellSelectedEventArgs();

                    e.Vector = _selectedCell.CellVector;

                    SelectedCellChanged(this, e);
                }
            }
        }

        public VectorCell[] Cells
        {
            get
            {
                return _grid.Cells;
            }
        }

        #endregion

        #region Private Functions

        private void DrawGrid(Graphics graphics)
        {
           // Thread t = new Thread(() =>
           // {
                //buf = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                //using(Graphics g = Graphics.FromImage(buf))
               // {
                    for (int x = 0; x < _grid.MaxX; x++)
                    {
                        graphics.DrawLine(new Pen(GridColor),
                            new Point((int)(x * _cellWidth) + _cellWidth, 0),
                            new Point((int)(x * _cellWidth) + _cellWidth, (MaxY * _cellHeight) + _cellHeight));
                    }

                    for (int y = 0; y < _grid.MaxY; y++)
                    {
                        graphics.DrawLine(new Pen(GridColor),
                            new Point(0, (y * _cellHeight) + _cellHeight),
                            new Point((MaxX * _cellWidth) + _cellWidth, (y * _cellHeight) + _cellHeight));
                    }

                    
               // }
            //});

            //t.Start();

        }



        private void DrawFullCells(Graphics g, PaintEventArgs e)
        {
            var keys = from key in _cellColors.Keys 
                       where e.ClipRectangle.IntersectsWith(new Rectangle((key.X * _cellWidth), (key.Y * _cellHeight), _cellWidth, _cellWidth)) 
                       select key;
            try
            {
                foreach (Vector colorKey in keys)
                {
                    DrawCell(colorKey, g);
                }
            }
            catch
            {

            }
        }

        #endregion

        #region Drawing Functions

        public void DrawCellChrome(Color color, VectorCell cell, int offset)
        {
            DrawCellChrome(color, cell.X, cell.Y, offset);
        }

        public void DrawCellChrome(Color color, Vector vector, int offset)
        {
            DrawCellChrome(color, vector.X, vector.Y, offset);
        }

        public void DrawCellChrome(Color color, int x, int y, int offset)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.DrawRectangle(new Pen(color), new Rectangle(
                    (x * _cellWidth) + offset,
                    (y * _cellHeight) + offset,
                    _cellWidth - (offset + offset),
                    _cellHeight - (offset + offset)));
            }
        }
        
        public void FillCell(Vector vector, Color color)
        {
            FillCell(vector.X, vector.Y, color);
        }

        public void FillCell(VectorCell cell, Color color)
        {
            FillCell(cell.X, cell.Y, color);
        }

        public void FillCell(int x, int y, Color color)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.FillRectangle(new SolidBrush(color), new Rectangle(
                    (x * _cellWidth),
                    (y * _cellHeight),
                    _cellWidth,
                    _cellWidth));
            }
        }

        public void FillCell(Vector vector, Image image)
        {
            FillCell(vector.X, vector.Y, image);
        }

        public void FillCell(VectorCell cell, Image image)
        {
            FillCell(cell.X, cell.Y, image);
        }

        public void FillCell(int x, int y, Image image)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.DrawImage(image,  new Rectangle(
                    (x * _cellWidth) + 2, (y * _cellHeight) + 2, 
                    _cellWidth - 4, _cellWidth - 4));
            }
        }

        public void DrawCell(int x, int y)
        {
            DrawCell(_grid[x, y]);
        }

        public void DrawCell(Vector vector)
        {
            DrawCell(_grid[vector.X, vector.Y]);
        }

        public void DrawCell(VectorCell cell)
        {
            Vector vector = cell.CellVector;

            //need to draw only the border of the cells instead of the entire grid.
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {

                if (_cellImages.ContainsKey(vector))
                {
                    FillCell(vector, _cellImages[vector], g);
                }
                else if (_cellColors.ContainsKey(vector))
                {
                    FillCell(vector, _cellColors[vector], g);
                }
                else
                {
                    FillCell(vector, BackColor, g);
                }

                if (cell == (VectorCell)_selectedCell)
                {
                    DrawCellChrome(SelectedColor, vector, 1, g);
                }
                else if (cell == (VectorCell)_hoveredCell)
                {
                    DrawCellChrome(HoverColor, vector, 1, g);   
                }

                if (ShowGrid)
                {
                    DrawGrid(g);
                }

                DrawBorder(g);
            }
        }

        private void DrawCellBorder(VectorCell cell, VectorCell adjacentCell)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                if (cell.X > adjacentCell.X)
                {
                    DrawLeftBorder(g, cell, adjacentCell);
                }
                else if (cell.Y > adjacentCell.Y)
                {
                    DrawTopBorder(g, cell, adjacentCell);
                }
                else if (cell.X < adjacentCell.X)
                {
                    DrawRightBorder(g, cell, adjacentCell);
                }
                else if (cell.Y < adjacentCell.Y)
                {
                    DrawBottomBorder(g, cell, adjacentCell);
                }
            }
        }

        private void DrawLeftBorder(Graphics g, VectorCell cell, VectorCell adjacentCell)
        {
            if ((adjacentCell.X >= 0 && adjacentCell.X < MaxX) && (adjacentCell.Y >= 0 && adjacentCell.Y < MaxY))
            {
                Color borderColor = IsEmpty(adjacentCell) ? GridColor : FullColor;

                g.DrawLine(new Pen(borderColor),
                   new Point(cell.X * _cellWidth, cell.Y * _cellHeight),
                   new Point(cell.X * _cellWidth, (cell.Y * _cellHeight) + _cellHeight));
            }  
        }

        private void DrawRightBorder(Graphics g, VectorCell cell, VectorCell adjacentCell)
        {
            if ((adjacentCell.X >= 0 && adjacentCell.X < MaxX) && (adjacentCell.Y >= 0 && adjacentCell.Y < MaxY))
            {
                Color borderColor = IsEmpty(adjacentCell) ? GridColor : FullColor;

                g.DrawLine(new Pen(borderColor),
                   new Point((cell.X * _cellWidth) + _cellWidth, cell.Y * _cellHeight),
                   new Point((cell.X * _cellWidth) + _cellWidth, (cell.Y * _cellHeight) + _cellHeight));
            }
        }

        private void DrawTopBorder(Graphics g, VectorCell cell, VectorCell adjacentCell)
        {
            if ((adjacentCell.X >= 0 && adjacentCell.X < MaxX) && (adjacentCell.Y >= 0 && adjacentCell.Y < MaxY))
            {
                Color borderColor = IsEmpty(adjacentCell) ? GridColor : FullColor;

                    g.DrawLine(new Pen(borderColor), 
                        new Point(cell.X * _cellWidth, cell.Y * _cellHeight), 
                        new Point((cell.X * _cellWidth) + _cellWidth, cell.Y * _cellHeight)); 
            }
        }

        private void DrawBottomBorder(Graphics g, VectorCell cell, VectorCell adjacentCell)
        {
            if ((adjacentCell.X >= 0 && adjacentCell.X < MaxX) && (adjacentCell.Y >= 0 && adjacentCell.Y < MaxY))
            {
                Color borderColor = IsEmpty(adjacentCell) ? GridColor : FullColor;

                g.DrawLine(new Pen(borderColor),
                    new Point(cell.X * _cellWidth, (cell.Y * _cellHeight) + _cellHeight),
                    new Point((cell.X * _cellWidth) + _cellWidth, (cell.Y * _cellHeight)+ _cellHeight));
            }
        }
        private void DrawBorder()
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                g.DrawRectangle(new Pen(BorderColor), 
                    new Rectangle(0, 0, (MaxX * _cellWidth), (MaxY * _cellHeight)));
            }
        }

        #endregion
        
        #region Internal Functions

        internal int GetCellX()
        {
            int x;
            int MousePosX = this.PointToClient(MousePosition).X;

            for (x = 0; x < MaxX; x++)
            {
                if (MousePosX < (x * _cellWidth) + _cellWidth && MousePosX >= x * _cellWidth)
                {
                    break;
                }
            }

            return x;
        }

        internal int GetCellY()
        {
            int y;
            int MousePosY = this.PointToClient(MousePosition).Y;

            for (y = 0; y < MaxY; y++)
            {
                if (MousePosY < (y * _cellHeight) + _cellHeight && MousePosY >= y * _cellHeight)
                {
                    break;
                }
            }

            return y;
        }

        internal int GetCellZ()
        {
            return _grid.ZIndex;
        }

        #endregion

        #region Event Handlers

        private CellClickEventArgs CreateClickEventArgs(MouseEventArgs e)
        {
            CellClickEventArgs args = new CellClickEventArgs(e);

            int x = GetCellX();
            int y = GetCellY();
            int z = GetCellZ();

            args.CellX = x;
            args.CellY = y;
            args.CellZ = z;

            args.Cell = (VectorCell)_grid[x, y];

            return args;
        }

        private void GridControl_MouseClick(object sender, MouseEventArgs e)
        {
            CellClickEventArgs args = CreateClickEventArgs(e);

            if(!_selectEmpty ? !IsEmpty(args.Cell) : true)
            {
                SelectCell(args.Cell);

                if (e.Clicks > 1)
                {
                    OnCellDoubleClick(args);
                }
                else
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Right:
                            OnCellRightClick(args);
                            break;
                        case MouseButtons.Left:
                            OnCellClick(args);
                            break;
                    }
                }
            }
        }

        private void GridControl_MouseMove(object sender, MouseEventArgs e)
        {
            VectorCell oldCell = _hoveredCell;

            HoverCell();

            if (_hoveredCell != VectorCell.Empty)
            {
                DrawHoveredCell();
            }

            DrawCell(oldCell);
        }

        private void DrawHoveredCell()
        {
            if (_hotTrack)
            {
                if (!_selectEmpty ? !IsEmpty(_hoveredCell) : true)
                {
                    DrawCell(_hoveredCell);

                    OnCellHover();
                }
                else
                {
                    _hoveredCell = VectorCell.Empty;
                }
            }
            else
            {
                _hoveredCell = VectorCell.Empty;
            }
        }

        private void HoverCell()
        {
            int x = GetCellX();
            int y = GetCellY();

            if (_grid.GetCell(x, y) != VectorCell.Empty)
            {
                _hoveredCell = (VectorCell)_grid[x, y];
            }
            else
            {
                _hoveredCell = VectorCell.Empty;
            }
        }

        private void GridControl_MouseLeave(object sender, EventArgs e)
        {
            VectorCell oldCell = _hoveredCell;

            _hoveredCell = VectorCell.Empty;

            DrawCell(oldCell);
        }

        public bool IsEmpty(VectorCell cell)
        {
            bool empty = false;
            Vector key = cell.CellVector;

            if (!_cellColors.ContainsKey(key) && !_cellImages.ContainsKey(key))
            {
                empty = true;
            }

            return empty;
        }
        #endregion

        #region Event Callers

        private void OnCellClick(CellClickEventArgs e)
        {
            if (CellClick != null)
            {
                CellClick(this, e);
            }
        }

        private void OnCellRightClick(CellClickEventArgs e)
        {
            if (CellRightClick != null)
            {
                CellRightClick(this, e);
            }
        }

        private void OnCellDoubleClick(CellClickEventArgs e)
        {
            if (CellDoubleClick != null)
            {
                CellDoubleClick(this, e);
            }
        }

        private void OnCellHover()
        {
            CellSelectedEventArgs args = new CellSelectedEventArgs();

            if (_hoveredCell != VectorCell.Empty)
            {
                args.Vector = _hoveredCell.CellVector;

                if (HoveredCellChanged != null)
                {
                    HoveredCellChanged(this, args);
                }
            }
        }
        #endregion

        public void DrawCell(Vector vector, Graphics g)
        {
            VectorCell cell = _grid[vector.X, vector.Y];

            if (_cellImages.ContainsKey(vector))
            {
                FillCell(vector, _cellImages[vector], g);
            }
            else if (_cellColors.ContainsKey(vector))
            {
                FillCell(vector, _cellColors[vector], g);
            }
            else
            {
                FillCell(vector, BackColor, g);
            }

            if (cell == _selectedCell)
            {
                DrawCellChrome(SelectedColor, vector, 1, g);
            }
            else if (cell == _hoveredCell)
            {
                DrawCellChrome(HoverColor, vector, 1, g);
            }

            //DrawCellBorder(cell, g);
        }

        public void FillCell(Vector vector, Image image, Graphics g)
        {
            FillCell(vector.X, vector.Y, image, g);
        }

        public void FillCell(int x, int y, Image image, Graphics g)
        {
            g.DrawImage(image, new Rectangle(
                (x * _cellWidth) + 2, 
                (y * _cellHeight) + 2,
                _cellWidth - 4, 
                _cellWidth - 4));
        }

        public void FillCell(Vector vector, Color color, Graphics g)
        {
            FillCell(vector.X, vector.Y, color, g);
        }

        public void FillCell(int x, int y, Color color, Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(
                (x * _cellWidth),
                (y * _cellHeight),
                _cellWidth,
                _cellWidth));
        }

        public void DrawCellChrome(Color color, Vector vector, int offset, Graphics g)
        {
            DrawCellChrome(color, vector.X, vector.Y, offset, g);
        }

        public void DrawCellChrome(Color color, int x, int y, int offset, Graphics g)
        {
            g.DrawRectangle(new Pen(color), new Rectangle(
                (x * _cellWidth) + offset,
                (y * _cellHeight) + offset,
                _cellWidth - (offset + offset),
                _cellHeight - (offset + offset)));
        }

        private void DrawBorder(Graphics g)
        {
            g.DrawRectangle(new Pen(BorderColor),
                new Rectangle(0, 0, (MaxX * _cellWidth), (MaxY * _cellHeight)));
        }

        private void DrawCellBorder(VectorCell cell, Graphics g)
        {
            if (!IsEmpty(cell))
            {
                DrawCellChrome(FullColor, cell.CellVector, 0, g);
            }
            else
            {
                VectorCell[] adjacentCells = new VectorCell[] 
                {
                    new VectorCell(cell.X - 1, cell.Y, cell.Z), //Left
                    new VectorCell(cell.X + 1, cell.Y, cell.Z), //Right
                    new VectorCell(cell.X, cell.Y - 1, cell.Z), //Above
                    new VectorCell(cell.X, cell.Y + 1, cell.Z), //Below
                };

                foreach (VectorCell adjacentCell in adjacentCells)
                {
                    DrawCellBorder(cell, adjacentCell, g);
                }
            }
        }

        private void DrawCellBorder(VectorCell cell, VectorCell adjacentCell, Graphics g)
        {
            if (cell.X > adjacentCell.X)
            {
                DrawLeftBorder(g, cell, adjacentCell);
            }
            else if (cell.Y > adjacentCell.Y)
            {
                DrawTopBorder(g, cell, adjacentCell);
            }
            else if (cell.X < adjacentCell.X)
            {
                DrawRightBorder(g, cell, adjacentCell);
            }
            else if (cell.Y < adjacentCell.Y)
            {
                DrawBottomBorder(g, cell, adjacentCell);
            }
        }

    }
}
