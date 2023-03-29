function igwhat()
    y = [18 15 15 12 9 5 4 9 12 11 12 21 21 28 27 32];
    ti = [35, 50, 75, 100, 200, 250, 350, 500, 650, 800, 1000, 1500, 2000, 3000, 5000, 7000];
    t1start(ti, y)
    %fitmodel1([800], ti, y)
    %linpar1([800], ti, y)
    %[A, B, T1] = fitsplit(ti, y, 1)
    %[A, B, T1] = fitsplit(ti, y, 3)
    %[A, B, T1] = fitx(ti, y, 1, 1)
end
