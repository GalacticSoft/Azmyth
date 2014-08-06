using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfiniteGrid
{
    public class DebugForm : Form
    {
        private Debug debug;
        private TextBox textBox;

        public DebugForm(Debug debug)
        {
            this.debug = debug;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            textBox = new TextBox();

            SuspendLayout();

            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.ReadOnly = true;

            this.AutoSize = true;

            Controls.Add(textBox);

            debug.RaiseCollectionChangedEvent += HandleCollectionChangedEvent;

            ResumeLayout();
        }

        void HandleCollectionChangedEvent(object sender, Debug.DebugEventArgs e)
        {
            StringBuilder s = new StringBuilder();
            foreach (KeyValuePair<string, object> item in debug)
            {
                s.AppendFormat("{0} = {1}",  item.Key, item.Value.ToString());
                s.AppendLine();
            }
            textBox.Text = s.ToString();
        }
    }

    public class Debug : IDictionary<string, object>
    {
        protected Dictionary<string, object> items = new Dictionary<string, object>();
        protected DebugForm form;

        public class DebugEventArgs : EventArgs
        {
            private KeyValuePair<string, object> item;

            public DebugEventArgs(KeyValuePair<string, object> item)
            {
                this.item = item;
            }

            public DebugEventArgs(string key, object value)
            {
                this.item = new KeyValuePair<string, object>(key, value);
            }

            public KeyValuePair<string, object> Item
            {
                get { return item; }
            }
        }

        public event EventHandler<DebugEventArgs> RaiseCollectionChangedEvent;

        protected virtual void OnRaiseCollectionChangedEvent(DebugEventArgs e)
        {
            EventHandler<DebugEventArgs> handler = RaiseCollectionChangedEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Add(string key, object value)
        {
            items.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return items.Keys; }
        }

        public bool Remove(string key)
        {
            return items.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return items.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return items.Values; }
        }

        public object this[string key]
        {
            get { return items[key]; }
            set 
            { 
                items[key] = value;

                if (RaiseCollectionChangedEvent != null)
                {
                    RaiseCollectionChangedEvent(this, new DebugEventArgs(key, value));
                }
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            items.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return items.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return items.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void Show()
        {
            if (null == form)
            {
                form = new DebugForm(this);
            }
            form.Show();
        }
    }

    
}
