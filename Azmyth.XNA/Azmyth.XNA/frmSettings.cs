using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using XnaGUILib;
using Azmyth;

namespace Azmyth.XNA
{
    /// <summary>
    /// Azmyth Main Menu
    /// </summary>
    public class frmSettings : XGControl
    {
        public AzmythGame Game;
        private GraphicsDeviceManager m_graphicsManager;

        public XGPanel pnlMain { get; protected set; }

        public XGListBox lstResolutions { get; protected set; }

        public XGCheckBox chkFullScreen { get; protected set; }

        public XGCheckBox chkSimpleMap { get; protected set; }

        public XGButton btnApply { get; protected set; }
        
        public XGButton btnExit { get; protected set; }

        public frmSettings(AzmythGame game, GraphicsDeviceManager graphics)
            : base(new Rectangle(0, 0, 300, 300), true)
        {
            Game = game;
            m_graphicsManager = graphics;

            Rectangle = new Rectangle((game.GraphicsDevice.Viewport.Width / 2)-150, (game.GraphicsDevice.Viewport.Height / 2) - 120, 300, 240);

            XnaGUIManager.Controls.Add(this);

            pnlMain = new XGPanel(new Rectangle(0, 0, 300, 240));
            lstResolutions = new XGListBox(new Rectangle(10, 10, 280, 150));

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if(mode.Width >= 800 && mode.Height >=600)
                lstResolutions.Items.Add(mode.Width + " x " + mode.Height);
            }

            lstResolutions.SelectedIndex = 0;
            chkFullScreen = new XGCheckBox(new Rectangle(10, 170, 25, 25), "Fullscreen");
            chkFullScreen.Checked = false;

            chkSimpleMap = new XGCheckBox(new Rectangle(150, 170, 25, 25), "Simple Map");
            chkSimpleMap.Checked = false;

            btnApply = new XGButton(new Rectangle(300-220, 200, 100, 30), "Apply", this.btnApply_Clicked);
            btnExit = new XGButton(new Rectangle(300-110, 200, 100, 30), "Exit", this.btnExit_Clicked);

            Children.Add(pnlMain);
            pnlMain.Children.Add(lstResolutions);
            pnlMain.Children.Add(chkFullScreen);
            pnlMain.Children.Add(chkSimpleMap);
            pnlMain.Children.Add(btnApply);
            pnlMain.Children.Add(btnExit);
        }

        void btnExit_Clicked(XGControl sender)
        {
            Game.ShowSettings(false);
            Game.ShowMenu(true);
        }

        public void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        public void btnApply_Clicked(XGControl sender)
        {
            Close();
            m_graphicsManager.IsFullScreen = chkFullScreen.Checked;

            m_graphicsManager.PreferredBackBufferWidth = int.Parse(lstResolutions.SelectedItem.Value.ToString().Split('x')[0].Trim());
            m_graphicsManager.PreferredBackBufferHeight = int.Parse(lstResolutions.SelectedItem.Value.ToString().Split('x')[1].Trim());
            m_graphicsManager.ApplyChanges();

            if (chkSimpleMap.Checked)
                Game.MapType = MapType.Simple;
            else
                Game.MapType = MapType.Textured;

            Show();
        }

        public void Close()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((Game.GraphicsDevice.Viewport.Width / 2) - 150, (Game.GraphicsDevice.Viewport.Height / 2) - 120, 300, 240);
            base.Update(gameTime);
        }
    }
}
