using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Stats
{
    public interface IHasStats
    {
        void SetStat(StatVector vector, string name, double baseValue);
        List<IStat> GetStats();
        Dictionary<VectorID, IStat> GetStats(StatVector vector);
        void SetStatMod(StatVector vector, string name, double modifier);
        IStat GetStat(VectorID statID);
        IStat GetStat(StatVector vector, string name);
        void AddStat(IStat stat);
        void RemoveStat(IStat stat); 
        void RemoveStats();
        void RemoveStats(long vector);
    }
}
