using System;

namespace ZombiesVsHumans
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void ShowError(string msg)
        {
            Console.Error.WriteLine(msg);
        }

        public void RenderWorld(IReadOnlyWorld world)
        {
            for (int y = 0; y < world.YDim; y++)
            {
                for (int x = 0; x < world.XDim; x++)
                {

                    if (!world.IsOccupied(x, y))
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Agent agent = world.GetAgentAt(x, y);
                        if (agent.Kind == AgentKind.Human)
                            Console.Write('h');
                        else
                            Console.Write('z');

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
