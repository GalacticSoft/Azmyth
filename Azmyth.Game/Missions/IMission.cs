using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Game.Missions
{
    public enum MissionTypes
    {
        Rescue,
        Retreive,
        Defeat,
        Assist,
        Recon,
        Escort,
        Defend,
    }

    public static class MissionData
    {
        /*public static Dictionary<MissionTypes, Dictionary<MissionTypes, MissionTypes[]>> MissionProgression = new Dictionary<MissionTypes, Dictionary<MissionTypes, MissionTypes[]>>() 
        {
            {MissionTypes.Rescue, new Dictionary<  MissionTypes,  {MissionTypes.Assist, MissionTypes.Escort}},
            {MissionTypes.Assist, new MissionTypes[2] {MissionTypes.Retreive, MissionTypes.Recon}},
            {MissionTypes.Recon, new MissionTypes[2] {MissionTypes.Defeat, MissionTypes.Rescue}},
            {MissionTypes.Escort, new MissionTypes[2] {MissionTypes.Defeat, MissionTypes.Defend}},
            {MissionTypes.Defeat, new MissionTypes[2] {MissionTypes.Retreive, MissionTypes.Defend}},
            {MissionTypes.Retreive, new MissionTypes[2] {MissionTypes.Defend, MissionTypes.Escort}},
            {MissionTypes.Defend, new MissionTypes[2] {MissionTypes.Assist, MissionTypes.Recon}},
        };*/
        
    }
    
    public interface IMission
    {
        
    

        int Level { get; }

        MissionTypes MissionType
        {
            get;
        }

        string MissionOverview 
        { 
            get; 
        }

        string MissionComplete 
        { 
            get; 
        }

        void Generate();

        void Complete();

    }
}
