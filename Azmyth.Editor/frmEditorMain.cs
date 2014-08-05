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
        private VectorID _lifeID = null;
        private MapViewer _lifeView = null;
        LifeCell[,] newCells;

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
  
            areaID = Azmyth.Assets.Assets.CreateArea(int.Parse(txtWidth.TextBoxText), int.Parse(txtHeight.TextBoxText));

            areaView = new MapViewer(areaID);

            areaView.AreaNode = _root.Nodes.Add(Azmyth.Assets.Assets.Store[areaID].Name);
            areaView.AreaNode.Tag = areaID;

            areaView.Dock = DockStyle.Fill;
            areaView.CellClick += areaView_CellClick;
            areaView.CellHover += areaView_CellHover;

            tabMain.TabPages.Add(areaID.ToString(), Azmyth.Assets.Assets.Store[areaID].Name);
            tabMain.SelectedIndex = tabMain.TabPages.IndexOfKey(areaID.ToString());
            tabMain.SelectedTab.Controls.Add(areaView);

            areaView.AreaPage = tabMain.SelectedTab;

            _selected = Azmyth.Assets.Assets.Store[areaID];
        }

        void areaView_CellHover(object sender, CellSelectedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker) delegate { areaView_CellHover(sender, e); });
            }
            else
            {
                int r = 10; // radius
                int ox = e.Vector.X, oy = e.Vector.Y; // origin
                Room room = null;

                if ((Control.MouseButtons & MouseButtons.Left) != 0)
                {
                    switch (cboTool1.SelectedValue)
                    {
                        case "Fill":
                            switch (cboShape1.SelectedValue)
                            {
                                case "Pixel":
                                    //if (_selected[new VectorID(e.Vector.X, e.Vector.Y)] != null)
                                    {
                                        VectorID roomID = Azmyth.Assets.Assets.CreateRoom();
                                        room = Azmyth.Assets.Assets.GetRoom(roomID);
                                        room.GridX = e.Vector.X;
                                        room.GridY = e.Vector.Y;
                                        _selected.AddObject(room);
                                        room.Name = "Default Room";
                                        tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
                                        ((MapViewer)sender).SetCellColor(e.Vector, Color.Red);
                                    }

                                    break;
                                case "Circle":
                                    for (int x = -r; x < r; x++)
                                    {
                                        int height = (int)System.Math.Sqrt(r * r - x * x);

                                        for (int y = -height; y < height; y++)
                                        {
                                            VectorID roomID = Azmyth.Assets.Assets.CreateRoom();
                                            room = Azmyth.Assets.Assets.GetRoom(roomID);
                                            room.GridX = e.Vector.X;
                                            room.GridY = e.Vector.Y;
                                            _selected.AddObject(room);
                                            room.Name = "Default Room";
                                            tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
                                            ((MapViewer)sender).SetCellColor(new Vector(x + ox, y + oy, 1), Color.Red);
                                            ((MapViewer)sender).RepaintMap();

                                        }
                                    }




                                    break;
                            }
                            break;
                    }
                }
            }
        }

        void areaView_CellClick(object sender, CellClickEventArgs e)
        {
            int r = 10; // radius
            int ox = e.CellX, oy = e.CellY; // origin
            VectorID roomID = null;
            Room room = null;
            switch (cboTool1.SelectedValue)
            {
                case "Fill":
                    switch (cboShape1.SelectedValue)
                    {
                        case "Pixel":
                            //if (_selected[new VectorID(e.CellX, e.CellY)] != null)
                            {
                                roomID = Azmyth.Assets.Assets.CreateRoom();
                                room = Azmyth.Assets.Assets.GetRoom(roomID);

                                room.GridX = e.CellX;
                                room.GridY = e.CellY;

                                _selected.AddObject(room);

                                room.Name = "Default Room";

                                tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
                                ((MapViewer)sender).SetCellColor(e.Cell.CellVector, Color.Red);
                            }

                            break;
                        case "Circle":
                            for (int x = -r; x < r; x++)
                            {
                                int height = (int)System.Math.Sqrt(r * r - x * x);

                                for (int y = -height; y < height; y++)
                                {
                                    roomID = Azmyth.Assets.Assets.CreateRoom();
                                    room = Azmyth.Assets.Assets.GetRoom(roomID);
                                    room.GridX = e.CellX;
                                    room.GridY = e.CellY;
                                    _selected.AddObject(room);
                                    room.Name = "Default Room";
                                    tvwWorld.SelectedNode.Nodes.Add(room.Name).Tag = roomID;
                                    ((MapViewer)sender).SetCellColor(new Vector(x + ox, y + oy, 1), Color.Red);

                                }
                            }

                            ((MapViewer)sender).RepaintMap();
                            this.Invalidate();

                            break;
                    }
                    break;
            }
        }

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

        private void btnLife_Click(object sender, EventArgs e)
        {
            VectorID lifeID = Assets.Assets.CreateLife(100, 100);
            Life life = Assets.Assets.Store[lifeID] as Life;
            TreeNode lifeNode = tvwWorld.Nodes.Add(life.Name);
            MapViewer lifeView = new MapViewer(lifeID);

            _lifeID = lifeID;
            _lifeView = lifeView;
            _selected = Azmyth.Assets.Assets.Store[lifeID];

            lifeView.AreaNode = lifeNode;
            lifeView.AreaNode.Tag = lifeID;

            lifeView.Dock = DockStyle.Fill;
            lifeView.CellClick += lifeView_CellClick;
            lifeView.CellHover += lifeView_CellHover;

            tabMain.TabPages.Add(lifeID.ToString(), Azmyth.Assets.Assets.Store[lifeID].Name);
            tabMain.SelectedIndex = tabMain.TabPages.IndexOfKey(lifeID.ToString());
            tabMain.SelectedTab.Controls.Add(lifeView);

            lifeView.AreaPage = tabMain.SelectedTab;

            life.LifeCells = new LifeCell[life.GridX, life.GridY];
            newCells = new LifeCell[life.GridX, life.GridY];

            for (int index = 0; index < (life.GridX * life.GridY); index++)
            {
                int x = index / (int)life.GridX;
                int y = index % (int)life.GridX;

                LifeCell cell;

                cell.Life = _lifeID;
                cell.X = x;
                cell.Y = y;
                cell.Alive = false;

                newCells[x, y].X = x;
                newCells[x, y].Y = y;
                newCells[x, y].Life = _lifeID;
                newCells[x, y].Alive = false;

                life.LifeCells[x, y] = cell;
                lifeView.SetCellColor(new Vector(x, y, 0), Color.White);
                Application.DoEvents();
            }            
        }

        void lifeView_CellHover(object sender, CellSelectedEventArgs e)
        {
            Life life = Assets.Assets.Store[_lifeID] as Life;

            if (life.LifeCells[e.Vector.X, e.Vector.Y].Alive)
            {
                _lifeView.SetCellColor(e.Vector, Color.Black);
            }
            else
            {
                _lifeView.SetCellColor(e.Vector, Color.White);
            }
        }

        void lifeView_CellClick(object sender, CellClickEventArgs e)
        {
            Life life = Assets.Assets.Store[_lifeID] as Life;

            life.LifeCells[e.CellX, e.CellY].Alive = !life.LifeCells[e.CellX, e.CellY].Alive;

            if (life.LifeCells[e.CellX, e.CellY].Alive)
            {
                _lifeView.SetCellColor(e.Cell.CellVector, Color.Black);
            }
            else
            {
                _lifeView.SetCellColor(e.Cell.CellVector, Color.White);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCells();
           // Thread t = new Thread(() =>
            //{
                //Life life = Assets.Assets.Store[_lifeID] as Life;

                //while (true)
                //{
                //    DrawingControl.SuspendDrawing(_lifeView);

                //    _lifeView.ClearCells();
                //    LifeCell[,] newCells = new LifeCell[life.GridX, life.GridY];
                    
                //    for (int index = 0; index < (life.GridX * life.GridY); index++)
                //    {
                //        int x = index / (int)life.GridX;
                //        int y = index % (int)life.GridX;

                //        LifeCell cell = new LifeCell();

                //        cell.Life = _lifeID;
                //        cell.X = x;
                //        cell.Y = y;
                //        cell.Alive = life.LifeCells[x, y].Update();

                //        if (cell.Alive)
                //        {
                //            _lifeView.SetCellColor(new Vector(x, y, 0), Color.Black, true);
                //        }
                //        else
                //        {
                //            _lifeView.SetCellColor(new Vector(x, y, 0), Color.White, true);
                //        }

                //        newCells[x, y] = cell;
                //        //Application.DoEvents();
                //    }


                //    //_lifeView.RepaintMap();

                //    life.LifeCells = newCells;
                //    DrawingControl.ResumeDrawing(_lifeView);
                //    //Thread.Sleep(0);
                //    //Application.DoEvents();
                //}
            //});

            //t.Start();
        }

        public void UpdateCells()
        {
            Life life = Assets.Assets.Store[_lifeID] as Life;

            while (true)
            {
                LifeCell[,] newc = (LifeCell[,])newCells.Clone();
                DrawingControl.SuspendDrawing(_lifeView);

                var liveCells = from LifeCell item in life.LifeCells
                                where item.Update()
                                select item;

                _lifeView.ClearCells();

                foreach (LifeCell cell in liveCells)
                {
                    newc[cell.X, cell.Y].Alive = cell.Update();// life.LifeCells[cell.X, cell.Y].Update();

                    if (newc[cell.X, cell.Y].Alive)
                    {
                        _lifeView.SetCellColor(new Vector(cell.X, cell.Y, 0), Color.Black);
                    }
                    //else
                    //{
                    //    _lifeView.SetCellColor(new Vector(cell.X, cell.Y, 0), Color.White, true);
                    //}

                    //Application.DoEvents();
                }

                life.LifeCells = newc;
                
                DrawingControl.ResumeDrawing(_lifeView);
                _lifeView.RepaintMap();
                Application.DoEvents();
            }
        }
    }


    public class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(MapViewer parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(MapViewer parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }
}
