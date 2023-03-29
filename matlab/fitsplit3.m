function c = fitsplit3(ti, y)
    c = zeros(1, 2);
    x0 = t2start(ti, y);
    n = numel(ti);
    tt = reshape(ti, n, 1);
    yy = reshape(y, n, 1);
    fitmodel = @(x, t) fitmodel3(x, t, yy);
    [x, ~, ~, ~, ~] = lsqcurvefit(fitmodel, x0, tt, yy);
    lin = linpar3(x, tt, yy);
    c(1, 1) = lin(1);
    c(1, 2) = x(1);
end