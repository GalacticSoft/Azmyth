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
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(-200, -200, 500, 500));

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
