public class SymbolTable
{
    private Dictionary<string, int> symbolMap = new();
    private int currentVariableLocation = 16;

    public SymbolTable()
    {
        // Add in hard-coded symbols
        for (int i = 0; i < 16; i++)
            this.symbolMap.Add($"R{i}", i);

        this.symbolMap.Add("SCREEN", 16384);
        this.symbolMap.Add("KBD", 24576);
        this.symbolMap.Add("SP", 0);
        this.symbolMap.Add("LCL", 1);
        this.symbolMap.Add("ARG", 2);
        this.symbolMap.Add("THIS", 3);
        this.symbolMap.Add("THAT", 4);
    }

    /// <summary>
    /// Adds a variable to the symbol map and returns its position in the data memory.
    /// </summary>
    /// <param name="name">The variable to add.</param>
    /// <returns>The position in the data memory.</returns>
    public int AddVariable(string name)
    {
        if (!this.symbolMap.ContainsKey(name))
            this.symbolMap.Add(name, currentVariableLocation++);

        return this.symbolMap[name];
    }

    /// <summary>
    /// Adds a label to the symbol map with the specified position in the instruction memory.
    /// </summary>
    /// <param name="name">The label to add.</param>
    /// <param name="position">The label's position in instruction memory.</param>
    public void AddLabel(string name, int position)
    {
        if (!this.symbolMap.ContainsKey(name))
            this.symbolMap.Add(name, position);
    }

    public IEnumerable<string> GetSymbols()
    {
        foreach (string key in this.symbolMap.Keys)
            yield return key;
    }

    public int GetSymbolPosition(string name) => this.symbolMap[name];

    public void DecrementPosition(string name, int decrementAmount) => this.symbolMap[name] -= decrementAmount;
}