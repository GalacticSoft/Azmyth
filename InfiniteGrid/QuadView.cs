using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace InfiniteGrid
{
    public class QuadView : UserControl
    {
        private QuadTreeRenderer renderer;

        private QuadTree<Item> m_quadTree;

        public QuadTree<Item> QuadTree
        {
            get { return m_quadTree; }
            set 
            { 
                m_quadTree = value;
                renderer = new QuadTreeRenderer(value);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(renderer != null)
            {
                renderer.Render(e.Graphics);
            }

            base.OnPaint(e);
        }
    }
}
