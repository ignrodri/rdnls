function checknan(y, imdir, s)
    yy = isnan(y);
    if (sum(yy(:)) > 0)
        ftext = fopen(fullfile(imdir, 'error.txt'), 'w');
        fprintf(ftext, s);
        fclose(ftext);
    end
end