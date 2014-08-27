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

        private double m_coastLine = 0;
        private double m_shoreLine = 02;
        private double m_treeLine = 20;
        private double m_snowLine = 50;
        private double m_terrainHeight = 1024;

        public MapViewer Map {get; set;}
        private PropertyGrid m_parent;
        

        [Category("Terrain Values")]
        [Editor(typeof(CoastLineEditor), typeof(UITypeEditor))]
        public double CoastLine
        {
            get 
            { 
                return m_coastLine; 
            }
            set 
            { 
                m_coastLine = value;
                m_world.CoastLine = value;

                Map.RepaintMap();
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(ShoreLineEditor), typeof(UITypeEditor))]
        public double ShoreLine
        {
            get
            {
                return m_shoreLine;
            }
            set
            {
                m_coastLine = value;
                m_world.ShoreLine = value / 100;

                Map.RepaintMap();
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(TreeLineEditor), typeof(UITypeEditor))]
        public double TreeLine
        {
            get
            {
                return m_treeLine;
            }
            set
            {
                m_treeLine = value;
                m_world.TreeLine = value / 100;

                Map.RepaintMap();
            }
        }

        [Category("Terrain Values")]
        [Editor(typeof(SnowLineEditor), typeof(UITypeEditor))]
        public double SnowLine
        {
            get
            {
                return m_snowLine;
            }
            set
            {
                m_snowLine = value;
                m_world.SnowLine = value / 100;

                Map.RepaintMap();
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
        [Editor(typeof(TerrainHeightEditor), typeof(UITypeEditor))]
        public double TerrainHeight
        {
            get 
            { 
                return m_terrainHeight; 
            }
            set 
            {
                m_terrainHeight = value;
                m_coastLine = 0;
                m_world.CoastLine = 0;
                m_world.TerrainHeight = value;
                Map.RepaintMap(); 
            }
        }

        public WorldAdapter(PropertyGrid grid, World world)
        {
            m_parent = grid;
            m_world = world;
        }
    }
}
