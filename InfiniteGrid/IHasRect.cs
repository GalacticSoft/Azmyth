using System;
using System.Drawing;

namespace InfiniteGrid
{
    /// <summary>
    /// An interface that defines and object with a rectangle
    /// </summary>
    public interface IHasRect
    {
        Rectangle Rectangle { get; }
    }
}
