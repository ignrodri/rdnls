using System;
using System.Collections;
using System.IO;

class UtilBruker {

	public static double[] ReadPar(string f, int n1, string name) {
		double[] res = new double[n1];
		string[] str = File.ReadAllLines(f);
		int ns = 0;
		for (int i = 0; i < str.Length; i++) {
			if (str[i].Contains(name)) {
				i++;
				while ((!str[i].StartsWith("#")) && (!str[i].StartsWith("$"))) {
					string[] xstr = str[i].Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < xstr.Length; j++) {
					//	Console.WriteLine(xstr[j]);
						res[ns] = Double.Parse(xstr[j]);
						ns++;
					}
					i++;
				}
			}
		}
		return res;
	}
	
	public static string ReadParString(string f, string name) {
		string[] str = File.ReadAllLines(f);
		for (int i = 0; i < str.Length; i++) {
			if (str[i].Contains(name)) {
				string[] xstr = str[i].Split(new string[] {"="}, StringSplitOptions.RemoveEmptyEntries);
				if (xstr.Length == 2) {
					return xstr[1];
				}
			}
		}
		return "";
	}
	
	public static double[,] Scale16(string dir, int np, int nt) {
		double[] scalefactor = ReadPar(Path.Combine(dir, "visu_pars"), nt, "VisuCoreDataSlope");
		double[] scaleoffset = ReadPar(Path.Combine(dir, "visu_pars"), nt, "VisuCoreDataOffs");
		double[,] im = new double[np, nt];
		int ni;
		using (BinaryReader br = new BinaryReader(new FileStream(Path.Combine(dir, "2dseq"), FileMode.Open, FileAccess.Read, FileShare.None))) {
			for (int it = 0; it < nt; it++) {
				for (int i = 0; i < np; i++) {
					ni = br.ReadInt16();
					im[i, it] = scaleoffset[it] + ni * scalefactor[it];
				}
			}
		}
		return im;
	}
	
	public static double[,] Scale32(string dir, int np, int nt) {
		double[] scalefactor = ReadPar(Path.Combine(dir, "visu_pars"), nt, "VisuCoreDataSlope");
		double[] scaleoffset = ReadPar(Path.Combine(dir, "visu_pars"), nt, "VisuCoreDataOffs");
	//	Console.WriteLine("{0} {1} {2}", dir, scalefactor[4], scaleoffset[4]);
		double[,] im = new double[np, nt];
		int ni;
		using (BinaryReader br = new BinaryReader(new FileStream(Path.Combine(dir, "2dseq"), FileMode.Open, FileAccess.Read, FileShare.None))) {
			for (int it = 0; it < nt; it++) {
				for (int i = 0; i < np; i++) {
					ni = br.ReadInt32();
					im[i, it] = scaleoffset[it] + ni * scalefactor[it];
				}
			}
		}
		return im;
	}
	
	static int GetB(bool[] b) {
		int res = 0;
		for (int i = 0; i < b.Length; i++) {
			if (b[i]) {
				res = res + 1;
			}
		}
		return res;
	}
	
	public static double[,] FilterIsa(double[,] origim, bool[] b) {
		int np = origim.GetLength(0);
		int nt = origim.GetLength(1);
		int nb = GetB(b);
		double[,] im = new double[np, nb];
		int j = 0;
		for (int it = 0; it < nt; it++) {
			if (b[it]) {
				for (int ip = 0; ip < np; ip++) {
					im[ip, j] = origim[ip, it];
				}
				j++;
			}
		}
		return im;
	}
	
}