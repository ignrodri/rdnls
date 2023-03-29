function s = sumsqr(ti, y, p, modelfun)
    s = 0;
    for i = 1 : length(ti)
        yy = y(i) - modelfun(p, ti(i));
        s = s + yy .* yy;
    end
end