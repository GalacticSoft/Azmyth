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
using Azmyth.XNA;

namespace XnaGUILib
{
    public class XGListBoxItem
    {
        public object Value;
        public object Tag;

        public XGListBoxItem(object value)
        {
            Value = value;
            Tag = null;
        }

        public XGListBoxItem(object value, object tag)
        {
            Value = value;
            Tag = tag;
        }
    }

    internal delegate void XGListChangedEvent(object sender);


    public class XGListBoxItemCollection
    {
        internal XGListChangedEvent listChangedHandler = null;

        private List<XGListBoxItem> Items = new List<XGListBoxItem>();

        public XGListBoxItem this[int index]
        {
            get 
            { 
                return Items[index]; 
            }
        }

        public XGListBoxItemCollection()
        {
        }

        public int Count { get { return Items.Count; } }

        private void NotifyListChanged()
        {
            XGListChangedEvent handler = listChangedHandler;
            if (handler != null)
                handler(this);
        }

        public void Add(XGListBoxItem item)
        {
            Items.Add(item);
            NotifyListChanged();
        }

        public void Add(string value)
        {
            Items.Add(new XGListBoxItem(value));
            NotifyListChanged();
        }

        public void Clear()
        {
            Items.Clear();
            NotifyListChanged();
        }

        public void Remove(XGListBoxItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                NotifyListChanged();
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Items.RemoveAt(index);
                NotifyListChanged();
            }
        }
    }

    public class XGListBox : XGControl
    {
        XGVScrollbar scrollBar;

        public readonly XGListBoxItemCollection Items = new XGListBoxItemCollection();
        private int _selectedIndex = -1;
        public int SelectedIndex 
        { 
            get { return _selectedIndex; }
            set
            {
                if (value < Items.Count)
                    _selectedIndex = value;
            }
        }
        public XGListBoxItem SelectedItem
        {
            get
            {
                if (SelectedIndex >= 0)
                    return Items[SelectedIndex];
                return null;
            }
        }
        public int TopItemIndex { get; protected set; }
        public int DisplayItemCount { get; protected set; }

        public XGListBox(Rectangle rect)
            : base(rect, true)
        {
            scrollBar = new XGVScrollbar(new Rectangle(Rectangle.Width - 10, 1, 10, Rectangle.Height - 2), this.Scrollbar_ValueChanged);
            scrollBar.Scale = 100.0f;
            Children.Add(scrollBar);
            SelectedIndex = -1;
            Items.listChangedHandler = this.Items_ListChangedEvent;
        }

        void Items_ListChangedEvent(object sender)
        {
            // Recalc the scrollbar scale
            if (SelectedIndex >= Items.Count)
                SelectedIndex = -1;
            DoLayout();
        }

        void DoLayout()
        {
            if (TopItemIndex >= Items.Count)
                TopItemIndex = Items.Count - 1;
            if (TopItemIndex < 0)
                TopItemIndex = 0;
            if (Items.Count > 0)
            {
                scrollBar.Scale = Items.Count - 1;
                scrollBar.Value = (float)TopItemIndex;// / (float)Items.Count; // keep scrollbar in place for current item
            }
            else
            {
                scrollBar.Scale = 1.0f;
                scrollBar.Value = 0.0f;
            }
            DisplayItemCount = Rectangle.Height / XnaGUIManager.spriteFont.LineSpacing + 1;
        }

        void Scrollbar_ValueChanged(XGControl sender)
        {
            XGVScrollbar scrollBar = sender as XGVScrollbar;
            if (scrollBar == null)
                return;

            float value = scrollBar.Value;
            TopItemIndex = (int)value;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check Mouse Input

            if (XnaGUIManager.GetFocusControl() == this)
            {
                Point mousePoint = new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y);

                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed)
                {
                    SelectItemAtPoint(mousePoint);
                }

                // Check for Keyboard input

                if (XnaGUIManager.keyState.IsKeyDown(Keys.Down) && XnaGUIManager.prevKeyState.IsKeyUp(Keys.Down))
                {
                    if (SelectedIndex + 1 < Items.Count)
                        SelectedIndex++;
                    if (SelectedIndex - TopItemIndex > MaxLinesDisplable)
                        TopItemIndex++;
                }
                if (XnaGUIManager.keyState.IsKeyDown(Keys.Up) && XnaGUIManager.prevKeyState.IsKeyUp(Keys.Up))
                {
                    if (SelectedIndex > 0)
                        SelectedIndex--;
                    if (SelectedIndex < TopItemIndex)
                        TopItemIndex = SelectedIndex;
                }
                
                //Check for Gamepad input

                if(InputManager.ThumbDownPressed(PlayerIndex.One, ThumbSticks.Right))
                {
                    if (SelectedIndex + 1 < Items.Count)
                        SelectedIndex++;
                    if (SelectedIndex - TopItemIndex > MaxLinesDisplable)
                        TopItemIndex++;
                }

                if(InputManager.ThumbUpPressed(PlayerIndex.One, ThumbSticks.Right))
                {
                    if (SelectedIndex > 0)
                        SelectedIndex--;
                    if (SelectedIndex < TopItemIndex)
                        TopItemIndex = SelectedIndex;
                }
            }

            base.Update(gameTime);
        }

        int MaxLinesDisplable
        {
            get
            {
                int count = (Rectangle.Height - 6) / XnaGUIManager.spriteFont.LineSpacing;
                return count;
            }
        }

        private void SelectItemAtPoint(Point point)
        {
            Rectangle rect = ToScreen(Rectangle);
            rect.Inflate(-4, -4);
            rect.Width -= 6;

            if (rect.Contains(point))
            {
                int index = TopItemIndex + (int )((point.Y - rect.Y) / XnaGUIManager.spriteFont.LineSpacing);
                if (index >= 0 && index < Items.Count)
                    SelectedIndex = index;
            }
        }

        public override void Draw(float frameTime)
        {
            try
            { 
                Rectangle rect = ToScreen(Rectangle);
                /*
                if (XnaGUIManager.HasFocus(this))
                    DrawBorder(rect, 1, ForeColor, true);
                else
                    DrawBorder(rect, 1, ControlColor, true);
                */
                DrawBorder(rect, 1, ThisControlColor, true);

                XnaGUIManager.spriteBatch.End();
                XnaGUIManager.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, new RasterizerState() { ScissorTestEnable = true });
                Rectangle oldScissor = XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle;
                Rectangle scissorRect = rect;
                scissorRect.Inflate(-4, -4);
                scissorRect.Width -= 6; // don't overwrite verticle scroll bar

                XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle = scissorRect;

                Vector2 pos = new Vector2(rect.Left + 4, rect.Y + 3);

                int lastItem = TopItemIndex + DisplayItemCount;
                if (lastItem > Items.Count)
                    lastItem = Items.Count;

                for (int i = TopItemIndex; i < lastItem; i++)
                {
                    if (i == SelectedIndex)
                    {
                        Rectangle itemRect = new Rectangle((int )pos.X - 1, (int )pos.Y, rect.Width - 3, (int )XnaGUIManager.spriteFont.LineSpacing - 1);
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, itemRect, ControlDarkColor);
                    }
                    XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Items[i].Value.ToString(), pos, ThisForeColor);
                    pos.Y += XnaGUIManager.spriteFont.LineSpacing;
                }

                XnaGUIManager.spriteBatch.End();
                XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle = oldScissor;
                XnaGUIManager.spriteBatch.Begin();
            }
            catch
            {

            }

            base.Draw(frameTime);
        }
    }
}
