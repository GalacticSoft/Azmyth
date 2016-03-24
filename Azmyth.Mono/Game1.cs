#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Azmyth.Assets;
#endregion

namespace Azmyth.Mono
{



    /// <summary>
    /// This is the main type for your game
    /// </summary>
    ///
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch m_spriteBatch;
        //World m_world;

        PlaneManager m_planeManager;

        //TerrainChunk water;
        //TerrainChunk earth;
        //TerrainChunk air;
        //TerrainChunk fire;

        //TerrainChunk plane5;
        //TerrainChunk plane6;
        //TerrainChunk plane7;
        //TerrainChunk plane8;

        Texture2D m_dirt;
        Texture2D m_ocean;
        Texture2D m_fire;
        Texture2D m_air;

        private float m_offsetY = 0;
        private float m_offsetX = 0;

        private float m_tileSize = 16;
        private float m_zoom = 1.0f;

        public Game1()
            : base()
        {
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
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1024;   // set this value to the desired height of your window
            graphics.ApplyChanges();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_dirt = Content.Load<Texture2D>("DirtBase");
            m_ocean = Content.Load<Texture2D>("OceanBase");
            m_fire = Content.Load<Texture2D>("FireBase");
            m_air = Content.Load<Texture2D>("AirBase");

            m_planeManager = new PlaneManager(5555);

            //plane5 = new TerrainChunk(m_world, -300, -300, 100);
            //plane6 = new TerrainChunk(m_world, 300,300, 100);
            //plane7 = new TerrainChunk(m_world, -300, 300, 100);
            //plane8 = new TerrainChunk(m_world, 300, -300, 100);
            // Create a new SpriteBatch, which can be used to draw textures.

            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);


           // if (m_lastTime == null)
             //   m_lastTime = gameTime.TotalGameTime;

            if (InputManager.PadPressed(PlayerIndex.One, Buttons.Back) || InputManager.KeyReleased(Keys.OemTilde))
            {
                Exit();
            }

           // if (InputManager.KeyPressed(Keys.Escape) || InputManager.PadPressed(PlayerIndex.One, Buttons.Start))
           // {
                //m_stateManager.SetState(GameStates.MainMenu);
            //}

            
            m_tileSize += ((float)InputManager.MouseWheel());

            if (m_tileSize < 0)
                m_tileSize = 0;

            if (InputManager.KeyHeld(Keys.W) || InputManager.ThumbUpPressed(PlayerIndex.One, ThumbSticks.Left))
            {
                Vector2 v = Vector2.Transform(new Vector2(m_offsetX, m_offsetY), Matrix.CreateTranslation(0, -1, 0));

                m_offsetX = v.X;
                m_offsetY = v.Y;
            }

            if (InputManager.KeyHeld(Keys.S) || InputManager.ThumbDownPressed(PlayerIndex.One, ThumbSticks.Left))
            {
                Vector2 v = Vector2.Transform(new Vector2(m_offsetX, m_offsetY), Matrix.CreateTranslation(0, 1, 0));
                m_offsetX = v.X;
                m_offsetY = v.Y;
            }

            if (InputManager.KeyHeld(Keys.A) || InputManager.ThumbLeftPressed(PlayerIndex.One, ThumbSticks.Left))
            {
                Vector2 v = Vector2.Transform(new Vector2(m_offsetX, m_offsetY), Matrix.CreateTranslation(-1, 0, 0));
                m_offsetX = v.X;
                m_offsetY = v.Y;
            }

            if (InputManager.KeyHeld(Keys.D) || InputManager.ThumbRightPressed(PlayerIndex.One, ThumbSticks.Left))
            {
                Vector2 v = Vector2.Transform(new Vector2(m_offsetX, m_offsetY), Matrix.CreateTranslation(1, 0, 0));
                m_offsetX = v.X;
                m_offsetY = v.Y;
            }

            //m_elapsedTime += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (m_planeManager != null)
            {

                System.Drawing.RectangleF viewportRect = ScreenToTile(new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));


                m_spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

