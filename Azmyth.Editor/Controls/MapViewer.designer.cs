namespace Azmyth.Editor
{
    partial class MapViewer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridControl2 = new Azmyth.Editor.GridControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridControl1 = new Azmyth.Editor.GridControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gridControl3 = new Azmyth.Editor.GridControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(974, 611);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridControl2);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(966, 582);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Terrain Map";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridControl2
            // 
            this.gridControl2.BackColor = System.Drawing.Color.White;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.HeightMap = false;
            this.gridControl2.Location = new System.Drawing.Point(3, 3);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.ShowDebug = false;
            this.gridControl2.Size = new System.Drawing.Size(960, 576);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.TempMap = false;
            this.gridControl2.Text = "gridControl2";
            this.gridControl2.ToolShape = Azmyth.Editor.ToolShape.Point;
            this.gridControl2.ToolSize = 40;
            this.gridControl2.CellHover += new System.EventHandler<System.Drawing.Point>(this.gridControl2_CellHover);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(966, 582);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Height Map";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridControl1
            // 
            this.gridControl1.BackColor = System.Drawing.Color.White;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.HeightMap = true;
            this.gridControl1.Location = new System.Drawing.Point(3, 3);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.ShowDebug = false;
            this.gridControl1.Size = new System.Drawing.Size(960, 576);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.TempMap = false;
            this.gridControl1.Text = "gridControl1";
            this.gridControl1.ToolShape = Azmyth.Editor.ToolShape.Point;
            this.gridControl1.ToolSize = 40;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.gridControl3);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(966, 582);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Temp Map";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // gridControl3
            // 
            this.gridControl3.BackColor = System.Drawing.Color.White;
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.HeightMap = true;
            this.gridControl3.Location = new System.Drawing.Point(3, 3);
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.ShowDebug = false;
            this.gridControl3.Size = new System.Drawing.Size(960, 576);
            this.gridControl3.TabIndex = 1;
            this.gridControl3.TempMap = true;
            this.gridControl3.Text = "gridTemp";
            this.gridControl3.ToolShape = Azmyth.Editor.ToolShape.Point;
            this.gridControl3.ToolSize = 40;
            // 
            // MapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "MapViewer";
            this.Size = new System.Drawing.Size(974, 611);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public GridControl gridControl2;
        public GridControl gridControl1;
        private System.Windows.Forms.TabPage tabPage3;
        public GridControl gridControl3;

     
    }
}
