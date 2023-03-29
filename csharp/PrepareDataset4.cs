using System;
using System.Collections;
using System.IO;

class PrepareDataset4 {

	static double[] t = new double[64];
	static string orig;
	static string isa;
	
	static void Main(string[] args) {
		if (args.Length == 1) {
			Exec(args[0]);
		}
	}
	
	public static void Exec(string sdir) {
		string dir = Path.Combine(sdir, "bruker", "29", "pdata");
		orig = Path.Combine(dir, "1");
		isa = Path.Combine(dir, "2");
		for (int i = 0; i < 64; i++) {
			t[i] = 7 * i + 7;
		}
		string xdir = Path.Combine(sdir, "transformedimages", "dataset4");
		PrepareIsa(xdir);
		PrepareTest(xdir);
		PrepareIsa(xdir);
	}
	
	static void PrepareTest(string dir) {
		Util.CreateDirectory(dir);
		double[,] im = new double[327680, 64];
		byte[,] mask = MakeMask(dir);
		double[,] xim;
		xim = UtilBruker.Scale16(orig, 65536, 320);
		for (int i = 0; i < 5; i++) {
			for (int ip = 0; ip < 65536; ip++) {
				for (int ix = 0; ix < 64; ix++) {
					im[65536 * i + ip, ix] = xim[ip, 64 * i + ix];
				}
			}
		}
		Util.SaveIm(Path.Combine(dir, "original.image"), im);
		Util.SaveB(Path.Combine(dir, "mask.image"), mask);
		Util.SaveT(dir, t);
		Util.SaveEmpty(dir, 327680, 2, new int[] {2, 3, 4, 5});
	}

	static void PrepareIsa(string dir) {
		Util.CreateDirectory(dir);
		double[,] res = new double[327680, 5];
		double[,] xres = UtilBruker.Scale32(isa, 65536, 25);
		for (int i = 0; i < 5; i++) {
			for (int ip = 0; ip < 65536; ip++) {
				for (int ix = 0; ix < 5; ix++) {
					res[65536 * i + ip, ix] = xres[ip, 5 * i + ix];
				}
			}
		}
		res = UtilBruker.FilterIsa(res, new bool[] {true, false, true, false, false});
		Util.SaveIm(Path.Combine(dir, "map1.image"), res);
	}

	static byte[,] MakeMask(string dir) {
		double[,] im = Util.LoadIm(Path.Combine(dir, "map1.image"));
		byte[,] res = new byte[327680, 1];
		for (int i = 0; i < 327680; i++) {
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