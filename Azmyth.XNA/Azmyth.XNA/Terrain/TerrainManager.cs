using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth;
using Azmyth.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Azmyth.XNA
{
    public class TerrainManager : Microsoft.Xna.Framework.GameComponent
    {
        private World m_world;
        private AzmythGame m_game;
        private SpriteFont m_spriteFont;
        private Texture2D m_borderPixel;
        private SpriteBatch m_spriteBatch;
        private GraphicsDevice m_graphicsDevice;
        private Rectangle m_chunkRectangle;
        private Dictionary<TerrainTypes, Texture2D> m_cellTextures;

        private float m_offsetY = 0;
        private float m_offsetX = 0;

        private int m_tileSize = 64;
        private int m_chunkSize = 15;

        public int ChunkSize
        {
            get 
            { 
                return m_chunkSize;
            }
            set
            {
                m_chunkSize = value;
            }
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
            }
        }

        private Viewport Viewport 
        { 
            get 
            { 
                return m_graphicsDevice.Viewport; 
            } 
        }

        public TerrainManager(AzmythGame game) : base(game)
        {
            m_game = game;
 
            m_cellTextures = new Dictionary<TerrainTypes, Texture2D>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            m_graphicsDevice = m_game.GraphicsDevice;
            m_spriteBatch = new SpriteBatch(m_graphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// Loads terrain tiles
        /// </summary>
        public void LoadContent()
        {
            Texture2D texture;

            texture = m_game.Content.Load<Texture2D>("Water/OceanBase");
            m_cellTextures.Add(TerrainTypes.Ocean, texture);

            texture = m_game.Content.Load<Texture2D>("Dirt/DirtBase");
            m_cellTextures.Add(TerrainTypes.Dirt, texture);

            texture = m_game.Content.Load<Texture2D>("Grass/GrassBase");
            m_cellTextures.Add(TerrainTypes.Grass, texture);

            texture = m_game.Content.Load<Texture2D>("Stone/StoneBase");
            m_cellTextures.Add(TerrainTypes.Stone, texture);
            m_cellTextures.Add(TerrainTypes.Road, texture);

            m_spriteFont = m_game.Content.Load<SpriteFont>("Fonts/Font");

            m_borderPixel = new Texture2D(m_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            m_borderPixel.SetData(new[] { Microsoft.Xna.Framework.Color.Red });
        }

        /// <summary>
        /// Updates chunk states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            UpdateChunks(m_game.PlayerManager.Position);

            CenterTile((int)m_game.PlayerManager.Position.X, (int)m_game.PlayerManager.Position.Y);
        }

        /// <summary>
        /// Draws the loaded chunks visible in the viewport
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            if (m_world != null)
            {
                System.Drawing.RectangleF viewportRect = ScreenToTileF(new System.Drawing.RectangleF(Viewport.X, Viewport.Y, Viewport.Width, Viewport.Height));

                List<TerrainChunk> chunks = m_world.GetChunks(viewportRect);

                m_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

                foreach (TerrainChunk chunk in chunks)
                {
                    List<TerrainTile> tiles = chunk.GetTiles(viewportRect);

                    foreach (TerrainTile t in tiles)
                    {
                        Rectangle tileRect = TileToScreen(t.Bounds);
                        //tileRect.Offset((m_tileSize / 2) * -1, (m_tileSize / 2) * -1);
                        m_spriteBatch.Draw(m_cellTextures[t.Terrain], tileRect, Color.White);

                        #if DEBUG
                            DrawBorder(tileRect, 1, Color.Black);
                        #endif
                    }

                    #if DEBUG
                        Rectangle chunkRect = TileToScreen(chunk.Bounds);
                        //chunkRect.Offset((m_tileSize / 2) * -1, (m_tileSize / 2) * -1);
                        string chunkString = "Chunk: (" + (chunk.Bounds.X / m_chunkSize) + "," + (chunk.Bounds.Y / m_chunkSize) + ")";

                        DrawBorder(chunkRect, 1, Color.Red);

                        m_spriteBatch.DrawString(m_spriteFont, chunkString, new Vector2(chunkRect.X + 5, chunkRect.Y), Color.Red);
                    #endif
                }

                m_spriteBatch.End();
            }
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

        /// <summary>
        /// Converts tile rectangle into screen position.
        /// </summary>
        /// <param name="tileRect"></param>
        public System.Drawing.RectangleF TileToScreenF(System.Drawing.RectangleF tileRect)
        {
            return new System.Drawing.RectangleF(
                ((tileRect.X + m_offsetX) * m_tileSize),
                ((tileRect.Y + m_offsetY) * m_tileSize),
                (tileRect.Width * m_tileSize),
                (tileRect.Height * m_tileSize));
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
                (float)screenRect.Width / (float)m_tileSize, 
                (float)screenRect.Height / (float)m_tileSize);
        }

        public int GetNodeCount()
        {
            return m_world.GetNodeCount();
        }

        public int GetChunkCount()
        {
            return m_world.GetChunkCount();
        }

        /// <summary>
        /// Converts screen rectangle into tiles
        /// </summary>
        /// <param name="screenRect"></param>
        public System.Drawing.RectangleF ScreenToTileF(System.Drawing.RectangleF screenRect)
        {
            return new System.Drawing.RectangleF(
                m_offsetX * -1,
                m_offsetY * -1,
                (float)screenRect.Width / (float)m_tileSize,
                (float)screenRect.Height / (float)m_tileSize);
        }

        /// <summary>
        /// Load a chunk at specified x and y position (in chunks)
        /// </summary>
        /// <param name="chunkX"></param>
        /// <param name="chunkY"></param>
        public void LoadChunk(float chunkX, float chunkY)
        {
            if (m_world != null)
            {
                System.Drawing.RectangleF chunkBounds;

                //Converts chunk bounds to tiles.
                chunkBounds = new System.Drawing.RectangleF(chunkX * m_chunkSize, chunkY * m_chunkSize, m_chunkSize, m_chunkSize);

                m_world.LoadChunk(chunkBounds);
            }
        }

        /// <summary>
        /// Will center terrain at specified coordinates. (in tiles)
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        public void CenterTile(float tileX, float tileY)
        {
            System.Drawing.RectangleF viewportRect = ScreenToTile(new Rectangle((int)Viewport.X, (int)Viewport.Y, (int)Viewport.Width, (int)Viewport.Height));

            m_offsetX = (int)((tileX * -1) + (viewportRect.Width / 2));
            m_offsetY = (int)((tileY * -1) + (viewportRect.Height / 2));
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            m_spriteBatch.Draw(m_borderPixel, 
                new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), 
                null, borderColor, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            // Draw left line
            m_spriteBatch.Draw(m_borderPixel, 
                new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), 
                null, borderColor, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            // Draw right line
            m_spriteBatch.Draw(m_borderPixel, 
                new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), 
                null, borderColor, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            // Draw bottom line
            m_spriteBatch.Draw(m_borderPixel, 
                new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), 
                null, borderColor, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public void UpdateChunks(Vector2 position)
        {
            if(m_world != null)
            {
                m_world.UpdateChunks((int)position.X, (int)position.Y, ChunkSize);

            }
            
            /*int chunkX = (int)Numbers.ConvertCoordinate(position.X, ChunkSize);
            int chunkY = (int)Numbers.ConvertCoordinate(position.Y, ChunkSize);

            List<Vector2> newChunks = new List<Vector2>()
            {
                new Vector2(chunkX - 1,  chunkY - 1),
                new Vector2(chunkX,      chunkY - 1),
                new Vector2(chunkX + 1,  chunkY - 1),
                new Vector2(chunkX - 1,  chunkY),
                new Vector2(chunkX,      chunkY),
                new Vector2(chunkX + 1,  chunkY),
                new Vector2(chunkX - 1,  chunkY + 1),
                new Vector2(chunkX,      chunkY + 1),
                new Vector2(chunkX + 1,  chunkY + 1)

            };

            if (m_world != null)
            {
                List<TerrainChunk> AllChunks = m_world.GetChunks();

                foreach (TerrainChunk chunk in AllChunks)
                {
                    if (!newChunks.Contains(new Vector2(chunk.Bounds.X, chunk.Bounds.Y)))
                    {
                        m_world.UnloadChunk(new System.Drawing.RectangleF(chunk.Bounds.X, chunk.Bounds.Y, m_chunkSize, m_chunkSize));
                    }
                }

                foreach (Vector2 chunk in newChunks)
                {
                    LoadChunk(chunk.X, chunk.Y);
                }
            }*/
            
        }
    }
}
