using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Azmyth.Math;

namespace Azmyth.Editor
{
    public partial class MapGrid : UserControl
    {
        private int _cellWidth;
        private int _cellHeight;

        private int _zIndex;
        private VectorMap _map;


        private float _zoomLevel;
        ToolStripSlider _zoomSlider;

        [Browsable(true)]
        [Category("Cell Actions")]
        public event CellClickEvent CellClick;

        [Browsable(true)]
        [Category("Cell Actions")]
        public event CellClickEvent CellRightClick;

        [Browsable(true)]
        [Category("Cell Actions")]
        public event CellClickEvent CellDoubleClick;

        [Browsable(true)]
        [Category("Cell Actions")]
        [Description("Triggered when the selected cell changes.")]
        public event CellSelectEvent SelectedCellChanged;

        [Browsable(true)]
        [Category("Cell Actions")]
        [Description("Triggered when the hovered cell changes.")]
        public event CellSelectEvent HoveredCellChanged;

        [Browsable(true)]
        [Category("Behavior")]
        public float MaxZoom { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        public float MinZoom { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public int CellWidth
        {
            get
            {
                return _cellWidth;
            }
            set
            {
                _cellWidth = value;

                gridView.CellSize = new Size((int)(_cellWidth * _zoomLevel), (int)(_cellHeight * _zoomLevel));
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public bool ShowGrid
        {
            get
            {
                return gridView.ShowGrid; ;
            }
            set
            {
                gridView.ShowGrid = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public int CellHeight
        {
            get
            {
                return _cellHeight;
            }
            set
            {
                _cellHeight = value;

                gridView.CellSize = new Size((int)(_cellWidth * _zoomLevel), (int)(_cellHeight * _zoomLevel));
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool HotTrack
        {
            get 
            { 
                return gridView.HotTrack; 
            }
            set 
            { 
                gridView.HotTrack = value; 
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public bool SelectEmpty
        {
            get
            {
                return gridView.SelectEmpty;
            }
            set
            {
                gridView.SelectEmpty = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color GridColor 
        { 
            get
            {
                return gridView.GridColor;
            }
            set
            {
                gridView.GridColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color FullColor
        {
            get
            {
                return gridView.FullColor;
            }
            set
            {
                gridView.FullColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color HoverColor
        {
            get
            {
                return gridView.HoverColor;
            }
            set
            {
                gridView.HoverColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color BorderColor
        {
            get
            {
                return gridView.BorderColor;
            }
            set
            {
                gridView.BorderColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor
        {
            get
            {
                return gridView.SelectedColor;
            }
            set
            {
                gridView.SelectedColor = value;
            }
        }

        public MapGrid()
        {
            _zIndex = 0;
            _cellWidth = 16;
            _cellHeight = 16;
            _zoomLevel = 0.0f;

            InitializeComponent();

            CreateZoomToolBar();

            this.DoubleBuffered = true;
        }

        private void CreateZoomToolBar()
        {
            int i = toolStrip.Items.Add(new ToolStripSlider());

            _zoomSlider = toolStrip.Items[i] as ToolStripSlider;
            
            _zoomSlider.Alignment = ToolStripItemAlignment.Right;

            _zoomSlider.ValueChanged += new EventHandler(slider_ValueChanged);

            toolStrip.Items.Remove(btnZoomOut);
            toolStrip.Items.Add(btnZoomOut);

            toolStrip.Items.Remove(toolStripSeparator2);
            toolStrip.Items.Add(toolStripSeparator2);

            this.toolStrip.PerformLayout();
        }

        void slider_ValueChanged(object sender, EventArgs e)
        {
            _zoomLevel = (float)_zoomSlider.Value / 100f;

            Size cellSize = gridView.CellSize;

            gridView.CellSize = new Size((int)(_cellWidth * _zoomLevel), (int)(_cellHeight * _zoomLevel));
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_zoomLevel < 2.00)
            {
                _zoomSlider.Value += (int)(0.10f * 100.00f);
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_zoomLevel > 0.50)
            {
                _zoomSlider.Value -= (int)(0.10f * 100.00f);
            }
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            gridView.SelectCell(gridView.SelectedCell.X, gridView.SelectedCell.Y - 1);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            gridView.SelectCell(gridView.SelectedCell.X - 1, gridView.SelectedCell.Y);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            gridView.SelectCell(gridView.SelectedCell.X + 1, gridView.SelectedCell.Y);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            gridView.SelectCell(gridView.SelectedCell.X, gridView.SelectedCell.Y + 1);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (_zIndex < _map.MaxZ - 1)
            {
                _zIndex++;

                VectorCell cell = gridView.SelectedCell;

                gridView.SelectedGrid = _map[_zIndex];

                gridView.SelectCell(new Vector(cell.X, cell.Y, _zIndex));
                
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (_zIndex > 0)
            {
                _zIndex--;

                VectorCell cell = gridView.SelectedCell;

                gridView.SelectedGrid = _map[_zIndex];

                gridView.SelectCell(new Vector(cell.X, cell.Y, _zIndex));
            }
        }

        public void SetCellColor(Vector vector, Color color, bool draw = false)
        {
            gridView.SetCellColor(vector, color, draw);
        }

        public void SetCellImage(Vector vector, Image image)
        {
            gridView.SetCellImage(vector, image);
        }

        public void ClearCellImage(Vector vector)
        {
            gridView.ClearCellImage(vector);
        }

        public void ClearCellColor(Vector vector)
        {
            gridView.ClearCellColor(vector);
        }

        public void ClearCell(Vector vector)
        {
            gridView.ClearCellColor(vector);
            gridView.ClearCellImage(vector);
        }

        public void ClearCellImages()
        {
            gridView.ClearCellImages();
        }

        public void ClearCellColors()
        {
            gridView.ClearCellColors();
        }

        public void ClearCells()
        {
            gridView.ClearCells();
        }

        public VectorCell[] Cells
        {
            get
            {
                return gridView.Cells;
            }
        }

        public void RepaintMap()
        {
            gridView.RepaintMap();
        }

        public bool IsEmpty(VectorCell cell)
        {
            return gridView.IsEmpty(cell);
        }

        private void MapGrid_Paint(object sender, PaintEventArgs e)
        {
            //gridView.Invalidate();
        }

        private void gridView_CellClick(object sender, CellClickEventArgs e)
        {
            if (CellClick != null)
            {
                CellClick(sender, e);
            }
        }

        private void gridView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (CellDoubleClick != null)
            {
                CellDoubleClick(sender, e);
            }
        }

        private void gridView_CellRightClick(object sender, CellClickEventArgs e)
        {
            if (CellRightClick != null)
            {
                CellRightClick(sender, e);
            }
        }

        private void gridView_HoveredCellChanged(object sender, CellSelectedEventArgs e)
        {
            if (HoveredCellChanged != null)
            {
                HoveredCellChanged.BeginInvoke(this, e, null, null);
            }
        }

        private void gridView_SelectedCellChanged(object sender, CellSelectedEventArgs e)
        {
            if (SelectedCellChanged != null)
            {
                SelectedCellChanged(sender, e);
            }
        }

        public void CreateGrid(int sizeX, int sizeY)
        {
            gridView.CreateGrid(sizeX, sizeY);
        }
    }
}
