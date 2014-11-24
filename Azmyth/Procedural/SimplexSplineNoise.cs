using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Procedural
{
    /*
     * SimplexValue Noise in Java.
     * by Kurt Spencer
     *
     * v1.0.1
     * - Slight change to seed RNG
     * - Removed default permutation array in favor of
     *   default seed.
     */
 
    public class SimplexSplineNoise : Noise
    {
	    private const double SQUISH_CONSTANT = 0.366025403784439; //(Math.sqrt(2+1)-1)/2;
	    private const double NORM_CONSTANT = 156;
	
	    private short[] perm;

        //Initializes the class using a permutation array generated from a 64-bit seed.
        //Generates a proper permutation (i.e. doesn't merely perform N successive pair swaps on a base array)
        //Uses a simple 64-bit LCG.
        public SimplexSplineNoise(double persistence, double frequency, double amplitude, int octaves, long seed)
            : base(persistence, frequency, amplitude, octaves, seed)
        {
            perm = new short[256];
            short[] source = new short[256];
            for (short i = 0; i < 256; i++)
                source[i] = i;
            seed = seed * 6364136223846793005 + 1442695040888963407;
            seed = seed * 6364136223846793005 + 1442695040888963407;
            seed = seed * 6364136223846793005 + 1442695040888963407;
            for (int i = 255; i >= 0; i--)
            {
                seed = seed * 6364136223846793005 + 1442695040888963407;
                int r = (int)((seed + 31) % (i + 1));
                if (r < 0)
                    r += (i + 1);
                perm[i] = source[r];
                source[r] = source[i];
            }
        }
	
	    //2D Simplex-Value-Spline Noise.
	    public override double GetValue(double x, double y) {
		
		    //Place input coordinates on triangular grid.
		    double squishOffset = (x + y) * SQUISH_CONSTANT;
		    double xs = x + squishOffset;
		    double ys = y + squishOffset;
		
		    //Floor to get base coordinate of containing square/rhombus.
		    int xsb = fastFloor(xs);
		    int ysb = fastFloor(ys);
		
		    //Compute grid coordinates relative to rhombus origin.
		    double xins = xs - xsb;
		    double yins = ys - ysb;
		
		    double value;
		    if (xins > yins) { //We're inside the x>y triangle of the rhombus
		
			    //Get our 12 surrounding vertex values
			    //Using type "byte" works here because type "byte" in Java is signed
			    short yp;
			    yp = perm[(ysb - 1) & 0xFF];
			    byte h1 = (byte)perm[(yp + xsb - 1) & 0xFF]; //(-1,-1)
			    byte h2 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0,-1)
			    byte h3 = (byte)perm[(yp + xsb + 1) & 0xFF]; //( 1,-1)
			    yp = perm[(ysb + 0) & 0xFF];
			    byte h4 = (byte)perm[(yp + xsb - 1) & 0xFF]; //(-1, 0)
			    byte h5 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0, 0)
			    byte h6 = (byte)perm[(yp + xsb + 1) & 0xFF]; //( 1, 0)
			    byte h7 = (byte)perm[(yp + xsb + 2) & 0xFF]; //( 2, 0)
			    yp = perm[(ysb + 1) & 0xFF];
			    byte h8 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0, 1)
			    byte h9 = (byte)perm[(yp + xsb + 1) & 0xFF]; //( 1, 1)
			    byte h10 = (byte)perm[(yp + xsb + 2) & 0xFF];//( 2, 1)
			    yp = perm[(ysb + 2) & 0xFF];
			    byte h11 = (byte)perm[(yp + xsb + 1) & 0xFF];//( 1, 2)
			    byte h12 = (byte)perm[(yp + xsb + 2) & 0xFF];//( 2, 2)
			
			    value = interpolate(xins, yins, h1, h2, h3,
				    h4, h5, h6, h7, h8, h9, h10, h11, h12);
		    } else { //We're inside the y>x triangle of the rhombus
		
			    //Get our 12 surrounding vertex values
			    //Using type "byte" works here because type "byte" in Java is signed
			    short yp;
			    yp = perm[(ysb - 1) & 0xFF];
			    byte h1 = (byte)perm[(yp + xsb - 1) & 0xFF]; //(-1,-1)
			    byte h4 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0,-1)
			    yp = perm[(ysb + 0) & 0xFF];
			    byte h2 = (byte)perm[(yp + xsb - 1) & 0xFF]; //(-1, 0)
			    byte h5 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0, 0)
			    byte h8 = (byte)perm[(yp + xsb + 1) & 0xFF]; //( 1, 0)
			    yp = perm[(ysb + 1) & 0xFF];
			    byte h3 = (byte)perm[(yp + xsb - 1) & 0xFF]; //(-1, 1)
			    byte h6 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0, 1)
			    byte h9 = (byte)perm[(yp + xsb + 1) & 0xFF]; //( 1, 1)
			    byte h11 = (byte)perm[(yp + xsb + 2) & 0xFF];//( 2, 1)
			    yp = perm[(ysb + 2) & 0xFF];
			    byte h7 = (byte)perm[(yp + xsb + 0) & 0xFF]; //( 0, 2)
			    byte h10 = (byte)perm[(yp + xsb + 1) & 0xFF];//( 1, 2)
			    byte h12 = (byte)perm[(yp + xsb + 2) & 0xFF];//( 2, 2)
			
			    value = interpolate(yins, xins, h1, h2, h3,
				    h4, h5, h6, h7, h8, h9, h10, h11, h12);
		    }
		    return value / NORM_CONSTANT;
	    }
	
	    private double interpolate(double x, double y,
		    byte h1, byte h2, byte h3, byte h4, byte h5, byte h6,
		    byte h7, byte h8, byte h9, byte h10, byte h11, byte h12) {
		
		    //An absolutely ridiculous polynomial spline. Works on the skewed grid so it has nice coefficients.
		    //Could probably be optimized a bit if the compiler doesn't already do it.
		    double value = (h1/2.0 - h4/3.0 - h5 + h8/2.0 - h3/2.0 + h6 + h7/3.0 - h10/2.0)*x*x*x*x*x
		    + ((4*h4)/3.0 - (4*h1)/3.0 + h2/6.0 + 2*h5 - (4*h8)/3.0 + (7*h3)/6.0 - 3*h6 + h9/6.0
		    - h7/3.0 + (7*h10)/6.0 - y*((5*h1)/6.0 - (5*h4)/6.0 + (5*h2)/6.0 - (5*h5)/2.0 + (5*h8)/3.0
		    - (5*h3)/3.0 + (5*h6)/2.0 - (5*h9)/6.0 + (5*h7)/6.0 - (5*h10)/6.0))*x*x*x*x + (((5*h2)/3.0
		    - (10*h5)/3.0 + (5*h8)/3.0 - (5*h3)/3.0 + (10*h6)/3.0 - (5*h9)/3.0)*y*y + ((8*h1)/3.0
		    - (8*h4)/3.0 + (4*h2)/3.0 - 4*h5 + (8*h8)/3.0 - 4*h3 + 6*h6 - 2*h9 + (2*h7)/3.0 - (2*h10)/3.0)*y
		    + h1 - 2*h4 - h2/3.0 + (2*h5)/3.0 + h8 - (2*h3)/3.0 + (4*h6)/3.0 - h9/3.0 - (2*h10)/3.0)*x*x*x
		    + (((5*h5)/3.0 - (5*h8)/3.0 - (10*h6)/3.0 + (10*h9)/3.0 + (5*h7)/3.0 - (5*h10)/3.0)*y*y*y
		    + (5*h5- 5*h2 + 5*h3 - 5*h6)*y*y + (3*h4 - 3*h1 + h2 - h5 + 2*h3 - 2*h6)*y + (4*h4)/3.0
		    - (8*h5)/3.0 + (4*h6)/3.0)*x*x + (((5*h8)/3.0 - (5*h5)/6.0 - (5*h2)/6.0 + (5*h3)/6.0
		    + (5*h6)/2.0 - (5*h9)/2.0 - (5*h11)/6.0 - (5*h7)/3.0 + (5*h10)/6.0 + (5*h12)/6.0)*y*y*y*y
		    + ((8*h2)/3.0 - 2*h5 - (2*h8)/3.0 - (8*h3)/3.0 + (8*h6)/3.0 - (2*h9)/3.0 + (2*h11)/3.0
		    - (2*h7)/3.0 + (4*h10)/3.0 - (2*h12)/3.0)*y*y*y + (2*h2 + h5 - 3*h8 - 2*h3 - h6 + 3*h9)*y*y
		    + ((4*h1)/3.0 - (4*h4)/3.0 - (4*h2)/3.0 + (8*h5)/3.0 - (4*h8)/3.0 - (4*h6)/3.0 + (4*h9)/3.0)*y
		    - h1/6.0 - h4/3.0 + h2/6.0 - h8/6.0 + h6/3.0 + h9/6.0)*x + (h2/2.0 - h8/2.0 - h3/3.0 - h6 + h9
		    + h11/3.0 + h7/2.0 - h12/2.0)*y*y*y*y*y + (h5 - h2/2.0 - h8/2.0 + h3/2.0 - h6/2.0 - h9/2.0
		    + h11/2.0 + h7/3.0 - (2*h10)/3.0 + h12/3.0)*y*y*y*y + ((5*h8)/3.0 - (5*h2)/3.0 + (2*h3)/3.0
		    + (4*h6)/3.0 - (4*h9)/3.0 - (2*h11)/3.0)*y*y*y + ((4*h2)/3.0 - (8*h5)/3.0 + (4*h8)/3.0)*y*y
		    + (h4/6.0 - h1/6.0 - h2/3.0 + h8/3.0 - h6/6.0 + h9/6.0)*y + h5;
		    return value;
	    }
	
	    private static int fastFloor(double x) {
		    int xi = (int)x;
		    return x < xi ? xi - 1 : xi;
	    }


        public override double GetValue(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        public override double GetValue(double x, double y, double z, double t)
        {
            throw new NotImplementedException();
        }
    }
}
