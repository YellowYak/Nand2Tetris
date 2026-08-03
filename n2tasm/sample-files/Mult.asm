// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

// The algorithm is based on repetitive addition.
// In short, we:
//  1.) Initialize R2 = 0
//  2.) So long as R0 > 0...
//  3.)     Set R2 = R0 + R1
//  4.)     Set R0 = R0 - 1

// INITIALIZATION
// Set R2 to 0
@R2
M=0


// Loop until R0 <= 0
(LOOP)
    // If R0 <= 0 then jump to END
    @R0
    D=M
    @END
    D;JLE


    // Set R2 = R2 + R1
    @R2
    D=M

    @R1
    D=D+M

    @R2
    M=D

    // Set R0 = R0 - 1
    @R0
    M=M-1

    // Return to start of LOOP
    @LOOP
    0;JMP


(END)
    // Infinite loop
    @END
    0;JMP