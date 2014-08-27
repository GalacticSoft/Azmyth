using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using Azmyth.Assets;
using System.Diagnostics;

namespace Azmyth.Editor
{
    public partial class TerrainHeightScroll : UserControl
    {
        public TerrainHeightScroll()
        {
            InitializeComponent();
        }

        private int m_value;

        private WorldAdapter m_world;

        public WorldAdapter World 
        {
            get
            {
                return m_world;
            }
            set
            {
                m_world = value;

                trackBar1.Minimum = 0;
                trackBar1.Maximum = 1024;
            }
        }

        public int Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                trackBar1.Value = value;
            }
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            this.m_value = this.trackBar1.Value;
            World.TerrainHeight = m_value;
        }
    }

    public class TerrainHeightEditor : UITypeEditor
    {
        private WorldAdapter m_world;

        public TerrainHeightEditor()
        {

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object newObject = value;

            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            m_world = context.Instance as WorldAdapter;

            if (svc != null)
            {
                TerrainHeightScroll ctrl = new TerrainHeightScroll();
                ctrl.World = m_world;
                ctrl.Value = int.Parse(value.ToString());

                svc.DropDownControl(ctrl);

                newObject = ctrl.Value;
            }

            return newObject;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
