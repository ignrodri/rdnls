using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Threading;

struct LocalMinimum {

	public double trel;
	public double error;
	
}

class MinimumComparer : IComparer<LocalMinimum> {

	public int Compare(LocalMinimum x, LocalMinimum y) {
		return x.error.CompareTo(y.error);
	}
}	

class TestModel {

	public int npar;
	public double minT;
	public double maxT;
	public double minRangeT;
	public double maxRangeT;
	public double ttol;
	
	public int a;
	public int b;
	public int c = 10000;
	
	public double[] mresidual;
	
	public double[] trel;
	public double[] t;
	
	public virtual void SetT(double[] tt) {
	}
	
	public virtual void SetY(double[] y) {
	}
	
	public int NumLocal() {
		int res = 0;
		for (int i = 1 ; i < (c - 1); i++) {
			res = res + IsLocal(i);
		}
		return res;
	}
	
	public int IsLocal(int i) {
		if (mresidual[i - 1] < mresidual[i]) {
			return 0;
		}
		if (mresidual[i + 1] < mresidual[i]) {
			return 0;
		}
		return 1;
	}

	public bool InRangeT(double x1, double x2) {
		if (x1 < minRangeT) {
			return false;
		}
		if (x1 > maxRangeT) {
			return false;
		}
		if (x2 < minRangeT) {
			return false;
		}
		if (x2 > maxRangeT) {
			return false;
		}
		return true;
	}
	
	public bool IsDifferentLocal(double tref1, double tref2) {
		int nloc = 0;
		int lt = 0;
		for (int i = 0; i < c; i++) {
			lt = Util.LessThan(trel[i], tref1) + Util.LessThan(trel[i], tref2);
			if (lt == 1) {
				nloc = nloc + IsLocal(i);
			}
		}
		return (nloc > 1);
	}
		
	public double[] FitResult() {
		int i;
		int nmin = 0;
		double vmin = mresidual[0];
		for (i = 0; i < c; i++) {
			if (mresidual[i] < vmin) {
				nmin = i;
				vmin = mresidual[i];
			}
		}
		return FitResult(nmin);
	}
	
	public virtual double[] FitResult(int nmin) {
		return new double[0];
	}
	
	public double CalcError(double[] t, double[] y, double[] pars) {
		double res = 0;
		double x;
		for (int i = 0; i < t.Length; i++) {
			x = y[i] - CalcFunc(t[i], pars);
			res = res + x * x;
		}
		return res;
	}
	
	public virtual double CalcFunc(double t, double[] pars) {
		return 0;
	}
	
	public bool Ok(double[] pars) {
		return ((pars[npar - 1] > minT) && (pars[npar - 1] < maxT));
	}
	
	public virtual double StartTRel() {
		return 0;
	}
	
	public LocalMinimum[] Minima() {
		List<LocalMinimum> res = new List<LocalMinimum>();
		LocalMinimum lm;
		for (int i = 1 ; i < (c - 1); i++) {
			if (IsLocal(i) > 0) {
				if (InRangeT(trel[i], trel[i])) {
					lm.trel = trel[i];
					lm.error = mresidual[i];
					res.Add(lm);
				}
			}
		}
		res.Sort(new MinimumComparer());
		return res.ToArray();
	}
}