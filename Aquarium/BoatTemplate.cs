namespace asciiquarium.Aquarium;

internal sealed class BoatTemplate : IAquariumTemplate
{

    public int Width => 8;
    public int Height => 5;
    CharInfo?[][]? Cache;

    public CharInfo?[][] GetFrame(EDirection direction)
    {
        if (Cache is not null)
        {
            return Cache;
        }

        var rows = Frame.Split(Environment.NewLine);
        var frame = new CharInfo?[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];
            frame[i] = new CharInfo?[row.Length];
            for (int n = 0; n < row.Length; n++)
            {
                var c = row[n];
                if (c == ' ')
                {
                    frame[i][n] = null;
                    continue;
                }
                ConsoleColor fg = Fun.Rainbow.GetRandomColor();
                if (c == '=')
                    fg = ConsoleColor.DarkYellow;
                if (c == '~')
                    fg = ConsoleColor.Yellow;
                if (c == '|' || c == '\\' || c == '/')
                    fg = ConsoleColor.DarkGray;
                if (c == '_')
                    fg = ConsoleColor.Gray;
                frame[i][n] = new CharInfo(c, fg);
            }
        }
        Cache = frame;
        return frame;
    }

    const string Frame = @"
   ,~
   |\
  /| \
 /_|__\
~======~";
}



