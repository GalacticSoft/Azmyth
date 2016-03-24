using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Game;

namespace Azmyth.Assets
{
    public static class Stats
    {
        public static VectorID Strength = new VectorID(0, 1);
        public static VectorID Constitution = new VectorID(0, 2);
        
        public static VectorID Agility = new VectorID(0, 3);
        public static VectorID Dexterity = new VectorID(0, 4);

        public static VectorID Wisdom = new VectorID(0, 5);
        public static VectorID Intelligence = new VectorID(0, 6);

        public static Dictionary<VectorID, Stat> Prototypes = new Dictionary<VectorID, Stat>()
        {
            {
                Strength, new Stat() 
                {   
                    ID = Strength, 
                    Abbreviation = "STR", 
                    Name = "Strength", 
                    Description="Strength affects your melee damage and carry weight.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
            {
                Constitution, new Stat() 
                {   
                    ID = Constitution, 
                    Abbreviation = "CON", 
                    Name = "Constitution", 
                    Description = "Constituion affects your hitpoints and resistance to damage.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
            {
                Agility, new Stat() 
                {   
                    ID = Agility, 
                    Abbreviation = "AGI", 
                    Name = "Agility", 
                    Description = "Agility affects how well you dodge and how quickly you can move.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
            {
                Dexterity, new Stat() 
                {   
                    ID = Dexterity, 
                    Abbreviation = "DEX", 
                    Name = "Dexterity", 
                    Description = "Dexterity affects your coordination and ranged attacks.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
            {
                Wisdom, new Stat() 
                {   
                    ID = Wisdom, 
                    Abbreviation = "WIS", 
                    Name = "Wisdom", 
                    Description = "Wisdom affects how quickly you can learn new abilities and earn experience.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
            {
                Intelligence, new Stat() 
                {   
                    ID = Wisdom, 
                    Abbreviation = "WIS", 
                    Name = "Wisdom", 
                    Description = "Intellengence affects how many abilities you can learn and how well you can use them.", 
                    BaseValue = 0,  
                    Modifier = 0,
                    MaxBase = 100,  
                    MaxMod = 100, 
                    MinBase = 0, 
                    MinMod = 0
                }
            },
        };
    }
}
