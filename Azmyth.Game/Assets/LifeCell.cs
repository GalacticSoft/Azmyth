using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Assets;

namespace Azmyth.Assets
{
    public struct LifeCell
    {
        public int X;
        public int Y;
        public VectorID Life;
        public bool Alive;

        private List<LifeCell> GetNeighbors()
        {
            Life life = (Life)Assets.Store[Life];
            List<LifeCell> neighbors = new List<LifeCell>();

            try
            {

                if (life.LifeCells[X - 1, Y].Alive)
                {
                    neighbors.Add(life.LifeCells[X - 1, Y]);
                }

                if (life.LifeCells[X + 1, Y].Alive)
                {
                    neighbors.Add(life.LifeCells[X + 1, Y]);
                }

                if (life.LifeCells[X, Y - 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X, Y - 1]);
                }

                if (life.LifeCells[X, Y + 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X, Y + 1]);
                }

                if (life.LifeCells[X + 1, Y + 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X + 1, Y + 1]);
                }

                if (life.LifeCells[X - 1, Y - 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X - 1, Y - 1]);
                }

                if (life.LifeCells[X + 1, Y - 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X + 1, Y - 1]);
                }

                if (life.LifeCells[X - 1, Y + 1].Alive)
                {
                    neighbors.Add(life.LifeCells[X - 1, Y + 1]);
                }
            }
            catch
            {
                ;
            }
            return neighbors;
        }

        public int NeighborCount()
        {
            return GetNeighbors().Count;
        }

        public bool Update()
        {
            bool alive = Alive;
            List<LifeCell> neighbors = GetNeighbors();
            
            if(Alive)
            {
                if (neighbors.Count < 2)
                    alive = false;
                else if (neighbors.Count > 3)
                    alive = false;
            }
            else
            {
                if (neighbors.Count == 3)
                    alive = true;
            }

            return alive;
        }
    }
}
