using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Ansi
{
    internal class ModifierData
    {
        internal string Name { get; private set; }
        internal string EscapeSequence { get; private set; }
        internal string RtfSequence { get; private set; }

        internal ModifierData(string name, string escapeSequence, string rtfSequence)
        {
            Name = name;
            EscapeSequence = escapeSequence;
            RtfSequence = rtfSequence;
        }
    }
}
