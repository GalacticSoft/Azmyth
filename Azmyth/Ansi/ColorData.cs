using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Azmyth.Ansi
{
    internal enum ColorTypes { ForeGround, BackGround, Modifier }

    internal class ColorData
    {
        internal string Name { get; private set; }
        internal string EscapeSequence { get; private set; }

        internal Color Color { get; set; }
        internal ColorTypes Type { get; private set; }

        internal int ColorTableIndex { get; set; }

        internal ColorData(string name, string escapeSequence, Color color, ColorTypes type)
        {
            Name = name;
            EscapeSequence = escapeSequence;
            Color = color;
            Type = type;
        }
    }
}
