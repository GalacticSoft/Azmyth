namespace Azmyth.Editor
{
    partial class MapGrid
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnNorth = new System.Windows.Forms.ToolStripButton();
            this.btnWest = new System.Windows.Forms.ToolStripButton();
            this.btnEast = new System.Windows.Forms.ToolStripButton();
            this.btnSouth = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gridView = new Azmyth.Editor.GridViewer();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUp,
            this.btnNorth,
            this.btnWest,
            this.btnEast,
            this.btnSouth,
            this.btnDown,
            this.btnZoomIn,
            this.btnZoomOut,
            this.toolStripSeparator1,
            this.toolStripSeparator2});
            this.toolStrip.Location = new System.Drawing.Point(0, 368);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(681, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnNorth
            // 
            this.btnNorth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNorth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNorth.Name = "btnNorth";
            this.btnNorth.Size = new System.Drawing.Size(23, 22);
            this.btnNorth.Click += new System.EventHandler(this.btnNorth_Click);
            // 
            // btnWest
            // 
            this.btnWest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnWest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnWest.Name = "btnWest";
            this.btnWest.Size = new System.Drawing.Size(23, 22);
            this.btnWest.Click += new System.EventHandler(this.btnWest_Click);
            // 
            // btnEast
            // 
            this.btnEast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEast.Name = "btnEast";
            this.btnEast.Size = new System.Drawing.Size(23, 22);
            this.btnEast.Click += new System.EventHandler(this.btnEast_Click);
            // 
            // btnSouth
            // 
            this.btnSouth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSouth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSouth.Name = "btnSouth";
            this.btnSouth.Size = new System.Drawing.Size(23, 22);
            this.btnSouth.Click += new System.EventHandler(this.btnSouth_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // gridView
            // 
            this.gridView.BorderColor = System.Drawing.Color.Black;
            this.gridView.CellSize = new System.Drawing.Size(16, 16);
            this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView.FullColor = System.Drawing.Color.Black;
            this.gridView.GridColor = System.Drawing.Color.DarkGray;
            this.gridView.HoverColor = System.Drawing.Color.Black;
            this.gridView.Location = new System.Drawing.Point(0, 0);
            this.gridView.Margin = new System.Windows.Forms.Padding(4);
            this.gridView.Name = "gridView";
            this.gridView.SelectedColor = System.Drawing.Color.White;
            this.gridView.Size = new System.Drawing.Size(681, 368);
            this.gridView.TabIndex = 1;
            this.gridView.CellClick += new Azmyth.Editor.CellClickEvent(this.gridView_CellClick);
            this.gridView.CellRightClick += new Azmyth.Editor.CellClickEvent(this.gridView_CellRightClick);
            this.gridView.CellDoubleClick += new Azmyth.Editor.CellClickEvent(this.gridView_CellDoubleClick);
            this.gridView.SelectedCellChanged += new Azmyth.Editor.CellSelectEvent(this.gridView_SelectedCellChanged);
            this.gridView.HoveredCellChanged += new Azmyth.Editor.CellSelectEvent(this.gridView_HoveredCellChanged);
            // 
            // MapGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridView);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MapGrid";
            this.Size = new System.Drawing.Size(681, 393);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapGrid_Paint);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ToolStripButton btnNorth;
        private System.Windows.Forms.ToolStripButton btnWest;
        private System.Windows.Forms.ToolStripButton btnEast;
        private System.Windows.Forms.ToolStripButton btnSouth;
        private System.Windows.Forms.ToolStripButton btnDown;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Azmyth.Editor.GridViewer gridView;
    }
}
