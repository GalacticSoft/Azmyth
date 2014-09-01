using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Procedural
{
    public interface INoise
    {
        double GetValue(double x, double y);
    }
}
