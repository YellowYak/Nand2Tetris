// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel. When no key is pressed,
// the screen should be cleared.


// R0 is a pointer to the screen RAM that we are currently writing to
//      We initially write to @SCREEN, then increment until we reach KBD
// R1 contains the value we are writing to MEM[R0] (either 0 for white or -1 for black)


(LOOP)
    // Initialize R0 to point to start of screen RAM
    @SCREEN
    D=A
    @R0
    M=D


    // Determine the value to assign R1...
    // Default to R1=0 (white)
    @R1
    M=0     // white

    // If NO key is being pressed then start our FILL loop
    @KBD
    D=M
    @FILL
    D;JEQ

    // Otherwise set R1=-1 (black) and fall through to FILL loop
    @R1
    M=-1    // black


    (FILL)
        // Set RAM[R0] = R1
        @R1
        D=M     // D = R1 (either 0 or -1 for white or black, respectively)

        @R0
        A=M     // Set A = R0
        M=D     // Set RAM[A] = D, that is, RAM[R0] = R1


        // Increment the screen register pointer
        @R0
        M=M+1   // Set RAM[R0] = RAM[R0] + 1


        // Determine if we've reached the end
        @R0
        D=M     // D = R0 (the current address in screen RAM we are writing to)
        @KBD    // A = KBD (the END of the screen RAM)
        D=A-D   // D = KBD - R0 (returns how many words of memory remain in screen RAM)
        @FILL
        D;JGT   // If D>0 then repeat FILL


    // The FILL logic has completed, jump back up to LOOP
    @LOOP
    0;JMP