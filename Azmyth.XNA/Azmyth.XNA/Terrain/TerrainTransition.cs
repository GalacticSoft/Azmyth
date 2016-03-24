using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Azmyth.Assets;

namespace Azmyth.XNA.Terrain
{
    public class TransitionTemplate
    {
        TerrainTypes[][] m_template;

        public TransitionTemplate(TerrainTypes[][] template)
        {
            m_template = template;
        }
    }
    public class TerrainTransition
    {
        private int m_order;
        
        private Texture2D m_texture;

        private TransitionTemplate m_template;

        public TerrainTransition(int order, Texture2D transition, TransitionTemplate template)
        {
            m_order = order;
            m_texture = transition;
            m_template = template;
        }
        
    }
}
