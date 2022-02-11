namespace asciiquarium.Aquarium;

using System;


internal sealed class FishFancyTemplate : IAquariumTemplate
{
    public int Width => 16;
    public int Height => 5;

    const string FrameRight = @"
      /`·.¸
     /¸...¸`:·
 ¸.·´◞◞¸◞◞◞`·.¸.·´)
: © ):´;◞◞◞◞◞◞◞¸◞◞{
 `·.¸◞`·◞◞¸.·´\`·¸)
     `\\´´\¸.·´";
    const string FrameLeft = @"
        ¸.·`\
     ·:`¸...¸\
(´·.¸.·`◟◟◟¸◟◟´·.¸
 } ◟¸◟◟◟◟◟◟;´:( © :
 (¸·`/´·.¸◟◟·`◟¸.·`
     ´·.¸´´`//´";

    CharInfo?[][]? Cache;
    public CharInfo?[][] GetFrame(EDirection direction)
    {
        if (Cache is not null) return Cache;

        string fish = "";
        if (direction == EDirection.Left)
            fish = FrameRight;
        else
            fish = FrameLeft;

        var rows = fish.Split(Environment.NewLine);
        var frame = new CharInfo?[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            string row = rows[i];
            frame[i] = new CharInfo?[row.Length];
            ConsoleColor fg = Fun.Rainbow.GetRandomColor();
            for (int n = 0; n < row.Length; n++)
            {
                var c = row[n];
                if (c == ' ')
                {
                    frame[i][n] = null;
                    continue;
                }
                if (c == ':')
                    fg = ConsoleColor.Green;
                else if (c == 'o')
                    fg = ConsoleColor.Red;
                else if (c == '<' || c == '>')
                    fg = ConsoleColor.Yellow;
                frame[i][n] = new CharInfo(c, fg);
            }
        }
        Cache = frame;
        return frame;
    }
}

internal sealed class FishDoraSmolTemplate : IAquariumTemplate
{

    const string F = @"
      /`·.¸
     /¸...¸`:·
 ¸.·´  ¸   `·.¸.·´)
: © ):´;      ¸  {
 `·.¸ `·  ¸.·´\`·¸)
     `\\´´\¸.·´";

    public int Width => 16;
    public int Height => 5;

    const string FrameRight = @"
  ;,//;,    ,;/
 o:::::::;;///
>::::::::;;\\\
  ''\\\\\'"" ';\";
    const string FrameLeft = @"

\;,    ,;\\,;
 \\\;;:::::::o
   /;;::::::::<
 /;' ""'//''";

    CharInfo?[][]? Cache;
    public CharInfo?[][] GetFrame(EDirection direction)
    {
        if (Cache is not null) return Cache;

        string fish = "";
        if (direction == EDirection.Left)
            fish = FrameRight;
        else
            fish = FrameLeft;

        var rows = fish.Split(Environment.NewLine);
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
                if (c == ':')
                    fg = ConsoleColor.Green;
                else if (c == 'o')
                    fg = ConsoleColor.Red;
                else if (c == '<' || c == '>')
                    fg = ConsoleColor.Yellow;
                frame[i][n] = new CharInfo(c, fg);
            }
        }
        Cache = frame;
        return frame;
    }
}

internal sealed class DefaultFishTemplate : IAquariumTemplate
{

    public int Width => 23;
    public int Height => 5;

    const string FrameRight = @"
      /""*._         _
  .-*'`▪▪▪▪`*-.._.-'/
<.*▪))▪▪▪▪▪,▪▪▪▪▪▪▪( 
  `*-._`._(__.--*""`.\";
    const string FrameLeft = @"
_         _.*""\
 \'-._..-*`▪▪▪▪`'*-.
  )▪▪▪▪▪▪▪,▪▪▪▪▪((▪*.>
 /.`""*--.__)_.`_.-*`";


    CharInfo?[][]? Cache;
    public CharInfo?[][] GetFrame(EDirection direction)
    {
        if (Cache is not null) return Cache;
        string fish = "";
        if (direction == EDirection.Left)
            fish = FrameRight;
        else
            fish = FrameLeft;

        var rows = fish.Split(Environment.NewLine);
        var frame = new CharInfo?[rows.Length][];
        ConsoleColor fg = Fun.Rainbow.GetRandomColor();
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
                if (c == '▪')
                    frame[i][n] = new CharInfo(c, ConsoleColor.DarkGray);
                else
                    frame[i][n] = new CharInfo(c, fg);
            }
        }
        Cache = frame;
        return frame;
    }
}

