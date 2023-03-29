using System;
using System.IO;

// Model 2
// Inversion Recovery or MOLLI
// S = Abs(A - B * Exp(-T / Trel))

class TestModel2 : TestModel {

	StrVec2[] vx;
	StrVec2[] vy;
	
	public TestModel2() {
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
		b = t.Length;
		double[] x = new double[a];
		vx = new StrVec2[c];
		int i, j;
		for (i = 0; i < c; i++) {
			for (j = 0; j < a; j++) {
				x[j] = -Math.Exp(-t[j] / trel[i]);
			}
			vx[i] = new StrVec2(x);
		}
	}
	
	public override void SetY(double[] y) {
		int i, j, k;
		double res, nres, ssxy;
		vy = new StrVec2[b];
		double[] yy = new double[a];
		for (j = 0; j < b; j++) {
			for (i = 0; i < a; i++) {
				yy[i] = (j > i ? -y[i] : y[i]);
			}
			vy[j] = new StrVec2(yy);
		}
		for (i = 0; i < c; i++) {
			ssxy = 0;
			for (k = 0; k < a; k++) {
				ssxy = ssxy + vx[i].vv[k] * vy[0].v[k];
			}
			res = vy[0].ssv2 - ssxy * ssxy / vx[i].ssv2;
			for (j = 1; j < b; j++) {
				ssxy = ssxy - 2 * vx[i].vv[j - 1] * vy[0].v[j - 1];
				nres = vy[j].ssv2 - ssxy * ssxy / vx[i].ssv2;
				if (nres < res) {
					res = nres;
				}
			}
			mresidual[i] = res;
		}
	}

	public override double[] FitResult(int nmin) {
		int j;
		LinearResult res;
		LinearResult nres;
		double[] result = new double[4];
		LinearModel2.CalcFit(vx[nmin], vy[0], out res);
		for (j = 1; j < b; j++) {
			LinearModel2.CalcFit(vx[nmin], vy[j], out nres);
			if (nres.residual < res.residual) {
				res = nres;
			}
		}
		result[0] = res.intercept;
		result[1] = res.slope;
		result[2] = trel[nmin];
		result[3] = res.residual;
		return result;
	}

	public override double CalcFunc(double t, double[] pars) {
		return Math.Abs(pars[0] - pars[1] * Math.Exp(-t / pars[2]));
	}

	public override double StartTRel() {
		double res = trel[0];
		double vmin = vy[0].v[0];
		for (int i = 0; i < vy[0].n; i++) {
			if (vy[0].v[i] < vmin) {
				vmin = vy[0].v[i];
				res = t[i];
			}
		}
		return res / Math.Log(2);
	}
}