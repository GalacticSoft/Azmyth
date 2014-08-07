using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoatly.Util
{
    /// <summary>
    /// Provides a set of methods that help monitor variables in your code.
    /// </summary>
    /// <example>
    /// <code>
    /// Watch.Set("myvar", 5);
    /// Watch.Set("myvar", 6);
    /// Watch.Set("newvar", new Point(1, 2));
    /// </code>
    /// </example>
    public static class Watch
    {
        public static event EventHandler RaiseCollectionChangedEvent;

        private static ConcurrentDictionary<string, object> s_dict;
        private static object s_lock = new object();

        static Watch()
        {
            s_dict = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Sets the value associated with the specified key. If the key is 
        /// not in the collection, the key and value are added to the 
        /// dictionary, otherwise  value associated with that key is replaced 
        /// by the assigned value.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to be associated with the specified key.</param>
        /// <exception cref="ArgumentNullException">Key is null</exception>
        public static void Set(string key, object value)
        {
            s_dict[key] = value;
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(null, null);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key. If the 
        /// specified key is not found, throws a KeyNotFoundException.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="ArgumentNullException">Key is null</exception>
        /// <exception cref="KeyNotFoundException">Key does not exist in the collection.</exception>
        public static object Get(string key)
        {
            object value = s_dict[key];
            return value;
        }

        /// <summary>
        /// Removes the value with the specified key from the collection. 
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed; 
        /// otherwise, false. This method returns false if key is not found 
        /// in the collection.</returns>
        /// <exception cref="ArgumentNullException">Key is null</exception>
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

        /// <summary>
        /// Removes all keys and values from the collection.
        /// </summary>
        public static void Clear()
        {
            s_dict.Clear();
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(null, null);
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the collection.
        /// </summary>
        public static int Count()
        {
            return s_dict.Count();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <example>
        /// <code>
        /// IEnumerator&lt;KeyValuePair&lt;string, object&gt;&gt; enumerator = Watch.GetEnumerator();
        /// while(enumerator.MoveNext())
        /// {
        ///     KeyValuePair&lt;string, object&gt; pair = enumerator.Current;
        /// }
        /// </code>
        /// </example>
        public static IEnumerator<KeyValuePair<string, object>> GetEnumerator()
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
