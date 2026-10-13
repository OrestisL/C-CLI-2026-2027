namespace ArgumentParser;

// function signature for argument callbacks
public delegate void ArgumentCallback(string s);

/// <summary>
/// Command line argument base class.
/// </summary>
public class Argument(string type, string name, string cmd, string description, ArgumentCallback callback, bool hasOptions = true)
{
    public static readonly List<Argument> ArgsList = new List<Argument>();
    private readonly string _name = name;
    public string Name => _name;
    private readonly string _type = type;
    public string Type => _type;

    private readonly string _cmd = cmd;
    public string Cmd => _cmd;
    private readonly string _description = description;
    public string Description => _description;
    private readonly ArgumentCallback _callback = callback;
    public ArgumentCallback Callback => _callback;
    public readonly bool HasOptions = hasOptions;
    public string GetDetails()
    {
        string value = HasOptions ? $"value ({Type})" :"" ;

        return $"Argument: {Name}\n" +
            $"Description: {Description}\n" +
            $"Usage: {Cmd} {value}\n";
    }

    private static void ProcessArgument(string[] input, int idx)
    {
        string cmd = input[idx];
        // check if argument command exists in the list
        foreach (var argument in Argument.ArgsList)
        {
            if (argument.Cmd == cmd)
            {
                if (!argument.HasOptions)
                {
                    argument.Callback("");
                }
                else
                {
                    try
                    {
                        // call callback for the next index in the array
                        if (idx < input.Length)
                            argument.Callback(input[idx + 1]);
                        else if (idx == input.Length)
                            argument.Callback(input[idx]);
                        else
                            throw new IndexOutOfRangeException();
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine($"Error: No value provided for argument '{cmd}'.");
                    }
                }
            }
        }
    }
    public static void ProcessArguments(string[] args)
    {
        for (int i = 0; i < args.Length; i++) 
        {
            ProcessArgument(args, i);
        }
    }
}