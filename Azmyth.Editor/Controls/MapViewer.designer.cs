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
            this.tabMain = new Azmyth.Editor.FixedTabControl();
            this.pgeMap = new System.Windows.Forms.TabPage();
            this.mapArea = new Azmyth.Editor.MapGrid();
            this.pgeProperties = new System.Windows.Forms.TabPage();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtVector = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabMain.SuspendLayout();
            this.pgeMap.SuspendLayout();
            this.pgeProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabMain.Controls.Add(this.pgeMap);
            this.tabMain.Controls.Add(this.pgeProperties);
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(974, 611);
            this.tabMain.TabIndex = 0;
            // 
            // pgeMap
            // 
            this.pgeMap.Controls.Add(this.mapArea);
            this.pgeMap.Location = new System.Drawing.Point(4, 4);
            this.pgeMap.Name = "pgeMap";
            this.pgeMap.Padding = new System.Windows.Forms.Padding(3);
            this.pgeMap.Size = new System.Drawing.Size(966, 582);
            this.pgeMap.TabIndex = 0;
            this.pgeMap.Text = "Map";
            this.pgeMap.UseVisualStyleBackColor = true;
            // 
            // mapArea
            // 
            this.mapArea.BorderColor = System.Drawing.Color.Black;
            this.mapArea.CellHeight = 16;
            this.mapArea.CellWidth = 16;
            this.mapArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapArea.FullColor = System.Drawing.Color.Black;
            this.mapArea.GridColor = System.Drawing.Color.DarkGray;
            this.mapArea.HotTrack = true;
            this.mapArea.HoverColor = System.Drawing.Color.LightSteelBlue;
            this.mapArea.Location = new System.Drawing.Point(3, 3);
            this.mapArea.Margin = new System.Windows.Forms.Padding(4);
            this.mapArea.MaxZoom = 0F;
            this.mapArea.MinZoom = 0F;
            this.mapArea.Name = "mapArea";
            this.mapArea.SelectedColor = System.Drawing.Color.MidnightBlue;
            this.mapArea.SelectEmpty = true;
            this.mapArea.ShowGrid = false;
            this.mapArea.Size = new System.Drawing.Size(960, 576);
            this.mapArea.TabIndex = 0;
            this.mapArea.CellClick += new Azmyth.Editor.CellClickEvent(this.mapArea_CellClick);
            this.mapArea.HoveredCellChanged += new Azmyth.Editor.CellSelectEvent(this.mapArea_CellHover);
            // 
            // pgeProperties
            // 
            this.pgeProperties.Controls.Add(this.txtID);
            this.pgeProperties.Controls.Add(this.txtVector);
            this.pgeProperties.Controls.Add(this.label4);
            this.pgeProperties.Controls.Add(this.label2);
            this.pgeProperties.Controls.Add(this.txtHeight);
            this.pgeProperties.Controls.Add(this.txtWidth);
            this.pgeProperties.Controls.Add(this.txtName);
            this.pgeProperties.Controls.Add(this.label3);
            this.pgeProperties.Controls.Add(this.label1);
            this.pgeProperties.Location = new System.Drawing.Point(4, 4);
            this.pgeProperties.Name = "pgeProperties";
            this.pgeProperties.Padding = new System.Windows.Forms.Padding(3);
            this.pgeProperties.Size = new System.Drawing.Size(966, 582);
            this.pgeProperties.TabIndex = 1;
            this.pgeProperties.Text = "Properties";
            this.pgeProperties.UseVisualStyleBackColor = true;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(201, 25);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(64, 22);
            this.txtID.TabIndex = 15;
            // 
            // txtVector
            // 
            this.txtVector.Location = new System.Drawing.Point(74, 25);
            this.txtVector.Name = "txtVector";
            this.txtVector.Size = new System.Drawing.Size(64, 22);
            this.txtVector.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Vector ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "Height:";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(201, 113);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(64, 22);
            this.txtHeight.TabIndex = 11;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(74, 113);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(64, 22);
            this.txtWidth.TabIndex = 10;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(74, 70);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(191, 22);
            this.txtName.TabIndex = 9;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Width:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name:";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(966, 582);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMain);
            this.Name = "MapViewer";
            this.Size = new System.Drawing.Size(974, 611);
            this.tabMain.ResumeLayout(false);
            this.pgeMap.ResumeLayout(false);
            this.pgeProperties.ResumeLayout(false);
            this.pgeProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage pgeMap;
        private MapGrid mapArea;
        private System.Windows.Forms.TabPage pgeProperties;
        private System.Windows.Forms.TextBox txtVector;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtID;
        private FixedTabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
    }
}
