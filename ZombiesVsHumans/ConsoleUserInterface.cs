// License: GPLv3
// Author: Nuno Fachada

using System;
using System.Text;

namespace ZombiesVsHumans
{
    public class ConsoleUserInterface : IUserInterface
    {
        public readonly int MaxXRenderDim = 30;
        public readonly int MaxYRenderDim = 30;
        public readonly ConsoleColor defaultColorBg = Console.BackgroundColor;
        public readonly ConsoleColor defaultColorFg = Console.ForegroundColor;
        public readonly ConsoleColor zombieColorBg = ConsoleColor.Black;
        public readonly ConsoleColor zombieColorFg = ConsoleColor.Red;
        public readonly ConsoleColor playerZombieColorBg = ConsoleColor.DarkMagenta;
        public readonly ConsoleColor playerZombieColorFg = ConsoleColor.DarkRed;
        public readonly ConsoleColor humanColorBg = ConsoleColor.Black;
        public readonly ConsoleColor humanColorFg = ConsoleColor.Green;
        public readonly ConsoleColor playerHumanColorBg = ConsoleColor.DarkMagenta;
        public readonly ConsoleColor playerHumanColorFg = ConsoleColor.DarkGreen;

        private int xDim;
        private int yDim;

        public ConsoleUserInterface()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void Initialize(int xDim, int yDim)
        {
            this.xDim = xDim;
            this.yDim = yDim;
            Console.Clear();
        }

        public void RenderError(string msg)
        {
            Console.Error.WriteLine(msg);
        }

        public void RenderMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        public void RenderTurn(int i)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.Write($"******** Turn {i} *********");
        }

        public void RenderWorld(IReadOnlyWorld world)
        {
            int xRenderDim = Math.Min(world.XDim, MaxXRenderDim);
            int yRenderDim = Math.Min(world.YDim, MaxYRenderDim);
            bool renderXFog = world.XDim > MaxXRenderDim;
            bool renderYFog = world.YDim > MaxYRenderDim;

            Console.CursorTop = 1;
            Console.CursorLeft = 0;

            for (int y = 0; y < yRenderDim; y++)
            {
                for (int x = 0; x < xRenderDim; x++)
                {
                    Coord coord = new Coord(x, y);

                    if (!world.IsOccupied(coord))
                    {
                        SetDefaultColor();
                        Console.Write("... ");
                    }
                    else
                    {
                        Agent agent = world.GetAgentAt(coord);
                        string agentID = agent.ToString();

                        if (agentID.Length > 3) agentID =
                            agentID.Substring(0, 3);

                        SetAgentColor(agent.Kind, agent.Movement);

                        Console.Write(agentID);
                        SetDefaultColor();
                        Console.Write(" ");

                    }
                }

                if (renderXFog) Console.Write("~~~ ");
                Console.WriteLine();
            }
            if (renderYFog)
            {
                for (int x = 0; x < xRenderDim + (renderXFog ? 1 : 0); x++)
                {
                    Console.Write("~~~ ");
                }
            }
            Console.WriteLine();
        }

        public Direction InputDirection(string id)
        {
            Console.WriteLine($"Where to move {id}? ");
            Console.WriteLine("   Q W E          ↖ ↑ ↗");
            Console.WriteLine("   A   D    or    ←   →");
            Console.WriteLine("   Z X C          ↙ ↓ ↘");
            Console.Write(">> ");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.D8:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Console.WriteLine(Direction.Up);
                        return Direction.Up;
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.D7:
                    case ConsoleKey.Home:
                    case ConsoleKey.Q:
                        Console.WriteLine(Direction.UpLeft);
                        return Direction.UpLeft;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Console.WriteLine(Direction.Left);
                        return Direction.Left;
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                    case ConsoleKey.End:
                    case ConsoleKey.Z:
                        Console.WriteLine(Direction.DownLeft);
                        return Direction.DownLeft;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.X:
                        Console.WriteLine(Direction.Down);
                        return Direction.Down;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                    case ConsoleKey.PageDown:
                    case ConsoleKey.C:
                        Console.WriteLine(Direction.DownRight);
                        return Direction.DownRight;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.D6:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Console.WriteLine(Direction.Right);
                        return Direction.Right;
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.D9:
                    case ConsoleKey.PageUp:
                    case ConsoleKey.E:
                        Console.WriteLine(Direction.UpRight);
                        return Direction.UpRight;
                }
            }
        }

        private void SetDefaultColor()
        {
            Console.BackgroundColor = defaultColorBg;
            Console.ForegroundColor = defaultColorFg;
        }
        private void SetAgentColor(AgentKind kind, AgentMovement mov)
        {
            if (kind == AgentKind.Zombie && mov == AgentMovement.AI)
            {
                Console.BackgroundColor = zombieColorBg;
                Console.ForegroundColor = zombieColorFg;
            }
            else if (kind == AgentKind.Zombie && mov == AgentMovement.Player)
            {
                Console.BackgroundColor = playerZombieColorBg;
                Console.ForegroundColor = playerZombieColorFg;
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.AI)
            {
                Console.BackgroundColor = humanColorBg;
                Console.ForegroundColor = humanColorFg;
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.Player)
            {
                Console.BackgroundColor = playerHumanColorBg;
                Console.ForegroundColor = playerHumanColorFg;
            }
        }
    }
}
