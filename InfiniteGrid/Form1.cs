using Stoatly.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfiniteGrid
{
    public partial class Form1 : Form
    {
        //TODO: need to change to int.min and int.max but values don't currently work.
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(-5000, -5000, int.MaxValue, int.MaxValue));

        public Form1()
        {
            InitializeComponent();

            //for (int x = 0; x < 200; x++)
            // //   for (int y = 0; y < 200; y++ )
            //        items.Insert(new Item(new Rectangle(x, y, 1, 1)) { Value = false, Color=Utility.RandomColor });

            gridControl1.m_quadTree = items;
            gridControl2.m_quadTree = items;

            numericUpDown1_ValueChanged(this, null);
            numericUpDown2_ValueChanged(this, null);
            numericUpDown3_ValueChanged(this, null);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            double amplitude = 1.0f;
            if (double.TryParse(numericUpDown1.Text, out amplitude))
            {
                gridControl1.Amplitude = amplitude;
                gridControl2.Amplitude = amplitude;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            double frequency = 1;
            if (double.TryParse(numericUpDown2.Text, out frequency))
            {
                gridControl1.Frequency = frequency;
                gridControl2.Frequency = frequency;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int seed = 1;
            if (int.TryParse(numericUpDown3.Text, out seed))
            {
                gridControl1.Seed = seed;
                gridControl2.Seed = seed;
            }
        }
    }

    public class Item : IHasRect
    {
        public Color Color;
        private Rectangle m_rectangle;
        public bool Value = true;
    
        public Item(Rectangle rectanlge)
        {
            m_rectangle = rectanlge;
        }

        public Rectangle Rectangle
        {
	        get { return m_rectangle; }
            set { m_rectangle = value; }
        }
    }
}
