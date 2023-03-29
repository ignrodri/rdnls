function c = fitx2(ti, y, neldermead)
    c = zeros(1, 3);
    maxy = max(y(:));
    st1 = t1start(ti, y);
    modelfun = @(p,x) abs(p(1) - p(2) .* exp(-x ./ p(3)));
    startval = [maxy, 2 .* maxy, st1];
    if (neldermead > 0)
        minf = @(x) sumsqr(ti, y, x, modelfun);
        options = optimset('MaxFunEvals', 10000);
        res = fminsearch(minf, startval, options);
        c(1, 1) = res(1);
        c(1, 2) = res(2);
        c(1, 3) = res(3);
    else
        startval = [maxy 2 .* maxy st1];
        mdl = fitnlm(ti, y, modelfun, startval);
        c(1, 1) = mdl.Coefficients.Estimate(1);
        c(1, 2) = mdl.Coefficients.Estimate(2);
        c(1, 3) = mdl.Coefficients.Estimate(3);
    end
end