using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Util {

	public static Random ran = new Random();

	public static int LessThan(double a, double b) {
		return ((a < b) ? 1 : 0);
	}

	public static double[] TRel(double tmin, double tmax, int n) {
		double[] res = new double[n];
		double x = (tmax - tmin) / (n - 1);
		for (int i = 0; i < n; i++) {
			res[i] = tmin + x * i;
		}
		return res;
	}

	public static void SaveEmpty(string dir, int np, int nt, int[] lnum) {
		String f;
		double[,] im = MakeEmpty(np, nt);
		foreach (int i in lnum) {
			f = Path.Combine(dir, String.Format("map{0}.image", i));
			if (!File.Exists(f)) {
				SaveIm(f, im);
			}
		}
		byte[,] nlocal = new byte[np, 1];
		for (int ip = 0; ip < np; ip++) {
			nlocal[ip, 0] = 0;
		}
		f = Path.Combine(dir, "local.image");
		if (!File.Exists(f)) {
			SaveB(f, nlocal);
		}
	}

	public static double[,] MakeEmpty(int np, int nt) {
		double[,] res = new double[np, nt];
		for (int it = 0; it < nt; it++) {
			for (int ip = 0; ip < np; ip++) {
				res[ip, it] = 0;
			}
		}
		return res;
	}
	
	public static double[,] LoadIm(string f) {
		double[,] res = new double[0, 0];
		int np = 0;
		int nt = 0;
		using (BinaryReader br = new BinaryReader(new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.None))) {
			np = br.ReadInt32();
			nt = br.ReadInt32();
			res = new double[np, nt];
			for (int it = 0; it < nt; it++) {
				for (int ip = 0; ip < np; ip++) {
					res[ip, it] = br.ReadDouble();
				}
			}
		}
		return res;
	}
	
	public static void SaveIm(string f, double[,] im) {
		MakeParent(f);
		using (BinaryWriter bw = new BinaryWriter(new FileStream(f, FileMode.Create, FileAccess.Write, FileShare.None))) {
			bw.Write((int)im.GetLength(0));
			bw.Write((int)im.GetLength(1));
			for (int it = 0; it < im.GetLength(1); it++) {
				for (int ip = 0; ip < im.GetLength(0); ip++) {
					bw.Write(im[ip, it]);
				}
			}
		}
	}

	public static byte[,] LoadB(string f) {
		byte[,] res = new byte[0, 0];
		int np = 0;
		int nt = 0;
		using (BinaryReader br = new BinaryReader(new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.None))) {
			np = br.ReadInt32();
			nt = br.ReadInt32();
			res = new byte[np, nt];
			for (int it = 0; it < nt; it++) {
				for (int ip = 0; ip < np; ip++) {
					res[ip, it] = br.ReadByte();
				}
			}
		}
		return res;
	}
	
	public static void SaveB(string f, byte[,] im) {
		MakeParent(f);
		using (BinaryWriter bw = new BinaryWriter(new FileStream(f, FileMode.Create, FileAccess.Write, FileShare.None))) {
			bw.Write((int)im.GetLength(0));
			bw.Write((int)im.GetLength(1));
			for (int it = 0; it < im.GetLength(1); it++) {
				for (int ip = 0; ip < im.GetLength(0); ip++) {
					bw.Write(im[ip, it]);
				}
			}
		}
	}

	public static double[] LoadT(string dir) {
		double[] res = new double[0];
		string f = Path.Combine(dir, "t.vector");
		using (BinaryReader br = new BinaryReader(new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.None))) {
			res = new double[br.ReadInt32()];
			for (int i = 0; i < res.Length; i++) {
				res[i] = br.ReadDouble();
			}
		}
		return res;
	}

	public static void SaveT(string xdir, double[] ti) {
		CreateDirectory(xdir);
		string f = Path.Combine(xdir, "t.vector");
		using (BinaryWriter bw = new BinaryWriter(new FileStream(f, FileMode.Create, FileAccess.Write, FileShare.None))) {
			bw.Write((int)ti.Length);
			for (int i = 0; i < ti.Length; i++) {
				bw.Write(ti[i]);
			}
		}
	}
	
	public static void MakeParent(string f) {
		Thread.Sleep(1000);
		CreateDirectory(Path.GetDirectoryName(f));
		Thread.Sleep(1000);
	}
	
	public static void CreateDirectory(string dir) {
		if (!Directory.Exists(dir)) {
			MakeParent(dir);
			Directory.CreateDirectory(dir);
		}
	}
	
	public static void Destroy(string f) {
		string[] x;
		int i;
		if (Directory.Exists(f)) {
			x = Directory.GetDirectories(f);
			for (i = 0; i < x.Length; i++) {
				Destroy(x[i]);
			}
			x = Directory.GetFiles(f);
			for (i = 0; i < x.Length; i++) {
				Destroy(x[i]);
			}
			Thread.Sleep(1000);
		}
		if (File.Exists(f)) {
			Thread.Sleep(1000);
			File.Delete(f);
		}
	}

	public static void ClearText(string f) {
		string[] x;
		int i;
		if (Directory.Exists(f)) {
			x = Directory.GetDirectories(f);
			for (i = 0; i < x.Length; i++) {
				ClearText(x[i]);
			}
			x = Directory.GetFiles(f, "*.txt");
			for (i = 0; i < x.Length; i++) {
				ClearText(x[i]);
			}
			x = Directory.GetFiles(f, "*.csv");
			for (i = 0; i < x.Length; i++) {
				ClearText(x[i]);
			}
			Thread.Sleep(1000);
		}
		if (File.Exists(f)) {
			Thread.Sleep(1000);
			File.Delete(f);
		}
	}
}