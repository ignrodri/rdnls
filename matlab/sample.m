function y = sample()
    map = zeros(1, 2);
    im = loadim('i:\documentos\t1test\transformedimages\dataset3\original.image');
    t = loadt('i:\documentos\t1test\transformedimages\dataset3');
    y = im(29282, :);
    map(1, :) = fitx9(t, y, 0);
    y = map;
end