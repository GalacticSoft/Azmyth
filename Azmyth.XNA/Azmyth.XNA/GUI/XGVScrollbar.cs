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
    public class XGVScrollbar : XGPanel
    {
        XGValueChangedEvent valueChangedEvent = null;

        private float _scale;
        public float Scale 
        {
            get { return _scale; }
            set
            {
                float temp = _value * _scale;
                _scale = value;
                if (_scale < 0.01f)
                    _scale = 0.01f;
                Value = temp; // reset value based on new scale
            }
        }
        private float _value = 0.0f; // ranges from 0.0f to 1.0f
        public float Value 
        {
            get { return _value * _scale; }
            set
            {
                _value = value / _scale;
            }
        }

        public XGVScrollbar(Rectangle rect, XGValueChangedEvent valueChangedHandler)
            : base(rect, false)
        {
            valueChangedEvent = valueChangedHandler;
            _scale = 1.0f;
            _value = 0.0f;
            BorderThickness = 1;
        }

        float lastMouseY = 0;
        bool mouseCaptured = false;

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (XnaGUIManager.GetFocusControl() == this)
            if (XnaGUIManager.HasFocus(Parent)) // Scrollbar does not get focus (parent can)
            {
                Point mousePoint = new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y);

                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Released)
                    mouseCaptured = false;

                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (!mouseCaptured)
                    {
                        if (thumbRect.Contains(mousePoint))
                            mouseCaptured = true; // first time we clicked on it
                        lastMouseY = mousePoint.Y;
                    }
                }

                if (mouseCaptured)
                {
                    Rectangle rect = ToScreen(Rectangle);
                    if (mousePoint.Y < rect.Y + 2)
                    {
                        mousePoint.Y = rect.Y + 2;
                        Mouse.SetPosition(mousePoint.X, mousePoint.Y);
                    }
                    if (mousePoint.Y > rect.Bottom - 2)
                    {
                        mousePoint.Y = rect.Bottom - 2;
                        Mouse.SetPosition(mousePoint.X, mousePoint.Y);
                    }

                    float diff = mousePoint.Y - lastMouseY;
                    if (diff != 0.0f)
                        MoveThumb(diff);

                    lastMouseY = mousePoint.Y;
                }
                else if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed && XnaGUIManager.prevMouseState.LeftButton == ButtonState.Released)
                {
                    // maybe they clicked above or below the thumb?
                    CalcThumbRect();
                    Rectangle rect = ToScreen(Rectangle);
                    rect.Inflate(-2, -2);
                    if (rect.Contains(mousePoint))
                    {
                        int step = (int)(thumbTravel / _scale);
                        if (step < 1) step = 1;
                        int where = mousePoint.Y - thumbRect.Top;
                        if (where < 0)
                            MoveThumb(-step);
                        else
                            MoveThumb(step);
                    }
                }
            }

            base.Update(gameTime);
        }

        private void MoveThumb(float amount)
        {
            CalcThumbRect();
            float thumb = _value * thumbTravel;
            thumb += amount;
            if (thumb > thumbTravel)
                thumb = thumbTravel;
            if (thumb < 0.0f)
                thumb = 0.0f;

            _value = thumb / thumbTravel;

            if (valueChangedEvent != null)
                valueChangedEvent(this);
        }

        Rectangle thumbRect = new Rectangle();
        float thumbTravel = 0.0f;
        float thumbTop = 0.0f;

        private void CalcThumbRect()
        {
            Rectangle rect = ToScreen(Rectangle);
            rect.Inflate(-2, -2);
            thumbTop = rect.Top;
            thumbTravel = rect.Height - rect.Width; // scroll thumb is width x width in size
            thumbTop = rect.Top + (int)(_value * thumbTravel);
            thumbRect = new Rectangle(rect.Left, (int )thumbTop, rect.Width, rect.Width);
        }

        public override void Draw(float frameTime)
        {
            base.Draw(frameTime);
            /*
            Rectangle rect = ToScreen(Rectangle);
            DrawBorder(rect, 1, ControlColor, true);
            */

            CalcThumbRect();
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, thumbRect, ThisControlColor);
        }
    }
}
