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
    public class SnowLineEditor : UITypeEditor
    {
        private WorldAdapter m_world;

        public SnowLineEditor()
        {

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object newObject = value;

            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            m_world = context.Instance as WorldAdapter;

            if (svc != null)
            {
                SnowLineScroll ctrl = new SnowLineScroll();
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
