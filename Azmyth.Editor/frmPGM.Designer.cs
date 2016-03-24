namespace Azmyth.Editor
{
    partial class frmPGM
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEV3PGMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeEV3PGMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEV3PGMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid1 = new Azmyth.Editor.Grid();
            this.colorTool1 = new Azmyth.Editor.ColorTool();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(130, 166);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(332, 19);
            this.lblName.TabIndex = 3;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(2, 3);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(2);
            this.trackBar1.Maximum = 48;
            this.trackBar1.Minimum = 4;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(976, 20);
            this.trackBar1.TabIndex = 9;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 12;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(1001, 11);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1018, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openEV3PGMToolStripMenuItem,
            this.closeEV3PGMToolStripMenuItem,
            this.saveEV3PGMToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openEV3PGMToolStripMenuItem
            // 
            this.openEV3PGMToolStripMenuItem.Name = "openEV3PGMToolStripMenuItem";
            this.openEV3PGMToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openEV3PGMToolStripMenuItem.Text = "&Open EV3 PGM";
            this.openEV3PGMToolStripMenuItem.Click += new System.EventHandler(this.openEV3PGMToolStripMenuItem_Click);
            // 
            // closeEV3PGMToolStripMenuItem
            // 
            this.closeEV3PGMToolStripMenuItem.Name = "closeEV3PGMToolStripMenuItem";
            this.closeEV3PGMToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.closeEV3PGMToolStripMenuItem.Text = "Close EV3 PGM";
            this.closeEV3PGMToolStripMenuItem.Click += new System.EventHandler(this.closeEV3PGMToolStripMenuItem_Click);
            // 
            // saveEV3PGMToolStripMenuItem
            // 
            this.saveEV3PGMToolStripMenuItem.Name = "saveEV3PGMToolStripMenuItem";
            this.saveEV3PGMToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.saveEV3PGMToolStripMenuItem.Text = "&Save EV3 PGM";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(982, 11);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 630);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 25);
            this.panel1.TabIndex = 14;
            // 
            // grid1
            // 
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackColor = System.Drawing.Color.White;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid1.CellSize = 12;
            this.grid1.InvertScale = false;
            this.grid1.Location = new System.Drawing.Point(0, 69);
            this.grid1.Margin = new System.Windows.Forms.Padding(0);
            this.grid1.MultiSelect = false;
            this.grid1.Name = "grid1";
            this.grid1.ShowGrid = true;
            this.grid1.Size = new System.Drawing.Size(1018, 562);
            this.grid1.TabIndex = 7;
            this.grid1.SelectionMade += new System.Action<System.Drawing.Rectangle>(this.grid1_SelectionMade);
            // 
            // colorTool1
            // 
            this.colorTool1.BackColor = System.Drawing.Color.White;
            this.colorTool1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorTool1.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorTool1.Location = new System.Drawing.Point(0, 24);
            this.colorTool1.Margin = new System.Windows.Forms.Padding(0);
            this.colorTool1.Name = "colorTool1";
            this.colorTool1.Size = new System.Drawing.Size(1018, 45);
            this.colorTool1.TabIndex = 13;
            // 
            // frmPGM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 655);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.colorTool1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "frmPGM";
            this.Text = "EV3 Portable Graymap Editor";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private Grid grid1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEV3PGMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveEV3PGMToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ToolStripMenuItem closeEV3PGMToolStripMenuItem;
        private ColorTool colorTool1;
        private System.Windows.Forms.Panel panel1;
    }
}