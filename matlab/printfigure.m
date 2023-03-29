function printfigure(im, f, t)
    figure;
    colormap(jet);
    colorbar;
    imagesc(im, [level(im(im ~= 0), 0.02), level(im(im ~= 0), 0.98)]);
    colorbar;
    axis off;
    title(t);
    %saveas(gcf, f);
    print('-dtiff', '-r300', f);
end    