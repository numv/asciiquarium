namespace console_draw.Fun;

using System;

internal class Rainbow
{
    Random random = new Random();
    int lastTick = 0;

    public Rainbow(ConsoleRenderer renderer)
    {
        renderer.OnUpdate += Renderer_OnUpdate;
    }

    private void Renderer_OnUpdate(ref ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        clearBeforeNextFrame = false;
        if (System.Environment.TickCount - lastTick < 1000)
            return;

        lastTick = System.Environment.TickCount;

        for (int x = 0; x < buffer.Length; x++)
        {
            //var change = random.Next(0, 2);
            //if (change == 0)
            //    continue;
            //change = random.Next(0, 2);
            //if (change == 0)
            //    continue;

            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            var colorFg = consoleColors.GetValue(random.Next(consoleColors.Length));
            var colorBg = consoleColors.GetValue(random.Next(consoleColors.Length));
            if (colorFg is ConsoleColor fg && colorBg is ConsoleColor bg)
                buffer.Set(x, new CharInfo((byte)'=', fg, bg));
        }
    }
}