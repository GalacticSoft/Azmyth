using System;
using System.Windows;
using System.Drawing;

namespace Azmyth.Assets
{
    public interface IQuadObject
    {
        RectangleF Bounds { get; }
        event EventHandler BoundsChanged;
    }
}