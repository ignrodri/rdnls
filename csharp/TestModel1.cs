using System;
using System.IO;

// Model 1
// Inversion Recovery
// S = A + B * Abs(1 - 2 * Exp(-T / Trel))
// A >= 0 

class TestModel1 : TestModel {

	StrVec1[] vx;
	StrVec1 vy;
	
	public TestModel1() {
		npar = 3;
		minT = 100;
		maxT = 3000;
		minRangeT = 600;
		maxRangeT = 1000;
		ttol = 1;
		mresidual = new double[c];
		trel = Util.TRel(minT / 2, 1.1 * maxT, c);
	}
	
	public override void SetT(double[] tt) {
		t = new double[tt.Length];
		Array.Copy(tt, t, t.Length);
		a = t.Length;
		b = 1;
		double[] x = new double[a];
		vx = new StrVec1[c];
		int i, j;
		for (i = 0; i < c; i++) {
			for (j = 0; j < a; j++) {
				x[j] = Math.Abs(1 - 2 * Math.Exp(-t[j] / trel[i]));
			}
			vx[i] = new StrVec1(x);
		}
	}
	
	public override void SetY(double[] y) {
		int i;
		LinearResult res;
		vy = new StrVec1(y);
		for (i = 0; i < c; i++) {
			LinearModel1.CalcFit(vx[i], vy, out res);
			mresidual[i] = res.residual;
		}
	}
	
	public override double[] FitResult(int nmin) {
		if (nmin == 0) {
			Console.WriteLine("Error gordo");
		}
		LinearResult res;
		double[] result = new double[4];
		LinearModel1.CalcFit(vx[nmin], vy, out res);
		result[0] = res.intercept;
		result[1] = res.slope;
		result[2] = trel[nmin];
		result[3] = res.residual;
		return result;
	}

	public override double CalcFunc(double t, double[] pars) {
		return pars[0] + Math.Abs(pars[1] * (1 - 2 * Math.Exp(-t / pars[2])));
	}

	public override double StartTRel() {
		double res = trel[0];
		double vmin = vy.v[0];
		for (int i = 0; i < vy.n; i++) {
			if (vy.v[i] < vmin) {
				vmin = vy.v[i];
				res = t[i];
			}
		}
		return res / Math.Log(2);
	}
}