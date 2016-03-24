using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Game;
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
    public class PlayerManager : Microsoft.Xna.Framework.GameComponent
    {
        private AzmythGame m_game = null;
        private SpriteBatch m_spriteBatch = null;
        private GraphicsDevice m_graphicsDevice = null;
        
        // Textures
        private Texture2D m_characterTexture = null;

        private Player m_player = new Player();
        private Vector2 m_position = Vector2.Zero;

        public PlayerManager(AzmythGame game) : base(game)
        {
            m_game = game;
            m_player.Bounds = new System.Drawing.RectangleF(0, 0, 1, 1);
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            m_characterTexture = m_game.Content.Load<Texture2D>("Character1");
        }

        public float GetChunkX(float chunkSize)
        {
            return Numbers.ConvertCoordinate(m_position.X, chunkSize);
        }

        public float GetChunkY(float chunkSize)
        {
            return Numbers.ConvertCoordinate(m_position.Y, chunkSize);
        }

        public Vector2 Position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

        public void SetPosition(int x, int y)
        {
            m_position.X = x;
            m_position.Y = y;

            m_player.Bounds = new System.Drawing.RectangleF(x, y, 1, 1);
        }

        public void Move(Directions direction)
        {
            switch(direction)
            {
                case Directions.North:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(0, -1, 0));
                    break;
                case Directions.Northeast:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(1, -1, 0));
                    break;
                case Directions.East:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(1, 0, 0));
                    break;
                case Directions.Northwest:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(-1, -1, 0));
                    break;
                case Directions.Southeast:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(1, 1, 0));
                    break;
                case Directions.South:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(0, 1, 0));
                    break;
                case Directions.Southwest:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(-1, 1, 0));
                    break;
                case Directions.West:
                    m_position = Vector2.Transform(m_position, Matrix.CreateTranslation(-1, 0, 0));
                    break;              
            }

            m_player.Bounds = new System.Drawing.RectangleF(m_position.X, m_position.Y, 1, 1);
        }

        Texture2D createCircleText(int radius)
        {
            Texture2D texture = new Texture2D(m_graphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        /// <summary>
        /// Draws the loaded chunks visible in the viewport
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            int centerX = 0;
            int centerY = 0;

            centerX = (m_graphicsDevice.Viewport.Width / m_game.TileSize) / 2;
            centerY = (m_graphicsDevice.Viewport.Height / m_game.TileSize) / 2;

            m_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));

            m_spriteBatch.Draw(createCircleText(2 * m_game.TileSize), new Rectangle((centerX) * (m_game.TileSize), (centerY) * (m_game.TileSize), 2*m_game.TileSize, 2*m_game.TileSize), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            m_spriteBatch.Draw(m_characterTexture, new Rectangle((centerX) * m_game.TileSize, (centerY) * m_game.TileSize, m_game.TileSize, m_game.TileSize), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
            m_spriteBatch.End();
        }

        /// <summary>
        /// Updates Player states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

        }
    }
}
