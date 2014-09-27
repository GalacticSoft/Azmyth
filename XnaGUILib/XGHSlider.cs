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
    public class XGHSlider : XGControl
    {

        protected float _scale;

        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                if (_scale < 0.01f)
                    _scale = 0.01f;
                _max = _min + _scale;
            }
        }

        protected float _min = 0.0f;
        public float MinValue
        {
            get { return _min; }
            set
            {
                if (value < _max)
                {
                    _min = value;
                    Scale = _max - _min;
                }
            }
        }

        protected float _max = 1.0f;
        public float MaxValue
        {
            get { return _max; }
            set
            {
                if (value > _min)
                {
                    _max = value;
                    Scale = _max - _min;
                }
            }
        }

        protected float _value; // 0.0f to 1.0f

        public float Value
        {
            get { return _value * _scale + _min; }
            set
            {
                _value = (value - _min) / _scale;
            }
        }

        public void SetRange(float value, float min, float max)
        {
            MinValue = min;
            MaxValue = max;
            Value = value;
        }

        public XGHSlider(Rectangle rect, float value)
            : base(rect, true)
        {
            Value = value;
        }

        public XGHSlider(Rectangle rect, float value, float scale)
            : base(rect, true)
        {
            Scale = scale;
            Value = value;
        }

        float keyTimer = 0.0f;
        bool repeat = false;

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyTimer > 0.0f)
                keyTimer -= frameTime;

            float oldValue = 0.0f;
            Vector2 downPos = Vector2.Zero;
            bool mouseDown = false;
            Vector2 mousePos = new Vector2(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y);
            Point mousePoint = new Point((int)mousePos.X, (int)mousePos.Y);

            if (XnaGUIManager.GetFocusControl() == this)
            {
                if (XnaGUIManager.keyState.IsKeyDown(Keys.Right) || XnaGUIManager.keyState.IsKeyDown(Keys.Left))
                {
                    if (keyTimer <= 0.0f)
                    {
                        if (XnaGUIManager.keyState.IsKeyDown(Keys.Right))
                        {
                            _value += 0.01f;
                            if (_value > 1.0f)
                                _value = 1.0f;
                        }
                        else
                        {
                            _value -= 0.01f;
                            if (_value < 0.0f)
                                _value = 0.0f;
                        }

                        keyTimer = 0.5f;
                        if (repeat)
                            keyTimer = 0.1f;
                        repeat = true;
                    }
                }
                else
                {
                    keyTimer = 0.0f;
                    repeat = false;
                }

                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed && Contains(mousePoint))
                {
                    if (!mouseDown)
                    {
                        downPos = mousePos;
                        float pos = downPos.X - (ToScreen(Rectangle).X + 2);
                        if (pos < 0)
                            pos = 0;
                        if (pos > (Rectangle.Width - 4))
                            pos = Rectangle.Width - 4;
                        float v = pos / (Rectangle.Width - 4);
                        _value = v;
                        oldValue = _value;
                    }
                    mouseDown = true;
                }
                else
                {
                    mouseDown = false;
                }

                if (mouseDown)
                {
                    float diff = downPos.X - mousePos.X;
                    _value = _value + (diff / (Rectangle.Width - 4));
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(float frameTime)
        {
            base.Draw(frameTime);

            Rectangle rect = ToScreen(Rectangle);
            int v = rect.Top + ((rect.Bottom - rect.Top) / 2 - 1);
            rect = new Rectangle(rect.Left, v, rect.Width, 2);

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisControlColor);

            int pos = (int )(_value * (Rectangle.Width - 4)) + 2;
            
            rect = ToScreen(Rectangle);
            rect.X += pos - 2;
            rect.Y += (int)Padding.Y;
            rect.Height -= (int)Padding.Y * 2; ;
            rect.Width = 4;
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisControlColor);
        }
    }
}
