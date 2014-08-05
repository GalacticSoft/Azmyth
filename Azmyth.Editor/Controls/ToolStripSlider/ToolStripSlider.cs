using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;

namespace Azmyth.Editor
{
    public class ToolStripSlider : ToolStripControlHost
    {
        private TrackBar _trackBar;

        public event EventHandler ValueChanged
        {
            add 
            {
                _trackBar.ValueChanged += value;
            }
            remove
            {
                _trackBar.ValueChanged -= value;
            }
        }

        [Browsable(true)]
        public int Value
        {
            get
            {
                return _trackBar.Value;
            }
            set
            {
                if (value > _trackBar.Maximum)
                {
                    _trackBar.Value = _trackBar.Maximum;
                }
                else if (value < _trackBar.Minimum)
                {
                    _trackBar.Value = _trackBar.Minimum;
                }
                else
                {
                    _trackBar.Value = value;
                }
            }
        }

        public ToolStripSlider() : base(new TrackBar())
        {
            _trackBar = this.Control as TrackBar;

            _trackBar.TickStyle = TickStyle.None;

            _trackBar.AutoSize = false;
            _trackBar.Height = 16;
            _trackBar.Width = this.Width;

            _trackBar.Minimum = 0;//(int)(0.50 * 100.00);
            _trackBar.Maximum = 100;// (int)(2.00 * 100.00);

            _trackBar.SmallChange = (int)(0.10 * 100.00);
            _trackBar.SmallChange = (int)(0.10 * 100.00);

            _trackBar.Value = (int)0;           
        }
    }
}
