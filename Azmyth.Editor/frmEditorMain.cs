using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Azmyth;
using Azmyth.Assets;
using Azmyth.Math;
using Azmyth.Game;
using System.Runtime.InteropServices;

namespace Azmyth.Editor
{
    public enum PanelPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }
    public partial class frmEditorMain : Form
    {
        private TreeNode _root = null;
        private Asset _selected = null;
        private VectorID _worldID = null;

        private Ansi.AnsiToRtfBuilder _ansiBuilder = new Ansi.AnsiToRtfBuilder();

        public frmEditorMain()
        {
            Theme.ColorTable = new RibbonProfesionalRendererColorTableBlack();
            
            InitializeComponent();

            _root = tvwWorld.Nodes.Add("Assets");
            _worldID = Azmyth.Assets.Assets.CreateWorld();
            _root.Tag =  _worldID;

            int splitterPosition = this.gridProperties.GetInternalLabelWidth();
            this.gridProperties.MoveSplitterTo(splitterPosition + 200);

            pnlTop.Height = Properties.Settings.Default.TopPanelHeight;
            pnlLeft.Width = Properties.Settings.Default.LeftPanelWidth;
            pnlRight.Width = Properties.Settings.Default.RightPanelWidth;
            pnlBottom.Height = Properties.Settings.Default.BottomPanelHeight;

            ShowPanel(PanelPosition.Top, Properties.Settings.Default.TopPanelVisible);
            ShowPanel(PanelPosition.Bottom, Properties.Settings.Default.BottomPanelVisible);
            ShowPanel(PanelPosition.Left, Properties.Settings.Default.LeftPanelVisible);
            ShowPanel(PanelPosition.Right, Properties.Settings.Default.RightPanelVisible);

            Output("&B&+wInitializing...&N");
        }

        private void tvwWorld_AfterSelect(object sender, TreeViewEventArgs e)
        {
            VectorID id = e.Node.Tag as VectorID;

            gridProperties.SelectedObject = Azmyth.Assets.Assets.Store[id];
        }

        private void tvwWorld_DoubleClick(object sender, EventArgs e)
        {
            Area area = Azmyth.Assets.Assets.GetArea((VectorID)tvwWorld.SelectedNode.Tag);

            if(area != null)
            {
                tabMain.SelectedIndex = tabMain.TabPages.IndexOfKey(area.AssetID.ToString());
                
                _selected = area;
            }
        }

        private void btnNewArea_Click(object sender, EventArgs e)
        {
            VectorID areaID = null;
            MapViewer areaView = null;
  
            areaID = Azmyth.Assets.Assets.CreateArea(100, 100);

            areaView = new MapViewer(areaID);

            areaView.AreaNode = _root.Nodes.Add(Azmyth.Assets.Assets.Store[areaID].Name);
            areaView.AreaNode.Tag = areaID;

            areaView.Dock = DockStyle.Fill;

            tabMain.TabPages.Add(areaID.ToString(), Azmyth.Assets.Assets.Store[areaID].Name);
            tabMain.SelectedIndex = tabMain.TabPages.IndexOfKey(areaID.ToString());
            tabMain.SelectedTab.Controls.Add(areaView);

            areaView.AreaPage = tabMain.SelectedTab;

            _selected = Azmyth.Assets.Assets.Store[areaID];
        }

        //void areaView_CellHover(object sender, CellSelectedEventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke((MethodInvoker) delegate { areaView_CellHover(sender, e); });
        //    }
        //    else
        //    {
        //        int r = 10; // radius
        //        int ox = e.Vector.X, oy = e.Vector.Y; // origin
        //        Room room = null;

        //        if ((Control.MouseButtons & MouseButtons.Left) != 0)
        //        {
        //            switch (cboTool1.SelectedValue)
        //            {
        //                case "Fill":
        //                    switch (cboShape1.SelectedValue)
        //                    {
        //                        case "Pixel":
        //                            //if (_selected[new VectorID(e.Vector.X, e.Vector.Y)] != null)
        //                            {
        //                                VectorID roomID = Azmyth.Assets.Assets.CreateRoom();
        //                                room = Azmyth.Assets.Assets.GetRoom(roomID);
        //                                room.GridX = e.Vector.X;
        //                                room.GridY = e.Vector.Y;
        //                                _selected.AddObject(room);
        //                                room.Name = "Default Room";
        //                                tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
        //                                ((MapViewer)sender).SetCellColor(e.Vector, Color.Red);
        //                            }

        //                            break;
        //                        case "Circle":
        //                            for (int x = -r; x < r; x++)
        //                            {
        //                                int height = (int)System.Math.Sqrt(r * r - x * x);

        //                                for (int y = -height; y < height; y++)
        //                                {
        //                                    VectorID roomID = Azmyth.Assets.Assets.CreateRoom();
        //                                    room = Azmyth.Assets.Assets.GetRoom(roomID);
        //                                    room.GridX = e.Vector.X;
        //                                    room.GridY = e.Vector.Y;
        //                                    _selected.AddObject(room);
        //                                    room.Name = "Default Room";
        //                                    tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
        //                                    ((MapViewer)sender).SetCellColor(new Vector(x + ox, y + oy, 1), Color.Red);
        //                                    ((MapViewer)sender).RepaintMap();

        //                                }
        //                            }




        //                            break;
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //}

        //void areaView_CellClick(object sender, CellClickEventArgs e)
        //{
        //    int r = 10; // radius
        //    int ox = e.CellX, oy = e.CellY; // origin
        //    VectorID roomID = null;
        //    Room room = null;
        //    switch (cboTool1.SelectedValue)
        //    {
        //        case "Fill":
        //            switch (cboShape1.SelectedValue)
        //            {
        //                case "Pixel":
        //                    //if (_selected[new VectorID(e.CellX, e.CellY)] != null)
        //                    {
        //                        roomID = Azmyth.Assets.Assets.CreateRoom();
        //                        room = Azmyth.Assets.Assets.GetRoom(roomID);

        //                        room.GridX = e.CellX;
        //                        room.GridY = e.CellY;

        //                        _selected.AddObject(room);

        //                        room.Name = "Default Room";

        //                        tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
        //                        ((MapViewer)sender).SetCellColor(e.Cell.CellVector, Color.Red);
        //                    }

        //                    break;
        //                case "Circle":
        //                    for (int x = -r; x < r; x++)
        //                    {
        //                        int height = (int)System.Math.Sqrt(r * r - x * x);

        //                        for (int y = -height; y < height; y++)
        //                        {
        //                            roomID = Azmyth.Assets.Assets.CreateRoom();
        //                            room = Azmyth.Assets.Assets.GetRoom(roomID);
        //                            room.GridX = e.CellX;
        //                            room.GridY = e.CellY;
        //                            _selected.AddObject(room);
        //                            room.Name = "Default Room";
        //                            tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
        //                            ((MapViewer)sender).SetCellColor(new Vector(x + ox, y + oy, 1), Color.Red);

        //                        }
        //                    }

        //                    ((MapViewer)sender).RepaintMap();
        //                    this.Invalidate();

        //                    break;
        //            }
        //            break;
        //    }
        //}

        private void frmEditorMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        public void ShowPanel(PanelPosition position, bool show)
        {
            switch (position)
            {
                case PanelPosition.Top:
                    pnlTop.Visible = show;
                    splitTop.Visible = show;
                    Properties.Settings.Default.TopPanelVisible = show;
                    break;
                case PanelPosition.Bottom:
                    pnlBottom.Visible = show;
                    splitBottom.Visible = show;
                    Properties.Settings.Default.BottomPanelVisible = show;
                    break;
                case PanelPosition.Left:
                    pnlLeft.Visible = show;
                    splitLeft.Visible = show;
                    Properties.Settings.Default.LeftPanelVisible = show;
                    break;
                case PanelPosition.Right:
                    pnlRight.Visible = show;
                    splitRight.Visible = show;
                    Properties.Settings.Default.RightPanelVisible = show;
                    break;
            }

            Properties.Settings.Default.Save();
        }

        public void ShowPanel(PanelPosition position)
        {
            switch (position)
            {
                case PanelPosition.Top:
                    pnlTop.Visible = true;
                    splitTop.Visible = true;
                    Properties.Settings.Default.TopPanelVisible = true;
                    break;
                case PanelPosition.Bottom:
                    pnlBottom.Visible = true;
                    splitBottom.Visible = true;
                    Properties.Settings.Default.BottomPanelVisible = true;
                    break;
                case PanelPosition.Left:
                    pnlLeft.Visible = true;
                    splitLeft.Visible = true;
                    Properties.Settings.Default.LeftPanelVisible = true;
                    break;
                case PanelPosition.Right:
                    pnlRight.Visible = true;
                    splitRight.Visible = true;
                    Properties.Settings.Default.RightPanelVisible = true;
                    break;
            }

            Properties.Settings.Default.Save();
        }

        public void CollapsePanel(PanelPosition position)
        {
            switch (position)
            {
                case PanelPosition.Top:
                    pnlTop.Visible = false;
                    splitTop.Visible = false;
                    Properties.Settings.Default.TopPanelVisible = false;
                    break;
                case PanelPosition.Bottom:
                    pnlBottom.Visible = false;
                    splitBottom.Visible = false;
                    Properties.Settings.Default.BottomPanelVisible = false;
                    break;
                case PanelPosition.Left:
                    pnlLeft.Visible = false;
                    splitLeft.Visible = false;
                    Properties.Settings.Default.LeftPanelVisible = false;
                    break;
                case PanelPosition.Right:
                    pnlRight.Visible = false;
                    splitRight.Visible = false;
                    Properties.Settings.Default.RightPanelVisible = false;
                    break;
            }

            Properties.Settings.Default.Save();
        }

        private void splitTop_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default.TopPanelHeight = pnlTop.Height;
            Properties.Settings.Default.Save();
        }

        private void splitLeft_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default.LeftPanelWidth = pnlLeft.Width;
            Properties.Settings.Default.Save();
        }

        private void splitRight_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default.RightPanelWidth = pnlRight.Width;
            Properties.Settings.Default.Save();
        }

        private void splitBottom_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default.BottomPanelHeight = pnlBottom.Height;
            Properties.Settings.Default.Save();
        }

        private void pnlAssets_Close(object arg1, EventArgs arg2)
        {
            CollapsePanel(PanelPosition.Left);
        }

        private void btnAssetsShow_Click(object sender, EventArgs e)
        {
            tvwWorld.Visible = true;
            pnlAssets.Visible = true;
            pnlAssetsCollapsed.Visible = false;
            pnlLeft.Width = Properties.Settings.Default.LeftPanelWidth; 
            splitLeft.Visible = true;
        }

        private void pnlAssets_Minimize(object arg1, EventArgs arg2)
        {
            tvwWorld.Visible = false;
            pnlAssets.Visible = false;
            pnlAssetsCollapsed.Visible = true;
            pnlLeft.Width = 25;
            pnlAssetsCollapsed.Dock = DockStyle.Fill;
            splitLeft.Visible = false;
        }

        private void btnAssetsShow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(btnAssetsShow.BackColor), btnAssetsShow.ClientRectangle);
            g.DrawImage(btnAssetsShow.Image, new Point(2, 2));

            SizeF szF = g.MeasureString(btnAssetsShow.Text, btnAssetsShow.Font);
            g.TranslateTransform((float)btnAssetsShow.ClientRectangle.Width / (float)2 + (szF.Height - 2) / (float)2, 20);
            g.RotateTransform(90);

            g.DrawString(btnAssetsShow.Text, btnAssetsShow.Font, new SolidBrush(btnAssetsShow.ForeColor), 0, 0);
        }

        private void btnAssetsShow_MouseEnter(object sender, EventArgs e)
        {
            btnAssetsShow.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnAssetsShow_MouseLeave(object sender, EventArgs e)
        {
            btnAssetsShow.BackColor = SystemColors.Control;
        }

        private void btnPropertiesCollapsed_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(btnPropertiesShow.BackColor), btnPropertiesShow.ClientRectangle);
            g.DrawImage(btnPropertiesShow.Image, new Point(2, 2));

            SizeF szF = g.MeasureString(btnPropertiesShow.Text, btnPropertiesShow.Font);
            g.TranslateTransform((float)btnPropertiesShow.ClientRectangle.Width / (float)2 + (szF.Height - 2) / (float)2, 20);
            g.RotateTransform(90);

            g.DrawString(btnPropertiesShow.Text, btnPropertiesShow.Font, new SolidBrush(btnPropertiesShow.ForeColor), 0, 0);
        }

        private void btnPropertiesCollapsed_MouseLeave(object sender, EventArgs e)
        {
            btnPropertiesShow.BackColor = SystemColors.Control;
        }

        private void btnPropertiesCollapsed_MouseEnter(object sender, EventArgs e)
        {
            btnPropertiesShow.BackColor = SystemColors.GradientActiveCaption;
        }

        private void pnlProperties_Close(object arg1, EventArgs arg2)
        {
            CollapsePanel(PanelPosition.Right);
        }

        private void pnlProperties_Minimize(object arg1, EventArgs arg2)
        {
            gridProperties.Visible = false;
            pnlProperties.Visible = false;
            
            pnlPropertiesCollapsed.Visible = true;
            pnlPropertiesCollapsed.Dock = DockStyle.Fill;
            
            pnlRight.Width = 25;
            
            splitRight.Visible = false;
        }

        private void btnPropertiesShow_Click(object sender, EventArgs e)
        {
            gridProperties.Visible = true;
            pnlProperties.Visible = true;

            pnlPropertiesCollapsed.Visible = false;

            pnlRight.Width = Properties.Settings.Default.RightPanelWidth;

            splitRight.Visible = true;
        }

        private void splitPanelTop_Minimize(object sender, EventArgs e)
        {
            splitPanelTop.Visible = false;

            pnlTopCollapsed.Visible = true;
            //pnlTopCollapsed.Dock = DockStyle.Top;

            pnlTop.Height = 32;

            splitTop.Visible = false;
        }

        private void splitPanelTop_Close(object sender, EventArgs e)
        {
            CollapsePanel(PanelPosition.Top);
        }

        private void btnTopShow_Click(object sender, EventArgs e)
        {
            splitPanelTop.Visible = true;
            
            pnlTopCollapsed.Visible = false;

            pnlTop.Height = Properties.Settings.Default.TopPanelHeight;

            splitTop.Visible = true;
        }

        public void Output(string output)
        {
            _ansiBuilder.AppendLine("&B&+l[&N&B&+m " + DateTime.Now.ToLongTimeString() + "&B&+l ]&N " + output);

            txtOutput.Rtf = _ansiBuilder.ToString();
        }

        private void rbnMarkov_Click(object sender, EventArgs e)
        {
            frmMarkov frm = new frmMarkov();

            frm.Show();
        }
    }
}
