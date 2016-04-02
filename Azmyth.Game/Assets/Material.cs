using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public enum MaterialType
    {
        None        = 0,
        Animal      = 1,
        Wood        = 2,
        Stone       = 3,
        Metal       = 4,
        Gemstone    = 5,
        Elemental   = 6,
        Max         = 6
    }

    public enum Materials
    {
        None        = 0,
        
        Cloth       = 1,
        Fur         = 2,
        Feather     = 3,
        Leather     = 4,
        Chitin      = 5,
        Bone        = 6,
        Scale       = 7,

        Leaf        = 8,
        Bark        = 9,
        Pine        = 10,
        Ash         = 11,
        Birch       = 12,
        Maple       = 13,
        Oak         = 14,

        Stone       = 15,
        Flint       = 16,
        Granite     = 17,
        Marble      = 18,
        Obsidian    = 19,
        Meteorite   = 20,
        Adamantium  = 21,

        Iron        = 22,
        Copper      = 23,
        Bronze      = 24,
        Silver      = 25,
        Gold        = 26,
        Platinum    = 27,
        Mithril     = 28,

        Pearl       = 29,
        Opal        = 30,
        Sapphire    = 31,
        Emerald     = 32,
        Ruby        = 33,
        Diamond     = 34,
        Electrum    = 35,

        Ice         = 36,
        Flame       = 37,
        Earth       = 38,
        Water       = 39,
        Air         = 40,
        Smoke       = 41,
        Electric    = 42,

        Max         = 42
    }

    public class ItemMaterial
    {
        public MaterialType MaterialType { get; set; }
        public Materials Material { get; set; }
        public string Name { get; set; }

        public float PriceMod { get; set; }
        public float DamageMod { get; set; }
        public float WeightMod { get; set; }
        public float DropMod { get; set; }


        public static Dictionary<MaterialType, float> MaterialTypeDropChances = new Dictionary<MaterialType, float>()
        {
            {   MaterialType.Animal,      0f        /* 35.41% chance of drop */ },
            {   MaterialType.Wood,        35.412f   /* 24.13% chance of drop */ },
            {   MaterialType.Stone,       59.5379f  /* 16.44% chance of drop */ },
            {   MaterialType.Metal,       75.97469f /* 11.20% chance of drop */ },
            {   MaterialType.Gemstone,    87.17295f /*  7.63% chance of drop */ },
            {   MaterialType.Elemental,   94.80223f /*  5.19% chance of drop */ },
        };

        public static Dictionary<Materials, ItemMaterial> MaterialList = new Dictionary<Materials, ItemMaterial>()
        {
                                    /* None                 Material Type,         Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.None,        new ItemMaterial() { MaterialType = MaterialType.None,    Material = Materials.None,    Name = "None",           PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,      DropMod = 0f     /* 100% chance of drop      */   }},

                                    /* Animal               Material Type,         Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.Cloth,       new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Cloth,     Name = "Cloth",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {Materials.Fur,         new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Fur,       Name = "Fur",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Feather,     new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Feather,  Name =  "Feather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Leather,     new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Leather,   Name = "Leather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Chitin,      new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Chitin,    Name = "Chitin",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Bone,        new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Bone,      Name = "Bone",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Scale,       new ItemMaterial() { MaterialType = MaterialType.Animal,   Material = Materials.Scale,     Name = "Scale",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

                                    /* Wood                 Material Type,         Material,           Name,      Price Mod, Damage Mod, Weight Mod,       Drop Mod */
            {Materials.Leaf,        new ItemMaterial() { MaterialType = MaterialType.Wood,     Material = Materials.Leaf,     Name = "Leaf",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {Materials.Bark,        new ItemMaterial() { MaterialType = MaterialType.Wood,    Material =  Materials.Bark,     Name = "Bark",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Pine,        new ItemMaterial() { MaterialType = MaterialType.Wood,    Material =  Materials.Pine,    Name =  "Pine",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Ash,         new ItemMaterial() { MaterialType = MaterialType.Wood,     Material = Materials.Ash,     Name =  "Ash",          PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Birch,       new ItemMaterial() { MaterialType = MaterialType.Wood,    Material =  Materials.Birch,   Name =  "Birch",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Maple,       new ItemMaterial() { MaterialType = MaterialType.Wood,     Material = Materials.Maple,   Name =  "Maple",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Oak,         new ItemMaterial() { MaterialType = MaterialType.Wood,     Material = Materials.Oak,     Name =  "Oak",          PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

                                    /* Stone                Material Type,         Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.Stone,       new ItemMaterial() { MaterialType = MaterialType.Stone,    Material = Materials.Stone,     Name = "Stone",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {Materials.Flint,       new ItemMaterial() { MaterialType = MaterialType.Stone,    Material = Materials.Flint,     Name = "Flint",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Granite,     new ItemMaterial() { MaterialType = MaterialType.Stone,   Material =  Materials.Granite,   Name = "Granite",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Marble,      new ItemMaterial() { MaterialType = MaterialType.Stone,   Material =  Materials.Marble,    Name = "Marble",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Obsidian,    new ItemMaterial() { MaterialType = MaterialType.Stone,   Material =  Materials.Obsidian,  Name = "Obsidian",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Meteorite,   new ItemMaterial() { MaterialType = MaterialType.Stone,  Material =   Materials.Meteorite, Name = "Meteorite",   PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Adamantium,  new ItemMaterial() { MaterialType = MaterialType.Stone,  Material =   Materials.Adamantium,Name = "Adamantium",  PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

                                    /* Metal                Material Type,         Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.Iron,        new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Iron,      Name = "Iron",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {Materials.Copper,      new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Copper,    Name = "Copper",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Bronze,      new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Bronze,    Name = "Feather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Silver,      new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Silver,    Name = "Leather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Gold,        new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Gold,      Name = "Chitin",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Platinum,    new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Platinum,  Name = "Bone",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Mithril,     new ItemMaterial() { MaterialType = MaterialType.Metal,   Material =  Materials.Mithril,  Name =  "Scale",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

                                    /* Gemstone             Material Type,         Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.Pearl,       new ItemMaterial() { MaterialType = MaterialType.Gemstone, Material = Materials.Pearl,     Name = "Pearl",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {Materials.Opal,        new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Opal,      Name = "Opal",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Sapphire,    new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Sapphire,  Name = "Sapphire",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Emerald,     new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Emerald,   Name = "Emerald",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Ruby,        new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Ruby,      Name = "Ruby",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Diamond,     new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Diamond,   Name = "Diamond",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Electrum,    new ItemMaterial() { MaterialType = MaterialType.Gemstone,Material =  Materials.Electrum,  Name = "Electrum",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

                                    /* Elemental            Material Type,          Material,            Name,     Price Mod, Damage Mod, Weight Mod, Drop Mod */
            {Materials.Ice,         new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Ice,       Name = "Ice",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,      DropMod =  0f        /* 31.14604% chance of drop */ }},
            {Materials.Flame,       new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Flame,     Name = "Flame",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {Materials.Earth,       new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Earth,     Name = "Earth",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {Materials.Water,       new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Water,     Name = "Water",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,      DropMod =  69.69341f /* 11.60998% chance of drop */ }},
            {Materials.Air,         new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Air,       Name = "Air",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {Materials.Smoke,       new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Smoke,     Name = "Smoke",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {Materials.Electric,    new ItemMaterial() { MaterialType = MaterialType.Elemental,Material =  Materials.Electric,  Name = "Electric",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},
        };
    }
}
