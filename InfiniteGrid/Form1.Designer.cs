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
            this.quadView1 = new InfiniteGrid.QuadView();
            this.grid1 = new InfiniteGrid.Grid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // quadView1
            // 
            this.quadView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quadView1.Location = new System.Drawing.Point(3, 3);
            this.quadView1.Name = "quadView1";
            this.quadView1.QuadTree = null;
            this.quadView1.Size = new System.Drawing.Size(874, 908);
            this.quadView1.TabIndex = 1;
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
            this.grid1.ShowOrigin = false;
            this.grid1.ShowSelection = true;
            this.grid1.Size = new System.Drawing.Size(874, 908);
            this.grid1.TabIndex = 2;
            this.grid1.HoverChanged += new InfiniteGrid.CellEvent(this.grid1_HoverChanged);
            this.grid1.ViewportChanged += new InfiniteGrid.CellEvent(this.grid1_ViewportChanged);
            this.grid1.ViewportChanging += new InfiniteGrid.CellEvent(this.grid1_ViewportChanging);
            this.grid1.SelectionChanged += new InfiniteGrid.CellEvent(this.grid1_SelectionChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 943);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(880, 914);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grid View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.quadView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(880, 914);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quad View";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 943);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private QuadView quadView1;
        private Grid grid1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;


    }
}

