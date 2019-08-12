// License: GPLv3
// Author: Nuno Fachada

using System;
using System.Text;

namespace ZombiesVsHumans
{
    public class ConsoleUserInterface : IUserInterface
    {
        private int posLegendLeft;
        private int posAgentInfoLeft;
        private int posDialogTop;
        private int posMessagesLeft;
        private int posMessagesTop;
        private int worldXRenderNCells;
        private int worldYRenderNCells;
        private int worldXRenderLength;
        private int worldYRenderLength;
        private bool worldXRenderFog;
        private bool worldYRenderFog;

        private readonly ConsoleColor colDefaultBg = Console.BackgroundColor;
        private readonly ConsoleColor colDefaultFg = Console.ForegroundColor;
        private readonly ConsoleColor colAIZombieBg = ConsoleColor.Black;
        private readonly ConsoleColor colAIZombieFg = ConsoleColor.Red;
        private readonly ConsoleColor colPlayerZombieBg = ConsoleColor.DarkMagenta;
        private readonly ConsoleColor colPlayerZombieFg = ConsoleColor.DarkRed;
        private readonly ConsoleColor colAIHumanBg = ConsoleColor.Black;
        private readonly ConsoleColor colAIHumanFg = ConsoleColor.Green;
        private readonly ConsoleColor colPlayerHumanBg = ConsoleColor.DarkMagenta;
        private readonly ConsoleColor colPlayerHumanFg = ConsoleColor.DarkGreen;

        private readonly int worldXRenderNCellsMax = 30;
        private readonly int worldYRenderNCellsMax = 30;
        private readonly int worldCellLength = 4;
        private readonly int posTurnTop = 0;
        private readonly int posTurnLeft = 0;
        private readonly int posWorldTop = 1;
        private readonly int posWorldLeft = 0;
        private readonly int posLegendTop = 3;
        private readonly int posLegendLeftFromWorld = 3;
        private readonly int posAgentInfoTop = 10;
        private readonly int posAgentInfoLeftFromWorld = 3;
        private readonly int posDialogLeft = 0;
        private readonly int posDialogTopFromWorld = 2;
        private readonly int dialogWidth = 35;
        private readonly int dialogHeight = 10;
        private readonly int posMessagesLeftFromDialog = 2;
        private readonly int posMessagesTopFromWorld = 2;

        public ConsoleUserInterface()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void Initialize(int xDim, int yDim)
        {
            // Variables which define world size in console characters
            int worldLength;
            int worldHeight;

            // Determine maximum number of world cells to render
            worldXRenderNCells = Math.Min(xDim, worldXRenderNCellsMax);
            worldYRenderNCells = Math.Min(yDim, worldYRenderNCellsMax);

            // Render fog is world doesn't fit into screen
            worldXRenderFog = xDim > worldXRenderNCellsMax;
            worldYRenderFog = yDim > worldYRenderNCellsMax;

            // Determine world length in console characters
            worldLength = posWorldLeft +
                worldCellLength *
                (worldXRenderNCells + (worldXRenderFog ? 1 : 0));

            // Determine world height in console characters
            worldHeight = posTurnTop + yDim;

            // Determine left position of legend
            posLegendLeft = worldLength + posLegendLeftFromWorld;

            // Determine left position of agent info
            posAgentInfoLeft = worldLength + posAgentInfoLeftFromWorld;

            // Determine top position of player dialog
            posDialogTop = worldHeight + posDialogTopFromWorld;

            // Determine position of information messages
            posMessagesLeft = dialogWidth + posMessagesLeftFromDialog;
            posMessagesTop = worldHeight + posMessagesTopFromWorld;

            // Clear console, ready to start
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
            SetCursor(posTurnTop, posTurnLeft);
            Console.Write($"******** Turn {i} *********");
        }

        public void RenderWorld(IReadOnlyWorld world)
        {

            SetCursor(posWorldTop, posWorldLeft);

            for (int y = 0; y < worldYRenderNCells; y++)
            {
                for (int x = 0; x < worldXRenderNCells; x++)
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

                if (worldXRenderFog) Console.Write("~~~ ");
                Console.WriteLine();
            }
            if (worldYRenderFog)
            {
                for (int x = 0;
                    x < worldXRenderNCells + (worldXRenderFog ? 1 : 0);
                    x++)
                {
                    Console.Write("~~~ ");
                }
            }
            Console.WriteLine();
        }

        public Direction InputDirection(string id)
        {

            ClearDialog();
            SetCursor(posDialogTop, posDialogLeft);

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

        private void SetCursor(int top, int left)
        {
            Console.CursorTop = top;
            Console.CursorLeft = left;
        }

        private void SetDefaultColor()
        {
            Console.BackgroundColor = colDefaultBg;
            Console.ForegroundColor = colDefaultFg;
        }
        private void SetAgentColor(AgentKind kind, AgentMovement mov)
        {
            if (kind == AgentKind.Zombie && mov == AgentMovement.AI)
            {
                Console.BackgroundColor = colAIZombieBg;
                Console.ForegroundColor = colAIZombieFg;
            }
            else if (kind == AgentKind.Zombie && mov == AgentMovement.Player)
            {
                Console.BackgroundColor = colPlayerZombieBg;
                Console.ForegroundColor = colPlayerZombieFg;
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.AI)
            {
                Console.BackgroundColor = colAIHumanBg;
                Console.ForegroundColor = colAIHumanFg;
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.Player)
            {
                Console.BackgroundColor = colPlayerHumanBg;
                Console.ForegroundColor = colPlayerHumanFg;
            }
        }

        private void ClearDialog()
        {
            string blank = String.Format("{0," + dialogWidth + "}", " ");

            SetCursor(posDialogTop, posDialogLeft);
            SetDefaultColor();

            for (int i = 0; i < dialogHeight; i++)
                Console.WriteLine(blank);
        }
    }
}
