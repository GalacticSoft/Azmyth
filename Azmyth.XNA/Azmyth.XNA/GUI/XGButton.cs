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
    public delegate void XGClickedEvent(XGControl sender);

    public class XGButton : XGControl
    {
        XGClickedEvent ClickedHandler = null;

        public XGButton(Rectangle rect, string text, XGClickedEvent clickedHandler)
            : base(rect, true)
        {
            Text = text;
            ClickedHandler = clickedHandler;
            Alignment = GUIAlignment.HCenter | GUIAlignment.VCenter;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (hiliteTime > 0.0f)
                hiliteTime -= frameTime;

            if (XnaGUIManager.GetFocusControl() == this)
            {
                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (this.Contains(new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y)))
                    {
                        hiliteTime = 0.1f; // mouse down over button
                    }
                    else
                    {
                        hiliteTime = 0.0f; // moved off of button
                    }
                }
                else if (XnaGUIManager.prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (this.Contains(new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y)))
                    {
                        // Clicked
                        hiliteTime = 0.0f;
                        NotifyClicked();
                    }
                }

                if (KeyJustPressed(Keys.Enter, false, false, false) ||  KeyJustPressed(Keys.Space, false, false, false) || Azmyth.XNA.InputManager.PadPressed(PlayerIndex.One, Buttons.A))
                {
                    hiliteTime = 0.1f;
                    NotifyClicked();
                }
            }

            base.Update(gameTime);
        }

        void NotifyClicked()
        {
            XGClickedEvent handler = ClickedHandler;
            if (handler != null)
                handler(this);
        }

        float hiliteTime = 0.0f;

        public override void Draw(float frameTime)
        {
            Rectangle rect = ToScreen(Rectangle);

            Color color = ThisControlColor;
            if (hiliteTime > 0.0f)
                color = ControlDarkColor;

            DrawBorder(rect, 2, color, true);
            Rectangle fillRect = rect;
            fillRect.Inflate(-2, -2);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, fillRect, ControlDarkColor);

            Vector2 textPos = GetTextPosition();

            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, textPos, ThisForeColor);

            base.Draw(frameTime);
        }
    }
}
