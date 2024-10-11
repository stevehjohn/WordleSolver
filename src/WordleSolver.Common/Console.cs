namespace WordleSolver.Common;

public static class Console
{
    public static ConsoleColor ForegroundColor
    {
        get => System.Console.ForegroundColor;
        set => System.Console.ForegroundColor = value;
    }

    public static void OutputLine(string text = null)
    {
        if (text == null)
        {
            System.Console.WriteLine();
            
            return;
        }

        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '&')
            {
                var end = text.IndexOf(';', i);

                if (end > -1)
                {
                    var colour = text[(i + 1)..end] ?? string.Empty;

                    if (Enum.TryParse<ConsoleColor>(colour, out var consoleColor))
                    {
                        System.Console.ForegroundColor = consoleColor;
                    }

                    i += colour.Length + 2;
                }
            }

            System.Console.Write(text[i]);
        }
        
        System.Console.WriteLine();
    }
}