using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Azmyth.Assets;
using System.Diagnostics;

namespace Azmyth.Editor
{
    public partial class SnowLineScroll : UserControl
    {
        public SnowLineScroll()
        {
            InitializeComponent();
        }

        private int m_value;

        private WorldAdapter m_world;

        public WorldAdapter World 
        {
            get
            {
                return m_world;
            }
            set
            {
                m_world = value;

                World world = Assets.Assets.GetWorld(m_world.WorldID);
                
                trackBar1.Minimum = 0;
                trackBar1.Maximum = 100;
            }
        }

        public int Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                trackBar1.Value = value;
            }
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            this.m_value = this.trackBar1.Value;
            World.SnowLine = m_value;
        }
    }
}
