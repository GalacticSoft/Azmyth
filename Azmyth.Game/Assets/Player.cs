using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Stats;

namespace Azmyth.Assets
{
    public class Player : Asset, IHasStats
    {
        private Dictionary<VectorID, IStat> m_stats = new Dictionary<VectorID, IStat>();

        public Player()
        {
            foreach(VectorID statID in Stats.Prototypes.Keys)
            {
                m_stats.Add(statID, Stats.Prototypes[statID].Clone());
            }
        }

        public int GetValue(VectorID statID)
        {
            return m_stats[statID].Value;
        }

        public void SetValue(VectorID statID, int baseValue)
        {
            m_stats[statID].BaseValue = baseValue;
        }

        public IList<IStat> GetStats()
        {
            return m_stats.Values.ToList<IStat>();
        }

        public void SetModifier(VectorID statID, int modifier)
        {
            m_stats[statID].Modifier = modifier;
        }

        public void AddModifier(VectorID statID, int modifier)
        {
            m_stats[statID].Modifier += modifier;
        }

        public IStat GetStat(VectorID statID)
        {
            return m_stats[statID];
        }

        public void AddStat(IStat stat)
        {
            m_stats.Add(stat.ID, stat);
        }

        public void RemoveStat(IStat stat)
        {
            m_stats.Remove(stat.ID);
        }
    }
}
