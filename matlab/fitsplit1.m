function c = fitsplit1(ti, y)
    c = zeros(1, 3);
    x0 = t1start(ti, y);
    n = numel(ti);
    tt = reshape(ti, n, 1);
    yy = reshape(y, n, 1);
    fitmodel = @(x, t) fitmodel1(x, t, yy);
    [x, ~, ~, ~, ~] = lsqcurvefit(fitmodel, x0, tt, yy);
    lin = linpar1(x, tt, yy);
    c(1, 1) = lin(1);
    c(1, 2) = lin(2);
    c(1, 3) = x(1);
end