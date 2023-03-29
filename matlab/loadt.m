function t = loadt(imdir)
    x = fopen(fullfile(imdir, 't.vector'), 'r', 'ieee-le');
    n = fread(x, 1, 'int32');
    t = fread(x, n, 'float64');
    t = reshape(t, 1, n);
    fclose(x);
end