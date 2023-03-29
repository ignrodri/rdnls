function yEst = fitmodel1(x, ti, y)
    A = zeros(length(ti), 2);
    A(:, 1) = 1;
    A(:, 2) = abs(1 - 2 .* exp(-ti ./ x(1)));
    c = A \ y;
    yEst = A * c;
    if (c(1) < 0)
        A = zeros(length(ti), 1);
        A(:, 1) = abs(1 - 2 .* exp(-ti ./ x(1)));
        c = A \ y;
        yEst = A * c;
    end
end