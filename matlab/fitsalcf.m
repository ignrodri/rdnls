function c = fitsalcf1(ti, y, tmin, tmax, tstep, epsilon)
    c = zeros(1, 3);
    n = numel(ti);
    tt = reshape(ti, n, 1);
    yy = reshape(y, n, 1);
    x = tmax - tmin;
    if (x < epsilon)
    	xa = (tmin + tmax) / 2;
    	lin = linpar1(xa, 
    erra = linerr1(tmin, ti, y);
    xa = tmin;
    
    fitmodel = @(x, t) fitmodel1(x, t, yy);
    [x, ~, ~, ~, ~] = lsqcurvefit(fitmodel, x0, tt, yy);
    lin = linpar1(x, tt, yy);
    c(1, 1) = lin(1);
    c(1, 2) = lin(2);
    c(1, 3) = x(1);
end