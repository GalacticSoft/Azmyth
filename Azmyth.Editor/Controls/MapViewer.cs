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

        public MapViewer()
        {
            InitializeComponent();
        }

        public MapViewer(VectorID areaID)
        {
            //Area area = null;

            AreaID = areaID;

            //area = (Area)Assets.Assets.Store[areaID];

            InitializeComponent();
        }

        

        public void SetCellColor(Vector vector, Color color, bool draw = false)
        {

        }

        public void ClearCells()
        {
    
        }

        public void RepaintMap()
        {
       
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

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
