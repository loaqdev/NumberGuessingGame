using number_guessing_game;

internal class Program
{
    private static async Task Main()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel= true;
            cts.Cancel();
            Console.WriteLine();
            Console.WriteLine("Cancellation requested...");
        };

        var game = new AsyncGuessingGame();
        await game.StartAsync(cts.Token);
    }
}