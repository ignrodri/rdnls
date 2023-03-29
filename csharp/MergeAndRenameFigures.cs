using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

class MergeAndRenameFigures {

	static void Main(string[] args) {
		if (args.Length == 1) {
			string figdir = Path.Combine(args[0], "figures");
			CopyFigs(figdir, "trelvalues1.tif", "figure1.tif");
			CopyFigs(figdir, "trelvalues8.tif", "figure2.tif");
			CopyFigs(figdir, "ferror2.tif", "figure3.tif");
			for (int i = 0; i < 14; i++) {
				CopyFigs(figdir, String.Format("trelvalues{0}.tif", i + 1), String.Format("supfigure{0}.tif", i + 1));
			}
		}
	}
	
	static void CopyFigs(string dir, string o, string d) {
		string orig = Path.Combine(dir, o);
		string dest = Path.Combine(dir, d);
		File.Copy(orig, dest, true);
	}
}