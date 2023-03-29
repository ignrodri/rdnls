using System;
using System.Collections;
using System.IO;

class PrepareDataset2 {

	static double[] t = new double[] {35, 50, 75, 100, 200, 250, 350, 500, 650, 800, 1000, 1500, 2000, 3000, 5000, 7000};
	static string[] orig = new string[3];
	static string[] isa = new string[3];
	
	public static void Main(string[] args) {
		if (args.Length == 1) {
			Exec(args[0]);
		}
	}
	
	public static void Exec(string sdir) {
		string dir = Path.Combine(sdir, "bruker", "25", "pdata");
		for (int i = 0; i < 3; i++) {
			orig[i] = Path.Combine(dir, (i + 3).ToString());
			isa[i] = Path.Combine(dir, (2 * i + 7).ToString());
		}
		string xdir = Path.Combine(sdir, "transformedimages", "dataset2");
		PrepareIsa(xdir);
		PrepareTest(xdir);
		PrepareIsa(xdir);
	}
	
	static void PrepareTest(string dir) {
		Util.CreateDirectory(dir);
		double[,] im = new double[19200, 16];
		byte[,] mask = MakeMask(dir);
		double[,] xim;
		for (int i = 0; i < 3; i++) {
			xim = UtilBruker.Scale32(orig[i], 6400, 16);
			for (int ip = 0; ip < 6400; ip++) {
				for (int it = 0; it < 16; it++) {
					im[ip + 6400 * i, it] = xim[ip, it];
				}
			}
		}
		Util.SaveIm(Path.Combine(dir, "original.image"), im);
		Util.SaveB(Path.Combine(dir, "mask.image"), mask);
		Util.SaveT(dir, t);
		File.Delete(Path.Combine(dir, "map1.image"));
		Util.SaveEmpty(dir, 19200, 3, new int[] {1, 2, 3, 4});
	}

	static void PrepareIsa(string dir) {
		Util.CreateDirectory(dir);
		double[,] res = new double[19200, 3];
		double[,] xres;
		for (int i = 0; i < 3; i++) {
			xres = UtilBruker.Scale32(isa[i], 6400, 7);
			xres = UtilBruker.FilterIsa(xres, new bool[] {true, false, true, false, true, false, false});
			for (int ip = 0; ip < 6400; ip++) {
				for (int it = 0; it < 3; it++) {
					res[ip + 6400 * i, it] = xres[ip, it];
				}
			}
		}
		Util.SaveIm(Path.Combine(dir, "map1.image"), res);
	}

	static byte[,] MakeMask(string dir) {
		double[,] im = Util.LoadIm(Path.Combine(dir, "map1.image"));
		byte[,] res = new byte[19200, 1];
		for (int i = 0; i < 19200; i++) {
			if (im[i, 1] > 0) {
				res[i, 0] = 1;
			}
			else {
				res[i, 0] = 0;
			}
		}
		return res;
	}
}