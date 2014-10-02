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
    
    public enum MapType
    {
        Simple,
        Textured,
        HeightMap,
        TempuratureMap
    }

    public class AzmythGame : Microsoft.Xna.Framework.Game
    {
        private int m_offsetY = 0;
        private int m_offsetX = 0;
        private int m_frameRate = 0;
        private int m_frameCounter = 0;

        private int m_cellSize = 64;

        Texture2D m_characterTexture;
        public World World = null;
        private SpriteFont m_spriteFont;
        private SpriteBatch m_spriteBatch;
        private GraphicsDeviceManager m_graphics;
        private TimeSpan m_elapsedTime = TimeSpan.Zero;
        private MouseState m_lastMouseState = Mouse.GetState();
        private KeyboardState m_lastKeyboardState = Keyboard.GetState();
        private Dictionary<TerrainTypes, Texture2D> m_cellColors = new Dictionary<TerrainTypes, Texture2D>();
        private Dictionary<TerrainTypes, List<Texture2D>> m_cellTextures = new Dictionary<TerrainTypes, List<Texture2D>>();

        private frmMainMenu m_mainMenu;
        private frmSettings m_settings;
        
        RandomNoise m_randomNoise = new RandomNoise(1);

        private MapType m_mapType = MapType.Textured;

        public MapType MapType
        {
            get { return m_mapType; }
            set { m_mapType = value; }
        }

        public AzmythGame()
        {
            //world = new World();

            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            foreach(TerrainTypes type in Enum.GetValues(typeof(TerrainTypes)))
            {
                m_cellTextures.Add(type, new List<Texture2D>());
            }
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
            m_settings = new frmSettings(this, m_graphics);

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
            m_spriteFont = Content.Load<SpriteFont>("Font");
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            
            LoadTextures();
            LoadSimpleTextures();
            

        }

        private void LoadTextures()
        {
            Texture2D texture;

            m_characterTexture = Content.Load<Texture2D>("Character1");

            texture = Content.Load<Texture2D>("Ocean");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtLeft");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtRight");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtTop");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtBottom");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtTopLeft");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtTopRight");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtBottomLeft");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtBottomRight");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtTopRightCorner");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtTopLeftCorner");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtBottomLeftCorner");
            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("WaterDirtBottomRightCorner");

            m_cellTextures[TerrainTypes.Water].Add(texture);
            texture = Content.Load<Texture2D>("Dirt1");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);
            texture = Content.Load<Texture2D>("Dirt2");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtLeft");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtRight");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtTop");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtBottom");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtTopRight");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtTopLeft");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtBottomRight");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtBottomLeft");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtTopRightCorner");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtTopLeftCorner");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtBottomRightCorner");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("ForestDirtBottomLeftCorner");
            m_cellTextures[TerrainTypes.Dirt].Add(texture);

            texture = Content.Load<Texture2D>("Stone1");
            m_cellTextures[TerrainTypes.Mountain].Add(texture);

            //texture = Content.Load<Texture2D>("Stone2");
            //_cellTextures[TerrainTypes.Stone].Add(texture);

            //texture = Content.Load<Texture2D>("Stone3");
            //_cellTextures[TerrainTypes.Stone].Add(texture);

            texture = Content.Load<Texture2D>("Forest1");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("Forest2");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneRight");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneLeft");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneTop");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneBottom");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneTopLeftCorner");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneTopRightCorner");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneBottomRightCorner");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneBottomLeftCorner");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneBottomRight");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneBottomLeft");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneTopRight");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("ForestStoneTopLeft");
            m_cellTextures[TerrainTypes.Grass].Add(texture);

            texture = Content.Load<Texture2D>("Sand");
            m_cellTextures[TerrainTypes.Sand].Add(texture);

            texture = Content.Load<Texture2D>("Lava");
            m_cellTextures[TerrainTypes.Lava].Add(texture);

            texture = Content.Load<Texture2D>("Ice");
            m_cellTextures[TerrainTypes.Ice].Add(texture);

            texture = Content.Load<Texture2D>("Snow");
            m_cellTextures[TerrainTypes.Snow].Add(texture);

            //texture = Content.Load<Texture2D>("StoneRock");
            //_cellTextures[TerrainTypes.StoneRock].Add(texture);

            //texture = Content.Load<Texture2D>("ForestRock");
            //_cellTextures[TerrainTypes.ForestRock].Add(texture);

            //texture = Content.Load<Texture2D>("DirtRock");
            //_cellTextures[TerrainTypes.DirtRock].Add(texture);
        }

        private void LoadSimpleTextures()
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Blue });
            m_cellColors.Add(TerrainTypes.Water, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Brown });
            m_cellColors.Add(TerrainTypes.Dirt, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
            m_cellColors.Add(TerrainTypes.Snow, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.SandyBrown });
            m_cellColors.Add(TerrainTypes.Sand, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Gray });
            m_cellColors.Add(TerrainTypes.Mountain, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Red });
            m_cellColors.Add(TerrainTypes.Lava, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });
            m_cellColors.Add(TerrainTypes.Black, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Yellow });
            m_cellColors.Add(TerrainTypes.City, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Cyan });
            m_cellColors.Add(TerrainTypes.Ice, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Green });
            m_cellColors.Add(TerrainTypes.Grass, texture);

            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.LightGray });
            //texture = Content.Load<Texture2D>("Rock");
            //_cellColors.Add(TerrainTypes.DirtRock, texture);
            //_cellColors.Add(TerrainTypes.StoneRock, texture);
           // _cellColors.Add(TerrainTypes.ForestRock, texture);
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
            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            m_elapsedTime += gameTime.ElapsedGameTime;

            if (m_elapsedTime > TimeSpan.FromSeconds(1))
            {
                m_elapsedTime -= TimeSpan.FromSeconds(1);
                m_frameRate = m_frameCounter;
                m_frameCounter = 0;
            }

            if(m_lastMouseState.LeftButton == ButtonState.Pressed && newMouseState.LeftButton == ButtonState.Released)
            {
                if (m_mainMenu.Visible == false && m_settings.Visible == false)
                {
                    if (m_mapType == XNA.MapType.Simple)
                        m_mapType = XNA.MapType.Textured;
                    else
                        m_mapType = XNA.MapType.Simple;
                }
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.Escape) && newKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (World != null)
                {
                    ShowMenu(!m_showMenu);
                }
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.Space) && newKeyboardState.IsKeyUp(Keys.Space))
            {
                space = true;
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.A) && newKeyboardState.IsKeyUp(Keys.A))
            {
                m_offsetX--;
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.D) && newKeyboardState.IsKeyUp(Keys.D))
            {
                m_offsetX++;
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.W) && newKeyboardState.IsKeyUp(Keys.W))
            {
                m_offsetY--;
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.S) && newKeyboardState.IsKeyUp(Keys.S))
            {
                m_offsetY++;
            }

            if (m_lastKeyboardState.IsKeyDown(Keys.OemTilde) && newKeyboardState.IsKeyUp(Keys.OemTilde))
            {
                Exit();
            }

            m_lastMouseState = newMouseState;
            m_lastKeyboardState = newKeyboardState;
            XnaGUIManager.Update(gameTime);

            base.Update(gameTime);
        }

        bool space = false;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Dictionary<Vector2, string> cityNames = new Dictionary<Vector2, string>();
           
            if (World != null)
            {
                m_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

                int cellX = GraphicsDevice.Viewport.X;
                int cellY = GraphicsDevice.Viewport.Y;

                int totalCells = (((GraphicsDevice.Viewport.Width / m_cellSize) + 1) * ((GraphicsDevice.Viewport.Height / m_cellSize) + 1));

                for (int index = 0; index < totalCells; index++)
                {
                    Room room = World.GetRoom(cellX + m_offsetX, cellY + m_offsetY);

                    if (cellX == (GraphicsDevice.Viewport.Width / m_cellSize) / 2 && cellY == (GraphicsDevice.Viewport.Height / m_cellSize) / 2)
                    {
                        if (space)
                        {
                            room.Terrain = TerrainTypes.Dirt;
                            World.SaveRoom(room);
                            space = false;
                        }
                    }

                    switch(m_mapType)
                    { 
                        case MapType.Textured:
                            DrawTexturedMap(m_spriteBatch, room, cellX, cellY);
                            break;
                        case MapType.Simple:
                            DrawSimpleMap(m_spriteBatch, room, cellX, cellY);
                            break;
                    }

                    if (cellX== (GraphicsDevice.Viewport.Width / m_cellSize) / 2 && cellY == (GraphicsDevice.Viewport.Height / m_cellSize) / 2)
                    {
                        m_spriteBatch.Draw(m_characterTexture, new Rectangle(cellX * m_cellSize, cellY * m_cellSize, m_cellSize, m_cellSize), Color.White);
                    }

                    if (room.Terrain == TerrainTypes.City)
                    {
                        cityNames.Add(new Vector2((cellX * m_cellSize) + m_cellSize, (cellY * m_cellSize) + m_cellSize), room.Name);
                    }

                    cellX++;

                    if (cellX > (GraphicsDevice.Viewport.Width / m_cellSize))
                    {
                        cellY++;
                        cellX = GraphicsDevice.Viewport.X;
                    }
                }

                foreach (Vector2 p in cityNames.Keys)
                {
                    m_spriteBatch.DrawString(m_spriteFont, cityNames[p], p, Color.Black);
                }

                m_frameCounter++;

                string fps = string.Format("fps: {0}", m_frameRate);

                m_spriteBatch.DrawString(m_spriteFont, fps, new Vector2(1, 1), Color.Black);
                m_spriteBatch.DrawString(m_spriteFont, fps, new Vector2(2, 2), Color.White);

                m_spriteBatch.End();
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

        public void DrawSimpleMap(SpriteBatch spriteBatch, Room room, int x, int y)
        {
            var colorPercent = room.Height / World.TerrainHeight;
            var rgb = (int)Math.Max(Math.Min(255 * Math.Abs(colorPercent), 255), 0);

            if (x == (GraphicsDevice.Viewport.Width / m_cellSize) / 2 && y == (GraphicsDevice.Viewport.Height / m_cellSize) / 2)
            {
                spriteBatch.Draw(m_cellColors[TerrainTypes.Black], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
            }
            else
            {
                double rand = m_randomNoise.GetValue(x + m_offsetX, y + m_offsetY); ;
                Color c = Color.Lerp(Color.SandyBrown, Color.White, colorPercent);

                switch (room.Terrain)
                {
                    case TerrainTypes.Water:
                        spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.FromNonPremultiplied(0, 0, 255 - rgb, 255));
                        break;
                    case TerrainTypes.Dirt:
                        spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
                        break;
                    case TerrainTypes.Grass:
                        if (rand < .33)
                            c = Color.Green;
                        else if (rand < .66)
                            c = Color.ForestGreen;
                        else
                            c = Color.DarkGreen;

                        spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
                        break;
                    case TerrainTypes.Mountain:
                        if (rand < .33)
                            c = Color.LightGray;
                        else if (rand < .66)
                            c = Color.Gray;
                        else
                            c = Color.DarkGray;

                        spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), c);
                        break;
                    default:
                        spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        break;
                }
            }
        }

        public void DrawTexturedMap(SpriteBatch spriteBatch, Room room, int x, int y)
        {
            Dictionary<Point, Room> neighbors;
       
            {
                double noise = m_randomNoise.GetValue(x + m_offsetX, y + m_offsetY);

                switch (room.Terrain)
                {
                    case TerrainTypes.Grass:
                        neighbors = GetNeighbors(room);

                        if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
                              && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain))
                        {
                            //Top Left Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
                                && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain))
                        {
                            //Top Right Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
                            && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain))
                        {
                            //Bottom Right Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
                            && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain))
                        {
                            //Bottom Left Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Mountain)
                        {
                            //Right
                            spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Mountain)
                        {
                            //Left
                            spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Top
                            spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Bottom
                            spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Bottom Right Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Bottom Left Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Bottom Left Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Mountain)
                        {
                            //Bottom Left Corner
                            spriteBatch.Draw(m_cellTextures[room.Terrain][13], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }

                        else
                            spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        
                            
//////////////////////////////////////////////////////////////////////

                       
                            break;
                    case TerrainTypes.Water:
                            neighbors = GetNeighbors(room);
                            if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
                                  && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt))
                            {
                                //Top Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
                                && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt))
                            {
                                //Top Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
                              && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt))
                            {
                                //Bottom Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
                                  && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt))
                            {
                                //Bottom Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }

                            else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Dirt)
                            {
                                //Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][1], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Dirt)
                            {
                                //Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX, room.GridY+1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX, room.GridY-1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Top Right Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Top Left Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Top Left Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Dirt)
                            {
                                //Top Left Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else
                                spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            break;
                    case TerrainTypes.Dirt:
                            neighbors = GetNeighbors(room);
                            if (HasForestOn3Sides(room, neighbors))
                            {
                                spriteBatch.Draw(m_cellTextures[room.Terrain][1], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
                             && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass))
                            {
                                //Top Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][6], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
                                  && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass))
                            {
                                //Top Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][7], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
                                  && (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass))
                            {
                                //Bottom Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][8], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if ((neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
                                  && (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass))
                            {
                                //Bottom Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][9], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }

                            else if (neighbors[new Point(room.GridX + 1, room.GridY)].Terrain == TerrainTypes.Grass)
                            {
                                //Right
                                spriteBatch.Draw(m_cellTextures[room.Terrain][2], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX - 1, room.GridY)].Terrain == TerrainTypes.Grass)
                            {
                                //Left
                                spriteBatch.Draw(m_cellTextures[room.Terrain][3], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Top
                                spriteBatch.Draw(m_cellTextures[room.Terrain][4], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Bottom
                                spriteBatch.Draw(m_cellTextures[room.Terrain][5], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX + 1, room.GridY - 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Top Right Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][10], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX - 1, room.GridY - 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Top Left Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][11], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX + 1, room.GridY + 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Bottom Right Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][12], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else if (neighbors[new Point(room.GridX - 1, room.GridY + 1)].Terrain == TerrainTypes.Grass)
                            {
                                //Bottom Left Corner
                                spriteBatch.Draw(m_cellTextures[room.Terrain][13], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                            }
                            else
                                spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);

                            break;
                    default:
                        if(m_cellTextures[room.Terrain].Count > 0)
                        {
                            spriteBatch.Draw(m_cellTextures[room.Terrain][0], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(m_cellColors[room.Terrain], new Rectangle(x * m_cellSize, y * m_cellSize, m_cellSize, m_cellSize), Color.White);
                        }
                        break;
                }
            }
        }

        private Dictionary<Point, Room> GetNeighbors(Room room)
        {
            Dictionary<Point, Room> neighbors = new Dictionary<Point, Room>();

            for(int x = room.GridX - 1; x <= room.GridX+1; x++)
            {
                for(int y = room.GridY - 1; y <= room.GridY+1; y++)
                {
                    if(!(x == room.GridX && y == room.GridY))
                    {
                        Point p = new Point(x, y);
                        neighbors.Add(p, World.GetRoom(x, y));
                    }
                }
            }

            return neighbors;
            
        }

        private bool HasForestOn3Sides(Room room, Dictionary<Point, Room> neighbors)
        {
            int count = 0;

            if(neighbors[new Point(room.GridX+1, room.GridY)].Terrain ==  TerrainTypes.Grass)
            {
                count++;
            }
            if(neighbors[new Point(room.GridX-1, room.GridY)].Terrain ==  TerrainTypes.Grass)
            {
                count++;
            }
            if (neighbors[new Point(room.GridX, room.GridY + 1)].Terrain == TerrainTypes.Grass)
            {
                count++;
            }
            if (neighbors[new Point(room.GridX, room.GridY - 1)].Terrain == TerrainTypes.Grass)
            {
                count++;
            }

            return count >= 3;
        }
    }
}
