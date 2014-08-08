namespace InfiniteGrid
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.grid1 = new InfiniteGrid.Grid();
            this.quadView2 = new InfiniteGrid.QuadView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 56);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 591);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(880, 562);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grid View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.quadView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(880, 562);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quad View";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Maximum = 200;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(888, 56);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Value = 100;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // grid1
            // 
            this.grid1.BackColor = System.Drawing.Color.White;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.GridColor = System.Drawing.Color.Black;
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.OriginColor = System.Drawing.Color.Black;
            this.grid1.QuadTree = null;
            this.grid1.SelectionColor = System.Drawing.Color.DarkRed;
            this.grid1.ShowGrid = true;
            this.grid1.ShowOrigin = true;
            this.grid1.ShowSelection = true;
            this.grid1.Size = new System.Drawing.Size(874, 556);
            this.grid1.TabIndex = 3;
            this.grid1.Zoom = 1F;
            this.grid1.HoverChanged += new InfiniteGrid.CellEvent(this.grid1_HoverChanged);
            this.grid1.ViewportChanged += new InfiniteGrid.CellEvent(this.grid1_ViewportChanged);
            this.grid1.ViewportChanging += new InfiniteGrid.CellEvent(this.grid1_ViewportChanging);
            this.grid1.SelectionChanged += new InfiniteGrid.CellEvent(this.grid1_SelectionChanged);
            this.grid1.DoubleClick += new System.EventHandler(this.grid1_DoubleClick);
            // 
            // quadView2
            // 
            this.quadView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quadView2.Location = new System.Drawing.Point(3, 3);
            this.quadView2.Name = "quadView2";
            this.quadView2.QuadTree = null;
            this.quadView2.Size = new System.Drawing.Size(874, 556);
            this.quadView2.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 647);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.trackBar1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Grid grid1;
        private QuadView quadView2;
        private System.Windows.Forms.TrackBar trackBar1;


    }
}

