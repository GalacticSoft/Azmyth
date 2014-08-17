namespace Azmyth.Editor
{
    partial class GridViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.gridControl = new Azmyth.Editor.GridControlOld();
            this.scrollPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scrollPanel
            // 
            this.scrollPanel.AutoScroll = true;
            this.scrollPanel.BackColor = System.Drawing.Color.White;
            this.scrollPanel.Controls.Add(this.gridControl);
            this.scrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollPanel.Location = new System.Drawing.Point(0, 0);
            this.scrollPanel.Name = "scrollPanel";
            this.scrollPanel.Size = new System.Drawing.Size(471, 368);
            this.scrollPanel.TabIndex = 0;
            // 
            // gridControl
            // 
            this.gridControl.BackColor = System.Drawing.Color.White;
            this.gridControl.BorderColor = System.Drawing.Color.Black;
            this.gridControl.ForeColor = System.Drawing.Color.Black;
            this.gridControl.FullColor = System.Drawing.Color.White;
            this.gridControl.GridColor = System.Drawing.Color.DarkGray;
            this.gridControl.HoverColor = System.Drawing.Color.LightSteelBlue;
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.Name = "gridControl";
            this.gridControl.SelectedColor = System.Drawing.Color.White;
            this.gridControl.ShowGrid = true;
            this.gridControl.Size = new System.Drawing.Size(20, 20);
            this.gridControl.TabIndex = 0;
            this.gridControl.CellClick += new Azmyth.Editor.CellClickEvent(this.gridControl_CellClick);
            this.gridControl.CellRightClick += new Azmyth.Editor.CellClickEvent(this.gridControl_CellRightClick);
            this.gridControl.CellDoubleClick += new Azmyth.Editor.CellClickEvent(this.gridControl_CellDoubleClick);
            this.gridControl.HoveredCellChanged += new Azmyth.Editor.CellSelectEvent(this.gridControl_HoveredCellChanged);
            this.gridControl.SelectedCellChanged += new Azmyth.Editor.CellSelectEvent(this.gridControl_SelectedCellChanged);
            // 
            // GridViewer
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.scrollPanel);
            this.DoubleBuffered = true;
            this.Name = "GridViewer";
            this.Size = new System.Drawing.Size(471, 368);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GridViewer_Paint);
            this.scrollPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel scrollPanel;
        private GridControlOld gridControl;
    }
}
