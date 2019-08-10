// License: GPLv3
// Author: Nuno Fachada

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
                    Coord coord = new Coord(x, y);

                    if (!world.IsOccupied(coord))
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Agent agent = world.GetAgentAt(coord);
                        Console.Write(agent.ID);
                        /*if (agent.Kind == AgentKind.Human)
                            Console.Write('h');
                        else
                            Console.Write('z'); */

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public Direction InputDirection()
        {
            Console.WriteLine("Insert direction: ");
            Console.WriteLine("   Q W E          ↖ ↑ ↗");
            Console.WriteLine("   A   D    or    ←   →");
            Console.WriteLine("   Z X C          ↙ ↓ ↘");
            Console.Write(">> ");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.W:
                        Console.WriteLine(Direction.Up);
                        return Direction.Up;
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.Q:
                        Console.WriteLine(Direction.UpLeft);
                        return Direction.UpLeft;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.A:
                        Console.WriteLine(Direction.Left);
                        return Direction.Left;
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.Z:
                        Console.WriteLine(Direction.DownLeft);
                        return Direction.DownLeft;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.X:
                        Console.WriteLine(Direction.Down);
                        return Direction.Down;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.C:
                        Console.WriteLine(Direction.DownRight);
                        return Direction.DownRight;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.D:
                        Console.WriteLine(Direction.Right);
                        return Direction.Right;
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.E:
                        Console.WriteLine(Direction.UpRight);
                        return Direction.UpRight;
                }
            }
        }
    }
}
