namespace asciiquarium.Aquarium;

using System;

internal class Fish : AquariumObject
{

    private int lastTick;
    public int Speed { get; set; } = 80;

    public Fish(ConsoleRenderer renderer, IAquariumTemplate template, Position position, EDirection direction)
      : base(renderer, template, position, direction)
    {
    }

    public static Fish CreateFish(ConsoleRenderer renderer, Random random)
    {
        IAquariumTemplate template;
        var templateid = random.Next(3);
        switch (templateid)
        {
            case 1:
                template = new FishDoraSmolTemplate();
                break;
            case 2:
                template = new FishFancyTemplate();
                break;
            default:
                template = new DefaultFishTemplate();
                break;
        }


        var direction = (EDirection)random.Next(1, 3);
        var x = Console.WindowWidth;
        if (direction == EDirection.Right)
        {
            x = -template.Width;
        }
        var y = random.Next(Console.WindowHeight);
        if (y + template.Height < renderer.AreaHeight)
        {
            y -= template.Height;
        }

        if (y <= 0)
        {
            y = 1;
        }

        var fish = new Fish(renderer, template, new Position(x, y), direction);
        fish.Speed = random.Next(20, 100);
        fish.Spawn();
        return fish;
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
            if (Direction == EDirection.Left)
            {
                Position = new Position(Position.X - 1, Position.Y);
                if (Position.X + Template.Width < 0)
                {
                    isAlive = false;
                }
            }
            else
            {
                Position = new Position(Position.X + 1, Position.Y);
                if (Position.X > renderer.AreaWidth)
                {
                    isAlive = false;
                }
            }
            lastTick = System.Environment.TickCount;
        }
    }
}
