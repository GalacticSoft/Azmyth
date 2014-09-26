﻿using System;
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
    public class MainMenu : XGControl
    {
        public AzmythGame Game;

        public XGPanel pnlMain { get; protected set; }
        public XGButton btnCreateWorld { get; protected set; }
        public XGButton btnSaveWorld { get; protected set; }
        public XGButton btnLoadWorld { get; protected set; }
        public XGButton btnSettings { get; protected set; }
        public XGButton btnExit { get; protected set; }

        public MainMenu(AzmythGame game)
            : base(new Rectangle(0, 0, 300, 300), true)
        {
            Game = game;

            Rectangle = new Rectangle((game.GraphicsDevice.Viewport.Width / 2)-150, (game.GraphicsDevice.Viewport.Height / 2) - 105, 300, 210);

            XnaGUIManager.Controls.Add(this);

            pnlMain = new XGPanel(new Rectangle(0, 0, 300, 210));
            btnCreateWorld = new XGButton(new Rectangle(10, 10, 280, 30), "Create World", this.btnCreateWorld_Clicked);
            btnSaveWorld = new XGButton(new Rectangle(10, 50, 280, 30), "Save World", this.btnCreateWorld_Clicked);
            btnLoadWorld = new XGButton(new Rectangle(10, 90, 280, 30), "Load World", this.btnCreateWorld_Clicked);
            btnSettings = new XGButton(new Rectangle(10, 130, 280, 30), "Settings", this.btnCreateWorld_Clicked);
            btnExit = new XGButton(new Rectangle(10, 170, 280, 30), "Exit", this.btnExit_Clicked);

            btnSaveWorld.Enabled = false;
            btnLoadWorld.Enabled = false;
            btnSettings.Enabled = false;

            Children.Add(pnlMain);
            pnlMain.Children.Add(btnCreateWorld);
            pnlMain.Children.Add(btnSaveWorld);
            pnlMain.Children.Add(btnLoadWorld);
            pnlMain.Children.Add(btnSettings);
            pnlMain.Children.Add(btnExit);
        }

        void btnCreateWorld_Clicked(XGControl sender)
        {
            VectorID worldID = Azmyth.Assets.Assets.CreateWorld();
            Game.World = Azmyth.Assets.Assets.GetWorld(worldID);

            //ToggleVisible();
        }

        void btnExit_Clicked(XGControl sender)
        {
            Game.Exit();
        }

        public void ToggleVisible()
        {
            Visible = !Visible;
            btnCreateWorld.Enabled = !btnCreateWorld.Enabled;
            btnExit.Enabled = !btnExit.Enabled;
            btnSaveWorld.Enabled = false;
            btnLoadWorld.Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((Game.GraphicsDevice.Viewport.Width / 2) - 150, (Game.GraphicsDevice.Viewport.Height / 2) - 105, 300, 210);
            base.Update(gameTime);
        }
    }
}
