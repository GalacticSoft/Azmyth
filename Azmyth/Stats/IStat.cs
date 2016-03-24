using System;
using Azmyth;
using Azmyth.Assets;

namespace Azmyth.Stats
{



	public interface IStat
	{
		VectorID ID      	{ get; set; }

		string Name 			{ get;  }
		string Abbreviation		{ get;  }
		string Description 		{ get;  }

        int BaseValue     	{ get; set; }
        int Modifier { get; set; }

        int MinBase { get; set; }
        int MaxBase { get; set; }

        int MinMod { get; set; }
        int MaxMod { get; set; }

        int Minimum { get; }
        int Maximum { get; }

        int Value { get; }

        IStat Clone();
        void Roll();
	}
}

