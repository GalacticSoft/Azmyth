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
    public class XGLabeledSlider : XGControl
    {
        public XGLabel Label { get; protected set; }
        public XGHSlider Slider { get; protected set; }
        public XGLabel ValueLabel { get; protected set; }
        public string ValueLabelFormat { get; set; }

        public float Value { get { return Slider.Value; } set { Slider.Value = value; } }
        public float Scale { get { return Slider.Scale; } set { Slider.Scale = value; } }

        public XGLabeledSlider(Rectangle rect, int labelWidth, string text, int valueLabelWidth, float value, float min, float max)
            : base(rect, true)
        {
            ValueLabelFormat = "F3";

            Rectangle partRect = rect;
            partRect.X = partRect.Y = 0; // parent relative

            partRect.Width = labelWidth;
            Label = new XGLabel(partRect, text, GUIAlignment.Right | GUIAlignment.VCenter);
            Children.Add(Label);

            partRect.X += partRect.Width + 1;
            partRect.Width = rect.Width - (labelWidth + valueLabelWidth + 2);
            Slider = new XGHSlider(partRect, 0.0f);
            Slider.SetRange(value, min, max);
            Children.Add(Slider);

            partRect.X = Slider.Rectangle.X + Slider.Rectangle.Width + 1;
            partRect.Width = valueLabelWidth;
            ValueLabel = new XGLabel(partRect, "0.000");
            Children.Add(ValueLabel);
        }

        public override void Update(GameTime gameTime)
        {
            ValueLabel.Text = Slider.Value.ToString(ValueLabelFormat);
            base.Update(gameTime);
        }
    }
}
