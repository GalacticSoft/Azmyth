using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoatly.Util
{
    public static class Watch
    {
        public static event EventHandler RaiseCollectionChangedEvent;

        private static ConcurrentDictionary<string, object> s_dict;
        private static object s_lock = new object();

        static Watch()
        {
            s_dict = new ConcurrentDictionary<string, object>();
        }

        public static void Set(string key, object value)
        {
             
            s_dict[key] = value;
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(null, null);
            }
        }

        public static object Get(string key)
        {
            object value = s_dict[key];
            return value;
        }

        public static bool Remove(string key)
        {
            object obj;
            bool result = s_dict.TryRemove(key, out obj);
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(null, null);
            }
            return result;
        }

        public static void Clear()
        {
            s_dict.Clear();
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(null, null);
            }
        }

        public static int Count()
        {
            return s_dict.Count();
        }

        public static System.Collections.Generic.IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return s_dict.GetEnumerator();
        }

        private static void OnRaiseCollectionChangedEvent(EventArgs e)
        {
            EventHandler handler = RaiseCollectionChangedEvent;
            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
}
