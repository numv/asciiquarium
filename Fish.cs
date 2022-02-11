namespace console_draw;

using System;

internal readonly record struct Position(int X, int Y);

internal class Fish
{
    private readonly ConsoleRenderer renderer;
    readonly CharInfo[][] frame;
    private bool isAlive = false;

    Position position;
    private int lastTick;

    public Fish(ConsoleRenderer renderer)
    {
        this.renderer = renderer;

        frame = new CharInfo[][] {
            new CharInfo[] { new CharInfo((byte)'>', ConsoleColor.Yellow) }
        };

        position = new Position(0, 0);
    }

    public void Alive()
    {
        if (isAlive)
            return;

        renderer.OnUpdate += Renderer_OnUpdate;
        isAlive = true;
    }

    private void Renderer_OnUpdate(ref ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        clearBeforeNextFrame = true;
        UpdatePosition();
        DrawFish(buffer);
    }

    private void UpdatePosition()
    {
        if (System.Environment.TickCount - lastTick >= 2000)
        {
            position = new Position(position.X + 1, 0);
            lastTick = System.Environment.TickCount;
        }
    }

    private void DrawFish(ConsoleBuffer buffer)
    {
        if (!isAlive)
            return;

        for (int i = 0; i < frame.Length; i++)
        {
            var frameRow = frame[i];
            foreach (var cell in frameRow)
            {
                buffer.Set(position.X + i, position.Y + i, cell);
            }
        }
    }
}