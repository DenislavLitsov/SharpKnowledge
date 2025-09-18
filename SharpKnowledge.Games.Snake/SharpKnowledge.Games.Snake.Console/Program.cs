using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Games.Snake.Engine;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using System;
using System.Threading;

namespace SharpKnowledge.Games.Snake.Console;

class Program
{
    private static SnakeGame? _game;
    private static bool _gameRunning = true;
    private static readonly object _lockObject = new object();

    static void Main(string[] args)
    {
        _game = new SnakeGame(100, 100, new RandomGeneratorFactory(true, 10_000).GetRandomGenerator());
        _game.Initialize();

        int[] columnsWithRows = { 10_020, 1000, 100, 50, 4, 1 };
        var factory = new RandomBrainFactory(columnsWithRows);
        Brain mainBrain = factory.GetBrain();

        GameLoop(mainBrain);
    }

    private static int GetMapSize(string prompt, int min, int max)
    {
        int size;
        do
        {
            System.Console.Write(prompt);
            string? input = System.Console.ReadLine();
            if (int.TryParse(input, out size) && size >= min && size <= max)
            {
                return size;
            }
            System.Console.WriteLine($"Please enter a number between {min} and {max}");
        } while (true);
    }

    private static void GameLoop(Brain mainBrain)
    {
        const int frameDelay = 1000;
        int frameCount = 0;
        int maxFrames = System.Console.IsInputRedirected ? 50 : int.MaxValue; // Limit frames in test mode

        while (_gameRunning && frameCount < maxFrames)
        {
            lock (_lockObject)
            {
                if (_game!.GameState == GameState.Playing)
                {
                    _game.GetBrainInputs();
                    var outPut = mainBrain.CalculateOutputs(_game.GetBrainInputs());
                    _game.Update(outPut[0]);
                }
            }

            RenderGame();

            if (_game.GameState == GameState.GameOver)
            {
                System.Console.SetCursorPosition(0, _game.Height + 4);
                System.Console.WriteLine("GAME OVER! Press R to restart or ESC to quit.");
                
                if (System.Console.IsInputRedirected)
                {
                    break; // Exit in test mode
                }
            }

            Thread.Sleep(frameDelay);
            frameCount++;
        }
    }

    private static void RenderGame()
    {
        System.Console.Clear();
        lock (_lockObject)
        {
            System.Console.SetCursorPosition(0, 0);
            
            // Render top border
            System.Console.Write("┌");
            for (int i = 0; i < _game!.Width; i++)
                System.Console.Write("─");
            System.Console.WriteLine("┐");

            // Render game area
            for (int y = 0; y < _game.Height; y++)
            {
                System.Console.Write("│");
                for (int x = 0; x < _game.Width; x++)
                {
                    char cell = _game.GetCellContent(x, y);
                    switch (cell)
                    {
                        case 'H': // Snake head
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            System.Console.Write("█");
                            System.Console.ResetColor();
                            break;
                        case 'S': // Snake body
                            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                            System.Console.Write("█");
                            System.Console.ResetColor();
                            break;
                        case 'F': // Food
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.Write("●");
                            System.Console.ResetColor();
                            break;
                        default: // Empty space
                            System.Console.Write(" ");
                            break;
                    }
                }
                System.Console.WriteLine("│");
            }

            // Render bottom border
            System.Console.Write("└");
            for (int i = 0; i < _game.Width; i++)
                System.Console.Write("─");
            System.Console.WriteLine("┘");

            // Render game info
            System.Console.WriteLine($"Score: {_game.Score}");
            System.Console.WriteLine($"State: {_game.GameState}");
            System.Console.WriteLine("Controls: WASD/Arrows=Move, P=Pause, R=Restart, ESC=Quit");
        }
    }

    private static void HandleInput()
    {
        while (_gameRunning)
        {
            try
            {
                if (!System.Console.IsInputRedirected && System.Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = System.Console.ReadKey(true);
                    
                    lock (_lockObject)
                    {
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.Escape:
                                _gameRunning = false;
                                break;
                            case ConsoleKey.R:
                                _game!.Reset();
                                break;
                            case ConsoleKey.P:
                                if (_game!.GameState == GameState.Playing)
                                    _game.Pause();
                                else if (_game.GameState == GameState.Paused)
                                    _game.Resume();
                                break;
                            case ConsoleKey.W:
                            case ConsoleKey.UpArrow:
                                _game!.SetDirection(Direction.Up);
                                break;
                            case ConsoleKey.S:
                            case ConsoleKey.DownArrow:
                                _game!.SetDirection(Direction.Down);
                                break;
                            case ConsoleKey.A:
                            case ConsoleKey.LeftArrow:
                                _game!.SetDirection(Direction.Left);
                                break;
                            case ConsoleKey.D:
                            case ConsoleKey.RightArrow:
                                _game!.SetDirection(Direction.Right);
                                break;
                        }
                    }
                }
                else if (System.Console.IsInputRedirected)
                {
                    // In test mode, automatically change direction occasionally
                    Thread.Sleep(2000);
                    lock (_lockObject)
                    {
                        _game!.SetDirection(Direction.Down);
                    }
                    Thread.Sleep(2000);
                    lock (_lockObject)
                    {
                        _game!.SetDirection(Direction.Left);
                    }
                    break; // Exit input handling in test mode
                }
            }
            catch (Exception)
            {
                // Handle any console-related exceptions gracefully
                break;
            }
            
            Thread.Sleep(50); // Small delay to reduce CPU usage
        }
    }
}
