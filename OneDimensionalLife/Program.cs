using System;
using System.Threading;
using OneDimensionalLife.Enums;

namespace OneDimensionalLife
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = new Game("data.txt");
            if (game.Initialize() != GameOperationStatus.Ok)
            {
                Console.WriteLine("Failed to initialize game :(");
                return;
            }

            while (game.AliveCount > 0)
            {
                game.Draw();
                Thread.Sleep(1000);
                game.Update();
            }

            Console.Clear();
            Console.WriteLine("Everyone died :(");
        }
    }
}
