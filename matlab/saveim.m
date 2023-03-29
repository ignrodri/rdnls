function saveim(f, map)
    x = fopen(f, 'w', 'ieee-le');
    s = size(map);
    fwrite(x, s, 'int32');
    fwrite(x, map, 'float64');
    fclose(x);
    pause(10);
    fprintf('%s\n', datetime);
end    