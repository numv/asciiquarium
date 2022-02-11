namespace asciiquarium.Aquarium;

using System;

internal readonly record struct Position(int X, int Y);

internal class AquariumObject : IDisposable
{
    public bool isAlive { get; protected set; } = false;
    protected readonly ConsoleRenderer renderer;
    public IAquariumTemplate Template { get; private set; }
    public EDirection Direction { get; protected set; }
    public Position Position { get; protected set; }

    public AquariumObject(ConsoleRenderer renderer, IAquariumTemplate template, Position position, EDirection direction)
    {
        this.renderer = renderer;
        this.Template = template;
        this.Direction = direction;
        this.Position = position;
    }

    public void Dispose()
    {
        renderer.OnUpdate -= Renderer_OnUpdate;
    }

    private void Renderer_OnUpdate(ref ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        OnUpdate(buffer, out clearBeforeNextFrame);
    }

    protected virtual void OnUpdate(ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        clearBeforeNextFrame = true;
        Draw(buffer);
    }

    public void Spawn()
    {
        if (isAlive)
            return;

        renderer.OnUpdate += Renderer_OnUpdate;
        isAlive = true;
    }

    protected void Draw(ConsoleBuffer buffer)
    {
        if (!isAlive)
            return;

        var frame = Template.GetFrame(Direction);
        for (int y = 0; y < frame.Length; y++)
        {
            var frameRow = frame[y];
            for (int x = 0; x < frameRow.Length; x++)
            {
                var cell = frameRow[x];
                DrawChar(buffer, cell, x, y);
            }
        }
    }

    protected virtual void DrawChar(ConsoleBuffer buffer, CharInfo? cell, int x, int y)
    {
        buffer.Set(Position.X + x, Position.Y + y, cell);
    }
}

