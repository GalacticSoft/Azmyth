using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Procedural;
using Azmyth;
using Azmyth.Assets;

namespace Azmyth.Mono
{
    public enum Quadrant
    {
        North = 0,
        NorthEast = 1,
        East = 2,
        SouthEast = 3,
        South = 4,
        SouthWest = 5,
        West = 6,
        NorthWest = 7,
        Max = 8
    }

    public class PlaneManager
    {
        Plane prime = new Plane(Planes.Prime);
        Plane[] planes = new Plane[4];

        //water = new TerrainChunk(m_world, 0, -500, 200);
        //earth = new TerrainChunk(m_world, 0, 500, 200);
        //air = new TerrainChunk(m_world,  -500, 0, 200);
        //fire = new TerrainChunk(m_world, 500, 0, 200);

        //plane5 = new TerrainChunk(m_world, -300, -300, 100);
        //plane6 = new TerrainChunk(m_world, 300,300, 100);
        //plane7 = new TerrainChunk(m_world, -300, 300, 100);
        //plane8 = new TerrainChunk(m_world, 300, -300, 100);

        public PlaneManager(int seed)
        {
            Planes[] p_list = { Planes.Earth, Planes.Air, Planes.Fire, Planes.Water };
            int[] x_offsets = {0, 500, 0, -500 };
            int[] y_offsets = {-500, 0, 500, 0 };

            ShuffleArray.Seed = seed;

            p_list = ShuffleArray.Shuffle2<Planes>(p_list);
            
            prime.LoadPlane(seed, 0, 0, 250);

            // Randomize Primary Planes
            System.Threading.Tasks.Parallel.For(0, 4, d =>
            {
                planes[d] = new Plane(p_list[d]);
                planes[d].IsVisible = true;
                //planes[d].chunk = new Assets.TerrainChunk(world, x_offsets[d], y_offsets[d], 200);
                planes[d].LoadPlane(seed >> d, x_offsets[d], y_offsets[d], 200);
            });
        }

        static Random _random = new Random();

        /*public static Planes[] ShufflePlanes(Planes[] arr)
        {
            List<KeyValuePair<int, Planes>> list = new List<KeyValuePair<int, Planes>>();
            // Add all strings from array
            // Add new random int each time
            foreach (Planes i in arr)
            {
                list.Add(new KeyValuePair<int, Planes>(_random.Next(), i));
            }

            // Sort the list by the random number
            var sorted = from item in list
		                orderby item.Key
		                select item;

            // Allocate new string array
            Planes[] result = new Planes[arr.Length];

            // Copy values to array
            int index = 0;

            foreach (KeyValuePair<int, Planes> pair in sorted)
            {
	            result[index] = pair.Value;
	            index++;
            }
            // Return copied array
            return result;
        }*/

        public Plane this[Planes plane]
        {
            get 
            {
                Plane p = null;

                switch(plane)
                {
                    case  Planes.Prime:
                        p = prime;
                        break;
                    default:
                        foreach(Plane p1 in planes)
                        {
                            if (p1 != null)
                            {
                                if (p1.m_plane == plane)
                                {
                                    p = p1;
                                    break;
                                }
                            }
                        }
                        break;
                }

                return p;
            }
        }
    }
}
