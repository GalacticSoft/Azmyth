using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Editor
{
    public class Markov
    {
        private int m_order = 1;
        private int m_minLength = 9999999;
        private int m_maxLength = 0;

        Random m_random = new Random();
        private List<string> m_samples = null;
        private Dictionary<string, List<string>> m_chains = new Dictionary<string,List<string>>();
        
        public Markov(IEnumerable<string> samples, int order)
        {
            m_order = order;
            m_samples = samples.ToList<string>();

            foreach (string sample in samples)
            {
                if (m_minLength > sample.Length)
                    m_minLength = sample.Length;

                if (m_maxLength < sample.Length)
                    m_maxLength = sample.Length;

                for (int index = 0; index < sample.Length - order; index++)
                {
                    string token = sample.Substring(index, order);

                    List<string> entry = null;

                    if(!m_chains.ContainsKey(token))
                    {
                        entry = new List<string>();
                        m_chains.Add(token, entry);
                    }
                    else
                    {
                        entry = m_chains[token];
                    }

                    entry.Add(sample[index + order].ToString());
                }
            }
        }

        public string Next()
        {
            string name = "";
            int length = m_random.Next(m_minLength, m_maxLength);

            int rand = m_random.Next(m_samples.Count<string>());

            name = m_samples[rand].Substring(m_random.Next(0, m_samples[rand].Length - m_order), m_order);

            while (name.Length < length)
            {
                string token = name.Substring(name.Length - m_order, m_order);

                if (m_chains.ContainsKey(token))
                {
                    if (m_chains[token].Count > 0)
                    {
                        name += m_chains[token][m_random.Next(m_chains[token].Count)];
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return name;
        }
    }
}
