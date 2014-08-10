﻿namespace InfiniteGrid
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
            this.grid1 = new InfiniteGrid.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.quadView2 = new InfiniteGrid.QuadView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gridControl1 = new InfiniteGrid.GridControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 647);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(880, 618);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grid View";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.grid1.ShowOrigin = false;
            this.grid1.ShowSelection = true;
            this.grid1.Size = new System.Drawing.Size(874, 612);
            this.grid1.TabIndex = 3;
            this.grid1.Zoom = 1F;
            this.grid1.HoverChanged += new InfiniteGrid.CellEvent(this.grid1_HoverChanged);
            this.grid1.ViewportChanged += new InfiniteGrid.CellEvent(this.grid1_ViewportChanged);
            this.grid1.ViewportChanging += new InfiniteGrid.CellEvent(this.grid1_ViewportChanging);
            this.grid1.SelectionChanged += new InfiniteGrid.CellEvent(this.grid1_SelectionChanged);
            this.grid1.DoubleClick += new System.EventHandler(this.grid1_DoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.quadView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(880, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quad View";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // quadView2
            // 
            this.quadView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quadView2.Location = new System.Drawing.Point(3, 3);
            this.quadView2.Name = "quadView2";
            this.quadView2.QuadTree = null;
            this.quadView2.Size = new System.Drawing.Size(874, 612);
            this.quadView2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.gridControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(880, 618);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // gridControl1
            // 
            this.gridControl1.BackColor = System.Drawing.Color.White;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 3);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(874, 612);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.Text = "gridControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 647);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
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
        private Grid grid1;
        private QuadView quadView2;
        private System.Windows.Forms.TabPage tabPage3;
        private InfiniteGrid.GridControl gridControl1;


    }
}

