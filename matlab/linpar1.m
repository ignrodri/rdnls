function c = linpar1(x, ti, y)
    A = zeros(length(ti), 2);
    A(:, 1) = 1;
    A(:, 2) = abs(1 - 2 .* exp(-ti ./ x(1)));
    c = A \ y;
    if (c(1) < 0)
        A = zeros(length(ti), 1);
        A(:, 1) = abs(1 - 2 .* exp(-ti ./ x(1)));
        cc = A \ y;
        c = [0, cc(1)];
    end
end