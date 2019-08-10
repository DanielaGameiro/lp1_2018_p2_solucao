// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Program
    {
        public static IUserInterface UI { get; private set; }
        static void Main(string[] args)
        {
            UI = new ConsoleUserInterface();

            Options options = Options.ParseArgs(args);
            if (options.Error)
            {
                foreach (string error in options.ErrorMessages)
                    UI.RenderError("ERROR: " + error);
            }
            else
            {
                Game game = new Game(options);
                game.Play();
            }
        }
    }
}
