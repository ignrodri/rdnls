function im = loadb(f)
    x = fopen(f, 'r', 'ieee-le');
    np = fread(x, 1, 'int32');
    nt = fread(x, 1, 'int32');
    n = np .* nt;
    im = fread(x, n, 'int8');
    im = reshape(im, np, nt);
    fclose(x);
end