// See https://aka.ms/new-console-template for more information

using Cocona;

using console_draw;
using console_draw.Fun;

using Microsoft.Extensions.DependencyInjection;

Console.CancelKeyPress += (sender, e) => { Console.Clear(); };

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton<ConsoleRenderer>();
builder.Services.AddScoped<FramerateCounter>(n => new FramerateCounter(n.GetRequiredService<ConsoleRenderer>()));

var app = builder.Build();

app.AddCommand(async (CoconaAppContext ctx, ConsoleRenderer renderer, FramerateCounter framerate) =>
 {
     renderer.SetSize(50, 20);

     //var fish = new Fish(renderer);
     //fish.Alive();

     var rainbow = new Rainbow(renderer);

     while (true)
     {
         if (ctx.CancellationToken.IsCancellationRequested)
             return;
         await renderer.DoUpdateAsync();
     }
 });

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
var appTask = app.RunAsync(cancellationTokenSource.Token);
var inputTask = Task.Factory.StartNew(() =>
{
    while (true)
    {
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.Escape)
        {
            cancellationTokenSource.Cancel();
            return;
        }
    }
});

Task.WaitAll(appTask, inputTask);
Console.Clear();