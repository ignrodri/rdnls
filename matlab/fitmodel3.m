function yEst = fitmodel3(x, ti, y)
    modelfun = @(p,x) p(1) .* exp(-x ./ p(2));
    A = zeros(length(ti), 1);
    A(:, 1) = -exp(-ti ./ x(1));
    c = A \ y;
    yEst = A * c;
end