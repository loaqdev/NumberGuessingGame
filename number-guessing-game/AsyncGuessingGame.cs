namespace number_guessing_game;

internal enum Difficulty { Easy = 1, Medium = 2, Hard = 3 }

internal class AsyncGuessingGame
{
    private readonly Random _range = Random.Shared;
    private readonly IReadOnlyDictionary<Difficulty, int> _chancesByDifficulty =
        new Dictionary<Difficulty, int>
        {
            [Difficulty.Easy] = 10,
            [Difficulty.Medium] = 5,
            [Difficulty.Hard] = 3
        };

    public async Task StartAsync(CancellationToken ct = default)
    {
        PrintWelcome();

        while (!ct.IsCancellationRequested)
        {
            var difficulty = await AskForDifficultyAsync(ct);

            if (difficulty == null)
                return;

            await PlayRoundAsync(difficulty.Value, ct);

            Console.Write("Play again? (y/n): ");
            var again = await ReadLineAsync(ct);
            if (string.IsNullOrWhiteSpace(again) || !again.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                break;
        }

        Console.WriteLine("Thanks for playing. Goodbye!");
    }

    private static Task<string?> ReadLineAsync(CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return Task.FromCanceled<string?>(ct);

        return Task.Run(() =>
        {
            try
            {
                return Console.ReadLine();
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return null;
            }
        }, ct);
    }

    private async Task PlayRoundAsync(Difficulty difficulty, CancellationToken ct)
    {
        int secretNumber = _range.Next(1, 101);
        int attempts = _chancesByDifficulty[difficulty];

        Console.WriteLine($"Difficulty: {difficulty} ({attempts} attempts). I'm thinking of a number between 1 and 100.");

        while (attempts > 0 && !ct.IsCancellationRequested)
        {
            Console.Write("Enter your guess (1-100) or type 'exit' to quit: ");
            int? guess = await AskForGuessAsync(ct);

            if (guess == null)
            {
                Console.WriteLine("Round cancelled. Returning to main menu.");
                return;
            }

            int guessValue = guess.Value;

            if (guessValue == secretNumber)
            {
                Console.WriteLine($"Congratulations — you guessed the number {secretNumber}!");
                return;
            }

            if (guessValue < secretNumber)
                Console.WriteLine($"Too low. Attempts left: {--attempts}");
            else
                Console.WriteLine($"Too high. Attempts left: {--attempts}");

            if (attempts == 0)
            {
                Console.WriteLine($"Game over — the number was {secretNumber}.");
            }
        }
    }

    private async Task<int?> AskForGuessAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var input = await ReadLineAsync(ct);
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter a value.");
                Console.Write("Enter your guess (1-100) or type 'exit' to quit: ");
                continue;
            }

            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                return null;

            if (!int.TryParse(input, out var guess) || guess < 1 || guess > 100)
            {
                Console.WriteLine("Please enter an integer between 1 and 100.");
                Console.Write("Enter your guess (1-100) or type 'exit' to quit: ");
                continue;
            }

            return guess;
        }
        return null;
    }

    private async Task<Difficulty?> AskForDifficultyAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine("Choose difficulty:" +
                "\n1 - Easy (10 attempts)" +
                "\n2 - Medium (5 attempts)" +
                "\n3 - Hard (3 attempts)");

            Console.Write("Enter 1, 2, 3 or type 'exit' to quit: ");

            var input = await ReadLineAsync(ct);
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter a value.");
                continue;
            }

            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                return null;

            if (!int.TryParse(input, out var diff) || diff < 1 || diff > 3)
            {
                Console.WriteLine("Please enter a number between 1 and 3.");
                continue;
            }

            return (Difficulty)diff;
        }
        return null;
    }

    private static void PrintWelcome()
    {
        Console.WriteLine("Welcome to the Number Guessing Game!");
    }
}