                if (m_tileSize <= 1)
                {
                    Texture2D circle = CreateCircle(250);

                    // Change Color.Red to the colour you want
                    m_spriteBatch.Draw(circle, new Vector2((-250 + (int)m_offsetX), (-250 + (int)m_offsetY)), Color.Red);
                }
                else
                {
                    for (Planes p = Planes.Prime; p <= Planes.Fire; p++)
                    {
                        Plane plane = m_planeManager[p];

                        if (plane != null)
                        {
                            if (plane.IsVisible)
                            {
                                List<TerrainTile> tiles = plane.chunk.GetTiles(viewportRect);

                                foreach (TerrainTile t in tiles)
                                {
                                    switch (t.Terrain)
                                    {
                                        case TerrainTypes.Ocean:
                                            switch (plane.m_plane)
                                            {
                                                case Planes.Prime:
                                                    m_spriteBatch.Draw(m_ocean, TileToScreen(t.Bounds), Color.White);
                                                    break;
                                                case Planes.Air:
                                                    m_spriteBatch.Draw(m_air, TileToScreen(t.Bounds), Color.White);
                                                    break;
                                                case Planes.Earth:
                                                    m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                                                    break;
                                                case Planes.Water:
                                                    m_spriteBatch.Draw(m_ocean, TileToScreen(t.Bounds), Color.White);
                                                    break;
                                                case Planes.Fire:
                                                    m_spriteBatch.Draw(m_fire, TileToScreen(t.Bounds), Color.White);
                                                    break;

                                            }
                                            break;
                                        default:
                                            m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                                            break;
                                    }

                                }
                            }
                        }
                    }
                }

                //foreach (TerrainChunk chunk in chunks)
                //{
                    

                    /*tiles = water.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                   //     switch (t.Terrain)
                   //     {
                            //case TerrainTypes.Ocean:
                                m_spriteBatch.Draw(m_ocean, TileToScreen(t.Bounds), Color.White);
                             //   break;
                            //default:
                             //   m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                             //   break;
                     //   }
                    }

                    tiles = earth.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                             switch (t.Terrain)
                           {
                        case TerrainTypes.Ocean:
                             //m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                           break;
                        default:
                           m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                           break;
                           }

                    }

                    tiles = air.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                             switch (t.Terrain)
                             {
                        case TerrainTypes.Ocean:
                            m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                           break;
                        default:
                           m_spriteBatch.Draw(m_air, TileToScreen(t.Bounds), Color.White);
                           break;
                           }

                    }

                    tiles = fire.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        switch (t.Terrain)
                             {
                                case TerrainTypes.Ocean:
                                m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                                break;
                                default:
                                m_spriteBatch.Draw(m_fire, TileToScreen(t.Bounds), Color.White);
                                break;
                             }
                    }

                    tiles = plane5.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        m_spriteBatch.Draw(m_ocean, TileToScreen(t.Bounds), Color.White);
                    }

                    tiles = plane6.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        m_spriteBatch.Draw(m_air, TileToScreen(t.Bounds), Color.White);
                    }

                    tiles = plane7.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        m_spriteBatch.Draw(m_fire, TileToScreen(t.Bounds), Color.White);
                    }

                    tiles = plane8.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        m_spriteBatch.Draw(m_dirt, TileToScreen(t.Bounds), Color.White);
                    }*/

               
#if DEBUG
                        //Rectangle rect = TileToScreen(chunk.Bounds);
                        //string chunkString = "Chunk: (" + (chunk.Bounds.X / m_chunkSize) + "," + (chunk.Bounds.Y / m_chunkSize) + ")";

                       // DrawBorder(rect, 1, Color.Red);

                       // m_spriteBatch.DrawString(m_spriteFont, chunkString, new Vector2(rect.X + 5, rect.Y), Color.Red);
#endif
               // }

                m_spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Converts tile rectangle into screen position.
        /// </summary>
        /// <param name="tileRect"></param>
        public Rectangle TileToScreen(System.Drawing.RectangleF tileRect)
        {
            return new Rectangle(
                (int)((tileRect.X + m_offsetX) * m_tileSize), 
                (int)((tileRect.Y + m_offsetY) * m_tileSize), 
                (int)(tileRect.Width * m_tileSize), 
                (int)(tileRect.Height * m_tileSize));
        }

        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// Converts screen rectangle into tiles
        /// </summary>
        /// <param name="screenRect"></param>
        public System.Drawing.RectangleF ScreenToTile(Rectangle screenRect)
        {
            return new System.Drawing.RectangleF(
                m_offsetX * -1, 
                m_offsetY * -1, 
                (float)screenRect.Width / m_tileSize, 
                (float)screenRect.Height / m_tileSize);
        }
    }
}
