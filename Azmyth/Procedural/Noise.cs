using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Procedural
{
    public abstract class Noise : INoise
    {
        protected double m_persistence = 0;
        protected double m_frequency = 0;
        protected double m_amplitude = 0;
        protected double m_octaves = 0;
        protected long m_seed = 0;

        // Constructor
        public Noise()
        {
            m_persistence = 0;
            m_frequency = 0;
            m_amplitude  = 0;
            m_octaves = 0;
            m_seed = 0;
        }

        public Noise(double persistence, double frequency, double amplitude, int octaves, long seed)
        {
          m_persistence = persistence;
          m_frequency = frequency;
          m_amplitude  = amplitude;
          m_octaves = octaves;
          m_seed = seed;// 2 + seed * seed;
        }

        public virtual double GetHeight(double x, double y)
        {
            return m_amplitude * Total(x, y);
        }

        protected virtual double Total(double i, double j)
        {
            //properties of one octave (changing each loop)
            double t = 0.0f;
            double _amplitude = 1;
            double freq = m_frequency;

            for (int k = 0; k < m_octaves; k++)
            {
                t += GetValue(j * freq + m_seed, i * freq + m_seed) * _amplitude;
                _amplitude *= m_persistence;
                freq *= 2;
            }

            return t;
        }

        public abstract double GetValue(double x, double y);

        public abstract double GetValue(double x, double y, double z);

        public abstract double GetValue(double x, double y, double z, double t);
    }
}
