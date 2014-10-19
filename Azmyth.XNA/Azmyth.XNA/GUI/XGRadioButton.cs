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
    public class XGRadioGroup : XGControl
    {
        XGValueChangedEvent valueChangedEvent = null;

        public XGRadioGroup(Rectangle rect, string text)
            : base(rect, true)
        {
            Text = text;
        }

        public XGRadioGroup(Rectangle rect, string text, XGValueChangedEvent checkChangedEventHandler)
            : base(rect, true)
        {
            Text = text;
            valueChangedEvent = checkChangedEventHandler;
        }

        internal void NotifyChecked(XGRadioButton checkedRadio)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                XGRadioButton button = Children[i] as XGRadioButton;
                if (button != null && button != checkedRadio)
                {
                    button.Checked = false;
                }
            }

            XGValueChangedEvent handler = valueChangedEvent;
            if (handler != null)
                handler(this);
        }

        public override void NotifyChildCollectionChanged()
        {
            foreach (XGControl child in Children)
            {
                if (child.GetType() == typeof(XGRadioButton))
                    child.CanFocus = child.CanFocus; // Radio Buttons don't get focus directly
            }

            base.NotifyChildCollectionChanged();
        }


        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    XGRadioButton button = Children[i] as XGRadioButton;
                    if (button != null && button.Checked)
                    {
                        return i;
                    }
                }
                return -1;
            }
            set
            {
                if (value < 0 || value >= Children.Count)
                    throw new IndexOutOfRangeException("SelectedIndex Out of Range in XGRadioGroup");

                XGRadioButton button = Children[value] as XGRadioButton;
                if (button != null)
                    button.Checked = true;
            }
        }

        public XGRadioButton SelectedItem
        {
            get
            {
                int index = SelectedIndex;
                if (index >= 0)
                    return Children[index] as XGRadioButton;
                return null;
            }
            set
            {
                if (value != null)
                    value.Checked = true;
            }
        }

        internal override bool Activate(Point point)
        {
            if (base.Activate(point))
                return true;
            focusControl = SelectedItem;
            return true;
        }

        protected override void GetFocus()
        {
            focusControl = SelectedItem;
            base.GetFocus();
        }

        public override void Draw(float frameTime)
        {

            Rectangle rect = ToScreen(Rectangle);
            Vector2 pos = new Vector2(rect.X + 6, rect.Y);
            Vector2 size = XnaGUIManager.spriteFont.MeasureString(Text);

            rect.Y += (int )size.Y / 2 - 2;
            DrawBorder(rect, 1, ThisControlColor, false);
            rect.Height = 1;
            rect.Width = 4;

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisControlColor);
            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, pos, ThisForeColor);

            pos.X += size.X + 2;
            rect.X = (int )pos.X;
            rect.Width = Rectangle.Width - (int)size.X - 8;
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisControlColor);
            
            base.Draw(frameTime);
        }
    }

    public class XGRadioButton : XGControl
    {
        private bool _checked = false;
        public bool Checked 
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (_checked)
                {
                    XGRadioGroup group = Parent as XGRadioGroup;
                    if (group != null)
                        group.NotifyChecked(this);
                }
            }
        }

        public XGRadioButton(Rectangle rect, string text)
            : base(rect, true)
        {
            Text = text;
            Alignment = GUIAlignment.Left | GUIAlignment.VCenter;
        }

        public XGRadioButton(Rectangle rect, string text, bool isChecked)
            : base(rect, true)
        {
            Text = text;
            Alignment = GUIAlignment.Left | GUIAlignment.VCenter;
            Checked = isChecked;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (XnaGUIManager.GetFocusControl() == this)
            {
                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed &&
                    XnaGUIManager.prevMouseState.LeftButton == ButtonState.Released)
                {
                    Vector2 textPos = GetTextPosition();
                    Rectangle rect = ToScreen(Rectangle);
                    rect.Y += 1;
                    rect.Height -= 3;
                    rect.Width = rect.Height + 2;
                    rect.Width += (int )XnaGUIManager.spriteFont.MeasureString(Text).X + 1;

                    if (rect.Contains(new Point((int )XnaGUIManager.mouseState.X, (int )XnaGUIManager.mouseState.Y)))
                        Checked = true;
                }

                if (base.KeyJustPressed(Keys.Space, false, false, false))
                {
                    Checked = !Checked;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(float frameTime)
        {
            Vector2 textPos = GetTextPosition();
            //Vector2 checkPos = textPos;

            //Rectangle screenRect = ToScreen(Rectangle);

            Rectangle rect = ToScreen(Rectangle);
            rect.Y += 1;
            rect.Height -= 3;
            rect.Width = rect.Height;
            Rectangle top = new Rectangle(rect.Left, rect.Top, rect.Width, 2);
            Rectangle left = new Rectangle(rect.Left, rect.Top + 2, 2, rect.Height - 4);
            Rectangle right = new Rectangle(rect.Right - 2, rect.Top + 2, 2, rect.Height - 4);
            Rectangle bottom = new Rectangle(rect.Left, rect.Bottom - 2, rect.Width, 2);

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, top, ThisForeColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, left, ThisForeColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, right, ThisForeColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, bottom, ThisForeColor);

            textPos.X += rect.Width + 2;
            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, textPos, ThisForeColor);

            if (Checked)
            {
                rect.Inflate(-4, -4);
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisForeColor);
            }

            base.Draw(frameTime);
        }
    }
}
