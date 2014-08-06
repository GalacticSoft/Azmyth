using System;
using System.Collections.Generic;
using System.Drawing;

namespace InfiniteGrid
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
            if (m_quadTree != null)
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
                        if (graphics.ClipBounds.Contains(inside))
                        {
                            graphics.DrawRectangle(p, inside);
                        }

                    }

                });
            }

        }
    }


    public static class Utility
    {
        static Random m_rand = new Random(DateTime.Now.Millisecond);
        
        public static Color RandomColor
        {
            get
            {
                return Color.FromArgb(
                    255,
                    m_rand.Next(255),
                    m_rand.Next(255),
                    m_rand.Next(255));

            }
        }
    }
}
