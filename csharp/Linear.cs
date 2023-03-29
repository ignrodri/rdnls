using System;
using System.IO;

struct LinearResult {

	public double intercept;
	public double slope;
	public double residual;
	
}

struct StrVec1 {

	public int n;
	public double[] v;
	public double vm;
	public double[] vv;
	public double sv2;
	public double ssv2;
	
	public StrVec1(double[] nv) {
		int i;
		n = nv.Length;
		v = new double[n];
		vv = new double[n];
		vm = 0;
		sv2 = 0;
		ssv2 = 0;
		for (i = 0; i < n; i++) {
			v[i] = nv[i];
			vm = vm + v[i] / n;
			sv2 = sv2 + v[i] * v[i];
		}
		for (i = 0; i < n; i++) {
			vv[i] = v[i] - vm;
			ssv2 = ssv2 + vv[i] * vv[i];
		}
	}
}

struct StrVec2 {

	public int n;
	public double[] v;
	public double vm;
	public double[] vv;
	public double sv2;
	public double ssv2;
	
	public StrVec2(double[] nv) {
		int i;
		n = nv.Length;
		v = new double[n];
		vv = new double[n];
		vm = 0;
		sv2 = 0;
		ssv2 = 0;
		for (i = 0; i < n; i++) {
			v[i] = nv[i];
			vm = vm + v[i] / n;
			sv2 = sv2 + v[i] * v[i];
		}
		for (i = 0; i < n; i++) {
			vv[i] = v[i] - vm;
			ssv2 = ssv2 + vv[i] * vv[i];
		}
	}
}

struct StrVec3 {

	public int n;
	public double[] v;
	public double sv2;
	
	public StrVec3(double[] nv) {
		int i;
		n = nv.Length;
		v = new double[n];
		sv2 = 0;
		for (i = 0; i < n; i++) {
			v[i] = nv[i];
			sv2 = sv2 + v[i] * v[i];
		}
	}
}

class LinearModel1 {

	public static void CalcFit(StrVec1 x, StrVec1 y, out LinearResult res) {
		int i;
		double sxy = 0;
		for (i = 0; i < x.n; i++) {
			sxy = sxy + x.vv[i] * y.vv[i];
		}
		res.slope = sxy / x.ssv2;
		res.intercept = y.vm - res.slope * x.vm;
		if (res.intercept < 0) {
			sxy = sxy + x.n * x.vm * y.vm;
			res.slope = sxy / x.sv2;
			res.intercept = 0;
			res.residual = y.sv2 - sxy * res.slope;
		}
		else {
			res.residual = y.ssv2 - sxy * res.slope;
		}
	}
}

class LinearModel2 {

	public static void CalcFit(StrVec2 x, StrVec2 y, out LinearResult res) {
		double ssxy = 0;
		for (int i = 0; i < x.n; i++) {
			ssxy = ssxy + x.vv[i] * y.vv[i];
		}
		res.slope = ssxy / x.ssv2;
		res.intercept = y.vm - res.slope * x.vm;
		res.residual = y.ssv2 - ssxy * res.slope;
	}
}

class LinearModel3 {

	public static void CalcFit(StrVec3 x, StrVec3 y, out LinearResult res) {
		double sxy = 0;
		for (int i = 0; i < x.n; i++) {
			sxy = sxy + x.v[i] * y.v[i];
		}
		res.slope = sxy / x.sv2;
		res.intercept = 0;
		res.residual = y.sv2 - sxy * res.slope;
	}
}