using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Azmyth.Editor
{
    public partial class SplitPanel : UserControl
    {
        private string _text;
        public SplitPanel()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("Appearance")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public string Caption
        {
	          get 
	        {
                return _text;
	        }
	          set 
	        {
                _text = value;
                lblCaption.Text = value;
	        }
        }
        [Browsable(true)]
        [Category("Behavior")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event Action<object, EventArgs> Close;

        [Browsable(true)]
        [Category("Behavior")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event Action<object, EventArgs> Minimize;

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (Close != null)
            {
                Close(sender, e);
            }
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (Minimize != null)
            {
                Minimize(sender, e);
            }
        }
    }
}
