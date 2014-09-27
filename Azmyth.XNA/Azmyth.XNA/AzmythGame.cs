using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Azmyth.Assets;
using Azmyth;
using Azmyth.Procedural;
using XnaGUILib;

namespace Azmyth.XNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    
    public class AzmythGame : Microsoft.Xna.Framework.Game
    {
        private int offsetY = 0;
        private int offsetX = 0;
        private int frameRate = 0;
        private int frameCounter = 0;
       
        public World World = null;
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private KeyboardState oldState = Keyboard.GetState();
        private Dictionary<TerrainTypes, Texture2D> _textures = new Dictionary<TerrainTypes, Texture2D>();

        private frmMainMenu m_mainMenu;
        private frmSettings m_settings;
        public AzmythGame()
        {
            //world = new World();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            XnaGUIManager.Initialize(this);


            m_mainMenu = new frmMainMenu(this);
            m_settings = new frmSettings(this, graphics);

            ShowSettings(false);

            XGControl.BkgColor = Color.Black;
            XGControl.ControlColor = Color.Gray;
            XGControl.ForeColor = Color.White;

            this.IsMouseVisible = true; // display the GUI
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteFont = Content.Load<SpriteFont>("Font");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Blue});
            _textures.Add(TerrainTypes.Ocean, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Brown });
            _textures.Add(TerrainTypes.Dirt, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Blue});
            _textures.Add(TerrainTypes.River, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Brown  });
            _textures.Add(TerrainTypes.Mountain, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
            _textures.Add(TerrainTypes.Snow, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.SandyBrown });
            _textures.Add(TerrainTypes.Sand, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Gray});
            _textures.Add(TerrainTypes.Stone, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Red });
            _textures.Add(TerrainTypes.Lava, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });
            _textures.Add(TerrainTypes.Black, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Yellow });
            _textures.Add(TerrainTypes.City, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Cyan });
            _textures.Add(TerrainTypes.Ice, texture);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }
       
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            KeyboardState newState = Keyboard.GetState();

            if (oldState.IsKeyDown(Keys.Escape) && newState.IsKeyUp(Keys.Escape))
            {
                if (World != null)
                {
                    ShowMenu(!m_showMenu);
                }

            }

            if (oldState.IsKeyDown(Keys.Left) && newState.IsKeyUp(Keys.Left))
            {
                offsetX--;
            }

            if (oldState.IsKeyDown(Keys.Right) && newState.IsKeyUp(Keys.Right))
            {
                offsetX++;
            }

            if (oldState.IsKeyDown(Keys.Up) && newState.IsKeyUp(Keys.Up))
            {
                offsetY--;
            }

            if (oldState.IsKeyDown(Keys.Down) && newState.IsKeyUp(Keys.Down))
            {
                offsetY++;
            }

            if (oldState.IsKeyDown(Keys.OemTilde) && newState.IsKeyUp(Keys.OemTilde))
            {
                Exit();
            }


            //if(oldState.IsKeyDown(Keys.OemPlus))
            //{
            //    graphics.IsFullScreen = true;
            //    graphics.PreferredBackBufferHeight = 1080;
            //    graphics.PreferredBackBufferWidth = 1920;

            //     graphics.ApplyChanges();
            //}
            //else if(oldState.IsKeyDown(Keys.OemMinus))
            //{
            //    graphics.IsFullScreen = false;
            //    graphics.PreferredBackBufferHeight = 960;
            //    graphics.PreferredBackBufferWidth = 1280;

            //    graphics.ApplyChanges();
            //}

            oldState = newState;
            XnaGUIManager.Update(gameTime);
            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (World != null)
            {
                Dictionary<Vector2, string> cityNames = new Dictionary<Vector2, string>();
                
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,
                    Matrix.CreateTranslation(0, 0, 0));

                int cellX = GraphicsDevice.Viewport.X;
                int cellY = GraphicsDevice.Viewport.Y;

                int totalCells = ((GraphicsDevice.Viewport.Width / 24) * ((GraphicsDevice.Viewport.Height / 24) + 1));

                for (int index = 0; index < totalCells; index++)
                {
                    Room room = World.GetRoom(cellX + offsetX, cellY + offsetY);

                    if (cellX == (GraphicsDevice.Viewport.Width / 24) / 2 && cellY == (GraphicsDevice.Viewport.Height / 24) / 2)
                    {
                        spriteBatch.Draw(_textures[TerrainTypes.Black], new Rectangle(cellX * 24, cellY * 24, 24, 24), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_textures[room.Terrain], new Rectangle(cellX * 24, cellY * 24, 24, 24), Color.White);
                    }

                    if (room.Terrain == TerrainTypes.City)
                    {
                        cityNames.Add(new Vector2((cellX * 24) + 24, (cellY * 24) + 24), room.Name);
                    }

                    cellX++;

                    if (cellX > (GraphicsDevice.Viewport.Width / 24))
                    {
                        cellY++;
                        cellX = GraphicsDevice.Viewport.X;
                    }
                }

                foreach (Vector2 p in cityNames.Keys)
                {
                    spriteBatch.DrawString(spriteFont, cityNames[p], p, Color.Black);
                }

                frameCounter++;

                string fps = string.Format("fps: {0}", frameRate);

                spriteBatch.DrawString(spriteFont, fps, new Vector2(1, 1), Color.Black);
                spriteBatch.DrawString(spriteFont, fps, new Vector2(2, 2), Color.White);

                spriteBatch.End();
            }

            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            XnaGUIManager.Draw(frameTime);
            
            base.Draw(gameTime);
        }

        private bool m_showMenu = true;
        public void ShowMenu(bool blnShow)
        {
            m_showMenu = blnShow;

            if(blnShow)
            {
                if (m_mainMenu != null)
                    m_mainMenu.Show();
            }
            else
            {
                if (m_mainMenu != null)
                    m_mainMenu.Close();
            }
        }


        private bool m_showSettings = false;
        public void ShowSettings(bool blnShow)
        {
            m_showSettings = blnShow;

            if (blnShow)
            {
                if (m_settings != null)
                    m_settings.Show();
            }
            else
            {
                if (m_settings!= null)
                    m_settings.Close();
            }
        }
    }
}
