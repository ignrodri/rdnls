function test(xdir)
    dir1 = fullfile(xdir, 'transformedimages', 'dataset1');
    %dir2 = fullfile(xdir, 'transformedimages', 'dataset2');
    %dir3 = fullfile(xdir, 'transformedimages', 'dataset3');
    %dir4 = fullfile(xdir, 'transformedimages', 'dataset4');
    testx(dir1, 3, 1, 0);
    testx(dir1, 4, 1, 1);
    testsplit(dir1, 5, 1);
    %testx(dir2, 2, 2, 0);
    %testx(dir2, 3, 2, 1);
    %testsplit(dir2, 4, 2);
    %testx(dir3, 2, 2, 0);
    %testx(dir3, 3, 2, 1);
    %testsplit(dir3, 4, 2);
    %testx(dir4, 3, 3, 0);
    %testx(dir4, 4, 3, 1);
    %testsplit(dir4, 5, 3);
end

