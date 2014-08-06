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
        //Create Quad Tree for testing.
        //TODO: need to change to int.min and int.max but values don't currently work.
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(0, 0, int.MaxValue, int.MaxValue));
        Debug m_debug = new Debug();


        public Form1()
        {
            InitializeComponent();

            for (int x = 0; x < 500; x++)
                for (int y = 0; y < 500; y++ )
                    items.Insert(new Item(new Rectangle(x, y, 1, 1)) { Value = false, Color=Utility.RandomColor });

            grid1.QuadTree = items;
            quadView2.QuadTree = items;

            m_debug.Show();
        }

        private void grid1_ViewportChanged(object sender, CellEventArgs e)
        {
            m_debug["viewport"] = e.Cells;
        }

        private void grid1_ViewportChanging(object sender, CellEventArgs e)
        {
            m_debug["viewport"] = e.Cells;
        }

        private void grid1_HoverChanged(object sender, CellEventArgs e)
        {
            m_debug["hover"] = e.Cells;
        }

        private void grid1_SelectionChanged(object sender, CellEventArgs e)
        {
            m_debug["selection"] = e.Cells;
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
