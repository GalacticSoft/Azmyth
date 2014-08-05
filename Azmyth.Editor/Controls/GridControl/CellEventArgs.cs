using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Azmyth.Math;

namespace Azmyth.Editor
{
    public class CellClickEventArgs : EventArgs
    {
        public int CellX { get; set; }
        public int CellY { get; set; }
        public int CellZ { get; set; }

        public VectorCell Cell { get; set; }
        
        MouseEventArgs MouseEventArgs { get; set; }

        public CellClickEventArgs(MouseEventArgs e)
        {
            MouseEventArgs = e;
        }
    }

    public class CellSelectedEventArgs : EventArgs
    {
        public Vector Vector { get; set; }
    }
}
