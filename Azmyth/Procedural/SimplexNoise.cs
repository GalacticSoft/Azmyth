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
        * Public Domain
        */
 
    public class SimplexValueNoise : INoise 
    {
 
	    private const double SQUISH_CONSTANT = 0.366025403784439;	//(Math.sqrt(2+1)-1)/2;
	    private const double STRETCH_CONSTANT = -0.211324865405187;	//(1/Math.sqrt(2+1)-1)/2;
	    private const double NORM_CONSTANT = 2174;
	    private const long DEFAULT_SEED = 0;
	
	    private short[] perm;
	
	    public SimplexValueNoise() : this(DEFAULT_SEED) {
		    
	    }
	
	    public SimplexValueNoise(short[] perm) {
		    this.perm = perm;
	    }
	
	    //Initializes the class using a permutation array generated from a 64-bit seed.
	    //Generates a proper permutation (i.e. doesn't merely perform N successive pair swaps on a base array)
	    //Uses a simple 64-bit LCG.
	    public SimplexValueNoise(long seed) {
		    perm = new short[256];
		    short[] source = new short[256];
		    for (short i = 0; i < 256; i++)
			    source[i] = i;
		    seed = seed * 6364136223846793005 + 1442695040888963407;
		    seed = seed * 6364136223846793005 + 1442695040888963407;
		    seed = seed * 6364136223846793005 + 1442695040888963407;
		    for (int i = 255; i >= 0; i--) {
			    seed = seed * 6364136223846793005 + 1442695040888963407;
			    int r = (int)((seed + 31) % (i + 1));
			    if (r < 0)
				    r += (i + 1);
			    perm[i] = source[r];
			    source[r] = source[i];
		    }
	    }
	
	    //2D SimplexValue Noise.
	    public double GetValue(double x, double y) {
		
		    //Place input coordinates on triangular grid.
		    double squishOffset = (x + y) * SQUISH_CONSTANT;
		    double xs = x + squishOffset;
		    double ys = y + squishOffset;
		
		    //Floor to get base coordinate of containing square/rhombus.
		    int xsb = fastFloor(xs);
		    int ysb = fastFloor(ys);
		
		    //Skew out to get actual coordinates of rhombus origin. We'll need these later.
		    double stretchOffset = (xsb + ysb) * STRETCH_CONSTANT;
		    double xb = xsb + stretchOffset;
		    double yb = ysb + stretchOffset;
 
		    //Positions relative to origin point.
		    double dx = x - xb;
		    double dy = y - yb;
		
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
			
			    value = kernels(dx, dy, h1, h2, h3,
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
			
			    value = kernels(dy, dx, h1, h2, h3,
				    h4, h5, h6, h7, h8, h9, h10, h11, h12);
		    }
		    return value / NORM_CONSTANT;
	    }
	
	    private double KERNEL_RADIUS = Math.Sqrt(3);
	    private double kernels(double dx, double dy,
		    byte h1, byte h2, byte h3, byte h4, byte h5, byte h6,
		    byte h7, byte h8, byte h9, byte h10, byte h11, byte h12) {
		    double value = 0;
		    double dxv, dyv, attn;
		
		    dxv = dx + 1 + 2 * STRETCH_CONSTANT; dyv = dy + 1 + 2 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h1;
		    }
		
		    dxv = dx + 0 + STRETCH_CONSTANT; dyv = dy + 1 + STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h2;
		    // }
		
		    dxv = dx - 1; dyv = dy + 1;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h3;
		    }
		
		    dxv = dx + 1 + STRETCH_CONSTANT; dyv = dy + 0 + STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h4;
		    }
		
		    dxv = dx + 0; dyv = dy + 0;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h5;
		    // }
		
		    dxv = dx - 1 - STRETCH_CONSTANT; dyv = dy + 0 - STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h6;
		    // }
		
		    dxv = dx - 2 - 2 * STRETCH_CONSTANT; dyv = dy + 0 - 2 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h7;
		    }
		
		    dxv = dx + 0 - STRETCH_CONSTANT; dyv = dy - 1 - STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h8;
		    // }
		
		    dxv = dx - 1 - 2 * STRETCH_CONSTANT; dyv = dy - 1 - 2 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h9;
		    // }
		
		    dxv = dx - 2 - 3 * STRETCH_CONSTANT; dyv = dy - 1 - 3 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    // if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h10;
		    // }
		
		    dxv = dx - 1 - 3 * STRETCH_CONSTANT; dyv = dy - 2 - 3 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h11;
		    }
		
		    dxv = dx - 2 - 4 * STRETCH_CONSTANT; dyv = dy - 2 - 4 * STRETCH_CONSTANT;
		    attn = KERNEL_RADIUS - dxv * dxv - dyv * dyv;
		    if (attn > 0) {
			    attn *= attn;
			    value += attn * attn * h12;
		    }
		
		    return value;
	    }
	
	    private static int fastFloor(double x) {
		    int xi = (int)x;
		    return x < xi ? xi - 1 : xi;
	    }


        public double GetValue(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        public double GetValue(double x, double y, double z, double t)
        {
            throw new NotImplementedException();
        }
    }
}
