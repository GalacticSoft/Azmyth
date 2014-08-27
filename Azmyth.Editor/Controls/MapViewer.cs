﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Azmyth;
using Azmyth.Assets;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Azmyth.Editor
{

    public partial class MapViewer : UserControl
    {
        public VectorID WorldID;
        public TreeNode AreaNode;
        public TabPage AreaPage;

        public MapViewer()
        {
            InitializeComponent();
        }

        public MapViewer(VectorID worldID)
        {
            WorldID = worldID;

            InitializeComponent();

            gridControl1.m_world = Assets.Assets.GetWorld(worldID);
            gridControl2.m_world = Assets.Assets.GetWorld(worldID);
        }

        public void SetCellColor(Vector vector, Color color, bool draw = false)
        {

        }

        public void ClearCells()
        {
    
        }

        public void RepaintMap()
        {
            gridControl1.Invalidate();
            gridControl2.Invalidate();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        public event EventHandler<Point> CellHover;
        private void gridControl2_CellHover(object sender, Point e)
        {
            if (CellHover != null)
            {
                CellHover(sender, e);
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
