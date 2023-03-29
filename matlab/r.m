function res = r()
    res = 2;
    y = [0 1 3 2 4 NaN];
    yy = isnan(y);
    if (sum(yy(:)) > 0)
        res = 3;
    end
end