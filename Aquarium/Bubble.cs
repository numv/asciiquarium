namespace asciiquarium.Aquarium;

internal class Bubble : AquariumObject
{

    private int lastTick;
    public int Speed { get; set; } = 120;

    public Bubble(ConsoleRenderer renderer, IAquariumTemplate template, Position position)
      : base(renderer, template, position, EDirection.Up)
    {
    }

    protected override void OnUpdate(ConsoleBuffer buffer, out bool clearBeforeNextFrame)
    {
        clearBeforeNextFrame = true;
        UpdatePosition();
        Draw(buffer);
    }

    private void UpdatePosition()
    {
        if (System.Environment.TickCount - lastTick >= Speed)
        {
            if (Direction == EDirection.Up)
            {
                Position = new Position(Position.X, Position.Y - 1);
                if (Position.Y + Template.Height < 0)
                {
                    isAlive = false;
                }
            }
            lastTick = System.Environment.TickCount;
        }
    }
}

