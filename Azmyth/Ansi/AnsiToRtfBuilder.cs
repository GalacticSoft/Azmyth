using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Azmyth.Ansi
{
    public class AnsiToRtfBuilder
    {
        internal StringBuilder m_rtfData = new StringBuilder();

        internal string m_rtfHeader = "";
        internal string m_fontHeader = "";
        internal string m_colorTable = "";
        
        public AnsiToRtfBuilder()
        {
            m_colorTable = GenerateColorTable();
        }

        public void Append(string @string)
        {
            
            string appendString = @"\pard " + @string;
 
            foreach (ColorData cd in AnsiColor.AnsiColors)
            {
                switch (cd.Type)
                {
                    case ColorTypes.ForeGround:
                    {
                        appendString = appendString.Replace(cd.EscapeSequence, @"\cf" + cd.ColorTableIndex);
                        break;
                    }
                    case ColorTypes.BackGround:
                    {
                        appendString = appendString.Replace(cd.EscapeSequence, @"\highlight" + cd.ColorTableIndex);
                        break;
                    }
                }
            }

            foreach (ModifierData md in AnsiColor.AnsiModifiers)
            {
                appendString = appendString.Replace(md.EscapeSequence, md.RtfSequence);
            }

            appendString = appendString.Replace("\r\n", @"\par \r\n");
            appendString = appendString.Replace("\n\r", @"\par \r\n");
            appendString = appendString.Replace("\r", @"\par \r\n");
            appendString = appendString.Replace("\n", @"\par \n");

            m_rtfData.Append(appendString);
        }

        public void AppendLine(string @string)
        {
            Append(@string + @"\par \r\n");
        }

        public void Clear()
        {
            m_rtfData.Remove(0, m_rtfData.Length - 1);
        }

        internal string GenerateColorTable()
        {
            int colorIndex = 1;

            StringBuilder colorTable = new StringBuilder();

            colorTable.Append(@"{\colortbl;");

            foreach (ColorData colorData in AnsiColor.AnsiColors)
            {
                if (colorData.Type == ColorTypes.ForeGround || colorData.Type == ColorTypes.BackGround)
                {
                    Color color = colorData.Color;

                    if (color != null)
                    {
                        colorTable.AppendFormat(@"\red{0}\green{1}\blue{2};", color.R, color.G, color.B);

                        colorData.ColorTableIndex = colorIndex;

                        colorIndex++;
                    }
                    else 
                    {
                        colorData.ColorTableIndex = 0;
                    }
                }
            }

            colorTable.Append(@"}");

            return colorTable.ToString();
        }

        internal string GenerateFontHeader()
        {
            return @"";
        }

        internal string GenerateRtfHeader()
        {
            return @"\rtf1\ansi\ansicpg1252\deff0\deflang1033";
        }

        public override string ToString()
        {
            m_rtfHeader = GenerateRtfHeader();
            m_colorTable = GenerateColorTable();
            m_fontHeader = GenerateFontHeader();

            return "{" + m_rtfHeader + m_fontHeader + m_colorTable + m_rtfData.ToString() + "}";
        }
    }
}
