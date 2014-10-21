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
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;
using Azmyth.Assets;
using Azmyth.XNA;

namespace XnaGUILib
{
    public class XGMiniMap : XGControl
    {
        public bool Fill { get; set; }
        public bool DrawTopBorder { get; set; }
        private int borderThickness = 2;
        protected Rectangle _top;
        private Rectangle left, right, bottom;

        private int m_tileSize = 3;
        private int m_offsetX = 0;
        private int m_offsetY = 0;
        World m_world = null;

        public XGMiniMap(Rectangle rectangle, World world, int tileSize)
            : base(rectangle, true)
        {
            m_world = world;
            m_tileSize = tileSize;

            

            Init();

            CenterTile(0, 0);
        }

        public XGMiniMap(Rectangle rectangle, bool canFocus)
            : base(rectangle, canFocus)
        {
            Init();
        }

        private void Init()
        {
            //BkgColor = new Color(0.0f, 0.0f, 0.0f, 0.5f); // transparent black
            Fill = true;
            DrawTopBorder = true;
        }

        public int BorderThickness
        {
            get { return borderThickness; }
            set { borderThickness = value; }
        }


        public override void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (hiliteTime > 0.0f)
                hiliteTime -= frameTime;

            m_offsetX += (int)(5 * InputManager.ThumbPosition(PlayerIndex.One, ThumbSticks.Right).X);
            m_offsetY -= (int)(5 * InputManager.ThumbPosition(PlayerIndex.One, ThumbSticks.Right).Y);

            if(InputManager.KeyPressed(Keys.Right))
            {
                m_offsetX++;
            }
            
            if(InputManager.KeyPressed(Keys.Left))
            {
                m_offsetX--;
            }

            if(InputManager.KeyPressed(Keys.Up))
            {
                m_offsetY--;
            }


            if (InputManager.KeyPressed(Keys.Down))
            {
                m_offsetY++;
            }

            base.Update(gameTime);
        }

        float hiliteTime = 0.0f;

        protected override void GetFocus()
        {
            hiliteTime = 0.1f;
        }

        public void CenterTile(int x, int y)
        {
            Rectangle rect = ToScreen(Rectangle);

            m_offsetX = x - ((Rectangle.Width / m_tileSize) / 2);
            m_offsetY = y - ((Rectangle.Height / m_tileSize) / 2);

            ////m_offsetX = (Rectangle.Width / m_tileSize) / 2;
            ////m_offsetY = (Rectangle.Height / m_tileSize) / 2;

            position.X = x;
            position.Y = y;
        }

        Vector2 position = Vector2.Zero;

        public override void Draw(float frameTime)
        {
            //if (Fill)
            //    XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(Rectangle), BkgColor);

            Color bColor = ThisControlColor;

            int totalCells = ((int)(Rectangle.Width / m_tileSize) * ((int)((Rectangle.Height) / m_tileSize)));

            for (int index = 0; index < totalCells; index++)
            {
                int cellX = (int)(index / (((Rectangle.Height) / m_tileSize)));
                int cellY = (int)(index % (((Rectangle.Height) / m_tileSize)));

                TerrainTypes tile = m_world.GetTerrainType(cellX + m_offsetX, cellY + m_offsetY);
       
                Rectangle bounds = new Rectangle((cellX * m_tileSize) + Rectangle.X, (cellY * m_tileSize) + (Rectangle.Y), (int)m_tileSize, (int)m_tileSize);

                switch (tile)
                {
                    case TerrainTypes.Ocean:
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Blue);
                        break;
                    case TerrainTypes.Dirt:
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Brown);
                        break;
                    case TerrainTypes.Stone:
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Gray);
                        break;
                    case TerrainTypes.Grass:
                        XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Green);
                        break;
                }

                if(cellX + m_offsetX == 0 && cellY + m_offsetY==0)
                    XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Black);

                if (cellX + m_offsetX == position.X && cellY + m_offsetY == position.Y)
                    XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bounds), Color.Black);
            }

           
            if (DrawTopBorder)
            {
                _top = new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, borderThickness);
                XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(_top), bColor);
            }

            left = new Rectangle(Rectangle.Left, Rectangle.Top + borderThickness, borderThickness, Rectangle.Height - (borderThickness * 2));
            right = new Rectangle(Rectangle.Right - borderThickness, Rectangle.Top + borderThickness, borderThickness, (Rectangle.Height - (borderThickness * 2)));
            bottom = new Rectangle(Rectangle.Left, Rectangle.Bottom - borderThickness, Rectangle.Width, borderThickness);

            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(left), bColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(right), bColor);
            XnaGUIManager.spriteBatch.Draw(XnaGUIManager.whiteTex, ToScreen(bottom), bColor);

            base.Draw(frameTime);
        }
    }
}
