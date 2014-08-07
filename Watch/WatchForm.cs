using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stoatly.Util
{
    public class WatchForm : Form
    {
        private static WatchForm s_instance;
        private TextBox m_textBox;

        public WatchForm()
        {
            InitializeComponent();
        }

        public static WatchForm GetInstance()
        {
            if (s_instance == null || s_instance.IsDisposed)
            {
                s_instance = new WatchForm();
            }
            return s_instance;
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            m_textBox = new TextBox();
            m_textBox.Dock = DockStyle.Fill;
            m_textBox.Multiline = true;
            m_textBox.ReadOnly = true;
            m_textBox.ScrollBars = ScrollBars.Both;
            m_textBox.WordWrap = false;

            this.AutoSize = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Text = "Debug";

            Controls.Add(m_textBox);

            Watch.RaiseCollectionChangedEvent += HandleCollectionChangedEvent;

            ResumeLayout();
        }

        private void HandleCollectionChangedEvent(object sender, EventArgs e)
        {
            StringBuilder s = new StringBuilder();
            IEnumerator<KeyValuePair<string, object>> enumerator = Watch.GetEnumerator();
            while(enumerator.MoveNext())
            {
                KeyValuePair<string, object> pair = enumerator.Current;
                string valueString = "null";
                if (pair.Value != null)
                {
                    valueString = pair.Value.ToString();
                }
                s.AppendFormat("{0} = {1}", pair.Key, valueString);
                s.AppendLine();
            }
            m_textBox.Text = s.ToString();
        }
    }
}
