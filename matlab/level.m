function c = level(im, x)
    a = min(im(:));
    b = max(im(:));
    n = sum(im(:) >= a);
    %size(im)
    %size(im >= a)
    for i = 1 : 100
        c = (a + b) / 2;
        s = sum(im(:) <= c) / n;
        if s < x
            a = c;
        else
            b = c;
        end
    end
end