﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azmyth;
using Azmyth.Assets;
using System.Drawing;

namespace Azmyth.Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmEditorMain());
            Application.Run(new frmPGM());
            Application.Run(new frmMarkov());
        }
    }
}
