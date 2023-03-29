function figures(xdir)
    imdir = fullfile(xdir, 'transformedimages');
    map = loadim(fullfile(imdir, 'dataset1', 'map1.image'));
    map = reshape(map, 80, 80, 3, 3);
    map = squeeze(map(:, :, 2, 3));
    map = permute(map, [2, 1]);
    printfigure(map, fullfile(xdir, 'figure4.tif'), 'Mouse T1 map (ms)');
    map = loadim(fullfile(imdir, 'dataset3', 'map1.image'));
    map = reshape(map, 181, 217, 11, 3);
    map = squeeze(map(:, :, 6, 3));
    mapresized = zeros(217, 217);
    mapresized(19 : 199, :) = map;
    mapresized = permute(mapresized, [2, 1]);
    printfigure(mapresized, fullfile(xdir, 'figure5.tif'), 'Simulated T1 map (ms)');
end
    
     
    
    