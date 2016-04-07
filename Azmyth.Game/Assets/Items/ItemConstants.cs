using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    /* Determines Quality of Item Stats. */
    public enum Quality
    {
        Any         = -1,
        Poor        =  0,
        Average     =  1,
        Good        =  2,
        Excellent   =  3,
        Superior    =  4,
        Max         =  5
    }

    /* Rarity Determines Item Bonuses */
    public enum Rarity
    {
        Any         = -1,
        None        =  0,
        Common      =  1,
        Uncommon    =  2,
        Rare        =  3,
        Legendary   =  4,
        Epic        =  5,
        Mythic      =  6,
        Max         =  7
    }

    public enum ItemClass
    {
        Any         = -1,
        None        =  0,
        Armor       =  1,
        Clothing    =  2,
        Container   =  3,
        Jewelry     =  4,
        Light       =  5,
        Magical     =  6,
        Resource    =  7,
        Weapon      =  8,
        Max         =  9
    }

    public enum MaterialClass
    {
        Any         = -1,
        None        =  0,
        Animal      =  1,
        Wood        =  2,
        Stone       =  3,
        Metal       =  4,
        Gemstone    =  5,
        Elemental   =  6,
        Max         =  7
    }

    public enum WearLocation
    {
        None        = 0,
        Head        = 1,
        Ears        = 2,
        Neck        = 3,
        Shoulders   = 4,
        Body        = 5,
        Arms        = 6,
        Wrists      = 7,
        Hands       = 8,
        Fingers     = 9,
        Waist       = 10,
        Back        = 11,
        Legs        = 12,
        Feet        = 13,
        Max         = 14

    }
    public enum MaterialType
    {
        Any = -1,
        None = 0,

        Cloth = 1,
        Fur = 2,
        Feather = 3,
        Leather = 4,
        Chitin = 5,
        Bone = 6,
        Scale = 7,

        Leaf = 8,
        Bark = 9,
        Pine = 10,
        Ash = 11,
        Birch = 12,
        Maple = 13,
        Oak = 14,

        Stone = 15,
        Flint = 16,
        Granite = 17,
        Marble = 18,
        Obsidian = 19,
        Meteorite = 20,
        Adamantium = 21,

        Iron = 22,
        Copper = 23,
        Bronze = 24,
        Silver = 25,
        Gold = 26,
        Platinum = 27,
        Mithril = 28,

        Pearl = 29,
        Opal = 30,
        Sapphire = 31,
        Emerald = 32,
        Ruby = 33,
        Diamond = 34,
        Electrum = 35,

        Ice = 36,
        Flame = 37,
        Earth = 38,
        Water = 39,
        Air = 40,
        Smoke = 41,
        Electric = 42,

        Max = 42
    }
}
