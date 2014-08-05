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
        QuadTree<Item> items = new QuadTree<Item>(new RectangleF(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));

        public Form1()
        {
            InitializeComponent();
        }
    }

    public class Item : IHasRect
    {
        private Rectangle m_rectangle;
        public bool Value = true;
    
        public Rectangle Rectangle
        {
	        get { return m_rectangle; }
            set { m_rectangle = value; }
        }
    }
}
