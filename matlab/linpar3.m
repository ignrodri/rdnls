function c = linpar3(x, ti, y)
    A = zeros(length(ti), 1);
    A(:, 1) = exp(-ti ./ x(1));
    c = A \ y;
end