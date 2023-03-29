function testx(imdir, imap, model, neldermead)
    smap = sprintf('map%d.image', imap);
    t = loadt(imdir);
    im = loadim(fullfile(imdir, 'original.image'));
    mask = loadb(fullfile(imdir, 'mask.image'));
    s = size(im);
    map = loadim(fullfile(imdir, smap));
    pip = 0;
    tic;
    for ip = 1 : s(1)
        if (mask(ip, 1) > 0)
            y = squeeze(im(ip, :));
            %checknan(y, imdir, sprintf('Error in var y test %d point %d', imap, ip));
            pip = pip + 1;
            try
                if (model == 1)
                    %checknan(t1start(t, y), imdir, sprintf('Error in t1start test %d point %d', imap, ip));
                    map(ip, :) = fitx1(t, y, neldermead);
                end
                if (model == 2)
                    %checknan(t1start(t, y), imdir, sprintf('Error in t1start test %d point %d', imap, ip));
                    map(ip, :) = fitx2(t, y, neldermead);
                end
                if (model == 3)
                    %checknan(t2start(t, y), imdir, sprintf('Error in t2start test %d point %d', imap, ip));
                    map(ip, :) = fitx3(t, y, neldermead);
                end
            catch
                map(ip, :) = 0;
            end
        end
    end
    time = toc;
    ftext = fopen(fullfile(imdir, sprintf('time.%s.txt', smap)), 'w');
    fprintf(ftext, 'Exec time = %f ms', (1000 * time / pip));
    fclose(ftext);
    saveim(fullfile(imdir, smap), map);
end

