﻿using System;
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
            AddAssetType(typeof(TerrainTile));
            AddAssetType(typeof(City));
        }

        public static VectorID CreateWorld()
        {
            
            VectorID worldID = Store.CreateAsset(typeof(World));
            World world = Assets.GetWorld(worldID);
            world.Seed = new Random().Next(500, 9999);
            world.Name = "World " + world.AssetID.ID;

            return  worldID;
        }

        public static VectorID CreateCity()
        {
            return Store.CreateAsset(typeof(City));
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

        public static VectorID CreateArea(string name, int sizeX, int sizeY)
        {
            VectorID areaID = null;

            areaID = CreateArea(sizeX, sizeY);

            Assets.Store[areaID].Name = name;

            return areaID;
        }

        public static VectorID CreateRoom()
        {
            return Store.CreateAsset(typeof(TerrainTile));
        }

        public static World GetWorld(VectorID worldID)
        {
            return Store[worldID] as World;
        }

        public static Area GetArea(VectorID areaID)
        {
            return Store[areaID] as Area;
        }

        public static TerrainTile GetRoom(VectorID roomID)
        {
            return Store[roomID] as TerrainTile;
        }

        public static City GetCity(VectorID cityID)
        {
            return Store[cityID] as City;
        }
    }
}
