using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;


namespace XnaGUILib
{
    public class XGTabControl : XGControl
    {
        XGTabPage _currentPage = null;
        public XGTabPage CurrentTabPage 
        {
            get { return _currentPage; }
            set
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i] == value)
                    {
                        _currentPage = value;
                        Children[i].Enabled = true;
                        Children[i].Visible = true;
                    }
                    else
                    {
                        Children[i].Enabled = false;
                        Children[i].Visible = false;
                    }
                }
            }
        }

        public XGTabControl(Rectangle rect)
            : base(rect, false)
        {
        }

        internal Rectangle GetTabRect(XGTabPage tabPage)
        {
            if (tabPage == null)
                return Rectangle.Empty;

            Rectangle rect = ToScreen(Rectangle);
            rect.Height = XnaGUIManager.spriteFont.LineSpacing + 8;
            rect.Width = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                int tabWidth = (int )XnaGUIManager.spriteFont.MeasureString(Children[i].Text).X + 8;
                if (Children[i] == tabPage)
                {
                    rect.Width = tabWidth;
                    return rect;
                }
                rect.X += tabWidth;
            }
            return rect;
        }

        internal Rectangle GetPageRect(XGTabPage tabPage)
        {
            Rectangle pageRect = ToScreen(Rectangle);
            int tabHeight = XnaGUIManager.spriteFont.LineSpacing + 5;
            pageRect.Y += tabHeight;
            pageRect.Height -= tabHeight;
            return pageRect;
        }

        int lastPageCount = 0;

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (CurrentTabPage == null || Children.Count != lastPageCount)
            {
                lastPageCount = Children.Count;
                if (Children.Count > 0)
                    CurrentTabPage = Children[0] as XGTabPage;
            }
            base.Update(gameTime);
        }

        public override void Draw(float frameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Rectangle tabRect = GetTabRect(Children[i] as XGTabPage);
                
                Rectangle fillRect = Rectangle.Empty;

                fillRect = tabRect;
                fillRect.Inflate(-2, -2);
                if (CurrentTabPage == Children[i])
                    fillRect.Height += 1;
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, fillRect, BkgColor);

                int tabLineHeight = tabRect.Height - 2;
                if (Children[i] != CurrentTabPage)
                    tabLineHeight -= 2;

                Rectangle top = new Rectangle(tabRect.Left + 2, tabRect.Top, tabRect.Width - 4, 2);
                Rectangle left = new Rectangle(tabRect.Left, tabRect.Top + 1, 2, tabLineHeight);
                Rectangle right = new Rectangle(tabRect.Right - 2, tabRect.Top + 1, 2, tabLineHeight);

                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, top, ControlColor);
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, left, ControlColor);
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, right, ControlColor);

                Vector2 size = XnaGUIManager.spriteFont.MeasureString(Children[i].Text);
                Vector2 textPos = new Vector2(tabRect.Left + ((tabRect.Width - size.X) / 2.0f), tabRect.Top + ((tabRect.Height - size.Y) / 2.0f));

                XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Children[i].Text, textPos, Children[i].ThisForeColor);
            }
            base.Draw(frameTime);
        }

        internal override bool Activate(Point point)
        {
            Rectangle rect = ToScreen(Rectangle);
            int tabHeight = XnaGUIManager.spriteFont.LineSpacing + 8;
            rect.Height = tabHeight;

            if (rect.Contains(point))
            {
                // activate a tab;
                for (int i = 0; i < Children.Count; i++)
                {
                    Rectangle tabRect = GetTabRect(Children[i] as XGTabPage);
                    if (tabRect.Contains(point))
                    {
                        this.focusControl = Children[i];
                        CurrentTabPage = this.focusControl as XGTabPage;
                        return true;
                    }
                }
                return false;
            }

            return base.Activate(point);
        }
    }

    public class XGTabPage : XGControl
    {
        public XGTabControl TabControl { get { return Parent as XGTabControl; } }

        public XGTabPage(Rectangle rect, string text)
            : base(rect, true)
        {
            Text = text;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override Rectangle ToScreen(Rectangle rect)
        {
            rect.Y += TabControl.GetTabRect(this).Height; // drop child controls on pages down below Tabs
            if (Parent != null)
            {
                rect.Offset(Parent.Rectangle.Left, Parent.Rectangle.Top);
                rect = Parent.ToScreen(rect);
            }
            return rect;
        }

        public override void Draw(float frameTime)
        {
            Rectangle tabRect = TabControl.GetTabRect(this);
            Rectangle pageRect = TabControl.GetPageRect(this);

            DrawBorder(pageRect, 2, ControlColor, false);
            
            Rectangle fillRect = pageRect;
            fillRect.Inflate(-2, -2);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, fillRect, BkgColor);

            Rectangle leftOf = new Rectangle(pageRect.Left, pageRect.Top, tabRect.Left - pageRect.Left, 2);
            Rectangle rightOf = new Rectangle(tabRect.Right, pageRect.Top, pageRect.Right - tabRect.Right, 2);

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, leftOf, ControlColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rightOf, ControlColor);


            base.Draw(frameTime);
        }

        internal override bool Activate(Point point)
        {
            if (base.Activate(point))
            {
                TabControl.CurrentTabPage = this;
                return true;
            }
            return false;
        }
    }
}
