using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Threading;

class TestT {

	static void Main(string[] args) {
		if (args.Length == 1) {
			Application.CurrentCulture = new CultureInfo("en-US");
			string dir1 = Path.Combine(args[0], "transformedimages", "dataset1");
			string dir2 = Path.Combine(args[0], "transformedimages", "dataset2");
			string dir3 = Path.Combine(args[0], "transformedimages", "dataset3");
			string dir4 = Path.Combine(args[0], "transformedimages", "dataset4");
		//	Util.Destroy(Path.Combine(args[0], "transformedimages"));
			Util.ClearText(Path.Combine(args[0], "transformedimages"));
			PrepareDataset1.Exec(args[0]);
			MakeMap(dir1, new TestModel1(), 2);
			PrepareDataset2.Exec(args[0]);
			MakeMap(dir2, new TestModel2(), 1);
			PrepareDataset3.Exec(args[0]);
			MakeMap(dir3, new TestModel2(), 1);
			PrepareDataset4.Exec(args[0]);
			MakeMap(dir4, new TestModel3(), 2);
		}
	}
	
	public static double[] GetPixel(double[,] im, int ip) {
		double[] res = new double[im.GetLength(1)];
		for (int i = 0; i < res.Length; i++) {
			res[i] = im[ip, i];
		}
		return res;
	}

	public static void MakeMap(string dir, TestModel test, int imap) {
		List<string> rdata = new List<string>();
		LocalMinimum[] lm = new LocalMinimum[0];
		double[] t = Util.LoadT(dir);
		double[,] im = Util.LoadIm(Path.Combine(dir, "original.image"));
		byte[,] mask = Util.LoadB(Path.Combine(dir, "mask.image"));
		double[,] map = Util.LoadIm(Path.Combine(dir, String.Format("map{0}.image", imap)));
		byte[,] nlocal = Util.LoadB(Path.Combine(dir, "local.image"));
		int np = im.GetLength(0);
		int nt = im.GetLength(1);
		double[] x;
		double[] y;
		int ip, j;
		int pip = 1;
		DateTime start = DateTime.Now;
		test.SetT(t);
		for (ip = 0; ip < np; ip++) {
			if (mask[ip, 0] > 0) {
				y = GetPixel(im, ip);
				test.SetY(y);
				x = test.FitResult();
				if (test.NumLocal() > 1) {
					nlocal[ip, 0] = 1;
				}
				for (j = 0; j < test.npar; j++) {
					map[ip, j] = x[j];
				}
				pip++;
				if (test.Ok(x)) {
					lm = test.Minima();
					if (lm.Length == 2) {
						rdata.Add(String.Format("{0}\t{1}", lm[0].trel, lm[1].trel));
					}
				}
				else {
					mask[ip, 0] = 0;
				}
			}
		}
		double time = (DateTime.Now - start).TotalSeconds;
		Console.WriteLine("Dir: {0} Exec time = {1} ms", Path.Combine(dir, String.Format("map{0}.image", imap)), (1000 * time / pip).ToString("F2"));
		Util.SaveIm(Path.Combine(dir, String.Format("map{0}.image", imap)), map);
		Util.SaveB(Path.Combine(dir, "mask.image"), mask);
		Util.SaveB(Path.Combine(dir, "local.image"), nlocal);
		File.WriteAllLines(Path.Combine(dir, "error.txt"), rdata.ToArray());
	}
}