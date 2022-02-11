namespace asciiquarium;

internal class FramerateCounter
{
    int lastTick;
    int stableRate;
    int frameRate;

    bool autoUpdateWindowTitle;
    int lastTitleRate;

    int frameDelay;
    int maxFrameRate = 20;

    public FramerateCounter(ConsoleRenderer renderer, bool autoUpdateWindowTitle = true)
    {
        renderer.OnUpdateCompleted += DoTick;
        this.autoUpdateWindowTitle = autoUpdateWindowTitle;
    }

    public void DoTick()
    {
        var currentTick = System.Environment.TickCount;
        if (currentTick - lastTick >= 1000)
        {
            stableRate = frameRate;

            if (autoUpdateWindowTitle)
            {
                int currRate = Get();
                if (lastTitleRate != currRate)
                    Console.Title = $"Framerate: {currRate}";
                lastTitleRate = currRate;
            }

            if (stableRate >= maxFrameRate)
            {
                //HACK: i have no idea if this is stupid
                var targetTick = lastTick + maxFrameRate;
                var sleep = currentTick - targetTick;
                frameDelay = Convert.ToInt32(sleep / 1000d);
            }

            frameRate = 0;
            lastTick = System.Environment.TickCount;
        }
        frameRate++;

        if (frameDelay > 0)
        {
            Thread.Sleep(frameDelay);
        }
    }

    public int Get()
    {
        return stableRate;
    }
}
