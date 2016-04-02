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
using Azmyth;
using Azmyth.Assets;
using Azmyth.Procedural;
using XnaGUILib;

namespace Azmyth.XNA
{

    public enum GameStates
    {
        None = 0,
        Loading = 1,
        Intro = 2,
        MainMenu = 3,
        CreateWorld = 4,
        CreatePlayer = 5,
        Settings = 6,
        Playing = 7,
        Max = 8,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AzmythGame : Microsoft.Xna.Framework.Game
    {
        private PlayerManager m_playerManager = null;
        private World m_world = null;


        private int m_cellSize = 64;
        private int m_frameRate = 0;
        private int m_frameCounter = 0;

        public StateMachine<GameStates> m_stateManager;

        //Forms
        private frmMainMenu m_mainMenu;
        private frmSettings m_settings;
        private frmCreateWorld m_createWorld;
        private XGMiniMap m_miniMap;
        //private frmRightPanel m_rightPanel;

        //Components
        private TerrainManager m_terrainManager;
        private GraphicsDeviceManager m_graphics;

        //Sprites
        private SpriteFont m_spriteFont;
        private SpriteBatch m_spriteBatch;

        //Textures
        private Texture2D m_logoTexture;
        private Texture2D m_characterTexture;

        //Songs
        private Song m_introSong;

        //Timespans
        private TimeSpan m_lastTime;
        private TimeSpan m_elapsedTime = TimeSpan.Zero;

        private RandomNoise m_randomNoise = new RandomNoise(1);

        public SpriteBatch SpriteBatch
        {
            get { return m_spriteBatch; }
        }

        public World World
        {
            get
            {
                return m_world;
            }
            set
            {
                m_world = value;
                m_terrainManager.World = value;
            }
        }

        public TerrainManager TerrainManager
        {
            get 
            { 
                return m_terrainManager; 
            }
            set 
            { 
                m_terrainManager = value; 
            }
        }

        public PlayerManager PlayerManager
        {
            get
            {
                return m_playerManager;
            }
            set
            {
                m_playerManager = value;
            }
        }

        public int TileSize
        {
            get
            {
                return m_cellSize;
            }
            set
            {
                m_cellSize = value;
            }
        }

        public AzmythGame()
        {
            m_graphics = new GraphicsDeviceManager(this);

            m_stateManager = new StateMachine<GameStates>();
            m_stateManager.SetState(GameStates.Loading);

            m_playerManager = new PlayerManager(this);
            m_terrainManager = new TerrainManager(this);

            Components.Add(m_playerManager);
            Components.Add(m_terrainManager);

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

            InputManager.Initialize();

            XnaGUIManager.Initialize(this);
            XnaGUIManager.spriteFont = Content.Load<SpriteFont>("Fonts/guiFont");

            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            
            this.IsMouseVisible = true; // display the GUI
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           // m_introSong = Content.Load<Song>("intro");

            //MediaPlayer.IsRepeating = true;

            //MediaPlayer.Play(m_introSong);

            m_spriteFont = Content.Load<SpriteFont>("Fonts/Font");
            m_characterTexture = Content.Load<Texture2D>("Character1");
            m_logoTexture = Content.Load<Texture2D>("logo");

            m_playerManager.LoadContent();

            m_terrainManager.LoadContent();

            m_stateManager.SetState(GameStates.Intro);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }
       
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputManager.Update(gameTime);


            if(m_lastTime == null)
                m_lastTime = gameTime.TotalGameTime;

            if (InputManager.PadPressed(PlayerIndex.One, Buttons.Back) || InputManager.KeyReleased(Keys.OemTilde))
            {
                Exit();
            }

            m_elapsedTime += gameTime.ElapsedGameTime;

            if (m_elapsedTime > TimeSpan.FromSeconds(1))
            {
                m_elapsedTime -= TimeSpan.FromSeconds(1);
                m_frameRate = m_frameCounter;
                m_frameCounter = 0;
            }

            switch(m_stateManager.State)
            {
                case GameStates.Intro:
                    ShowMiniMap(false);
                    ShowCreateWorld(false);
                    ShowSettings(false);
                    ShowMenu(false);

                    if (InputManager.AnyKeyPressed() || InputManager.PadPressed(PlayerIndex.One, Buttons.Start) || InputManager.PadPressed(PlayerIndex.One, Buttons.A))
                    {
                        m_stateManager.SetState(GameStates.MainMenu);
                    }

                    if (World == null || gameTime.TotalGameTime.Subtract(m_lastTime).Seconds >= 5)
                    {
                        World = new World(new VectorID(1, 1), new Random().Next(500, 9999));

                        m_terrainManager.UpdateChunks(new Vector2(0, 0));

                        m_lastTime = gameTime.TotalGameTime;
                    }
                    break;
                case GameStates.MainMenu:
                    ShowMiniMap(false);
                    ShowCreateWorld(false);
                    ShowSettings(false);
                    ShowMenu(true);

                    if (InputManager.PadPressed(PlayerIndex.One, Buttons.B))
                    {
                        m_stateManager.PrevState();
                    }

                    if (InputManager.KeyPressed(Keys.Escape) || InputManager.PadPressed(PlayerIndex.One, Buttons.Start))
                    {
                        if(m_stateManager.LastState == GameStates.Playing)
                            m_stateManager.SetState(GameStates.Playing);
                    }
                    break;
                case GameStates.Settings:
                    ShowMiniMap(false);
                    ShowCreateWorld(false);
                    ShowMenu(false);
                    ShowSettings(true);

                    if(InputManager.PadPressed(PlayerIndex.One, Buttons.B))
                    {
                        m_stateManager.PrevState();
                    }
                    break;
                case GameStates.CreateWorld:
                    ShowSettings(false);
                    ShowMenu(false);
                    ShowCreateWorld(true);

                    if (InputManager.PadPressed(PlayerIndex.One, Buttons.B))
                    {
                        m_stateManager.PrevState();
                    }
                    break;
                case GameStates.CreatePlayer:
                    ShowSettings(false);
                    ShowMenu(false);
                    ShowCreateWorld(false);

                    if (InputManager.PadPressed(PlayerIndex.One, Buttons.B))
                    {
                        m_stateManager.PrevState();
                    }
                    break;
                case GameStates.Playing:
                    ShowCreateWorld(false);
                    ShowSettings(false);
                    ShowMenu(false);
                    ShowMiniMap(true);

                    XnaGUIManager.Activate(false);

                    if (InputManager.KeyPressed(Keys.Escape) || InputManager.PadPressed(PlayerIndex.One, Buttons.Start))
                    {
                        m_stateManager.SetState(GameStates.MainMenu);
                    }

                    if (InputManager.KeyPressed(Keys.W) || InputManager.ThumbUpPressed(PlayerIndex.One, ThumbSticks.Left))
                    {
                        m_playerManager.Move(Directions.North);
                    }

                    if (InputManager.KeyPressed(Keys.S) || InputManager.ThumbDownPressed(PlayerIndex.One, ThumbSticks.Left))
                    {
                        m_playerManager.Move(Directions.South);
                    }

                    if (InputManager.KeyPressed(Keys.A) || InputManager.ThumbLeftPressed(PlayerIndex.One, ThumbSticks.Left))
                    {
                        m_playerManager.Move(Directions.West);
                    }

                    if (InputManager.KeyPressed(Keys.D) || InputManager.ThumbRightPressed(PlayerIndex.One, ThumbSticks.Left))
                    {
                        m_playerManager.Move(Directions.East);
                    }

                    m_playerManager.Update(gameTime);

                    m_terrainManager.Update(gameTime);

                    m_miniMap.CenterTile((int)m_playerManager.Position.X, (int)m_playerManager.Position.Y);
                    
                    break;
            }

            XnaGUIManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int centerX = 0;
            int centerY = 0;
            int width = (GraphicsDevice.Viewport.Width / m_cellSize) + 1;
            int height = (GraphicsDevice.Viewport.Height / m_cellSize) + 1;
            int totalCells = width * height;
            
            GraphicsDevice.Clear(Color.Gray);

            centerX = (GraphicsDevice.Viewport.Width / m_cellSize) / 2;
            centerY = (GraphicsDevice.Viewport.Height / m_cellSize) / 2;

            m_terrainManager.Draw(gameTime);

            m_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

            switch (m_stateManager.State)
            {
                case GameStates.Playing:
                    m_playerManager.Draw(gameTime);
                    break;
            }
#if DEBUG
            string debug = "";
            debug = string.Format("fps: {0}, Nodes: {1}, Chunks: {2}, Tiles: {3}", m_frameRate, m_terrainManager.GetNodeCount(), m_terrainManager.GetChunkCount(), m_terrainManager.GetChunkCount() * m_terrainManager.ChunkSize * m_terrainManager.ChunkSize);
            m_spriteBatch.DrawString(m_spriteFont, debug, new Vector2(1, 1), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, debug, new Vector2(2, 0), Color.White);

            debug = string.Format("Seed: {0}, X: {1}, Y: {2}, Chunk X: {3}, Chunk Y: {4}", m_world.Seed, m_playerManager.Position.X, m_playerManager.Position.Y, m_playerManager.GetChunkX(m_terrainManager.ChunkSize).ToString(), m_playerManager.GetChunkY(m_terrainManager.ChunkSize).ToString());
            m_spriteBatch.DrawString(m_spriteFont, debug, new Vector2(1, GraphicsDevice.Viewport.Height - 24), Color.Black);
            m_spriteBatch.DrawString(m_spriteFont, debug, new Vector2(2, GraphicsDevice.Viewport.Height - 25), Color.White);
#endif
            switch (m_stateManager.State)
            {
                case GameStates.MainMenu:
                    break;
                case GameStates.Intro:
                    m_spriteBatch.Draw(m_logoTexture, new Rectangle(50, 10, GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 20), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    break;
                case GameStates.CreateWorld:
                    break;
            }

            m_spriteBatch.End();

            m_frameCounter++;
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            XnaGUIManager.Draw(frameTime);
            
            base.Draw(gameTime);
        }

        public void ShowMenu(bool blnShow)
        {
            if(blnShow)
            {
                if (m_mainMenu == null)
                {
                    m_mainMenu = new frmMainMenu(this);

                    XGControl.BkgColor = Color.Black;
                    XGControl.ControlColor = Color.Gray;
                    XGControl.ForeColor = Color.White;

                    XnaGUIManager.Activate(true);
                }
            }
            else
            {
                if (m_mainMenu != null)
                {
                    XnaGUIManager.Controls.Remove(m_mainMenu);

                    m_mainMenu = null;
                }
            }
        }

        public void ShowSettings(bool blnShow)
        {
            if (blnShow)
            {
                if (m_settings == null)
                {
                    m_settings = new frmSettings(this, m_graphics);

                    XGControl.BkgColor = Color.Black;
                    XGControl.ControlColor = Color.Gray;
                    XGControl.ForeColor = Color.White;

                    //XnaGUIManager.Activate(true);
                }
            }
            else
            {
                if (m_settings != null)
                {
                    XnaGUIManager.Controls.Remove(m_settings);

                    m_settings = null;
                }
            }
        }

        public void ShowCreateWorld(bool blnShow)
        {
            if (blnShow)
            {
                if (m_createWorld == null)
                {
                    m_createWorld = new frmCreateWorld(this);

                    XGControl.BkgColor = Color.Black;
                    XGControl.ControlColor = Color.Gray;
                    XGControl.ForeColor = Color.White;

                }
            }
            else
            {
                if (m_createWorld != null)
                {
                    XnaGUIManager.Controls.Remove(m_createWorld);

                    m_createWorld = null;
                    
                }
            }
        }

        public void ShowMiniMap(bool blnShow)
        {
            if (blnShow)
            {
                if (m_miniMap == null)
                {
                    m_miniMap = new XGMiniMap(new Rectangle(this.GraphicsDevice.Viewport.Width - 210, 10, 200, 200), this.World, 3);

                    XnaGUIManager.Controls.Add(m_miniMap);

                    XGControl.BkgColor = Color.Black;
                    XGControl.ControlColor = Color.Gray;
                    XGControl.ForeColor = Color.White;
                }
            }
            else
            {
                if (m_miniMap != null)
                {
                    XnaGUIManager.Controls.Remove(m_miniMap);

                    m_miniMap = null;

                }
            }
        }

        //public void DrawSimpleMap(SpriteBatch spriteBatch, TerrainTile room, int x, int y)
        //{
        //    var colorPercent = room.Height / World.TerrainHeight;
        //    var rgb = (int)Math.Max(Math.Min(255 * Math.Abs(colorPercent), 255), 0);

        //    if (x == (GraphicsDevice.Viewport.Width / m_cellSize) / 2 && y == (GraphicsDevice.Viewport.Height / m_cellSize) / 2)
        //    {
        //        spriteBatch.Draw(m_cellColors[TerrainTypes.Black], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //    }
        //    else
        //    {
        //        double rand = m_randomNoise.GetValue(x + m_offsetX, y + m_offsetY); ;
        //        Color c = Color.Lerp(Color.SandyBrown, Color.White, colorPercent);

        //        switch (room.Terrain)
        //        {
        //            case TerrainTypes.Water:
        //                spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.FromNonPremultiplied(0, 0, 255 - rgb, 255));
        //                break;
        //            case TerrainTypes.Dirt:
        //                spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
        //                break;
        //            case TerrainTypes.Grass:
        //                if (rand < .33)
        //                    c = Color.Green;
        //                else if (rand < .66)
        //                    c = Color.ForestGreen;
        //                else
        //                    c = Color.DarkGreen;

        //                spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
        //                break;
        //            case TerrainTypes.Stone:
        //                if (rand < .33)
        //                    c = Color.LightGray;
        //                else if (rand < .66)
        //                    c = Color.Gray;
        //                else
        //                    c = Color.DarkGray;

        //                spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
        //                break;
        //            default:
        //                spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                break;
        //        }
        //    }
        //}
        //public void DrawTexturedMap(TerrainTile room, int x, int y)
        //{
        //    switch (room.Terrain)
        //    {
        //        default:
        //            if (m_cellTextures[room.Terrain].Count > 0)
        //            {
        //                m_spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //            }
        //            else
        //            {
        //                m_spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //            }
        //            break;
        //    }
        //}


        //public void DrawFancyMap(SpriteBatch spriteBatch, Room room, int x, int y)
        //{
        //    List<Room> neighbors;
       
        //    {
        //        double noise = m_randomNoise.GetValue(x + m_offsetX, y + m_offsetY);

        //        switch (room.Terrain)
        //        {
        //            case TerrainTypes.Grass:
        //                neighbors = GetNeighbors(room);

        //                if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
        //                      && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain))
        //                {
        //                    //Top Left Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
        //                        && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain))
        //                {
        //                    //Top Right Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
        //                    && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain))
        //                {
        //                    //Bottom Right Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
        //                    && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain))
        //                {
        //                    //Bottom Left Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Right
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Left
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Top
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Bottom
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Bottom Right Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Bottom Left Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Bottom Left Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
        //                {
        //                    //Bottom Left Corner
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][13], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }

