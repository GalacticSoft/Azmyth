using System;
using System.Windows;
using System.Drawing;

namespace Azmyth.Assets
{
    public interface IHasRect
    {
        RectangleF Bounds { get; }
        event EventHandler BoundsChanged;
    }
}