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
            this.SuspendLayout();
            // 
            // quadView1
            // 
            this.quadView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quadView1.Location = new System.Drawing.Point(0, 463);
            this.quadView1.Name = "quadView1";
            this.quadView1.QuadTree = null;
            this.quadView1.Size = new System.Drawing.Size(888, 184);
            this.quadView1.TabIndex = 1;
            // 
            // grid1
            // 
            this.grid1.BackColor = System.Drawing.Color.White;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grid1.GridColor = System.Drawing.Color.Black;
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.OriginColor = System.Drawing.Color.Black;
            this.grid1.QuadTree = null;
            this.grid1.SelectionColor = System.Drawing.Color.DarkRed;
            this.grid1.ShowOrigin = false;
            this.grid1.ShowSelection = true;
            this.grid1.Size = new System.Drawing.Size(888, 463);
            this.grid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 647);
            this.Controls.Add(this.quadView1);
            this.Controls.Add(this.grid1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Grid grid1;
        private QuadView quadView1;

    }
}

