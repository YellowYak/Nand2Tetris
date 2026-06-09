// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.

load Inc16.hdl,
output-file Inc16.out,
compare-to Inc16.cmp,
output-list in%B1.16.1 out%B1.16.1;

set in %B0000000000000000,  // in = 0
eval,
output;

set in %B1111111111111111,  // in = -1
eval,
output;

set in %B0000000000000101,  // in = 5
eval,
output;

set in %B1111111111111011,  // in = -5
eval,
output;

set in %B0011100100000000,  // in = 14592
eval,
output;

set in %B0000110011001110,  // in = 3278
eval,
output;

set in %B1000110011010000,  // in = -29488
eval,
output;

set in %B0111110101100010,  // in = 32098
eval,
output;

set in %B0111001001001000,  // in = 29256
eval,
output;

set in %B0100011101110001,  // in = 18289
eval,
output;

set in %B0011010001111010,  // in = 13434
eval,
output;

set in %B0010110010000011,  // in = 11395
eval,
output;

set in %B1101100000000110,  // in = -10234
eval,
output;

set in %B0001000001000101,  // in = 4165
eval,
output;

set in %B0000111101000001,  // in = 3905
eval,
output;

set in %B0010111111111000,  // in = 12280
eval,
output;

set in %B0110111111110001,  // in = 28657
eval,
output;

set in %B0111011100011111,  // in = 30495
eval,
output;

set in %B0000110110010110,  // in = 3478
eval,
output;

set in %B0110010111001110,  // in = 26062
eval,
output;

set in %B1101011011001011,  // in = -10549
eval,
output;

set in %B0111000011011101,  // in = 28893
eval,
output;

set in %B1110010111111110,  // in = -6658
eval,
output;

set in %B1000111001101111,  // in = -29073
eval,
output;

set in %B0000001101010011,  // in = 851
eval,
output;

set in %B0101000110111110,  // in = 20926
eval,
output;

set in %B1101100001100000,  // in = -10144
eval,
output;

set in %B1010111000110101,  // in = -20939
eval,
output;

set in %B1000111001000101,  // in = -29115
eval,
output;

set in %B0100111110011011,  // in = 20379
eval,
output;

set in %B0110111000111101,  // in = 28221
eval,
output;

set in %B1010110001010110,  // in = -21418
eval,
output;

set in %B0011010001010100,  // in = 13396
eval,
output;

set in %B0010111101111100,  // in = 12156
eval,
output;

set in %B1100001010000101,  // in = -15739
eval,
output;

set in %B0011000110000100,  // in = 12676
eval,
output;

set in %B1011011111001100,  // in = -18484
eval,
output;

set in %B1011000000011010,  // in = -20454
eval,
output;

set in %B1000011101101111,  // in = -30865
eval,
output;

set in %B0001011000111111,  // in = 5695
eval,
output;
