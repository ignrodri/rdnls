function st2 = t2start(ti, y)
    A = zeros(length(ti), 2);
    m = max(y(:)) * 0.01;
    A(:, 1) = 1;
    A(:, 2) = -ti;
    n = numel(ti);
    yy = reshape(y, n, 1);
    yy(yy < m) = m;
    c = A \ log(yy);
    st2 = 1 ./ c(2);
end