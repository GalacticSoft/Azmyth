using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Game
{
    public enum Pulses
    {
        PulsesPerSec    = 4,
        MsPerPulse      = 1000 / PulsesPerSec,
    };

    public enum PlayerStatus 
    {
        None            = 0,
        Connecting      = 1,
        Connected       = 2,
        MainMenu        = 3,
        Playing         = 4,
        Menu            = 5,
        Dying           = 6,
        Dead            = 7,
        Disconnecting   = 8,
        Disconnected    = 9,
    };

    public enum EntityVector
    { 
        World           = 1,
        Area            = 2, 
        Mobile          = 3,
        Item            = 4, 
        Room            = 5, 
        Affect          = 6, 
        Stat            = 7, 
        Player          = 8, 
        Exit            = 9 
    };

	public enum StatID
	{
		STR 			= 0,
		INT 			= 1,
		WIS 			= 2,
		DEX 			= 3,
		AGI 			= 4,
		POW 			= 5,
		CON 			= 6,
		LUK 			= 7,
		CHA 			= 8,
		MAX 			= 9
	};





    public enum AbilityTypes
    {
        None            = 0,
        Spell           = 1,
        Skill           = 2,
        Song            = 3,
        Emote           = 4,
        Power           = 5,
        Craft           = 6,
        Max             = 7
    };
}
