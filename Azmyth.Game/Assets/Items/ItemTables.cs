using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public static class ItemTables
    {
        public static readonly float[] QualityDropChances = new float[(int)Quality.Max]
        {
            0f,  /* Poor 20% chance of drop */ 
            20f, /* Average 20% chance of drop */
            40f, /* Good 20% chance of drop */
            60f, /* Excellent 20% chance of drop */
            80f  /* Suerior 20% chance of drop */
        };

        public static readonly float[] MaterialTypeDropChances = new float[(int)MaterialClass.Max]
        {
           -1f,        /* MaterialType.None, No Chance of Drop */
            0f,        /* MaterialType.Animal 35.41% chance of drop */ 
            35.412f,   /* MaterialType.Wood 24.13% chance of drop */ 
            59.5379f,  /* MaterialType.Stone 16.44% chance of drop */ 
            75.97469f, /* MaterialType.Metal 11.20% chance of drop */ 
            87.17295f, /* MaterialType.Gemstone 7.63% chance of drop */ 
            94.80223f, /* MaterialType.Elemental 5.19% chance of drop */ 
        };

        public static readonly float[] RarityDropChances = new float[(int)Rarity.Max]
        {
           -1f,        /* Rarity.None, No Chance of Drop */
            0f,        /* Rarity.Common 35.41% chance of drop */ 
            35.412f,   /* Rarity.UNcommon 24.13% chance of drop */ 
            59.5379f,  /* Rarity.Rare 16.44% chance of drop */ 
            75.97469f, /* Rarity.Legendary 11.20% chance of drop */ 
            87.17295f, /* Rarity.Epic 7.63% chance of drop */ 
            94.80223f, /* Rarity.Mythic 5.19% chance of drop */ 
        };

        public static readonly float[] ItemTypeDropChances = new float[(int)ItemClass.Max]
        {
           -1f,     /* None, no chance */
            0f,     /* Armor 12.5% chance */
            12.5f,  /* Clothing 12.5% chance */
            25f,    /* Container 12.5% chance */
            37.5f,  /* Jewelry 12.5% chance */
            50f,    /* Light 12.5% chance */
            62.5f,  /* Magical 12.5% chance */
            75f,    /* Resource 12.5% chance */
            87.5f,  /* Weapon 12.5% chance */
        };

        public static readonly List<ItemType> ItemTypes = new List<ItemType>()
        { 
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Cap|Hat|Hood",                                                        WearLocation = WearLocation.Head,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Shirt|Blouse|Robe",                                                   WearLocation = WearLocation.Body,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Cape|Cloak",                                                          WearLocation = WearLocation.Shoulders,    Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Belt|Sash",                                                           WearLocation = WearLocation.Waist,        Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Gloves|Wraps",                                                        WearLocation = WearLocation.Hands,        Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Bracers",                                                             WearLocation = WearLocation.Wrists,       Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Pants|Pantaloons",                                                    WearLocation = WearLocation.Legs,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Clothing,    Names = "Shoes|Sandals",                                                       WearLocation = WearLocation.Feet,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },

            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Helm|Great helm|Mail Coif|Sallet",                                    WearLocation = WearLocation.Head,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Aventail|Bevor|Gorget|Pixane",                                        WearLocation = WearLocation.Neck,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Armor|Breastplate|Brigandine|Cuirass|Plackart|Hauberk|Jack of Plate", WearLocation = WearLocation.Body,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Pauldrons|Spaulders",                                                 WearLocation = WearLocation.Shoulders,    Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Vambraces|Sleeves",                                                   WearLocation = WearLocation.Arms,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Wristguards",                                                         WearLocation = WearLocation.Wrists,       Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Gauntlets",                                                           WearLocation = WearLocation.Hands,        Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Faulds",                                                              WearLocation = WearLocation.Waist,        Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Grieves|Chausses",                                                    WearLocation = WearLocation.Legs,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Boots|Sabatons",                                                      WearLocation = WearLocation.Feet,         Ranged = false, Materials = new[] { MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Armor,       Names = "Shield",                                                              WearLocation = WearLocation.Held,         Ranged = false, Materials = new[] { MaterialClass.Wood,  MaterialClass.Gemstone, MaterialClass.Metal, MaterialClass.Elemental } },

            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Sword|Rapier|Scimitar|Sabre|Broadsword|Cutlass",                      WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Axe|Broad Axe|Battle Axe",                                            WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Polearm|Halberd|Scepter",                                             WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Bow|Short Bow|Recurve Bow|Long Bow|Reflex Bow",                       WearLocation = WearLocation.TwoHanded,    Ranged = true,  Materials = new[] { MaterialClass.Wood,   MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Crossbow|Windlass",                                                   WearLocation = WearLocation.TwoHanded,    Ranged = true,  Materials = new[] { MaterialClass.Wood,   MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Dagger|Stiletto|Dirk|Kris",                                           WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Staff|Quarterstaff|Sceptre",                                          WearLocation = WearLocation.TwoHanded,    Ranged = false, Materials = new[] { MaterialClass.Wood,   MaterialClass.Gemstone, MaterialClass.Metal, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Long Sword|Flamberge|Claymore",                                       WearLocation = WearLocation.TwoHanded,    Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Double Axe|Pollaxe",                                                  WearLocation = WearLocation.TwoHanded,    Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Hammer|Mallet|Maul|War Hammer|Mace",                                  WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Metal,  MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Weapon,      Names = "Flail|Whip|Scourge",                                                  WearLocation = WearLocation.Wielded,      Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Gemstone, MaterialClass.Elemental } },

            new ItemType() { ItemClass = ItemClass.Container,   Names = "Bag|Pouch|Sack|Backpack",                                             WearLocation = WearLocation.Held,         Ranged = false, Materials = new[] { MaterialClass.Animal } },

            new ItemType() { ItemClass = ItemClass.Jewelry,     Names = "Earrings",                                                            WearLocation = WearLocation.Ears,         Ranged = false, Materials = new[] { MaterialClass.Stone, MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Jewelry,     Names = "Necklace",                                                            WearLocation = WearLocation.Neck,         Ranged = false, Materials = new[] { MaterialClass.Stone, MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Jewelry,     Names = "Bracelet",                                                            WearLocation = WearLocation.Wrists,       Ranged = false, Materials = new[] { MaterialClass.Stone, MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },
            new ItemType() { ItemClass = ItemClass.Jewelry,     Names = "Ring",                                                                WearLocation = WearLocation.Fingers,      Ranged = false, Materials = new[] { MaterialClass.Stone, MaterialClass.Metal, MaterialClass.Gemstone, MaterialClass.Elemental } },

            new ItemType() { ItemClass = ItemClass.Light,       Names = "Torch|Lantern",                                                       WearLocation = WearLocation.Held,         Ranged = false, Materials = new[] { MaterialClass.Wood, MaterialClass.Elemental } },

            /* TODO: Split into multiple types. */
            new ItemType() { ItemClass = ItemClass.Magical,     Names = "Orb|Spellbook|Wand|Potion|Totem",                                     WearLocation = WearLocation.Held,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },

            new ItemType() { ItemClass = ItemClass.Resource,    Names = "",                                                                    WearLocation = WearLocation.None,         Ranged = false, Materials = new[] { MaterialClass.Animal, MaterialClass.Elemental } },
        };

        public static readonly Dictionary<MaterialType, Material> Materials = new Dictionary<MaterialType, Material>()
        {
            /* None */
            {MaterialType.None,        new Material() { MaterialType = MaterialClass.None,      MaterialID = MaterialType.None,      Name = "None",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 100% chance of drop     */  }},

            /* Animal */
            {MaterialType.Cloth,       new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Cloth,     Name = "Hide",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Fur,         new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Fur,       Name = "Fur",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Feather,     new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Feather,   Name = "Feather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Leather,     new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Leather,   Name = "Leather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Chitin,      new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Chitin,    Name = "Cloth",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Bone,        new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Bone,      Name = "Bone",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Scale,       new Material() { MaterialType = MaterialClass.Animal,    MaterialID = MaterialType.Scale,     Name = "Scale",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

            /* Wood */
            {MaterialType.Leaf,        new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Leaf,      Name = "Leaf",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Bark,        new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Bark,      Name = "Bark",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Pine,        new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Pine,      Name = "Pine",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Ash,         new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Ash,       Name = "Ash",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Birch,       new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Birch,     Name = "Birch",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Maple,       new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Maple,     Name = "Maple",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Oak,         new Material() { MaterialType = MaterialClass.Wood,      MaterialID = MaterialType.Oak,       Name = "Oak",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},
             
            /* Stone */
            {MaterialType.Stone,       new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Stone,     Name = "Stone",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Flint,       new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Flint,     Name = "Flint",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Granite,     new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Granite,   Name = "Granite",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Marble,      new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Marble,    Name = "Marble",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Obsidian,    new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Obsidian,  Name = "Obsidian",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Meteorite,   new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Meteorite, Name = "Meteorite",   PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Adamantium,  new Material() { MaterialType = MaterialClass.Stone,     MaterialID = MaterialType.Adamantium,Name = "Adamantium",  PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

            /* Metal */
            {MaterialType.Iron,        new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Iron,      Name = "Iron",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Copper,      new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Copper,    Name = "Copper",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Bronze,      new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Bronze,    Name = "Feather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Silver,      new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Silver,    Name = "Leather",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Gold,        new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Gold,      Name = "Chitin",      PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Platinum,    new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Platinum,  Name = "Bone",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Mithril,     new Material() { MaterialType = MaterialClass.Metal,     MaterialID = MaterialType.Mithril,   Name = "Scale",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

            /* Gemstone */
            {MaterialType.Pearl,       new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Pearl,     Name = "Pearl",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Opal,        new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Opal,      Name = "Opal",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Sapphire,    new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Sapphire,  Name = "Sapphire",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Emerald,     new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Emerald,   Name = "Emerald",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Ruby,        new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Ruby,      Name = "Ruby",        PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Diamond,     new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Diamond,   Name = "Diamond",     PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Electrum,    new Material() { MaterialType = MaterialClass.Gemstone,  MaterialID = MaterialType.Electrum,  Name = "Electrum",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},

            /* Elemental */
            {MaterialType.Ice,         new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Ice,       Name = "Ice",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 0f        /* 31.14604% chance of drop */ }},
            {MaterialType.Flame,       new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Flame,     Name = "Flame",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 31.14604f /* 22.41535% chance of drop */ }},
            {MaterialType.Earth,       new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Earth,     Name = "Earth",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 53.56139f /* 16.13201% chance of drop */ }},
            {MaterialType.Water,       new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Water,     Name = "Water",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 69.69341f /* 11.60998% chance of drop */ }},
            {MaterialType.Air,         new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Air,       Name = "Air",         PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 81.30338f /* 8.355536% chance of drop */ }},
            {MaterialType.Smoke,       new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Smoke,     Name = "Smoke",       PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 89.65891f /* 6.013358% chance of drop */ }},
            {MaterialType.Electric,    new Material() { MaterialType = MaterialClass.Elemental, MaterialID = MaterialType.Electric,  Name = "Electric",    PriceMod = 100f,       DamageMod = 100f,       WeightMod = 100f,       DropMod = 95.67227f /* 4.327727% chance of drop */ }},
        };
    }
}
