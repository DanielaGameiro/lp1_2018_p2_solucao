using System;

namespace ZombiesVsHumans
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void ShowError(string msg)
        {
            Console.Error.WriteLine(msg);
        }

        public void RenderWorld(Agent[,] world)
        {
            for (int y = 0; y < world.GetLength(1); y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    if (world[x, y] == null)
                        Console.Write('.');
                    else if (world[x, y].Kind == AgentKind.Human)
                        Console.Write('h');
                    else
                        Console.Write('z');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}