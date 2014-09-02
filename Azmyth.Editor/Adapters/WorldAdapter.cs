using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Assets;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Diagnostics;

namespace Azmyth.Editor
{
    public class WorldAdapter
    {
        private World m_world;

        private float m_coastLine = 0;
        private float m_shoreLine = 05;
        private float m_treeLine = 40;
        private float m_snowLine = 50;
        private float m_terrainHeight = 1024;

        [Browsable(false)]
        public MapViewer Map {get; set;}
        private PropertyGrid m_parent;
        

        public string Name
        {
            get 
            { 
                return m_world.Name;
            }
            set 
            {
                m_world.Name = value;
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(CoastLineEditor), typeof(UITypeEditor))]
        public float CoastLine
        {
            get 
            { 
                return m_coastLine; 
            }
            set 
            { 
                m_coastLine = value;
                m_world.CoastLine = value;

                if (Map != null)
                {
                    Map.RepaintMap();
                }
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(ShoreLineEditor), typeof(UITypeEditor))]
        public float ShoreLine
        {
            get
            {
                return m_shoreLine;
            }
            set
            {
                m_shoreLine = value;
                m_world.ShoreLine = value / 100;

                if (Map != null)
                {
                    Map.RepaintMap();
                }
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(TreeLineEditor), typeof(UITypeEditor))]
        public float TreeLine
        {
            get
            {
                return m_treeLine;
            }
            set
            {
                m_treeLine = value;
                m_world.TreeLine = value / 100;

                if (Map != null)
                {
                    Map.RepaintMap();
                }
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(SnowLineEditor), typeof(UITypeEditor))]
        public float SnowLine
        {
            get
            {
                return m_snowLine;
            }
            set
            {
                m_snowLine = value;
                m_world.SnowLine = value / 100;

                if (Map != null)
                {
                    Map.RepaintMap();
                }
            }
        }

        public VectorID WorldID
        {
            get { return m_world.AssetID; }
            set { m_world.AssetID = value; }
        }

        public int Seed
        {
            get { return m_world.Seed; }
            set { m_world.Seed = value; }
        }

        [Category("Terrain Values")]
        [Editor(typeof(HeightEditor), typeof(UITypeEditor))]
        public float TerrainHeight
        {
            get 
            { 
                return m_terrainHeight; 
            }
            set 
            {
                m_terrainHeight = value;
                m_world.TerrainHeight = value;

                if(Map != null)
                    Map.RepaintMap(); 
            }
        }

        public WorldAdapter(PropertyGrid grid, World world)
        {
            m_parent = grid;
            m_world = world;

            Seed = world.Seed;
            WorldID = world.AssetID;
            SnowLine = world.SnowLine * 100;
            TreeLine = world.TreeLine * 100;
            ShoreLine = world.ShoreLine * 100;
            TerrainHeight = world.TerrainHeight;
            CoastLine = world.CoastLine;

        }
    }
}
