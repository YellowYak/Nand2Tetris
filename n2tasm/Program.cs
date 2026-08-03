/* n2tasm.exe ***
   ----------
   This command-line program processes a Hack assembly file (.asm) and produces output
   to a specified output file. It can output Hack machine code, stream-lined Hack
   assembly code, or the symbol table used by the assembler.
*****************/

const string FormatMachine = "machine";
const string FormatAsmLabels = "asml";
const string FormatAsmRaw = "asmr";
const string FormatAsmSymbolTable = "symb";

// Ensure that there's exactly one command-line argument
if (args.Length == 0 || (args.Length == 0 && (args.First() == "-h" || args.First() == "--help")))
{
    Console.WriteLine();
    Console.WriteLine("Usage: n2tasm [-f=<format>] <path-to-asm-file> [-o=<filepath>]");
    Console.WriteLine();
    Console.WriteLine("path-to-asm-file (required):");
    Console.WriteLine("  The path to the assembly file (.asm) to compile into the Hack machine code.");
    Console.WriteLine();
    Console.WriteLine("-f=<format> (optional):");
    Console.WriteLine("   One of the following output formats:");
    Console.WriteLine($"     {FormatMachine}\tFormats output as Hack machine-code (default)");
    Console.WriteLine($"     {FormatAsmLabels}\tFormats output as stripped-down assembly code with the labels and variable names in-tact");
    Console.WriteLine($"     {FormatAsmRaw}\tFormats output as stripped-down assembly code with the labels and variable names replaced with their position in memory");
    Console.WriteLine($"     {FormatAsmSymbolTable}\tOutputs the symbol table");
    Console.WriteLine();
    Console.WriteLine("-o=<filepath> (optional):");
    Console.WriteLine("   The filepath to save the output. Defaults to the same file name as the input file but with extension .hack.");
    return;
}


string format = FormatMachine;
string asmPath = string.Empty;
string outPath = string.Empty;

foreach (string arg in args)
{
    if (arg.Contains("="))
    {
        string[] pieces = arg.Split('=');
        if (pieces.Length == 2)
        {
            switch (pieces.First())
            {
                case "-f": format = pieces.Last(); break;
                case "-o": outPath = pieces.Last(); break;
                default:
                    Console.Error.WriteLine($"ERROR: Invalid paramater {arg}.");
                    return;
            }
        }
        else
        {
            Console.Error.WriteLine($"ERROR: Invalid paramater {arg}.");
            return;
        }
    }
    else
    {
        if (string.IsNullOrEmpty(asmPath))
            asmPath = arg;
        else
        {
            Console.Error.WriteLine($"ERROR: Invalid paramater {arg}.");
            return;
        }
    }
}

// Ensure asmPath is valid
if (string.Compare(Path.GetExtension(asmPath), ".asm", StringComparison.OrdinalIgnoreCase) != 0)
{
    Console.Error.WriteLine($"ERROR: {asmPath} must be a file with extension '.asm'.");
    return;
}
if (!File.Exists(asmPath))
{
    Console.Error.WriteLine($"ERROR: {asmPath} could not be found.");
    return;
}

// Ensure format is correct
if (format != FormatMachine && format != FormatAsmLabels && format != FormatAsmRaw && format != FormatAsmSymbolTable)
{
    Console.Error.WriteLine($"ERROR: Invalid format specified - {format}.");
    return;
}

// Determine outPath
if (string.IsNullOrWhiteSpace(outPath))
    outPath = Path.Combine(Path.GetDirectoryName(asmPath)!, Path.GetFileNameWithoutExtension(asmPath) + ".hack");


// Create the Parser
Parser parser = new(asmPath);
SymbolTable symbolTable = new();
List<Instruction> instructions = new(parser.Instructions.Count);

// Process symbols: First handle Labels, then Variables
foreach (var inst in parser.Instructions.Where(i => i.IsLabel))
{
    symbolTable.AddLabel(inst.Label, inst.LineNumber);
}
foreach (var inst in parser.Instructions.Where(i => i.IsVariable))
{
    symbolTable.AddVariable(inst.VariableName);
}

// Strip out Label instructions & updates the symbol table accordingly
// (UNLESS WE ARE FORMATTING ASM WITH LABELS)
if (format != FormatAsmLabels)
{
    parser.RemoveLabels(symbolTable);

    // Replace symbols with their addresses
    foreach (string symbol in symbolTable.GetSymbols())
    {
        int pos = symbolTable.GetSymbolPosition(symbol);
        parser.ReplaceVariable(symbol, pos);
    }
}

// Generate the output based on the format selection
List<string> output = new(parser.Instructions.Count);
switch (format)
{
    case FormatAsmSymbolTable:
        foreach (string symbol in symbolTable.GetSymbols())
            output.Add($"{symbol}: {symbolTable.GetSymbolPosition(symbol)}");
        break;

    case FormatAsmRaw:
    case FormatAsmLabels:
        foreach (var inst in parser.Instructions)
            output.Add(inst.RawInstruction);
        break;

    case FormatMachine:
        foreach (var inst in parser.Instructions)
            output.Add(inst.ToMachineCode());
    break;
}

// Output to the specified output file path
File.WriteAllLines(outPath, output);