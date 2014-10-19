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
    public delegate void XGValueChangedEvent(XGControl sender);

    public class XGControlCollection : List<XGControl>
    {
        internal XGControl Parent;

        internal XGControlCollection(XGControl owner)
        {
            Parent = owner;
        }

        public new void Add(XGControl control)
        {
            control.Parent = Parent;
            base.Add(control);

            if (Parent != null)
                Parent.NotifyChildCollectionChanged();
        }

        public new void Insert(int index, XGControl control)
        {
            control.Parent = Parent;
            base.Insert(index, control);

            if (Parent != null)
                Parent.NotifyChildCollectionChanged();
        }

        public new void Remove(XGControl control)
        {
            base.Remove(control);

            if (Parent != null)
                Parent.NotifyChildCollectionChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);

            if (Parent != null)
                Parent.NotifyChildCollectionChanged();
        }
    }

    
    [Flags]
    public enum GUIAlignment : short
    {
        HCenter = 0x03,
        Left = 0x01,
        Right = 0x02,
        VCenter = 0x0C,
        Top = 0x04,
        Bottom = 0x08,
    }

    public abstract class XGControl
    {
        public XGControl Parent { get; set; }
        public XGControlCollection Children;
        public Rectangle Rectangle { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public object Tag { get; set; }
        protected XGControl focusControl = null;
        public bool CanFocus { get; set; }
        public string Text { get; set; }
        private static int cnt = 0;
        public GUIAlignment Alignment { get; set; }
        public Vector2 Padding = new Vector2(2, 2);
        static Color _controlColor; // (enabled, no focus)
        static Color _controlDarkColor; // (disabled)
        static Color _controlHiColor; // (enabled, focus)
        static Color _foreColor; // (enabled, no focus)
        static Color _foreHiColor; // (enabled, focus)
        static Color _foreDarkColor; // (disabled)

        public static Color ControlColor 
        {
            get { return _controlColor; }
            set 
            { 
                _controlColor = value;
                _controlDarkColor = Color.Lerp(Color.Black, value, 0.4f);
                _controlHiColor = Color.Lerp(value, Color.White, 0.4f);
            } 
        }
        public Color ThisControlColor 
        {
            get
            {
                if (Enabled)
                {
                    if (XnaGUIManager.HasFocus(this))
                        return _controlHiColor;
                    return _controlColor;
                }
                return _controlDarkColor;
            }
        }
        public static Color ControlDarkColor { get { return _controlDarkColor; } }
        public static Color ForeColor 
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                _foreDarkColor = Color.Lerp(Color.Black, value, 0.707f);
                _foreHiColor = Color.Lerp(value, Color.White, 0.707f);
            }
        }
        public Color ThisForeColor
        {
            get
            {
                if (Enabled)
                {
                    if (XnaGUIManager.HasFocus(this))
                        return _foreHiColor;
                    return _foreColor;
                }
                return _foreDarkColor;
            }
        }
        //public static Color FocusColor { get { return _foreHiColor; } }
        public static Color BkgColor { get; set; }

        public XGControl(Rectangle rect, bool canFocus)
        {
            Children = new XGControlCollection(this);
            //Parent = parent;
            Rectangle = rect;
            CanFocus = canFocus;
            Visible = true;
            Enabled = true;
            Alignment = GUIAlignment.Top | GUIAlignment.Left;

            cnt++;
            Text = "XGControl-" + cnt.ToString();
            ControlColor = new Color(0.6f, 1.0f, 0.6f, 0.5f);
            ForeColor = new Color(0.6f, 1.0f, 0.6f, 0.707f);
            BkgColor = new Color(0.0f, 0.3f, 0.0f, 0.707f);
        }

        public virtual void NotifyChildCollectionChanged()
        {
        }


        protected bool KeyJustPressed(Keys key, bool shift, bool control, bool alt)
        {
            if (XnaGUIManager.keyState.IsKeyDown(key) && XnaGUIManager.prevKeyState.IsKeyUp(key))
            {
                if (shift && !(XnaGUIManager.keyState.IsKeyDown(Keys.LeftShift) || XnaGUIManager.keyState.IsKeyDown(Keys.RightShift)))
                    return false;
                if (control && !(XnaGUIManager.keyState.IsKeyDown(Keys.LeftControl) || XnaGUIManager.keyState.IsKeyDown(Keys.RightControl)))
                    return false;
                if (alt && !(XnaGUIManager.keyState.IsKeyDown(Keys.LeftAlt) || XnaGUIManager.keyState.IsKeyDown(Keys.RightAlt)))
                    return false;
                return true;
            }
            return false;
        }

        protected Vector2 GetTextPosition()
        {
            Rectangle rect = ToScreen(Rectangle);

            return GetTextPosition(rect, Text, Alignment, Padding);
        }

        public static Vector2 GetTextPosition(Rectangle rect, string Text, GUIAlignment Alignment, Vector2 Padding)
        {
            Vector2 size = XnaGUIManager.spriteFont.MeasureString(Text);
            size.Y = XnaGUIManager.spriteFont.MeasureString("A").Y;

            float x = rect.Left + Padding.X;
            if ((Alignment & GUIAlignment.HCenter) == GUIAlignment.Right)
                x = rect.Right - size.X - Padding.X;
            if ((Alignment & GUIAlignment.HCenter) == GUIAlignment.HCenter)
                x = rect.Left + ((rect.Width - size.X) / 2);

            float y = rect.Top + Padding.Y;
            if ((Alignment & GUIAlignment.VCenter) == GUIAlignment.Bottom)
                y = rect.Bottom - size.Y - Padding.Y;
            if ((Alignment & GUIAlignment.VCenter) == GUIAlignment.VCenter)
                y = rect.Top + ((rect.Height - size.Y) / 2);

            return new Vector2(x, y);
        }

        public XGControl GetFocusControl()
        {
            if (focusControl != null)
                return focusControl.GetFocusControl();
            return this;
        }

        public bool IsParent(XGControl control)
        {
            if (Parent == null)
                return false;

            if (Parent == control)
                return true;
            return Parent.IsParent(control);
        }


        internal virtual bool Activate(Point point)
        {
            if (this.Children.Count == 0 && CanFocus)
            {
                focusControl = null;
                GetFocus();
                return true;
            }

            if (focusControl != null && focusControl.Contains(point) && focusControl.CanFocus)
            {
                // we already have focus, just active the current control
                return focusControl.Activate(point);
            }

            if (Children.Count > 0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    XGControl control = Children[i];
                    if (control.Enabled && control.Contains(point))
                    {
                        focusControl = control;
                        if (control.Activate(point))
                            return true;
                        // If this child control can't get focus, perhaps the next control right can
                        // (ie, maybe this child control is a label, so give the control right of it focus)
                        if (i + 1 < Children.Count && Children[i + 1].CanFocus)
                        {
                            if (focusControl.Rectangle.Left < Children[i + 1].Rectangle.Left)
                            {
                                focusControl = Children[i + 1];
                                if (focusControl.Activate(point))
                                    return true;
                            }
                        }
                        focusControl = null; // not this child control
                    }
                }
            }
            if (CanFocus)
            {
                focusControl = null;
                GetFocus(); // we have the focus now.
                return true;
            }
            return false;
        }


        internal virtual void Activate()
        {
            if (focusControl != null)
            {
                focusControl.GetFocus();
                return;
            }
            else if (Children.Count > 0)
            {
                foreach (XGControl child in Children)
                {
                    if (child.Enabled && child.CanFocus)
                    {
                        focusControl = child;
                        focusControl.GetFocus();
                    }
                }
                return;
            }
            if (CanFocus)
                GetFocus();
        }

        /// <summary>
        /// Called on the control that currently has focus
        /// </summary>
        internal virtual bool ActivateNext()
        {
            if (Parent != null)
            {
                return Parent.ActivateNext(this);
            }
            return false;
        }

        /// <summary>
        /// Called to activate the control following the current control
        /// </summary>
        /// <param name="current"></param>
        /// <returns>true if focus given to next child, else false</returns>
        internal virtual bool ActivateNext(XGControl current)
        {
            int index = Children.IndexOf(current);
            if (index >= 0 && (index + 1) < Children.Count)
            {
                for (int i = index + 1; i < Children.Count; i++)
                {
                    if (Children[i].Enabled && Children[i].CanFocus)
                    {
                        focusControl = Children[i];
                        focusControl.ActivateFirst();
                        return true;
                    }
                }
            }
            if (Parent != null)
                return Parent.ActivateNext(this);

            return false;

        }

        internal virtual bool ActivatePrevious()
        {
            if (Parent != null)
            {
                return Parent.ActivatePrevious(this);
            }
            return false;
        }

        internal virtual bool ActivatePrevious(XGControl current)
        {
            int index = Children.IndexOf(current);
            if (index >= 0 && (index - 1) >= 0)
            {
                for (int i = (index - 1); i >= 0; i--)
                {
                    if (Children[i].Enabled && Children[i].CanFocus)
                    {
                        focusControl = Children[i];
                        focusControl.ActivateLast();
                        return true;
                    }
                }
            }
            if (Parent != null)
                return Parent.ActivatePrevious(this);
            return false;
        }

        /// <summary>
        /// Activates first child that CanFocus or self if CanFocus
        /// </summary>
        /// <returns></returns>
        internal bool ActivateFirst()
        {
            focusControl = null;
            if (Children.Count > 0)
            {
                foreach(XGControl child in Children)
                {
                    if (child.Enabled)
                    {
                        if (child.ActivateFirst())
                        {
                            focusControl = child;
                            focusControl.GetFocus();
                            return true;
                        }
                    }
                }
            }
            if (this.CanFocus)
            {
                this.GetFocus();
                return true;
            }
            return false;
        }

        
        internal bool ActivateLast()
        {
            focusControl = null;
            if (Children.Count > 0)
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    if (Children[i].Enabled)
                    {
                        if (Children[i].ActivateLast())
                        {
                            focusControl = Children[i];
                            focusControl.GetFocus();
                            return true;
                        }
                    }
                }
            }
            if (this.CanFocus)
            {
                this.GetFocus();
                return true;
            }
            return false;
        }


        protected virtual void GetFocus()
        {
        }

        public bool Contains(Point point)
        {
            Rectangle rect = ToScreen(Rectangle);
            return rect.Contains(point);
        }

        public virtual void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (XGControl control in Children)
            {
                if (control.Enabled)
                    control.Update(gameTime);
            }
        }

        internal virtual bool HandleInput()
        {
            if (Children.Count > 0)
            {
                if (focusControl != null)
                {
                    return focusControl.HandleInput();
                }
            }
            return false;
        }

        public virtual void Draw(float frameTime)
        {
            foreach (XGControl control in Children)
            {
                if (control.Visible)
                    control.Draw(frameTime);
            }                
        }

        public virtual Rectangle ToScreen(Rectangle rect)
        {
            if (Parent != null)
            {
                rect.Offset(Parent.Rectangle.Left, Parent.Rectangle.Top);
                rect = Parent.ToScreen(rect);
            }
            return rect;
        }

        protected float GetDepth()
        {
            return _GetDepth(1.0f);
        }

        private float _GetDepth(float depth)
        {
            if (Parent != null)
            {
                depth -= 0.01f;
                depth = Parent._GetDepth(depth);
            }
            return depth;
        }

        public static void DrawBorder(Rectangle rect, int width, Color color, bool drawTop)
        {
            Rectangle top = new Rectangle(rect.Left, rect.Top, rect.Width, width);
            Rectangle left = new Rectangle(rect.Left, rect.Top + width, width, rect.Height - (width * 2));
            Rectangle right = new Rectangle(rect.Right - width, rect.Top + width, width, (rect.Height - (width * 2)));
            Rectangle bottom = new Rectangle(rect.Left, rect.Bottom - width, rect.Width, width);

            if (drawTop)
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, top, color);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, left, color);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, right, color);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, bottom, color);

        }
    }
}
