using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Azmyth.Math;
using System.Windows;
using System.Windows.Forms;

namespace InfiniteGrid
{
    public class GridRenderer
    {
        int m_cellSize = 16;
        int m_cellOffsetX = 0;
        int m_cellOffsetY = 0;
        int m_offsetX = 0;
        int m_offsetY = 0;

        int m_originOffsetX = 0;

        Vector m_Origin = Vector.Origin;
 
        public void Render(Graphics graphics, int offsetX, int offsetY)
        {
            int width = (int)graphics.ClipBounds.Width;
            int height = (int)graphics.ClipBounds.Height;

            int rows = height / m_cellSize;
            int cols = width / m_cellSize;

            for(int index = 0; index <= rows; index++)
            {
                int yPos = ((index) * m_cellSize) + (Math.Abs(offsetY) % m_cellSize);

                graphics.DrawLine(Pens.Black, 
                    new Point(0, yPos), 
                    new Point(width, yPos));
            }

            for (int index = 0; index <= cols; index++)
            {
                int xPos = ((index) * m_cellSize) - (offsetX % m_cellSize);

                graphics.DrawLine(Pens.Black,
                    new Point(xPos, 0),
                    new Point(xPos, width));
            }

            m_originOffsetX = (offsetX);
            graphics.DrawString((offsetX).ToString() + " : " + m_originOffsetX.ToString(), new Font("Arial", 10), Brushes.Black,
                new Point(m_originOffsetX * -1, 0));

      
        }
    }



/*using QuadTreeLib;

namespace QuadTreeDemoApp
{
    /// <summary>
    /// Class draws a QuadTree
    /// </summary>
    class QuadTreeRenderer 
    {
        /// <summary>
        /// Create the renderer, give the QuadTree to render.
        /// </summary>
        /// <param name="quadTree"></param>
        public QuadTreeRenderer(QuadTree<Item> quadTree)
        {
            m_quadTree = quadTree;
        }
        
        QuadTree<Item> m_quadTree;

        /// <summary>
        /// Hashtable contains a colour for every node in the quad tree so that they are
        /// rendered with a consistant colour.
        /// </summary>
        Dictionary<QuadTreeNode<Item>, Color> m_dictionary = new Dictionary<QuadTreeNode<Item>, Color>();
        
        /// <summary>
        /// Get the colour for a QuadTreeNode from the hash table or else create a new colour
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Color GetColor(QuadTreeNode<Item> node)
        {
            if (m_dictionary.ContainsKey(node))
                return m_dictionary[node];

            Color color = Utility.RandomColor;
            m_dictionary.Add(node, color);
            return color;
        }

        /// <summary>
        /// Render the QuadTree into the given Graphics context
        /// </summary>
        /// <param name="graphics"></param>
        internal void Render(Graphics graphics)
        {
            m_quadTree.ForEach(delegate(QuadTreeNode<Item> node)
            {

                // draw the contents of this quad
                if (node.Contents != null)
                {
                    foreach (Item item in node.Contents)
                    {
                        using (Brush b = new SolidBrush(item.Color))
                            graphics.FillEllipse(b, Rectangle.Round(item.Rectangle));
                    }
                }

                // draw this quad

                // Draw the border
                Color color = GetColor(node);
                graphics.DrawRectangle(Pens.Black, Rectangle.Round(node.Bounds));
            
                // draw the inside of the border in a distinct colour
                using (Pen p = new Pen(color))
                {
                    Rectangle inside = Rectangle.Round(node.Bounds);
                    inside.Inflate(-1, -1);
                    graphics.DrawRectangle(p, inside);
                }

            });

        }
    }
}*/

}
