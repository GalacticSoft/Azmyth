using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class XnaGUIManager
    {
        public static List<XGControl> Controls = new List<XGControl>();
        public static int CurrentControlIndex = -1; // Who has focus?
        public static bool Active { get; private set; } // is this gui active (receives input)?
        public static Game Game;
        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;
        public static Texture2D whiteTex;
        internal static KeyboardState keyState;
        internal static KeyboardState prevKeyState = new KeyboardState();
        internal static MouseState mouseState;
        internal static MouseState prevMouseState = new MouseState();

        public static void Initialize(Game game)
        {
            Game = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            spriteFont = game.Content.Load<SpriteFont>("GuiSpriteFont");
            whiteTex = game.Content.Load<Texture2D>("WhiteRect");
            Active = false;
        }

        public static void BringToTop(XGControl control)
        {
            if (Controls.Contains(control))
            {
                Controls.Remove(control);
                Controls.Insert(0, control);
            }
        }

        public static void Activate(bool active)
        {
            Active = active;
            if (Active)
            {
                if (CurrentControlIndex < 0)
                {
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        if (Controls[i].Enabled)
                        {
                            if (Controls[i].ActivateFirst())
                            {
                                CurrentControlIndex = i;
                                return;
                            }
                        }
                    }
                }
                if (CurrentControlIndex >= 0)
                    Controls[CurrentControlIndex].Activate();
            }
        }

        public static void ActivateNext()
        {
            XGControl current = GetFocusControl();
            if (current != null)
            {
                if (current.ActivateNext() == false)
                {
                    if (CurrentControlIndex >= 0 && Controls[CurrentControlIndex].ActivateFirst())
                        return;

                    //if (current.ActivateFirst())
                    //    return;

                    CurrentControlIndex = -1;
                    // start over at the top (cycle around)
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        if (Controls[i].ActivateFirst())
                        {
                            CurrentControlIndex = i;
                            return;
                        }
                    }
                }
            }
            else
            {
                Activate(true);
            }
        }

        public static void ActivatePrevious()
        {
            XGControl current = GetFocusControl();
            if (current != null)
            {
                if (current.ActivatePrevious() == false)
                {
                    if (CurrentControlIndex >= 0 && Controls[CurrentControlIndex].ActivateLast())
                        return;

                    CurrentControlIndex = -1;
                    // start at the end (cycle backwards)
                    for (int i = Controls.Count - 1; i >= 0; i--)
                    {
                        if (Controls[i].Enabled)
                        {
                            if (Controls[i].ActivateLast())
                            {
                                CurrentControlIndex = i;
                                return;
                            }
                        }
                    }
                }
            }
            Activate(true);
        }

        static int _lastActiveControlIndex = -1;

        public static bool Activate(Point point)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                XGControl control = Controls[i];
                if (control.Enabled && control.Contains(point))
                {
                    if (control.Activate(point))
                    {
                        CurrentControlIndex = i;
                        return true;
                    }
                }
            }
            _lastActiveControlIndex = CurrentControlIndex;
            CurrentControlIndex = -1;
            return false;
        }

        public static XGControl GetFocusControl()
        {
            if (CurrentControlIndex >= 0)
            {
                return Controls[CurrentControlIndex].GetFocusControl();
            }
            return null;
        }

        public static bool HasFocus(XGControl control)
        {
            XGControl focus = GetFocusControl();
            if (focus == null)
                return false;

            if (control == focus)
                return true;

            return focus.IsParent(control);
        }

        public static bool IsActive { get { return GetFocusControl() != null; } }


        public static void Update(GameTime gameTime)
        {
            if (!Game.IsMouseVisible)
                return;

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed &&
                prevMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseState.X >= 0 && mouseState.Y >= 0)
                    Activate(new Point(mouseState.X, mouseState.Y));
            }

            if (keyState.IsKeyDown(Keys.Tab) && prevKeyState.IsKeyUp(Keys.Tab))
            {
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    ActivatePrevious();
                else
                    ActivateNext();
            }

            foreach (XGControl control in Controls)
            {
                if (control.Enabled)
                    control.Update(gameTime);
            }

            prevKeyState = keyState;
            prevMouseState = mouseState;
        }

        public static void Draw(float frameTime)
        {
            if (!Game.IsMouseVisible)
                return;

            foreach (XGControl control in Controls)
            {
                if (control.Visible)
                {
                    spriteBatch.Begin();
                    control.Draw(frameTime);
                    spriteBatch.End();
                }
            }
        }
    }
}
