// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Program
    {
        static void Main(string[] args)
        {
            IUserInterface ui = new ConsoleUserInterface();
            Options options = Options.ParseArgs(args);
            if (options.Error)
            {
                foreach (string error in options.ErrorMessages)
                    ui.ShowError("ERROR: " + error);
            }
            else
            {
                Game game = new Game(options, ui);
                game.Play();
            }
        }
    }
}
