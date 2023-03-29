using System;
using System.IO;

// Model 3
// Spin echo or Gradient echo (simplified)
// S = A * Exp(-T / Trel)

class TestModel3 : TestModel {

	StrVec3[] vx;
	StrVec3 vy;
	
	public TestModel3() {
		npar = 2;
		minT = 10;
		maxT = 500;
		minRangeT = 50;
		maxRangeT = 150;
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
		vx = new StrVec3[c];
		int i, j;
		for (i = 0; i < c; i++) {
			for (j = 0; j < a; j++) {
				x[j] = Math.Exp(-t[j] / trel[i]);
			}
			vx[i] = new StrVec3(x);
		}
	}
	
	public override void SetY(double[] y) {
		LinearResult res;
		vy = new StrVec3(y);
		for (int i = 0; i < c; i++) {
			LinearModel3.CalcFit(vx[i], vy, out res);
			mresidual[i] = res.residual;
		}
	}
	
	public override double[] FitResult(int nmin) {
		LinearResult res;
		double[] result = new double[3];
		LinearModel3.CalcFit(vx[nmin], vy, out res);
		result[0] = res.slope;
		result[1] = trel[nmin];
		result[2] = res.residual;
		return result;
	}

	public override double CalcFunc(double t, double[] pars) {
		return pars[0] * Math.Exp(-t / pars[1]);
	}

	public override double StartTRel() {
		LinearResult res;
		double[] vlog = new double[vy.n];
		for (int i = 0; i < vy.n; i++) {
			vlog[i] = (vy.v[i] > 0 ? Math.Log(vy.v[i]) : 0);
		}
		StrVec2 vl = new StrVec2(vlog);
		StrVec2 vt = new StrVec2(t);
		LinearModel2.CalcFit(vt, vl, out res);
		return 1.0 / res.slope;
	}
}