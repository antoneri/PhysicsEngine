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
	101 * ones(sum(i == 101), 1)
	102 * ones(sum(i == 102), 1)
	103 * ones(sum(i == 103), 1)
	104 * ones(sum(i == 104), 1)
	105 * ones(sum(i == 105), 1)
	106 * ones(sum(i == 106), 1)
	107 * ones(sum(i == 107), 1)
	108 * ones(sum(i == 108), 1)
	109 * ones(sum(i == 109), 1)
	110 * ones(sum(i == 110), 1)
	111 * ones(sum(i == 111), 1)
	112 * ones(sum(i == 112), 1)
	113 * ones(sum(i == 113), 1)
	114 * ones(sum(i == 114), 1)
	115 * ones(sum(i == 115), 1)
	116 * ones(sum(i == 116), 1)
	117 * ones(sum(i == 117), 1)
	118 * ones(sum(i == 118), 1)
	119 * ones(sum(i == 119), 1)
	120 * ones(sum(i == 120), 1)
	121 * ones(sum(i == 121), 1)
	122 * ones(sum(i == 122), 1)
	123 * ones(sum(i == 123), 1)
	124 * ones(sum(i == 124), 1)
	125 * ones(sum(i == 125), 1)
	126 * ones(sum(i == 126), 1)
	127 * ones(sum(i == 127), 1)
	128 * ones(sum(i == 128), 1)
	129 * ones(sum(i == 129), 1)
	130 * ones(sum(i == 130), 1)
	131 * ones(sum(i == 131), 1)
	132 * ones(sum(i == 132), 1)
	133 * ones(sum(i == 133), 1)
	134 * ones(sum(i == 134), 1)
	135 * ones(sum(i == 135), 1)
	136 * ones(sum(i == 136), 1)
	137 * ones(sum(i == 137), 1)
	138 * ones(sum(i == 138), 1)
	139 * ones(sum(i == 139), 1)
	140 * ones(sum(i == 140), 1)
	141 * ones(sum(i == 141), 1)
	142 * ones(sum(i == 142), 1)
	143 * ones(sum(i == 143), 1)
	144 * ones(sum(i == 144), 1)
	145 * ones(sum(i == 145), 1)
	146 * ones(sum(i == 146), 1)
	147 * ones(sum(i == 147), 1)
	148 * ones(sum(i == 148), 1)
	149 * ones(sum(i == 149), 1)
	150 * ones(sum(i == 150), 1)
	151 * ones(sum(i == 151), 1)
	152 * ones(sum(i == 152), 1)
	153 * ones(sum(i == 153), 1)
	154 * ones(sum(i == 154), 1)
	155 * ones(sum(i == 155), 1)
	156 * ones(sum(i == 156), 1)
	157 * ones(sum(i == 157), 1)
	158 * ones(sum(i == 158), 1)
	159 * ones(sum(i == 159), 1)
	160 * ones(sum(i == 160), 1)
	161 * ones(sum(i == 161), 1)
	162 * ones(sum(i == 162), 1)
	163 * ones(sum(i == 163), 1)
	164 * ones(sum(i == 164), 1)
	165 * ones(sum(i == 165), 1)
	166 * ones(sum(i == 166), 1)
	167 * ones(sum(i == 167), 1)
	168 * ones(sum(i == 168), 1)
	169 * ones(sum(i == 169), 1)
	170 * ones(sum(i == 170), 1)
	171 * ones(sum(i == 171), 1)
	172 * ones(sum(i == 172), 1)
	173 * ones(sum(i == 173), 1)
	174 * ones(sum(i == 174), 1)
	175 * ones(sum(i == 175), 1)
	176 * ones(sum(i == 176), 1)
	177 * ones(sum(i == 177), 1)
	178 * ones(sum(i == 178), 1)
	179 * ones(sum(i == 179), 1)
	180 * ones(sum(i == 180), 1)
	181 * ones(sum(i == 181), 1)
	182 * ones(sum(i == 182), 1)
	183 * ones(sum(i == 183), 1)
	184 * ones(sum(i == 184), 1)
	185 * ones(sum(i == 185), 1)
	186 * ones(sum(i == 186), 1)
	187 * ones(sum(i == 187), 1)
	188 * ones(sum(i == 188), 1)
	189 * ones(sum(i == 189), 1)
	190 * ones(sum(i == 190), 1)
	191 * ones(sum(i == 191), 1)
	192 * ones(sum(i == 192), 1)
	193 * ones(sum(i == 193), 1)
	194 * ones(sum(i == 194), 1)
	195 * ones(sum(i == 195), 1)
	196 * ones(sum(i == 196), 1)
	197 * ones(sum(i == 197), 1)
	198 * ones(sum(i == 198), 1)
	199 * ones(sum(i == 199), 1)
	200 * ones(sum(i == 200), 1)
	201 * ones(sum(i == 201), 1)
	202 * ones(sum(i == 202), 1)
	203 * ones(sum(i == 203), 1)
	204 * ones(sum(i == 204), 1)
	205 * ones(sum(i == 205), 1)
	206 * ones(sum(i == 206), 1)
	207 * ones(sum(i == 207), 1)
	208 * ones(sum(i == 208), 1)
	209 * ones(sum(i == 209), 1)
	210 * ones(sum(i == 210), 1)
	211 * ones(sum(i == 211), 1)
	212 * ones(sum(i == 212), 1)
	213 * ones(sum(i == 213), 1)
	214 * ones(sum(i == 214), 1)
	215 * ones(sum(i == 215), 1)
	216 * ones(sum(i == 216), 1)
	217 * ones(sum(i == 217), 1)
	218 * ones(sum(i == 218), 1)
	219 * ones(sum(i == 219), 1)
	220 * ones(sum(i == 220), 1)
	221 * ones(sum(i == 221), 1)
	222 * ones(sum(i == 222), 1)
	223 * ones(sum(i == 223), 1)
	224 * ones(sum(i == 224), 1)
	225 * ones(sum(i == 225), 1)
	226 * ones(sum(i == 226), 1)
	227 * ones(sum(i == 227), 1)
	228 * ones(sum(i == 228), 1)
	229 * ones(sum(i == 229), 1)
	230 * ones(sum(i == 230), 1)
	231 * ones(sum(i == 231), 1)
	232 * ones(sum(i == 232), 1)
	233 * ones(sum(i == 233), 1)
	234 * ones(sum(i == 234), 1)
	235 * ones(sum(i == 235), 1)
	236 * ones(sum(i == 236), 1)
	237 * ones(sum(i == 237), 1)
	238 * ones(sum(i == 238), 1)
	239 * ones(sum(i == 239), 1)
	240 * ones(sum(i == 240), 1)
	241 * ones(sum(i == 241), 1)
	242 * ones(sum(i == 242), 1)
	243 * ones(sum(i == 243), 1)
	244 * ones(sum(i == 244), 1)
	245 * ones(sum(i == 245), 1)
	246 * ones(sum(i == 246), 1)
	247 * ones(sum(i == 247), 1)
	248 * ones(sum(i == 248), 1)
	249 * ones(sum(i == 249), 1)
	250 * ones(sum(i == 250), 1)
	251 * ones(sum(i == 251), 1)
	252 * ones(sum(i == 252), 1)
	253 * ones(sum(i == 253), 1)
	254 * ones(sum(i == 254), 1)
	255 * ones(sum(i == 255), 1)
	256 * ones(sum(i == 256), 1)
	257 * ones(sum(i == 257), 1)
	258 * ones(sum(i == 258), 1)
	259 * ones(sum(i == 259), 1)
	260 * ones(sum(i == 260), 1)
	261 * ones(sum(i == 261), 1)
	262 * ones(sum(i == 262), 1)
	263 * ones(sum(i == 263), 1)
	264 * ones(sum(i == 264), 1)
	265 * ones(sum(i == 265), 1)
	266 * ones(sum(i == 266), 1)
	267 * ones(sum(i == 267), 1)
	268 * ones(sum(i == 268), 1)
	269 * ones(sum(i == 269), 1)
	270 * ones(sum(i == 270), 1)
	271 * ones(sum(i == 271), 1)
	272 * ones(sum(i == 272), 1)
	273 * ones(sum(i == 273), 1)
	274 * ones(sum(i == 274), 1)
	275 * ones(sum(i == 275), 1)
	276 * ones(sum(i == 276), 1)
	277 * ones(sum(i == 277), 1)
	278 * ones(sum(i == 278), 1)
	279 * ones(sum(i == 279), 1)
	280 * ones(sum(i == 280), 1)
	281 * ones(sum(i == 281), 1)
	282 * ones(sum(i == 282), 1)
	283 * ones(sum(i == 283), 1)
	284 * ones(sum(i == 284), 1)
	285 * ones(sum(i == 285), 1)
	286 * ones(sum(i == 286), 1)
	287 * ones(sum(i == 287), 1)
	288 * ones(sum(i == 288), 1)
	289 * ones(sum(i == 289), 1)
	290 * ones(sum(i == 290), 1)
	291 * ones(sum(i == 291), 1)
	292 * ones(sum(i == 292), 1)
	293 * ones(sum(i == 293), 1)
	294 * ones(sum(i == 294), 1)
	295 * ones(sum(i == 295), 1)
	296 * ones(sum(i == 296), 1)
	297 * ones(sum(i == 297), 1)
	298 * ones(sum(i == 298), 1)
	299 * ones(sum(i == 299), 1)
	300 * ones(sum(i == 300), 1)
];

% figure(1);
% boxplot(err, group);
% title('Convergence of error over number of iterations');
% xlabel('Number of iterations n'), ylabel('Absolute error \delta_n');
% 
% figure(2);
% boxplot(sigma, group);
% title('Change in constraint force');
% xlabel('Number of iterations n'), ylabel('\sigma');

[fitresult, gof] = createfit(i, log(err));
fitresult
xlabel('Antal iterationer n'), ylabel('ln \delta'),
legend('ln \delta', 'Linj�r anpassning');

f = 50:10:100;
timesteps = 1./f;

