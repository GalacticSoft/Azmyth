using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azmyth.Procedural;

namespace Azmyth.Editor
{
    public partial class frmMarkov : Form
    {
        MarkovNameGenerator markov;

        public frmMarkov()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            markov = new MarkovNameGenerator(textBox1.Text.Split(','), int.Parse(txtOrder.Text));
        }

        private void btnNextName_Click(object sender, EventArgs e)
        {
            lblName.Text = markov.Next().ToUpper();
        }

        private void txtOrder_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
