using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Azmyth.XNA
{
    public enum GamePadModes
    {
        Normal,
        MoveCursor,
        MoveCursorInverted,
    }

    public enum ThumbSticks
    {
        Left,
        Right
    }

    public static class InputManager
    {
        private static MouseState m_mouseState;
        private static MouseState m_lastMouseState;

        private static KeyboardState m_keyboardState;
        private static KeyboardState m_lastKeyboardState;

        private static GamePadState[] m_padState;
        private static GamePadState[] m_lastPadState;

        public static GamePadModes GamePadMode { get; set; }

        static InputManager()
        {
            GamePadMode = GamePadModes.Normal;

            m_padState = new GamePadState[4];
            m_lastPadState = new GamePadState[4];
        }

        public static void Initialize()
        {
            m_mouseState = Mouse.GetState();
            m_lastMouseState = Mouse.GetState();

            m_keyboardState = Keyboard.GetState();
            m_lastKeyboardState = Keyboard.GetState();

            for (int i = 0; i < 4; i++)
            {
                m_padState[i] = GamePad.GetState((PlayerIndex)i);
                m_lastPadState[i] = GamePad.GetState((PlayerIndex)i);
            }
        }
        public static void Update(GameTime gameTime)
        {
            m_lastMouseState = m_mouseState;
            m_mouseState = Mouse.GetState();

            m_lastKeyboardState = m_keyboardState;
            m_keyboardState = Keyboard.GetState();

            for (int i = 0; i < 4; i++)
            {
                m_lastPadState[i] = m_padState[i];
                m_padState[i] = GamePad.GetState((PlayerIndex)i);
            }

            switch(GamePadMode)
            {
                case GamePadModes.MoveCursor:
                    Mouse.SetPosition(Mouse.GetState().X + (int)(10 * m_lastPadState[0].ThumbSticks.Left.X), Mouse.GetState().Y + (int)(10 * m_padState[0].ThumbSticks.Left.Y));
                    break;
                case GamePadModes.MoveCursorInverted:
                    Mouse.SetPosition(Mouse.GetState().X + (int)(10 * m_lastPadState[0].ThumbSticks.Left.X), Mouse.GetState().Y - (int)(10 * m_padState[0].ThumbSticks.Left.Y));
                    break;
            }
        }

        public static bool AnyKeyPressed()
        {
            return m_lastKeyboardState != m_keyboardState;
        }

        public static bool AnyPadPressed()
        {
            for(int i = 0; i < 4; i++)
            {
                if(m_padState[i].Buttons != m_lastPadState[i].Buttons)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool PadPressed(PlayerIndex player, Buttons button)
        {
            return m_padState[(int)player].IsButtonDown(button) && m_lastPadState[(int)player].IsButtonUp(button);
        }

        public static bool PadHeld(PlayerIndex player, Buttons button)
        {
            return m_padState[(int)player].IsButtonDown(button);
        }

        public static bool KeyPressed(Keys key)
        {
            return m_keyboardState.IsKeyDown(key) && m_lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyHeld(Keys key)
        {
            return m_keyboardState.IsKeyDown(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return m_lastKeyboardState.IsKeyDown(key) && m_keyboardState.IsKeyUp(key);
        }

        public static bool ThumbUpPressed(PlayerIndex player, ThumbSticks stick)
        {            
            bool pressed = false;

            switch(stick)
            {
                case ThumbSticks.Left:
                    if (m_lastPadState[(int)player].ThumbSticks.Left.Y < 0.5f && m_padState[(int)player].ThumbSticks.Left.Y >= 0.5f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_lastPadState[(int)player].ThumbSticks.Right.Y < 0.5f && m_padState[(int)player].ThumbSticks.Right.Y >= 0.5f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbDownPressed(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch(stick)
            {
                case ThumbSticks.Left:
                    if (m_lastPadState[(int)player].ThumbSticks.Left.Y > -0.5f && m_padState[(int)player].ThumbSticks.Left.Y <= -0.5f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_lastPadState[(int)player].ThumbSticks.Right.Y > -0.5f && m_padState[(int)player].ThumbSticks.Right.Y <= -0.5f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbRightPressed(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_lastPadState[(int)player].ThumbSticks.Left.X < 0.5f && m_padState[(int)player].ThumbSticks.Left.X >= 0.5f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_lastPadState[(int)player].ThumbSticks.Right.X < 0.5f && m_padState[(int)player].ThumbSticks.Right.X >= 0.5f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static Vector2 ThumbPosition(PlayerIndex player, ThumbSticks stick)
        {
            Vector2 position = Vector2.Zero;

            switch (stick)
            {
                case ThumbSticks.Left:
                    position = m_padState[(int)player].ThumbSticks.Left;
                    break;
                case ThumbSticks.Right:
                    position = m_padState[(int)player].ThumbSticks.Right;
                    break;
            }

            return position;
        }

        public static bool ThumbRightHeld(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_padState[(int)player].ThumbSticks.Left.X > 0.0f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_padState[(int)player].ThumbSticks.Right.X > 0.0f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbLeftHeld(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_padState[(int)player].ThumbSticks.Left.X < 0.0f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_padState[(int)player].ThumbSticks.Right.X < 0.0f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbUpHeld(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_padState[(int)player].ThumbSticks.Left.Y > 0.0f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_padState[(int)player].ThumbSticks.Right.Y > 0.0f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbDownHeld(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_padState[(int)player].ThumbSticks.Left.Y < -0.0f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_padState[(int)player].ThumbSticks.Right.Y < -0.0f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static bool ThumbLeftPressed(PlayerIndex player, ThumbSticks stick)
        {
            bool pressed = false;

            switch (stick)
            {
                case ThumbSticks.Left:
                    if (m_lastPadState[(int)player].ThumbSticks.Left.X > -0.5f && m_padState[(int)player].ThumbSticks.Left.X <= -0.5f)
                    {
                        pressed = true;
                    }
                    break;
                case ThumbSticks.Right:
                    if (m_lastPadState[(int)player].ThumbSticks.Right.X > -0.5f && m_padState[(int)player].ThumbSticks.Right.X <= -0.5f)
                    {
                        pressed = true;
                    }
                    break;
            }

            return pressed;
        }

        public static void SetVibration(PlayerIndex player, float leftMotor, float rightMotor)
        {
            GamePad.SetVibration(player, leftMotor, rightMotor);
        }

        public static void Reset()
        {
            m_mouseState = Mouse.GetState();
            m_lastMouseState = Mouse.GetState();

            m_keyboardState = Keyboard.GetState();
            m_lastKeyboardState = Keyboard.GetState();

            for (int i = 0; i < 4; i++)
            {
                m_padState[i] = GamePad.GetState((PlayerIndex)i);
                m_lastPadState[i] = GamePad.GetState((PlayerIndex)i);
            }
        }
    }
}
