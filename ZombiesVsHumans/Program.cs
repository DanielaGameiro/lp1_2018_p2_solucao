// License: GPLv3
// Author: Nuno Fachada

using System;

namespace ZombiesVsHumans
{
    public class Program
    {
        static void Main(string[] args)
        {
            Options options = Options.ParseArgs(args);
            if (options.Error)
            {
                Console.WriteLine("ERROR: " + options.ErrorMessage);
            }
            else
            {
                Game game = new Game(options);
                game.Start();
            }
        }
    }
}
