using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Azmyth;
using Azmyth.Math;
using Azmyth.Assets;
using System.Runtime.InteropServices;

namespace Azmyth.Editor
{

    public partial class MapViewer : UserControl
    {
        public VectorID AreaID;
        public TreeNode AreaNode;
        public TabPage AreaPage;

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
        public event CellSelectEvent CellHover;

        public MapViewer()
        {
            InitializeComponent();
        }

        public MapViewer(VectorID areaID)
        {
            Area area = null;

            AreaID = areaID;

            area = (Area)Assets.Assets.Store[areaID];

            InitializeComponent();

            mapArea.CreateGrid((int)area.GridX, (int)area.GridY);

            txtName.Text = area.Name;
            txtHeight.Text = area.GridY.ToString();
            txtWidth.Text = area.GridX.ToString();
            txtVector.Text = area.AssetID.Vector.ToString();
            txtID.Text = area.AssetID.ID.ToString();
        }

        private void mapArea_CellClick(object sender, CellClickEventArgs e)
        {
            if (CellClick != null)
            {
                CellClick(this, e);
            }
        }

        private void mapArea_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (CellDoubleClick != null)
            {
                CellDoubleClick(this, e);
            }
        }

        private void mapArea_CellRightClick(object sender, CellClickEventArgs e)
        {
            if (CellRightClick != null)
            {
                CellRightClick(this, e);
            }
        }

        private void mapArea_CellHover(object sender, CellSelectedEventArgs e)
        {
            if (CellHover != null)
            {
                CellHover.DynamicInvoke(new Object[2] { this, e });
            }
        }

        private void mapArea_SelectedCellChanged(object sender, CellSelectedEventArgs e)
        {
            if (SelectedCellChanged != null)
            {
                SelectedCellChanged(this, e);
            }
        }

        public void SetCellColor(Vector vector, Color color, bool draw = false)
        {
            mapArea.SetCellColor(vector, color, draw);
        }

        public void ClearCells()
        {
            mapArea.ClearCells();
        }

        public void RepaintMap()
        {
            mapArea.RepaintMap();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Azmyth.Assets.Assets.Store[AreaID].Name = txtName.Text;
            
            if (AreaNode != null)
            {
                AreaNode.Text = txtName.Text;
            }

            if (AreaPage != null)
            {
                AreaPage.Text = txtName.Text;
            }
        }
    }

    class FixedTabControl : TabControl
    {

        protected override void OnHandleCreated(EventArgs e)
        {
            SetWindowTheme(this.Handle, "", "");
            base.OnHandleCreated(e);
        }

        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);
    }
}
