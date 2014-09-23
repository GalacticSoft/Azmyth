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

namespace Azmyth.XNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        int offsetY = 0;
        int offsetX = 0;
        World world;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<TerrainTypes, Texture2D> _textures = new Dictionary<TerrainTypes, Texture2D>();
        SpriteFont spriteFont;

        public Game1()
        {
            world = new World();

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteFont = Content.Load<SpriteFont>("Font");

            // Create a new SpriteBatch, which can be used to draw textures.
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
            texture.SetData(new Color[] { Color.Cyan });
            _textures.Add(TerrainTypes.Ice, texture);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        KeyboardState oldState = Keyboard.GetState();
        
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

            // Check to see whether the Spacebar is down.
            //if (newState.IsKeyDown(Keys.Space))
            //{
                // Key has just been pressed.
            //}
            // Otherwise, check to see whether it was down before.
            // (and therefore just released)
            if (oldState.IsKeyDown(Keys.Left) && newState.IsKeyUp(Keys.Left))
            {
                offsetX--;
            }

            if (oldState.IsKeyDown(Keys.Right) && newState.IsKeyUp(Keys.Right))
                offsetX++;
 
            if (oldState.IsKeyDown(Keys.Up) && newState.IsKeyUp(Keys.Up))
                offsetY--;

            if (oldState.IsKeyDown(Keys.Down) && newState.IsKeyUp(Keys.Down))
                offsetY++;
            
            if(oldState.IsKeyDown(Keys.OemPlus))
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferHeight = 1080;
                graphics.PreferredBackBufferWidth = 1920;

                 graphics.ApplyChanges();
            }
            else if(oldState.IsKeyDown(Keys.OemMinus))
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferHeight = 960;
                graphics.PreferredBackBufferWidth = 1280;

                graphics.ApplyChanges();
            }

            oldState = newState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, null,null,null,null,null,
                Matrix.CreateTranslation(0, 0, 0));

            int cellX = GraphicsDevice.Viewport.X;
            int cellY = GraphicsDevice.Viewport.Y;

            int totalCells = ((GraphicsDevice.Viewport.Width / 24) * ((GraphicsDevice.Viewport.Height / 24) + 1));
            
            for (int index = 0; index < totalCells; index++)
            {
                Room room = world.GetRoom(cellX + offsetX, cellY + offsetY);

                if (cellX == (GraphicsDevice.Viewport.Width / 24) / 2 && cellY == (GraphicsDevice.Viewport.Height / 24) / 2)
                    spriteBatch.Draw(_textures[TerrainTypes.Black], new Rectangle(cellX * 24, cellY * 24, 24, 24), Color.White);
                else
                    spriteBatch.Draw(_textures[room.m_terrain], new Rectangle(cellX * 24, cellY * 24, 24, 24), Color.White);
                   
                cellX++;

                if (cellX > (GraphicsDevice.Viewport.Width / 24))
                {
                    cellY++;
                    cellX = GraphicsDevice.Viewport.X;
                }
            }

            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.DrawString(spriteFont, fps, new Vector2(1, 1), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(3, 3), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
