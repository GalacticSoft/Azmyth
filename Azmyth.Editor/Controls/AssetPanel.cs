using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Azmyth.Editor
{
    public partial class AssetPanel : UserControl
    {
        public AssetPanel()
        {
            InitializeComponent();
        }

        private void AssetPanel_MouseEnter(object sender, EventArgs e)
        {
            //btnHide.Visible = true;
        }

        private void AssetPanel_MouseLeave(object sender, EventArgs e)
        {
            //btnHide.Visible = false;
        }

        private void tvwWorld_MouseEnter(object sender, EventArgs e)
        {
            //btnHide.Visible = true;
        }

        private void tvwWorld_MouseLeave(object sender, EventArgs e)
        {
            //btnHide.Visible = false;
        }

        private void splitContainer1_Load(object sender, EventArgs e)
        {
        }
    }
}
