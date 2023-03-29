function err = linerr2(x, ti, y)
    modelfun = @(p,x) abs(p(1) - p(2) .* exp(-x ./ p(3)));
    A = zeros(length(ti), 2);
    A(:, 1) = 1;
    A(:, 2) = -exp(-ti ./ x(1));
    yy = y;
    c = A \ y;
    err = sumsqr(ti, y, [c(1), c(2), x(1)], modelfun);
    for i = 1 : length(ti)
        yy(i) = -yy(i);
        c2 = A \ yy;
        err2 = sumsqr(ti, y, [c2(1), c2(2), x(1)], modelfun);
        if (err2 < err)
            err = err2;
        end
    end
end