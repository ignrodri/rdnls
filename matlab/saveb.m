function saveb(f, map)
    x = fopen(f, 'w', 'ieee-le');
    s = size(map);
    fwrite(x, s, 'int32');
    fwrite(x, map, 'int8');
    fclose(x);
    pause(10);
end    