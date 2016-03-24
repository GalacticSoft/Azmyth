using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Azmyth.Editor
{
        public partial class ColorTool : UserControl
        {
            bool m_selecting = false;
            Rectangle m_selected = Rectangle.Empty;
            int m_selectedColor = 0;

            public int SelectedColor
            {
                get
                {
                    return m_selectedColor;
                }
            }

            public ColorTool()
            {
                InitializeComponent();

                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);

                SelectColor(0);
               
            }

            private float ColorSize
            {
                get
                {
                    return (this.Width - this.Height) / 256f;
                }
            }
            private void Grid_Paint(object sender, PaintEventArgs e)
            {
                float colorSize = ColorSize;
                Graphics graphics = e.Graphics;
                
                for(int i = 0; i <= 255; i++)
                {
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(i, i, i)), i*colorSize, 0, colorSize, this.Height);
                }

                if(m_selected != null)
                {
                    graphics.DrawRectangle(new Pen(Brushes.Red, 2), m_selected.X, 1, m_selected.Width-1, m_selected.Height-4);
                }

                
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(m_selectedColor, m_selectedColor, m_selectedColor)), (int)(this.Width - this.Height), 0, (int)this.Height, (int)this.Height);
                graphics.DrawRectangle(new Pen(Brushes.Red, 3), this.Width - this.Height, 0, this.Height-3, this.Height-3);
            }
            private void Grid_MouseUp(object sender, MouseEventArgs e)
            {
                m_selecting = false;

            }

            private void Grid_MouseMove(object sender, MouseEventArgs e)
            {
                if (m_selecting)
                {
                    SelectColor(e.X);
                }
            }

            private void Grid_MouseDown(object sender, MouseEventArgs e)
            {

                SelectColor(e.X);

                m_selecting = true;
            }
          
            private void SelectColor(int x)
            {
                float colorSize = ColorSize;

                if (x <= (this.Width - this.Height))
                { 
                    m_selected = new Rectangle((int)((x) - (x % colorSize)), 0, (int)colorSize, this.Height);
                    m_selectedColor = (int)(((x) - (x % colorSize)) / colorSize);

                    if(m_selectedColor < 0)
                    {
                        m_selectedColor = 0;
                    }

                    if(m_selectedColor > 255)
                    {
                        m_selectedColor = 255;
                    }

                    Invalidate();
                }
            }

            private void ColorTool_Resize(object sender, EventArgs e)
            {
                m_selected = new Rectangle((int)(m_selectedColor * ColorSize), 0, (int)ColorSize, this.Height);
            }
        }
    }
