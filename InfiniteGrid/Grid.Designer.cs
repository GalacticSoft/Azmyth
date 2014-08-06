namespace InfiniteGrid
{
    partial class Grid
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
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DoubleBuffered = true;
            this.Name = "Grid";
            this.Size = new System.Drawing.Size(545, 370);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Grid_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseUp);
            this.Resize += new System.EventHandler(this.Grid_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
