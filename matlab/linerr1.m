function err = linerr1(x, ti, y)
    modelfun = @(p,x) p(1) + p(2) .* abs(1 - 2 .* exp(-x ./ p(3)));
    A = zeros(length(ti), 2);
    A(:, 1) = 1;
    A(:, 2) = abs(1 - 2 .* exp(-ti ./ x(1)));
    c = A \ y;
    err = sumsqr(ti, y, [c(1), c(2), x(1)], modelfun);
    if (c(1) < 0)
        modelfun = @(p,x) p(1) .* abs(1 - 2 .* exp(-x ./ p(2)));
        A = zeros(length(ti), 1);
        A(:, 1) = abs(1 - 2 .* exp(-ti ./ x(1)));
        c = A \ y;
        err = sumsqr(ti, y, [c(1), x(1)], modelfun);
    end
end