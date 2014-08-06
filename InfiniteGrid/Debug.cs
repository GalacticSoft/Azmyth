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
            textBox.ScrollBars = ScrollBars.Both;
            textBox.WordWrap = false;

            this.AutoSize = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Text = "Debug";

            Controls.Add(textBox);

            debug.RaiseCollectionChangedEvent += HandleCollectionChangedEvent;

            ResumeLayout();
        }

        void HandleCollectionChangedEvent(object sender, EventArgs e)
        {
            StringBuilder s = new StringBuilder();
            foreach (KeyValuePair<string, object> item in debug)
            {
                string valueString = "null";
                if (item.Value != null)
                {
                    valueString = item.Value.ToString();
                }
                s.AppendFormat("{0} = {1}", item.Key, valueString);
                s.AppendLine();
            }
            textBox.Text = s.ToString();
        }
    }

    public class Debug : IDictionary<string, object>
    {
        protected Dictionary<string, object> items = new Dictionary<string, object>();
        protected DebugForm form;

        public event EventHandler RaiseCollectionChangedEvent;

        protected virtual void OnRaiseCollectionChangedEvent(EventArgs e)
        {
            EventHandler handler = RaiseCollectionChangedEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Add(string key, object value)
        {
            items.Add(key, value);
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(this, null);
            }
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

            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(this, null);
            }
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
                    RaiseCollectionChangedEvent(this, null);
                }
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            items.Add(item.Key, item.Value);
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(this, null);
            }
        }

        public void Clear()
        {
            items.Clear();
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(this, null);
            }
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
            bool result = items.Remove(item.Key);
            if (RaiseCollectionChangedEvent != null)
            {
                RaiseCollectionChangedEvent(this, null);
            }
            return result;
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
