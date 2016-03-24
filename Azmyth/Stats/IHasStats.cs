using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Stats
{
    public interface IHasStats
    {
        int GetValue(VectorID statID);

        void SetValue(VectorID statID, int baseValue);

        void SetModifier(VectorID statID, int modifier);

        void AddModifier(VectorID statID, int modifier);

        IStat GetStat(VectorID statID);

        IList<IStat> GetStats();

        void AddStat(IStat stat);

        void RemoveStat(IStat stat); 
    }
}
