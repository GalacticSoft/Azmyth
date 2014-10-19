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
    public class XGCheckBox : XGControl
    {
        public bool Checked { get; set; }

        public XGCheckBox(Rectangle rect, string text)
            : base(rect, true)
        {
            Text = text;
            Alignment = GUIAlignment.Left | GUIAlignment.VCenter;
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (XnaGUIManager.GetFocusControl() == this)
            {
                if (XnaGUIManager.mouseState.LeftButton == ButtonState.Pressed &&
                    XnaGUIManager.prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePoint = new Point(XnaGUIManager.mouseState.X, XnaGUIManager.mouseState.Y);
                    Rectangle rect = ToScreen(Rectangle);
                    rect.Y += 1;
                    rect.Height -= 3;
                    rect.Width = rect.Height;
                    if (rect.Contains(mousePoint))
                        Checked = !Checked;
                }

                if (base.KeyJustPressed(Keys.Space, false, false, false) || Azmyth.XNA.InputManager.PadPressed(PlayerIndex.One, Buttons.A))
                    Checked = !Checked;
            }

            base.Update(gameTime);
        }

        public override void Draw(float frameTime)
        {
            Vector2 textPos = GetTextPosition();
            Vector2 checkPos = textPos;

            Rectangle screenRect = ToScreen(Rectangle);

            Rectangle rect = screenRect;
            rect.Y += 1;
            rect.Height -= 3;
            rect.Width = rect.Height;
            Rectangle top = new Rectangle(rect.Left, rect.Top, rect.Width, 2);
            Rectangle left = new Rectangle(rect.Left, rect.Top + 2, 2, rect.Height - 4);
            Rectangle right = new Rectangle(rect.Right - 2, rect.Top + 2, 2, rect.Height - 4);
            Rectangle bottom = new Rectangle(rect.Left, rect.Bottom - 2, rect.Width, 2);

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, top, ThisControlColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, left, ThisControlColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, right, ThisControlColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, bottom, ThisControlColor);

            textPos.X += rect.Width + 2;
            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, textPos, ThisForeColor);

            if (Checked)
            {
                rect.Inflate(-4, -4);
                checkPos.X += 2;
                XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, "X", checkPos, ThisForeColor);
            }

            base.Draw(frameTime);
        }
    }
}
