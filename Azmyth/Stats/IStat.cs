using System;
using Azmyth;
using Azmyth.Assets;

namespace Azmyth.Stats
{



	public interface IStat
	{
		VectorID Vector      	{ get; set; }

		string Name 			{ get;  }
		string Abbreviation		{ get;  }
		string Description 		{ get;  }

        double BaseValue     	{ get; set; }
		double Modifier			{ get; set; }

		double MinBase			{ get; set; }
		double MaxBase			{ get; set; }

		double MinMod			{ get; set; }
		double MaxMod			{ get; set; }

		double Minimum			{ get;  }
		double Maximum			{ get;  }

		double Value			{ get; }

		string VectorName		{ get; }
	}
}

