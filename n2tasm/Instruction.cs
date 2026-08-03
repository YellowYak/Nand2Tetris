using System.Text.RegularExpressions;

class Instruction
{
    public string RawInstruction { get; private set; }
    public int LineNumber { get; private set; }

    public bool IsLabel { get; private set; }
    public string Label { get; private set; }

    public bool IsAInstruction { get; private set; }
    public bool IsVariable { get; private set; }
    public string VariableName { get; private set; }
    public int Address { get; private set; }

    public bool IsCInstruction { get; private set; }

    public Instruction(string inst, int lineNumber)
    {
        this.RawInstruction = inst.Trim();
        this.LineNumber = lineNumber;

        this.IsLabel = Regex.IsMatch(this.RawInstruction, @"^\(.+\)$");
        this.Label = string.Empty;
        if (this.IsLabel)
            this.Label = this.RawInstruction.Substring(1, this.RawInstruction.Length - 2);

        this.IsAInstruction = Regex.IsMatch(this.RawInstruction, @"^@.+$");
        this.IsVariable = false;
        this.VariableName = string.Empty;
        this.Address = int.MinValue;
        if (this.IsAInstruction)
        {
            bool aInstIsAddress = Regex.IsMatch(this.RawInstruction, @"^@\d+$");
            if (aInstIsAddress)
                this.Address = Convert.ToInt32(this.RawInstruction.Substring(1));
            else
            {
                this.IsVariable = true;
                this.VariableName = this.RawInstruction.Substring(1);
            }
        }
    }

    /// <summary>
    /// Replaces the specified variable name with the specified positional value
    /// and updates the Address property accordingly.
    /// </summary>
    /// <param name="name">The case-sensitive variable name to match.</param>
    /// <param name="value">The positional value to replace it with.</param>
    public void ReplaceVariable(string name, int value)
    {
        if (string.IsNullOrEmpty(this.RawInstruction)) return;

        if (this.IsVariable && this.VariableName == name)
        {
            this.RawInstruction = this.RawInstruction.Replace(name, value.ToString());
            this.Address = value;
        }
    }

    public string ToMachineCode()
    {
        if (this.IsAInstruction)
            //  A instruction format: 0vvv vvvv vvvv vvvv
            return $"0{Convert.ToString(this.Address, 2).PadLeft(15, '0')}";
        else
        {
            // C Instruction!! (Recall we've nixed all the Labels by this point)
            //  C instruction format: 111a c1c2c3c4 c5c6d1d2 d3j1j2j3
            const string header = "111";
            string a = string.Empty;
            string comp = string.Empty;
            string dest = string.Empty;
            string jump = "000";

            string[] ps1 = this.RawInstruction.Split(';');
            if (ps1.Length > 2) throw new Exception($"Invalid C instruction: {this.RawInstruction}");
            if (ps1.Length > 1)
            {
                // Handle j1j2j3 bits
                switch (ps1.Last())
                {
                    case "JGT": jump = "001"; break;
                    case "JEQ": jump = "010"; break;
                    case "JGE": jump = "011"; break;
                    case "JLT": jump = "100"; break;
                    case "JNE": jump = "101"; break;
                    case "JLE": jump = "110"; break;
                    case "JMP": jump = "111"; break;
                    default: throw new Exception($"Invalid C instruction jump code: {this.RawInstruction}");
                }
            }

            if (string.IsNullOrWhiteSpace(ps1.First()))
                throw new Exception($"Invalid C instruction: {this.RawInstruction}");

            string[] ps2 = ps1.First().Split('=');
            if (ps2.Length > 2) throw new Exception($"Invalid C instruction: {this.RawInstruction}");
            if (ps2.Length == 2)
            {
                dest = ps2.First();
                comp = ps2.Last();
            }
            else
                comp = ps2.First();

            // Handle the a bit and the c1c2c3c4c5c6 bits
            switch (comp)
            {
                case "0": a = "0"; comp = "101010"; break;
                case "1": a = "0"; comp = "111111"; break;
                case "-1": a = "0"; comp = "111010"; break;
                case "D": a = "0"; comp = "001100"; break;
                case "A": a = "0"; comp = "110000"; break;
                case "M": a = "1"; comp = "110000"; break;
                case "!D": a = "0"; comp = "001101"; break;
                case "!A": a = "0"; comp = "110001"; break;
                case "!M": a = "1"; comp = "110001"; break;
                case "-D": a = "0"; comp = "001111"; break;
                case "-A": a = "0"; comp = "110011"; break;
                case "-M": a = "1"; comp = "110011"; break;
                case "D+1": a = "0"; comp = "011111"; break;
                case "A+1": a = "0"; comp = "110111"; break;
                case "M+1": a = "1"; comp = "110111"; break;
                case "D-1": a = "0"; comp = "001110"; break;
                case "A-1": a = "0"; comp = "110010"; break;
                case "M-1": a = "1"; comp = "110010"; break;
                case "D+A": a = "0"; comp = "000010"; break;
                case "D+M": a = "1"; comp = "000010"; break;
                case "D-A": a = "0"; comp = "010011"; break;
                case "D-M": a = "1"; comp = "010011"; break;
                case "A-D": a = "0"; comp = "000111"; break;
                case "M-D": a = "1"; comp = "000111"; break;
                case "D&A": a = "0"; comp = "000000"; break;
                case "D&M": a = "1"; comp = "000000"; break;
                case "D|A": a = "0"; comp = "010101"; break;
                case "D|M": a = "1"; comp = "010101"; break;
                default: throw new Exception($"Invalid C instruction comp code: {this.RawInstruction}");
            }

            // Handle the d1d2d3 bits
            switch (dest)
            {
                case "": dest = "000"; break;
                case "M": dest = "001"; break;
                case "D": dest = "010"; break;
                case "MD": dest = "011"; break;
                case "A": dest = "100"; break;
                case "AM": dest = "101"; break;
                case "AD": dest = "110"; break;
                case "ADM": dest = "111"; break;
                default: throw new Exception($"Invalid C instruction dest code: {this.RawInstruction}");
            }

            return $"{header}{a}{comp}{dest}{jump}";
        }
    }
}