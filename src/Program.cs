// License: GPLv3
// Author: Nuno Fachada

using System;

namespace zombies
{
    public class Program
    {
        static void Main(string[] args)
        {
            Options options = Options.ParseArgs(args);
            if (!options.Error)
            {
                Console.WriteLine("Options are valid and are as follows:");
                Console.WriteLine($"\tx = {options.XDim}");
                Console.WriteLine($"\ty = {options.YDim}");
                Console.WriteLine($"\tz = {options.Zombies}");
                Console.WriteLine($"\th = {options.Humans}");
                Console.WriteLine($"\tZ = {options.PlayerZombies}");
                Console.WriteLine($"\tH = {options.PlayerHumans}");
                Console.WriteLine($"\tt = {options.Turns}");
            }
            else
            {
                Console.WriteLine("ERROR: " + options.ErrorMessage);
            }
        }
    }
}
