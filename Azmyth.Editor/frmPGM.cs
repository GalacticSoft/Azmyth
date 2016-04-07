using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azmyth.Procedural;
using System.IO;

namespace Azmyth.Editor
{
    public partial class frmPGM : Form
    {
        private enum ScaleTypes
        {
            HeightMap,
            HeatMap
        }

        private  bool m_invertScale = false;
        private ScaleTypes m_scale = ScaleTypes.HeightMap;

        public frmPGM()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            grid1.CellSize = trackBar1.Value;
        }

        private void grid1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            grid1.ShowGrid = checkBox1.Checked;
        }

        private void openEV3PGMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = 174;
            int y = 128;
            string pgm = "";
            Stream f = null;
            int size = x * y;
            int[] cells = new int[size];
            openFileDialog1.Filter = "PGM | *.pgm";
            DialogResult res = openFileDialog1.ShowDialog(this);
            
            if(openFileDialog1.SafeFileName != "")
            {
                f = openFileDialog1.OpenFile();

                using (StreamReader reader = new StreamReader(f, Encoding.UTF8))
                {
                    pgm = reader.ReadToEnd();
                }

                grid1.ClearCells();

                pgm = pgm.Replace("\r", "");
                pgm = pgm.Replace("\n", "");

                string[] pgmCells = pgm.Split(',');
                StringBuilder strBuffer = new StringBuilder();

                int posX = 0;
                int posY = -1;

                grid1.SetBounds(x, y);

                for (int n = 0; n < size; n++)
                {
                    if (n % x == 0)
                    {
                        posX = 0;
                        posY++;
                    }

                    int color = int.Parse(pgmCells[n]);

                    switch (m_scale)
                    {
                        case ScaleTypes.HeightMap:
                            grid1.SetCell(posX, posY, Color.FromArgb(color, color, color));
                            break;
                        case ScaleTypes.HeatMap:
                            grid1.SetCell(posX, posY, Color.FromArgb(255 - color, 0, color));
                            break;
                    }

                   

                    posX++;

                   
                }

                grid1.Invalidate();

            }

            f.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            grid1.InvertScale = !grid1.InvertScale;
        }

        private void closeEV3PGMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid1.ClearCells();
        }

        private void grid1_SelectionMade(Rectangle obj)
        {
            grid1.SetCell(obj.X, obj.Y, Color.FromArgb(colorTool1.SelectedColor, colorTool1.SelectedColor, colorTool1.SelectedColor));
        }
    }
}
