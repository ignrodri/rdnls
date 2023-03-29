using System;
using System.Collections;
using System.IO;

class PrepareDataset3 {

	static double[] t = new double[] {35, 50, 75, 100, 200, 250, 350, 500, 650, 800, 1000, 1500, 2000, 3000, 5000, 7000};
	static int np = 181 * 217 * 11;
	
	static void Main(string[] args) {
		if (args.Length == 1) {
			Exec(args[0]);
		}
	}
	
	public static void Exec(string sdir) {
		string dir = Path.Combine(sdir, "brainweb");
		byte[,] mask = new byte[np, 1];
		double[,] im = new double[np, 16];
		double[,] xim = new double[np, 1];
		for (int i = 0; i < 16; i++) {
			xim = LoadBrainweb(Path.Combine(dir, String.Format("brainweb.181.217.181.{0}.rawb", t[i].ToString("F0"))));
			for (int ip = 0; ip < np; ip++) {
				im[ip, i] = xim[ip, 0];
			}
		}
		mask = MakeMask(xim);
		double[,] t1map = ReadT1(dir);
		string xdir = Path.Combine(sdir, "transformedimages", "dataset3");
		PrepareTest(xdir, im, mask, t1map);
	}
	
	static double[,] ReadT1(string dir) {
		double[] t1list = new double[] {0, 2569, 833, 500, 350, 900, 2569, 0, 833, 500};
		double[] pdlist = new double[] {0, 1, 0.86, 0.77, 1, 1, 1, 0, 0.86, 0.77};
		int i, ip;
		double s0, s1;
		double[,] xim = new double[np, 1];
		double[,] im = new double[np, 10];
		for (i = 0; i < 10; i++) {
			xim = LoadBrainweb(Path.Combine(dir, String.Format("brainweb.181.217.181.tissue.{0}.rawb", i)));
			for (ip = 0; ip < np; ip++) {
				im[ip, i] = xim[ip, 0];
			}
		}
		double[,] res = new double[np, 1];
		for (ip = 0; ip < np; ip++) {
			s0 = 0;
			s1 = 0;
			for (i = 0; i < 10; i++) {
				s0 = s0 + pdlist[i] * im[ip, i];
				s1 = s1 + t1list[i] * pdlist[i] * im[ip, i];
			}
			if (s0 > 0) {
				res[ip, 0] = s1 / s0;
			}
			else {
				res[ip, 0] = 0;
			}
		}
		return res;
	}
	
	static void PrepareTest(string dir, double[,] im, byte[,] mask, double[,] t1map) {
		Util.CreateDirectory(dir);
		Util.SaveIm(Path.Combine(dir, "original.image"), im);
		Util.SaveIm(Path.Combine(dir, "originalt1.image"), t1map);
		Util.SaveB(Path.Combine(dir, "mask.image"), mask);
		Util.SaveT(dir, t);
		Util.SaveEmpty(dir, np, 3, new int[] {1, 2, 3, 4});
	}

	static byte[,] MakeMask(double[,] im) {
		byte[,] res = new byte[np, 1];
		for (int ip = 0; ip < np; ip++) {
			res[ip, 0] = (byte) ((im[ip, 0] > 100) ? 1 : 0);
		}
		return res;
	}
	
	static double[,] LoadBrainweb(string f) {
		double[,] res = new double[np, 1];
		byte[] b = File.ReadAllBytes(f);
		for (int ix = 0; ix < 181; ix++) {
			for (int iy = 0; iy < 217; iy++) {
				for (int iz = 0; iz < 11; iz++) {
					res[ix + 181 * iy + 181 * 217 * iz, 0] = b[ix + 181 * iy + 181 * 217 * (iz + 85)];
				}
			}
		}
		return res;
	}
}