        //                else
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        
        //                    break;
        //            case TerrainTypes.Water:
        //                    //neighbors = m_roomBuffer[room];
        //                    neighbors = GetNeighbors(room);
        //                    if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
        //                          && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt))
        //                    {
        //                        //Top Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
        //                        && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt))
        //                    {
        //                        //Top Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
        //                      && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt))
        //                    {
        //                        //Bottom Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
        //                          && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt))
        //                    {
        //                        //Bottom Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }

        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][1], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX, room.GridY+1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX, room.GridY-1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Top Right Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Top Left Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Top Left Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
        //                    {
        //                        //Top Left Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    break;
        //            case TerrainTypes.Dirt:
        //                //neighbors = m_roomBuffer[room];
        //                    neighbors = GetNeighbors(room);
        //                    if (HasForestOn3Sides(room, neighbors))
        //                    {
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][1], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
        //                     && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass))
        //                    {
        //                        //Top Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
        //                          && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass))
        //                    {
        //                        //Top Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
        //                          && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass))
        //                    {
        //                        //Bottom Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
        //                          && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass))
        //                    {
        //                        //Bottom Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }

        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Right
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Left
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Top
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Bottom
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Top Right Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Top Left Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Bottom Right Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Grass)
        //                    {
        //                        //Bottom Left Corner
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][13], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                    }
        //                    else
        //                        spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);

        //                    break;
        //            default:
        //                if(m_cellTextures[room.Terrain].Count > 0)
        //                {
        //                    spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                else
        //                {
        //                    spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
        //                }
        //                break;
        //        }
        //    }
        //}
    }
}
