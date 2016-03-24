namespace Azmyth.Editor
{
    partial class ColorTool
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
            // ColorTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ColorTool";
            this.Size = new System.Drawing.Size(766, 45);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Grid_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseUp);
            this.Resize += new System.EventHandler(this.ColorTool_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
