# Number Guessing Game
A simple console-based number guessing game implemented in C#. The player chooses a difficulty level and tries to guess a randomly chosen number between 1 and 100 within a limited number of attempts.

This project was inspired by the roadmap project: https://roadmap.sh/projects/number-guessing-game

## Features
Three difficulty levels: Easy, Medium, Hard
Configurable attempts per difficulty
Clear feedback: “Too low” / “Too high” hints after each guess
Input validation
Asynchronous implementation

## Getting Started
### Prerequisites:
.NET SDK (recommended .NET 6.0 or newer)

### Clone the repository:
```
git clone <repository-url>
cd <repository-folder>
```
### Build and run:
```
dotnet build
dotnet run --project ./path/to/Project.csproj
```
Or run directly from Visual Studio / Rider by opening the solution and starting the project.

## How to Play
Start the application.

Choose a difficulty level:
- 1 – Easy (10 attempts)
- 2 – Medium (5 attempts)
- 3 – Hard (3 attempts)

 The game selects a random number between 1 and 100.

 Enter your guesses. For each guess you will get feedback:
 
- “Too low”
- “Too high”
- “Correct!”

The game ends when you guess correctly or run out of attempts. You can choose to play again.

## Configuration
The number range and attempts per difficulty are defined in code (easy to modify). Example locations to change:

- Number range: generation code using Random.Next(1, 101)
- Attempts per difficulty: an enum/dictionary mapping in the game class

## Project Structure
- Program.cs — entry point
- AsyncGuessingGame.cs — core game logic and input handling

## Contributing
Feel free to contribute.
