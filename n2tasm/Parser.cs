class Parser
{
    public string Path { get; private set; }
    public List<Instruction> Instructions { get; private set; }

    public Parser(string path)
    {
        this.Path = path;

        string[] lines = File.ReadAllLines(path);
        this.Instructions = new List<Instruction>(lines.Length);

        // Convert each non-empty, non-comment line into an Instruction
        int lineNumber = 0;
        foreach (string line in lines)
        {
            // Remove all leading & trailing whitespace
            string trimmedLine = line.Trim();

            // Skip over lines that are empty or comments
            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;
            if (trimmedLine.StartsWith("//", StringComparison.OrdinalIgnoreCase))
                continue;

            // If there are any comments to the right of the command, strip those!
            if (trimmedLine.Contains("//"))
                trimmedLine = trimmedLine.Substring(0, trimmedLine.IndexOf("//")).Trim();

            this.Instructions.Add(new Instruction(trimmedLine, lineNumber++));
        }
    }

    /// <summary>
    /// Removes all Label instructions, adjusting subsequent line numbers to avoid any gaps.
    /// Labels in the symbol table are also updated to account for their new line numbers.
    /// </summary>
    public void RemoveLabels(SymbolTable symbolTable)
    {
        List<Instruction> insts = new(this.Instructions.Count);
        int lineNumber = 0;
        int labelsRemoved = 0;
        foreach (var i in this.Instructions)
        {
            if (!i.IsLabel)
            {
                insts.Add(new Instruction(i.RawInstruction, lineNumber++));
            }
            else
            {
                // Update the symbol table accordingly
                symbolTable.DecrementPosition(i.Label, labelsRemoved++);
            }
        }

        this.Instructions = insts;
    }

    /// <summary>
    /// Replaces the specified variable name with the specified position in
    /// all variable instructions.
    /// </summary>
    public void ReplaceVariable(string name, int value)
    {
        foreach (var inst in this.Instructions.Where(i => i.IsVariable))
            inst.ReplaceVariable(name, value);
    }
}