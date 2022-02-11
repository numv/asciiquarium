namespace asciiquarium.Aquarium;

internal interface IAquariumTemplate
{
    int Width { get; }
    int Height { get; }


    CharInfo?[][] GetFrame(EDirection direction);
}

