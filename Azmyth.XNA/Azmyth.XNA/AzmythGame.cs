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
    public enum GameState
    {
        None,
        Loading,
        Intro,
        MainMenu,
        CreateWorld,
        Settings,
        Playing,

    }

    public enum MenuState
    {
        MainMenu,
        SystemMenu,
        NewWorld,
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AzmythGame : Microsoft.Xna.Framework.Game
    {
        public GameState State = GameState.None;
        private int m_frameRate = 0;
        private int m_frameCounter = 0;
        private World m_world = null;



        private frmMainMenu m_mainMenu;
        private frmSettings m_settings;
        private frmCreateWorld m_createWorld;

        //private frmRightPanel m_rightPanel;

        private SpriteFont m_spriteFont;
        private SpriteBatch m_spriteBatch;
        private Texture2D m_logoTexture;
        private Texture2D m_characterTexture;
        private TerrainManager m_terrainManager;
        private GraphicsDeviceManager m_graphics;
        private TimeSpan m_elapsedTime = TimeSpan.Zero;
        private RandomNoise m_randomNoise = new RandomNoise(1);
        private MouseState m_lastMouseState = Mouse.GetState();
        private KeyboardState m_lastKeyboardState = Keyboard.GetState();

        private int m_cellSize = 32;

        private TimeSpan m_lastTime;

        Song m_introSong;

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

       

        public AzmythGame()
        {
            State = GameState.Loading;

            m_graphics = new GraphicsDeviceManager(this);
            m_terrainManager = new TerrainManager(this);

            this.Components.Add(m_terrainManager);

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
            XnaGUIManager.spriteFont = Content.Load<SpriteFont>("Fonts/guiFont"); ;
           
            //this.IsFixedTimeStep = true;
            
            m_mainMenu = new frmMainMenu(this);
            m_settings = new frmSettings(this, m_graphics);
            m_createWorld = new frmCreateWorld(this);
            //m_rightPanel = new frmRightPanel(this, m_graphics);

            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            ShowMenu(false);
            ShowSettings(false);
            ShowCreateWorld(false);

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
            m_introSong = Content.Load<Song>("intro");

            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(m_introSong);

            m_spriteFont = Content.Load<SpriteFont>("Fonts/Font");
            m_characterTexture = Content.Load<Texture2D>("Character1");
            m_logoTexture = Content.Load<Texture2D>("logo");
            m_terrainManager.LoadContent();

            State = GameState.Intro;
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
            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            if(m_lastTime == null)
                m_lastTime = gameTime.TotalGameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.OemTilde) && newKeyboardState.IsKeyUp(Keys.OemTilde))
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

            switch(State)
            {
                case GameState.MainMenu:
                    ShowCreateWorld(false);
                    ShowSettings(false);
                    ShowMenu(true);

                    if (m_lastKeyboardState.IsKeyDown(Keys.Escape) && newKeyboardState.IsKeyUp(Keys.Escape))
                    {
                        State = GameState.Playing;
                    } 
                    break;
                case GameState.Intro:
                    if (m_lastKeyboardState.IsKeyDown(Keys.Enter) && newKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        State = GameState.MainMenu;
                    } 

                    if(World == null || gameTime.TotalGameTime.Subtract(m_lastTime).Seconds >= 5)
                    {
                        World = new World(new VectorID(1, 1), new Random().Next(500, 9999));

                        m_terrainManager.LoadChunk(0, 0);

                        m_terrainManager.ShowGrid = false;

                        m_lastTime = gameTime.TotalGameTime;
                    }
                    break;
                case GameState.Settings:
                    ShowCreateWorld(false);
                    ShowSettings(true);
                    ShowMenu(false);
                    break;
                case GameState.CreateWorld:
                    ShowCreateWorld(true);
                    ShowSettings(false);
                    ShowMenu(false);
                    break;
                case GameState.Playing:
                    ShowCreateWorld(false);
                    ShowSettings(false);
                    ShowMenu(false);
                    
                    if (m_lastKeyboardState.IsKeyDown(Keys.Escape) && newKeyboardState.IsKeyUp(Keys.Escape))
                    {
                        State = GameState.MainMenu;
                    }    
                    break;
            }

            m_lastMouseState = newMouseState;
            m_lastKeyboardState = newKeyboardState;

            XnaGUIManager.Update(gameTime);

            base.Update(gameTime);
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

            switch (State)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Intro:
                    m_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

                    m_spriteBatch.Draw(m_logoTexture, new Rectangle(50, 10, GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 20), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    
                    m_spriteBatch.End();
                    break;
                case GameState.CreateWorld:
                    break;
                case GameState.Playing:
                    string fps = string.Format("fps: {0}", m_frameRate);

                    m_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

                    m_spriteBatch.DrawString(m_spriteFont, fps, new Vector2(1, 1), Color.Black);
                    m_spriteBatch.DrawString(m_spriteFont, fps, new Vector2(2, 2), Color.White);

                    m_spriteBatch.Draw(m_characterTexture, new Rectangle((centerX) * m_cellSize, (centerY) * m_cellSize, m_cellSize, m_cellSize), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                    m_spriteBatch.End();
                    break;
            }

            m_frameCounter++;
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

        private bool m_showCreateWorld = false;
        public void ShowCreateWorld(bool blnShow)
        {
            m_showCreateWorld = blnShow;

            if (blnShow)
            {
                if (m_createWorld != null)
                    m_createWorld.Show();
            }
            else
            {
                if (m_createWorld != null)
                    m_createWorld.Close();
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
