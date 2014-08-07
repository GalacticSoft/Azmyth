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
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(0, 0, int.MaxValue, int.MaxValue));

        public Form1()
        {
            InitializeComponent();

            for (int x = 0; x < 2000; x++)
                for (int y = 0; y <2000; y++ )
                    items.Insert(new Item(new Rectangle(x, y, 1, 1)) { Value = false, Color=Utility.RandomColor });

            grid1.QuadTree = items;
            quadView2.QuadTree = items;

            WatchForm.GetInstance().Show();
        }

        private void grid1_ViewportChanged(object sender, CellEventArgs e)
        {
            Watch.Set("viewport", e.Cells);
        }

        private void grid1_ViewportChanging(object sender, CellEventArgs e)
        {
            Watch.Set("viewport", e.Cells);
        }

        private void grid1_HoverChanged(object sender, CellEventArgs e)
        {
            Watch.Set("hover", e.Cells);
        }

        private void grid1_SelectionChanged(object sender, CellEventArgs e)
        {
            Watch.Set("selection", e.Cells);
        }

        private void grid1_DoubleClick(object sender, EventArgs e)
        {
            grid1.MoveToOrigin();
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
