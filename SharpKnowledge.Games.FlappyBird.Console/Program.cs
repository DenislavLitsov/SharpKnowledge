namespace SharpKnowledge.Games.FlappyBird.Console
{
    using System;
    using System.Runtime.CompilerServices;
    using SharpKnowledge.Games.FlappyBird.Engine;
    using SharpKnowledge.Playing;

    internal class Program
    {
        static void Main(string[] args)
        {
            FlappyBirdGame game = new FlappyBirdGame(new SharpKnowledge.Common.RandomGenerators.RandomGeneratorFactory(true, 10_000).GetRandomGenerator());
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Score: {game.GetScore()}");
                //Console.WriteLine($"Bird Y: {game.BirdY}");
                //Console.WriteLine($"Pipe X: {game.PipeX}");
                //Console.WriteLine($"Pipe Gap Y: {game.PipeGapY}");
                Console.WriteLine("Press Space to flap, or any other key to do nothing.");

                var map = game.GetMap();
                var birdLocation = game.GetBirdLocation();
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (map[y, x] == 1)
                        {
                            Console.Write("#");
                        }
                        else if (x == birdLocation[0] && y == birdLocation[1])
                        {
                            Console.Write("O");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.WriteLine();
                }

                game.GetBrainInputs();

                var key = Console.ReadKey(true);
                GameResult result;

                if (key.Key == ConsoleKey.Spacebar)
                {
                    result = game.Update(new float[] { 1 });
                }
                else
                {
                    result = game.Update(new float[] { 0 });
                }

                if (result == GameResult.GameOver)
                {
                    break;
                }
            }
        }
    }
}
