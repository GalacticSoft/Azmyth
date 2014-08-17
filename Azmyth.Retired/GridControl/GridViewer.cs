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
    public partial class GridViewer : UserControl
    {
        private VectorGrid _grid;

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
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Toggles Hot Track")]
        public bool HotTrack
        {
            get
            {
                return gridControl.HotTrack;
            }
            set
            {
                gridControl.HotTrack = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Appearance")]
        public bool ShowGrid
        {
            get
            {
                return gridControl.ShowGrid;
            }
            set
            {
                gridControl.ShowGrid = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Allows you to select empty cells")]
        public bool SelectEmpty
        {
            get
            {
                return gridControl.SelectEmpty;
            }
            set
            {
                gridControl.SelectEmpty = value;
            }
        }

        [Browsable(true)]
        [Category("Layout")]
        [DefaultValue("16, 16")]
        [Description("Sets the width and height of the cells within the grid.")]
        public Size CellSize 
        { 
            get
            {
                return new Size(gridControl.CellWidth, gridControl.CellHeight);   
            }
            set
            {
                gridControl.CellWidth = value.Width;
                gridControl.CellHeight = value.Height;
            }
        }

        [Browsable(false)]
        public VectorGrid SelectedGrid
        {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;

                gridControl.Grid = _grid;
            }
        }

        [Browsable(false)]
        public VectorCell SelectedCell
        {
            get
            {
                return gridControl.SelectedCell;
            }
            set
            {
                gridControl.SelectedCell = value;
            }
        }

        public GridViewer()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color GridColor 
        { 
            get
            {
                return gridControl.GridColor;
            }
            set
            {
                gridControl.GridColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color FullColor
        {
            get
            {
                return gridControl.FullColor;
            }
            set
            {
                gridControl.FullColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color HoverColor
        {
            get
            {
                return gridControl.HoverColor;
            }
            set
            {
                gridControl.HoverColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color BorderColor
        {
            get
            {
                return gridControl.BorderColor;
            }
            set
            {
                gridControl.BorderColor = value;
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor
        {
            get
            {
                return gridControl.SelectedColor;
            }
            set
            {
                gridControl.SelectedColor = value;
            }
        }

        public void SelectCell(int x, int y)
        {
            gridControl.SelectCell(x, y);
        }

        public void SelectCell(VectorCell cell)
        {
            gridControl.SelectCell(cell);
        }

        public void SelectCell(Vector vector)
        {
            gridControl.SelectCell(vector);
        }

        public void SetCellColor(Vector vector, Color color, bool draw = false)
        {
            gridControl.SetCellColor(vector, color, draw);   
        }

        public void SetCellImage(Vector vector, Image image)
        {
            gridControl.SetCellImage(vector, image);
        }

        public void ClearCellImage(Vector vector)
        {
            gridControl.ClearCellImage(vector);
        }

        public void ClearCellColor(Vector vector)
        {
            gridControl.ClearCellColor(vector);
        }

        public void ClearCellImages()
        {
            gridControl.ClearCellImages();
        }

        public void ClearCellColors()
        {
            gridControl.ClearCellColors();
        }

        public void ClearCells()
        {
            gridControl.ClearCells();
        }

        public VectorCell[] Cells
        {
            get
            {
                return gridControl.Cells;
            }
        }

        public void RepaintMap()
        {
            gridControl.Invalidate();
        }

        public bool IsEmpty(VectorCell cell)
        {
            return gridControl.IsEmpty(cell);
        }

        private void GridViewer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridControl_HoveredCellChanged(object sender, CellSelectedEventArgs e)
        {
            if (HoveredCellChanged != null)
            {
                HoveredCellChanged(sender, e);
            }
        }

        private void gridControl_CellClick(object sender, CellClickEventArgs e)
        {
            if (CellClick != null)
            {
                CellClick(sender, e);
            }
        }

        private void gridControl_SelectedCellChanged(object sender, CellSelectedEventArgs e)
        {
            if (SelectedCellChanged != null)
            {
                SelectedCellChanged(sender, e);
            }
        }

        private void gridControl_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (CellDoubleClick != null)
            {
                CellDoubleClick(sender, e);
            }
        }

        private void gridControl_CellRightClick(object sender, CellClickEventArgs e)
        {
            if (CellRightClick != null)
            {
                CellRightClick(sender, e);
            }
        }

        public void CreateGrid(int SizeX, int SizeY)
        {
            gridControl.CreateGrid(SizeX, SizeY);
            RepaintMap();
        }
    }
}
