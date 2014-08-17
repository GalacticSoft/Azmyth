using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Azmyth.Ansi
{
    internal static class AnsiColor
    {
        internal static List<ColorData> AnsiColors = new List<ColorData> 
        {
            new ColorData("Black",  "\x1B[30m",  Color.DarkGray,   ColorTypes.ForeGround),
            new ColorData("Red",    "\x1B[31m",  Color.Red,     ColorTypes.ForeGround),
            new ColorData("Green",  "\x1B[32m",  Color.Green,   ColorTypes.ForeGround),
            new ColorData("Yellow", "\x1B[33m",  Color.Yellow,  ColorTypes.ForeGround),
            new ColorData("Blue",   "\x1B[34m",  Color.Blue,    ColorTypes.ForeGround),
            new ColorData("Magenta","\x1B[35m",  Color.Magenta, ColorTypes.ForeGround),
            new ColorData("Cyan",   "\x1B[36m",  Color.Cyan,    ColorTypes.ForeGround),
            new ColorData("White",  "\x1B[37m",  Color.White,   ColorTypes.ForeGround),

            new ColorData("Black",  "\x1B[40m",  Color.DarkGray,   ColorTypes.BackGround),
            new ColorData("Red",    "\x1B[41m",  Color.Red,     ColorTypes.BackGround),
            new ColorData("Green",  "\x1B[42m",  Color.Green,   ColorTypes.BackGround),
            new ColorData("Yellow", "\x1B[43m",  Color.Yellow,  ColorTypes.BackGround),
            new ColorData("Blue",   "\x1B[44m",  Color.Blue,    ColorTypes.BackGround),
            new ColorData("Magenta","\x1B[45m",  Color.Magenta, ColorTypes.BackGround),
            new ColorData("Cyan",   "\x1B[46m",  Color.Cyan,    ColorTypes.BackGround),
            new ColorData("White",  "\x1B[47m",  Color.White,   ColorTypes.BackGround),

            //TODO: Add a seperate way for higher level escape sequences, maybe convert into ansi escape sequences.
            // Currently doubles the color table size. Also need to add bold codes i.e. &+L, &+R, &+C...
            new ColorData("Black",  "&+l",  Color.DarkGray,   ColorTypes.ForeGround),
            new ColorData("Red",    "&+r",  Color.Red,     ColorTypes.ForeGround),
            new ColorData("Green",  "&+g",  Color.Green,   ColorTypes.ForeGround),
            new ColorData("Yellow", "&+y",  Color.Yellow,  ColorTypes.ForeGround),
            new ColorData("Blue",   "&+b",  Color.Blue,    ColorTypes.ForeGround),
            new ColorData("Magenta","&+m",  Color.Magenta, ColorTypes.ForeGround),
            new ColorData("Cyan",   "&+c",  Color.Cyan,    ColorTypes.ForeGround),
            new ColorData("White",  "&+w",  Color.White,   ColorTypes.ForeGround),

            new ColorData("Black",  "&-l",  Color.DarkGray,   ColorTypes.BackGround),
            new ColorData("Red",    "&-r",  Color.Red,     ColorTypes.BackGround),
            new ColorData("Green",  "&-g",  Color.Green,   ColorTypes.BackGround),
            new ColorData("Yellow", "&-y",  Color.Yellow,  ColorTypes.BackGround),
            new ColorData("Blue",   "&-b",  Color.Blue,    ColorTypes.BackGround),
            new ColorData("Magenta","&-m",  Color.Magenta, ColorTypes.BackGround),
            new ColorData("Cyan",   "&-c",  Color.Cyan,    ColorTypes.BackGround),
            new ColorData("White",  "&-w",  Color.White,   ColorTypes.BackGround),
        };

        internal static List<ModifierData> AnsiModifiers = new List<ModifierData>()
        {
            new ModifierData("Reset",          "\x1B[1m",   @"\b0\i0\strike0\highlight0 "       ),
            
            new ModifierData("Bold",           "\x1B[3m",   @"\b "           ),
            new ModifierData("Italic",         "\x1B[4m",   @"\i "           ),
            new ModifierData("Underline",      "\x1B[5m",   @"\ul "          ),
            new ModifierData("Blink",          "\x1B[5m",   @"\animtext2 "   ),
            new ModifierData("Blink Fast",     "\x1B[6m",   @"\animtext2 "   ),
            new ModifierData("Inverse",        "\x1B[7m",   @""              ),
            new ModifierData("Strike-Through", "\x1B[8m",   @"\strike "      ),

            new ModifierData("Bold Off",       "\x1B[22m",  @"\b0 "          ),
            new ModifierData("Italic Off",     "\x1B[23m",  @"\i0 "          ),
            new ModifierData("Underline Off",  "\x1B[24m",  @"\ul0 "         ),
            new ModifierData("Blink Off",      "\x1B[25m",  @""              ),
            new ModifierData("Inverse Off",    "\x1B[27m",  @""              ),
            new ModifierData("Strike Off",     "\x1B[29m",  @"\strike0 "     ),

            new ModifierData("Reset",          "&N",   @"\b0\i0\strike0\highlight0 "       ),
            new ModifierData("Reset",          "&n",   @"\b0\i0\strike0\highlight0 "       ),

            new ModifierData("Bold",           "&B",   @"\b "           ),
            new ModifierData("Italic",         "&I",   @"\i "           ),
            new ModifierData("Underline",      "&U",   @"\ul "          ),
            new ModifierData("Blink",          "&K",   @"\animtext2 "   ),
            new ModifierData("Blink Fast",     "&F",   @"\animtext2 "   ),
            new ModifierData("Inverse",        "&V",   @""              ),
            new ModifierData("Strike-Through", "&S",   @"\strike "      ),

            new ModifierData("Bold Off",       "&b",  @"\b0 "          ),
            new ModifierData("Italic Off",     "&i",  @"\i0 "          ),
            new ModifierData("Underline Off",  "&u",  @"\ul0 "         ),
            new ModifierData("Blink Off",      "&k",  @""              ),
            new ModifierData("Inverse Off",    "&f",  @""              ),
            new ModifierData("Strike Off",     "&s",  @"\strike0 "     ),
        };

        public static string RemoveColorCodes(this string @string)
        {
            string result = String.Empty;

            foreach (ColorData cd in AnsiColors)
            {
                result = @string.Replace(cd.EscapeSequence, String.Empty);
            }

            foreach (ModifierData md in AnsiModifiers)
            {
                result = @string.Replace(md.EscapeSequence, String.Empty);
            }

            return result;
        }

        public static ColorData GetColorData(string escapeSequence)
        {
            ColorData color = null;
            
            foreach (ColorData c in AnsiColors)
            {
                if (c.EscapeSequence == escapeSequence)
                {
                    color = c;
                }
            }

            return color;
        }

        public static bool HasColorData(this string @string)
        {
            bool result = false;

            foreach (ColorData c in AnsiColors)
            {
                if (@string.Contains(c.EscapeSequence))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
