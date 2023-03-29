function c = fitx3(ti, y, neldermead)
    c = zeros(1, 2);
    maxy = max(y(:));
    st2 = t2start(ti, y);
    modelfun = @(p,x) p(1) .* exp(-x ./ p(2));
    startval = [maxy, st2];
    if (neldermead > 0)
        minf = @(x) sumsqr(ti, y, x, modelfun);
        options = optimset('MaxFunEvals', 10000);
        res = fminsearch(minf, startval, options);
        c(1, 1) = res(1);
        c(1, 2) = res(2);
    else
        startval = [maxy st2];
        mdl = fitnlm(ti, y, modelfun, startval);
        c(1, 1) = mdl.Coefficients.Estimate(1);
        c(1, 2) = mdl.Coefficients.Estimate(2);
    end
end