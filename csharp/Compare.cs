using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Threading;

class Permutation {
	
	int[] val;

	int count;
	
	public bool HasElements {
		get {
			return (count > 0);
		}
	}
	
	public Permutation(int n) {
		val = new int[n];
		count = n;
		for (int i = 0; i < n; i++) {
			val[i] = i;
		}
	}
	
	public int Next() {
		int n = Util.ran.Next(count);
		int res = val[n];
		Array.Copy(val, n + 1, val, n, count - n - 1);
		count = count - 1;
		return res;
	}
}

class CompareT {

	static List<string> rdata = new List<string>();

	static void Main(string[] args) {
		if (args.Length == 1) {
			Application.CurrentCulture = new CultureInfo("en-US");
			string dir1 = Path.Combine(args[0], "transformedimages", "dataset1");
			string dir2 = Path.Combine(args[0], "transformedimages", "dataset2");
			string dir3 = Path.Combine(args[0], "transformedimages", "dataset3");
			string dir4 = Path.Combine(args[0], "transformedimages", "dataset4");
		//	Util.ClearText(Path.Combine(args[0], "transformedimages"));
			ProcMask(dir1, new TestModel1());
			ProcMask(dir2, new TestModel2());
			ProcMask(dir3, new TestModel2());
			ProcMask(dir4, new TestModel3());
			Compare(dir1, new TestModel1(), "map2.image", "map1.image", "test1");
			Compare(dir1, new TestModel1(), "map2.image", "map3.image", "test2");
			Compare(dir1, new TestModel1(), "map2.image", "map4.image", "test3");
			Compare(dir1, new TestModel1(), "map2.image", "map5.image", "test4");
			Compare(dir2, new TestModel2(), "map1.image", "map2.image", "test5");
			Compare(dir2, new TestModel2(), "map1.image", "map3.image", "test6");
			Compare(dir2, new TestModel2(), "map1.image", "map4.image", "test7");
			Compare(dir3, new TestModel2(), "map1.image", "map2.image", "test8");
			Compare(dir3, new TestModel2(), "map1.image", "map3.image", "test9");
			Compare(dir3, new TestModel2(), "map1.image", "map4.image", "test10");
			Compare(dir4, new TestModel3(), "map2.image", "map1.image", "test11");
			Compare(dir4, new TestModel3(), "map2.image", "map3.image", "test12");
			Compare(dir4, new TestModel3(), "map2.image", "map4.image", "test13");
			Compare(dir4, new TestModel3(), "map2.image", "map5.image", "test14");
		//	File.WriteAllLines(Path.Combine(args[0], "error.RR"), rdata.ToArray());
		}
	}
	
	public static void ProcMask(string dir, TestModel test) {
		byte[,] mask = Util.LoadB(Path.Combine(dir, "mask.image"));
		double[,] map;
		int np = mask.GetLength(0);
		string[] sfiles = Directory.GetFiles(dir, "map*.image");
		for (int i = 0; i < sfiles.Length; i++) {
			map = Util.LoadIm(sfiles[i]);
			for (int ip = 0; ip < np; ip++) {
				if (mask[ip, 0] > 0) {
					if (!test.Ok(GetPixel(map, ip))) {
						mask[ip, 0] = 0;
					}
				}
			}
		}
		Util.SaveB(Path.Combine(dir, "mask.image"), mask);
	}
	
	public static double[] GetPixel(double[,] im, int ip) {
		double[] res = new double[im.GetLength(1)];
		for (int i = 0; i < res.Length; i++) {
			res[i] = im[ip, i];
		}
		return res;
	}
		
