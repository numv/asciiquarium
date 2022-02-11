namespace asciiquarium.Fun;

using System;

internal class Rainbow
{
    static Random random = new Random();
    static ConsoleColor[]? _consoleColors;
    static ConsoleColor[] consoleColors
    {
        get
        {
            if (_consoleColors is null)
            {
                _consoleColors = Enum.GetValues<ConsoleColor>()
                  .Where(n =>
                      n != ConsoleColor.Black
                      && n != ConsoleColor.Gray
                      && n != ConsoleColor.DarkGray
                      ).ToArray();
            }
            return _consoleColors;
        }
    }
    int lastTick = 0;

    public Rainbow(ConsoleRenderer renderer)
    {
        renderer.OnUpdate += Renderer_OnUpdate;
    }

    private void Renderer_OnUpdate(ref ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        clearBeforeNextFrame = false;
        /* if (System.Environment.TickCount - lastTick < 1000) */
        /*     return; */

        lastTick = System.Environment.TickCount;

        for (int x = 0; x < buffer.Length; x++)
        {
            //var change = random.Next(0, 2);
            //if (change == 0)
            //    continue;
            //change = random.Next(0, 2);
            //if (change == 0)
            //    continue;

            var colorFg = GetRandomColor();
            var colorBg = GetRandomColor();
            if (colorFg is ConsoleColor fg && colorBg is ConsoleColor bg)
                buffer.Set(x, new CharInfo('▚', fg, bg));
        }
    }
    public static ConsoleColor GetRandomColor()
    {
        var r = random.Next(consoleColors.Length);
        return consoleColors[r];
    }
}
