using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Stats;

namespace Azmyth.Game
{
    public class Stat : IStat
    {
        private string m_name = "";
        private string m_abbreviation = "";
        private string m_description = "";

        public VectorID ID { get; set; }

        public int BaseValue { get; set; }
        public int Modifier { get; set; }

        public int MinBase { get; set; }
        public int MaxBase { get; set; }

        public int MinMod { get; set; }
        public int MaxMod { get; set; }

        public int Minimum
        {
            get
            {
                return MinBase + MinMod;
            }
        }
        public int Maximum
        {
            get
            {
                return MaxBase + MaxMod;
            }
        }

        public int Value
        {
            get
            {
                int value = BaseValue + Modifier;

                if (value < Minimum)
                {
                    value = Minimum;
                }

                if (value > Maximum)
                {
                    value = Maximum;
                }

                return value;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        public string Abbreviation
        {
            get
            {
                return m_abbreviation;
            }
            set
            {
                m_abbreviation = value;
            }
        }
        public string Description
        {
            get
            {
                return m_description;
            }
            set
            {
                m_description = value;
            }
        }

        public IStat Clone()
        {
            return MemberwiseClone() as IStat;
        }

        public void Roll()
        {
            BaseValue = Numbers.NumberRange(MinBase, MaxBase);
        }
    }
}
