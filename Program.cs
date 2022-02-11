using Cocona;

using asciiquarium;
using asciiquarium.Aquarium;
using asciiquarium.Fun;

using Microsoft.Extensions.DependencyInjection;

Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
    cancellationTokenSource.Cancel();
};

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton<ConsoleRenderer>();
builder.Services.AddScoped<FramerateCounter>(n => new FramerateCounter(n.GetRequiredService<ConsoleRenderer>()));

var app = builder.Build();

app.AddCommand("rainbow", async (CoconaAppContext ctx, ConsoleRenderer renderer, FramerateCounter framerate) =>
    {
        renderer.SetSize(Console.WindowWidth, Console.WindowHeight);
        var rainbow = new Rainbow(renderer);
        while (true)
        {
            await renderer.DoUpdateAsync();
        }
    });


app.AddCommand(async (CoconaAppContext ctx, ConsoleRenderer renderer, FramerateCounter framerate) =>
 {
     renderer.SetSize(Console.WindowWidth, Console.WindowHeight);

     Random random = new Random();
     List<Fish> fishies = new List<Fish>();



     IAquariumTemplate castleTemplate = new CastleTemplate();
     var castle = new AquariumObject(renderer,
         castleTemplate,
         new Position(Console.WindowWidth - castleTemplate.Width, Console.WindowHeight - castleTemplate.Height),
         EDirection.Left);
     castle.Spawn();

     var spawnCount = random.Next(3, 6);
     for (int i = 0; i < spawnCount; i++)
     {
         var fish = Fish.CreateFish(renderer, random);
         fishies.Add(fish);
     }

     IAquariumTemplate boatTemplate = new BoatTemplate();
     var boat = new Fish(renderer,
         boatTemplate,
         new Position(0, -1),
         EDirection.Right);
     boat.Spawn();
     fishies.Add(boat);


     while (true)
     {
         if (ctx.CancellationToken.IsCancellationRequested)
             return;
         await renderer.DoUpdateAsync();

         for (int i = fishies.Count - 1; i >= 0; i--)
         {
             Fish fish = fishies[i];
             if (!fish.isAlive)
             {
                 fishies.RemoveAt(i);
                 fish.Dispose();
             }
         }


         if (fishies.Count < spawnCount)
         {
             spawnCount = random.Next(3, 7);
             var fish = Fish.CreateFish(renderer, random);
             fishies.Add(fish);

             var spawnBoatCount = random.Next(3, 15);
             if (spawnBoatCount <= 3)
             {
                 var boaty = new Fish(renderer,
                     boatTemplate,
                     new Position(0, -1),
                     EDirection.Right);
                 boaty.Spawn();
                 fishies.Add(boaty);
             }
         }
     }
 });

var ct = cancellationTokenSource.Token;
var appTask = app.RunAsync(ct);
var inputTask = Task.Factory.StartNew(async () =>
{
    while (true)
    {
        if (ct.IsCancellationRequested)
            return;
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                cancellationTokenSource.Cancel();
                return;
            }
            await Task.Delay(50, ct);
        }
    }
}, ct);

Task.WaitAll(appTask, inputTask);
Console.Clear();