	public static void Compare(string dir, TestModel test, string smap1, string smap2, string stest) {
		double[] t = Util.LoadT(dir);
		double[,] im = Util.LoadIm(Path.Combine(dir, "original.image"));
		byte[,] mask = Util.LoadB(Path.Combine(dir, "mask.image"));
		byte[,] nlocal = Util.LoadB(Path.Combine(dir, "local.image"));
		double[,] map1 = Util.LoadIm(Path.Combine(dir, smap1));
		double[,] map2 = Util.LoadIm(Path.Combine(dir, smap2));
		int np = im.GetLength(0);
		int nt = im.GetLength(1);
		double err1, err2;
		double res1, res2;
		int n1 = 0;
		int n2 = 0;
		int numlocal = 0;
		int numtotal = 0;
		double[] y = new double[t.Length];
		double[] xmap1 = new double[test.npar];
		double[] xmap2 = new double[test.npar];
		double par1 = 0;
		double par2 = 0;
		int ip;
		using (StreamWriter t1writer = new StreamWriter(new FileStream(Path.Combine(dir, String.Format("t1.{0}.csv", stest)), FileMode.Create, FileAccess.Write, FileShare.None))) {
			t1writer.WriteLine("map1,map2");
			Permutation per = new Permutation(np);
			while (per.HasElements) {
				ip = per.Next();
				if (mask[ip, 0] > 0) {
					numtotal = numtotal + 1;
					y = GetPixel(im, ip);
					xmap1 = GetPixel(map1, ip);
					xmap2 = GetPixel(map2, ip);
					res1 = xmap1[test.npar - 1];
					res2 = xmap2[test.npar - 1];
					t1writer.WriteLine("{0},{1}", res1.ToString("F3"), res2.ToString("F3"));
					par1 = par1 + res1;
					par2 = par2 + res2;
					if (nlocal[ip, 0] > 0) {
						numlocal = numlocal + 1;
						SaveFitData(dir, test, t, y, xmap1, xmap2, stest, ip);
					}
					if (Math.Abs(res2 - res1) > test.ttol) {
						err1 = test.CalcError(t, y, xmap1);
						err2 = test.CalcError(t, y, xmap2);
						if (err1 < err2) {
							n1 = n1 + 1;
						}
						else {
							n2 = n2 + 1;
						}
					}
				}
			}
		}
		
		using (StreamWriter reswriter = new StreamWriter(new FileStream(Path.Combine(dir, String.Format("results.{0}.txt", stest)), FileMode.Create, FileAccess.Write, FileShare.None))) {
			reswriter.WriteLine("Minima: {0} %", loc(numlocal, numtotal));
			reswriter.WriteLine("Pixels: {0} %", loc(n1, numtotal));
			reswriter.WriteLine("Trel: {0}/{1}", trel(par1, numtotal), trel(par2, numtotal));
			reswriter.WriteLine("Bad pixels: {0}", n2);
			reswriter.WriteLine("Total pixels: {0}", numtotal);
			if (n2 > 0) {
				Console.WriteLine("Bad pixels test {0}", stest);
			}
		}
	}

	public static string trel(double p, int n) {
		return (p / n).ToString("F0");
	}
	
	public static string loc(double d, int n) {
		return (100 * d / n).ToString("F1");
	}
	
	public static void SaveFitData(string dir, TestModel test, double[] t, double[] y, double[] xmap1, double[] xmap2, string stest, int ip) {
		int i;
		string dstr = Path.Combine(dir, String.Format("d.{0}.csv", stest));
//		string dstr = Path.Combine(dir, String.Format("error.{0}.csv", ip));
		double res1 = xmap1[test.npar - 1];
		double res2 = xmap2[test.npar - 1];
		if (!File.Exists(dstr)) {
			test.SetT(t);
			test.SetY(y);
			if ((test.IsDifferentLocal(res1, res2)) && (test.InRangeT(res1, res2))) {
				using (StreamWriter dwriter = new StreamWriter(new FileStream(dstr, FileMode.Create, FileAccess.Write, FileShare.None))) {
					dwriter.WriteLine("x,y");
					for (i = 0; i < test.trel.Length; i++) {
						dwriter.WriteLine("{0},{1}", test.trel[i], test.mresidual[i]);
					}
				}
				using (StreamWriter dwriter = new StreamWriter(new FileStream(Path.Combine(dir, String.Format("d.{0}.txt", stest)), FileMode.Create, FileAccess.Write, FileShare.None))) {
//				using (StreamWriter dwriter = new StreamWriter(new FileStream(Path.Combine(dir, String.Format("error.{0}.txt", ip)), FileMode.Create, FileAccess.Write, FileShare.None))) {
					for (i = 0; i < test.npar; i++) {
						dwriter.WriteLine("{0}\t{1}", xmap1[i], xmap2[i]);
					}
				//	dwriter.WriteLine("TI\tS\tS1\tS2");
					for (i = 0; i < t.Length; i++) {
					//	dwriter.WriteLine("{0}\t{1}\t{2}\t{3}", t[i], y[i], test.CalcFunc(t[i], xmap1), test.CalcFunc(t[i], xmap2));
					}
				}
				rdata.Add(String.Format("\tproc_ferror(d, \"{0}\", {1})", Path.GetFileName(dir), ip));
			}
		}
	}
}