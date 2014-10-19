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
    public class XGLabel : XGControl
    {
        public XGLabel(Rectangle rect, string text)
            : base(rect, false)
        {
            Text = text;
            Alignment = GUIAlignment.Left | GUIAlignment.VCenter;
        }

        public XGLabel(Rectangle rect, string text, GUIAlignment alignment)
            : base(rect, false)
        {
            Text = text;
            Alignment = alignment;
        }

        public override void Draw(float frameTime)
        {
            //base.Draw(frameTime);

            Vector2 textPos = GetTextPosition();

            XnaGUIManager.spriteBatch.DrawString(XnaGUIManager.spriteFont, Text, textPos, ForeColor);
        }
    }
}
