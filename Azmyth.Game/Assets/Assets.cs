using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth;

namespace Azmyth.Assets
{
    public class Assets : AssetStore
    {
        public static Assets Store = new Assets();

        public Assets()
        {
            AddAssetType(typeof(World));
            AddAssetType(typeof(Area));
            AddAssetType(typeof(Room));
            AddAssetType(typeof(Life));
            AddAssetType(typeof(LifeCell));
        }

        public static VectorID CreateWorld()
        {
            return Store.CreateAsset(typeof(World));
        }

        public static VectorID CreateArea()
        {
            return Store.CreateAsset(typeof(Area));
        }

        public static VectorID CreateArea(int sizeX, int sizeY)
        {
            Area area = null;
            VectorID areaID = null;

            areaID = Store.CreateAsset(typeof(Area));

            area = Assets.GetArea(areaID);

            if (area != null)
            {
                area.Name = "Area " + areaID.ID;
                area.GridX = sizeX;
                area.GridY = sizeY;
            }

            return areaID;
        }

        public static VectorID CreateLife(int sizeX, int sizeY)
        {
            Life life = null;
            VectorID lifeID = null;

            lifeID = Store.CreateAsset(typeof(Life));

            life = (Life)Assets.Store[lifeID];

            if (life != null)
            {
                life.GridX = sizeX;
                life.GridY = sizeY;
                life.Name = "Life " + lifeID.ID;
            }

            return lifeID;
        }

        public static VectorID CreateArea(string name, int sizeX, int sizeY)
        {
            VectorID areaID = null;

            areaID = CreateArea(sizeX, sizeY);

            Assets.Store[areaID].Name = name;

            return areaID;
        }

        public static VectorID CreateRoom()
        {
            return Store.CreateAsset(typeof(Room));
        }

        public static World GetWorld(VectorID worldID)
        {
            return Store[worldID] as World;
        }

        public static Area GetArea(VectorID areaID)
        {
            return Store[areaID] as Area;
        }

        public static Room GetRoom(VectorID roomID)
        {
            return Store[roomID] as Room;
        }
    }
}
