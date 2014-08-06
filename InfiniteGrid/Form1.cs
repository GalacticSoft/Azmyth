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
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(-200, -200, 500, 500));
        Debug m_debug = new Debug();

        public Form1()
        {
            InitializeComponent();

            items.Insert(new Item(new Rectangle(0, 0, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(1, 1, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(-1, -1, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(-1, -21, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(10, 15, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(-1, -5, 1, 1)) { Value = false });
            items.Insert(new Item(new Rectangle(-3, -4, 1, 1)) { Value = false });

            grid1.QuadTree = items;
            quadView1.QuadTree = items;

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
    
        public Item(Rectangle rectangle)
        {
            m_rectangle = rectangle;
        }

        public Rectangle Rectangle
        {
	        get { return m_rectangle; }
            set { m_rectangle = value; }
        }
    }
}
