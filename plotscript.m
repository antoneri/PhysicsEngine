load('rope-convergence.txt');
rope_convergence = sortrows(rope_convergence);
i = rope_convergence(:,1);
err = rope_convergence(:,2);
sigma = rope_convergence(:,3);

group = [
	1 * ones(sum(i == 1), 1)
	2 * ones(sum(i == 2), 1)
	3 * ones(sum(i == 3), 1)
	4 * ones(sum(i == 4), 1)
	5 * ones(sum(i == 5), 1)
	6 * ones(sum(i == 6), 1)
	7 * ones(sum(i == 7), 1)
	8 * ones(sum(i == 8), 1)
	9 * ones(sum(i == 9), 1)
	10 * ones(sum(i == 10), 1)
	11 * ones(sum(i == 11), 1)
	12 * ones(sum(i == 12), 1)
	13 * ones(sum(i == 13), 1)
	14 * ones(sum(i == 14), 1)
	15 * ones(sum(i == 15), 1)
	16 * ones(sum(i == 16), 1)
	17 * ones(sum(i == 17), 1)
	18 * ones(sum(i == 18), 1)
	19 * ones(sum(i == 19), 1)
	20 * ones(sum(i == 20), 1)
	21 * ones(sum(i == 21), 1)
	22 * ones(sum(i == 22), 1)
	23 * ones(sum(i == 23), 1)
	24 * ones(sum(i == 24), 1)
	25 * ones(sum(i == 25), 1)
	26 * ones(sum(i == 26), 1)
	27 * ones(sum(i == 27), 1)
	28 * ones(sum(i == 28), 1)
	29 * ones(sum(i == 29), 1)
	30 * ones(sum(i == 30), 1)
	31 * ones(sum(i == 31), 1)
	32 * ones(sum(i == 32), 1)
	33 * ones(sum(i == 33), 1)
	34 * ones(sum(i == 34), 1)
	35 * ones(sum(i == 35), 1)
	36 * ones(sum(i == 36), 1)
	37 * ones(sum(i == 37), 1)
	38 * ones(sum(i == 38), 1)
	39 * ones(sum(i == 39), 1)
	40 * ones(sum(i == 40), 1)
	41 * ones(sum(i == 41), 1)
	42 * ones(sum(i == 42), 1)
	43 * ones(sum(i == 43), 1)
	44 * ones(sum(i == 44), 1)
	45 * ones(sum(i == 45), 1)
	46 * ones(sum(i == 46), 1)
	47 * ones(sum(i == 47), 1)
	48 * ones(sum(i == 48), 1)
	49 * ones(sum(i == 49), 1)
	50 * ones(sum(i == 50), 1)
	51 * ones(sum(i == 51), 1)
	52 * ones(sum(i == 52), 1)
	53 * ones(sum(i == 53), 1)
	54 * ones(sum(i == 54), 1)
	55 * ones(sum(i == 55), 1)
	56 * ones(sum(i == 56), 1)
	57 * ones(sum(i == 57), 1)
	58 * ones(sum(i == 58), 1)
	59 * ones(sum(i == 59), 1)
	60 * ones(sum(i == 60), 1)
	61 * ones(sum(i == 61), 1)
	62 * ones(sum(i == 62), 1)
	63 * ones(sum(i == 63), 1)
	64 * ones(sum(i == 64), 1)
	65 * ones(sum(i == 65), 1)
	66 * ones(sum(i == 66), 1)
	67 * ones(sum(i == 67), 1)
	68 * ones(sum(i == 68), 1)
	69 * ones(sum(i == 69), 1)
	70 * ones(sum(i == 70), 1)
	71 * ones(sum(i == 71), 1)
	72 * ones(sum(i == 72), 1)
	73 * ones(sum(i == 73), 1)
	74 * ones(sum(i == 74), 1)
	75 * ones(sum(i == 75), 1)
	76 * ones(sum(i == 76), 1)
	77 * ones(sum(i == 77), 1)
	78 * ones(sum(i == 78), 1)
	79 * ones(sum(i == 79), 1)
	80 * ones(sum(i == 80), 1)
	81 * ones(sum(i == 81), 1)
	82 * ones(sum(i == 82), 1)
	83 * ones(sum(i == 83), 1)
	84 * ones(sum(i == 84), 1)
	85 * ones(sum(i == 85), 1)
	86 * ones(sum(i == 86), 1)
	87 * ones(sum(i == 87), 1)
	88 * ones(sum(i == 88), 1)
	89 * ones(sum(i == 89), 1)
	90 * ones(sum(i == 90), 1)
	91 * ones(sum(i == 91), 1)
	92 * ones(sum(i == 92), 1)
	93 * ones(sum(i == 93), 1)
	94 * ones(sum(i == 94), 1)
	95 * ones(sum(i == 95), 1)
	96 * ones(sum(i == 96), 1)
	97 * ones(sum(i == 97), 1)
	98 * ones(sum(i == 98), 1)
	99 * ones(sum(i == 99), 1)
	100 * ones(sum(i == 100), 1)
];


figure(1);
boxplot(err, group);
title('Convergence of error over number of iterations');
xlabel('Number of iterations n'), ylabel('Absolute error \delta_n');

figure(2);
boxplot(sigma, group);
title('Change in constraint force');
xlabel('Number of iterations n'), ylabel('\sigma');

[fitresult, gof] = createfit(i, log(sigma));
fitresult

f = 50:10:100;
timesteps = 1./f;

