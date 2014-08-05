using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth
{
    public class VectorList<T> : IDictionary<VectorID, T>
    {
        public bool CanShrink { get; set; }

        Dictionary<long, Dictionary<VectorID, T>> _vectorList;

        public VectorList()
        {
            CanShrink = false;
            _vectorList = new Dictionary<long, Dictionary<VectorID, T>>();
        }

        #region IDictionary<VectorID,T> Members

        public void Add(VectorID key, T value)
        {
            if (!_vectorList.ContainsKey(key.Vector))
            {
                _vectorList.Add(key.Vector, new Dictionary<VectorID, T>());
            }

            if (!_vectorList[key.Vector].ContainsKey(key))
            {
                _vectorList[key.Vector].Add(key, value);
            }
        }

        public bool ContainsKey(VectorID key)
        {
            bool containsKey = false;

            if (key != null)
            {
                if (_vectorList.ContainsKey(key.Vector))
                {
                    if (_vectorList[key.Vector].ContainsKey(key))
                    {
                        containsKey = true;
                    }
                }
            }

            return containsKey;
        }

        public ICollection<long> Vectors
        {
            get
            {
                return _vectorList.Keys;
            }
        }

        public ICollection<VectorID> Keys
        {
            get 
            {
                List<VectorID> keys = new List<VectorID>();

                foreach (long vector in Vectors)
                {
                    keys.AddRange(_vectorList[vector].Keys);
                }

                return keys;
            }
        }

        public bool Remove(VectorID key)
        {
            bool removed = false;

            if (ContainsKey(key))
            {
                removed = _vectorList[key.Vector].Remove(key);

                if (CanShrink)
                {
                    if (_vectorList[key.Vector].Count == 0)
                    {
                        _vectorList.Remove(key.Vector);
                    }
                }
            }

            return removed;
        }

        public bool TryGetValue(VectorID key, out T value)
        {
            value = default(T);
            bool tryGetValue = false;

            if (ContainsKey(key))
            {
                value = _vectorList[key.Vector][key];
                tryGetValue = true;
            }

            return tryGetValue;
        }

        public ICollection<T> Values
        {
            get 
            {
                List<T> values = new List<T>();

                foreach (long vector in _vectorList.Keys)
                {
                    values.AddRange(_vectorList[vector].Values);
                }

                return values;
            }
        }

        public T this[long vector, long id]
        {
            get
            {
                T value = default(T);
                VectorID key = new VectorID(vector, id);

                if (ContainsKey(key))
                {
                    value = _vectorList[key.Vector][key];
                }

                return value;
            }

            set
            {
                VectorID key = new VectorID(vector, id);

                if (ContainsKey(key))
                {
                    _vectorList[key.Vector][key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public Dictionary<VectorID, T> this[long vector]
        {
            get
            {
                Dictionary<VectorID, T> values = new Dictionary<VectorID, T>();

                if (_vectorList.ContainsKey(vector))
                {
                    values = _vectorList[vector];
                }

                return values;
            }
        }

        public T this[VectorID key]
        {
            get
            {
                T value = default(T);

                if (ContainsKey(key))
                {
                    value = _vectorList[key.Vector][key];
                }

                return value;
            }
            set
            {
                if (ContainsKey(key))
                {
                    _vectorList[key.Vector][key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<VectorID,T>> Members

        public void Add(KeyValuePair<VectorID, T> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            foreach (long vector in Vectors)
            {
                _vectorList[vector].Clear();
            }

            _vectorList.Clear();
        }

        public bool Contains(KeyValuePair<VectorID, T> item)
        {
            bool contains = false;

            foreach (long vector in Vectors)
            {
                if (_vectorList[vector].Contains(item))
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }

        public void CopyTo(KeyValuePair<VectorID, T>[] array, int arrayIndex)
        {
            foreach (VectorID key in Keys)
            {
                array.SetValue(new KeyValuePair<VectorID, T>(key,_vectorList[key.Vector][key]), arrayIndex);

                arrayIndex++;
            }
        }

        public int Count
        {
            get 
            {
                int count = 0;

                foreach (long vector in Vectors)
                {
                    count += _vectorList[vector].Count;
                }

                return count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<VectorID, T> item)
        {
            bool removed = false;

            if (Contains(item))
            {
                removed = Remove(item.Key);    
            }

            return removed;
        }

        #endregion

        #region IEnumerable<KeyValuePair<VectorID,T>> Members

        public IEnumerator<KeyValuePair<VectorID, T>> GetEnumerator()
        {
            foreach (VectorID key in Keys)
            {
                yield return new KeyValuePair<VectorID, T>(key, _vectorList[key.Vector][key]);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (VectorID key in Keys)
            {
                yield return new KeyValuePair<VectorID, T>(key, _vectorList[key.Vector][key]);
            }
        }

        #endregion
    }
}
