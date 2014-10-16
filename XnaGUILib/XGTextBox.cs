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
    public class XGTextBox : XGPanel
    {
        Color Color;
        int textIndex = 0;

        public XGTextBox(Rectangle rect)
            : base(rect, true)
        {
            Color = new Color(1.0f, 1.0f, 1.0f, 0.707f);
            BorderThickness = 1;
            Alignment = GUIAlignment.Left | GUIAlignment.VCenter;
            Text = string.Empty;
        }

        private string TextLeftOfIndex()
        {
            if (textIndex <= Text.Length)
                return Text.Substring(0, textIndex);
            return string.Empty;
        }

        protected override void GetFocus()
        {
            textIndex = 0;
 	        base.GetFocus();
        }

        bool blinkOn = true;
        float blinkTime = 0.2f;
        float keyTimer = 0.0f;
        bool repeat = false;
        Keys prevKey = Keys.None;

        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (XnaGUIManager.GetFocusControl() == this)
            {
                blinkTime -= frameTime;
                if (blinkTime <= 0.0f)
                {
                    blinkTime = 0.1f;
                    blinkOn = !blinkOn;
                    if (blinkOn)
                        blinkTime = 0.25f;
                }

                bool ShiftPressed = XnaGUIManager.keyState.IsKeyDown(Keys.LeftShift) || XnaGUIManager.keyState.IsKeyDown(Keys.RightShift);

                if (keyTimer > 0.0f)
                    keyTimer -= frameTime;

                bool repeatKey = false;

                if (prevKey == Keys.None)
                {
                    keyTimer = 0.5f;
                }
                else if (XnaGUIManager.keyState.IsKeyDown(prevKey))
                {
                    if (keyTimer <= 0.0f)
                    {
                        keyTimer = 0.5f;
                        if (repeat)
                            keyTimer = 0.1f;
                        repeatKey = true;
                    }
                    repeat = true;
                }
                else
                {
                    keyTimer = 0.0f;
                    repeat = false;
                }

                Keys thisKey = Keys.None;

                // Check for alpha/numeric
                Keys[] keys = XnaGUIManager.keyState.GetPressedKeys();
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] != Keys.LeftShift && keys[i] != Keys.RightShift)
                        thisKey = keys[i];

                    if (repeatKey || XnaGUIManager.prevKeyState.IsKeyUp(keys[i]))
                    {
                        string c = string.Empty;
                        if ((keys[i] >= Keys.A && keys[i] <= Keys.Z))
                        {
                            c = keys[i].ToString();
                            if (!ShiftPressed)
                                c = c.ToLower();
                        }
                        else if (keys[i] >= Keys.D0 && keys[i] <= Keys.D9)
                        {
                            int k = (int)keys[i] - 48;
                            c = k.ToString();
                            thisKey = keys[i];

                        }
                        else if (keys[i] == Keys.OemPipe)
                        {
                            c = @"\";
                            thisKey = keys[i];

                        }
                        else if (keys[i] == Keys.OemPeriod)
                        {
                            c = ".";
                            thisKey = keys[i];

                        }
                        else if (keys[i] == Keys.OemMinus)
                        {
                            if (ShiftPressed)
                                c = "_";
                            else
                                c = @"-";
                        }
                        else if (keys[i] == Keys.Space)
                        {
                            c = " ";
                            thisKey = keys[i];
                        }
                        else if (keys[i] == Keys.OemSemicolon)
                        {
                            if (ShiftPressed)
                                c = ":";
                            else
                                c = ";";
                        }
                        else if (keys[i] == Keys.Home)
                        {
                            textIndex = 0;
                        }
                        else if (keys[i] == Keys.End)
                        {
                            textIndex = Text.Length;
                        }
                        else if (keys[i] == Keys.Right)
                        {
                            if (textIndex < Text.Length)
                                textIndex++;
                            thisKey = keys[i];
                        }
                        else  if (keys[i] == Keys.Left)
                        {
                            if (textIndex > 0)
                                textIndex--;
                            thisKey = keys[i];
                        }
                        else if (keys[i] == Keys.Back)
                        {
                            if (textIndex > 0)
                            {
                                textIndex--;
                                DeleteChar();
                            }
                            thisKey = keys[i];
                        }
                        else if (keys[i] == Keys.Delete)
                        {
                            DeleteChar();
                        }

                        if (!string.IsNullOrEmpty(c))
                        {
                            InsertString(c);
                            textIndex++;
                        }
                    }
                }
                if (prevKey != Keys.None && thisKey == Keys.None)
                    prevKey = thisKey;
                prevKey = thisKey;
            }

            base.Update(gameTime);
        }

        private void DeleteChar()
        {
            if (textIndex < Text.Length)
                Text = Text.Substring(0, textIndex) + Text.Substring(textIndex + 1);
        }

        private void InsertString(string c)
        {
            if (textIndex < Text.Length)
                Text = Text.Substring(0, textIndex) + c + Text.Substring(textIndex);
            else
                Text = Text.Substring(0, textIndex) + c;
        }

        public override void Draw(float frameTime)
        {
            base.Draw(frameTime);

            Vector2 textPos = GetTextPosition();

            Rectangle rects = ToScreen(Rectangle);

            XnaGUIManager.spriteBatch.End();
            XnaGUIManager.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, new RasterizerState() { ScissorTestEnable = true });
            
            Rectangle oldScissor = XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle;
            Rectangle scissorRect = rects;
            scissorRect.Inflate(-4, -4);

            XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle = scissorRect;

            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, textPos, ThisForeColor);

            if (XnaGUIManager.GetFocusControl() == this && blinkOn)
            {
                string text = TextLeftOfIndex();
                Vector2 size = XnaGUIManager.spriteFont.MeasureString(text);
                size.Y = XnaGUIManager.spriteFont.MeasureString("A").Y;
                Rectangle rect = new Rectangle((int)textPos.X + (int)size.X, (int)textPos.Y + 1, 1, (int)size.Y - 2);
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, rect, ThisForeColor);
            }

            XnaGUIManager.spriteBatch.End();
            XnaGUIManager.spriteBatch.GraphicsDevice.ScissorRectangle = oldScissor;
            XnaGUIManager.spriteBatch.Begin();
        }
    }
}
