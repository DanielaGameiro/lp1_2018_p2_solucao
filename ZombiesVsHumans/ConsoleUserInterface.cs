// License: GPLv3
// Author: Nuno Fachada

using System;
using System.Text;
using System.Collections.Generic;

namespace ZombiesVsHumans
{
    public class ConsoleUserInterface : IUserInterface
    {
        private int posLegendLeft;
        private int posDialogTop;
        private int posMessagesTop;
        private int worldXRenderNCells;
        private int worldYRenderNCells;
        private int worldXRenderLength;
        private int worldYRenderLength;
        private bool worldXRenderFog;
        private bool worldYRenderFog;
        private Queue<string> messageQueue;
        private string[,] cache;

        private const string EMPTY = null;
        private const string UNINITIALIZED = " ";

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
        private readonly ConsoleColor colTitleBg = ConsoleColor.Yellow;
        private readonly ConsoleColor colTitleFg = ConsoleColor.Black;
        private readonly ConsoleColor colMessagesBg = Console.BackgroundColor;
        private readonly ConsoleColor colMessagesFg = ConsoleColor.Gray;
        private readonly ConsoleColor colLastMessageBg = ConsoleColor.DarkGray;
        private readonly ConsoleColor colLastMessageFg = ConsoleColor.White;
        private readonly ConsoleColor colPlayerDialogBg = ConsoleColor.DarkGreen;
        private readonly ConsoleColor colPlayerDialogFg = ConsoleColor.White;

        private readonly int worldXRenderNCellsMax = 30;
        private readonly int worldYRenderNCellsMax = 30;
        private readonly int worldCellLength = 4;
        private readonly int posTitleTop = 0;
        private readonly int posTitleLeft = 0;
        private readonly int posWorldTop = 2;
        private readonly int posWorldLeft = 1;
        private readonly int posLegendTop = 3;
        private readonly int posLegendLeftFromWorld = 3;
        private readonly int posPlayerDialogLeft = 10;
        private readonly int posPlayerDialogTopFromWorld = 3;
        private readonly int playerDialogWidth = 35;
        private readonly int playerDialogHeight = 10;
        private readonly int posMessagesLeft = 2;
        private readonly int posMessagesTopFromWorld = 1;
        private readonly int messagesMaxNum = 11;
        private readonly int messagesMaxLength = 60;

        public ConsoleUserInterface()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            messageQueue = new Queue<string>(messagesMaxNum);
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
            worldHeight = worldYRenderNCells + (worldYRenderFog ? 1 : 0);

            // Determine left position of legend
            posLegendLeft = worldLength + posLegendLeftFromWorld;

            // Determine top position of player dialog
            posDialogTop = posWorldTop + worldHeight + posPlayerDialogTopFromWorld;

            // Determine position of information messages
            posMessagesTop = posWorldTop + worldHeight + posMessagesTopFromWorld;

            // Initialize visualization cache (contains IDs of agents in the
            // several world positions)
            cache = new string[xDim, yDim];
            for (int i = 0; i < xDim; i++)
                for (int j = 0; j < yDim; j++)
                    cache[i, j] = UNINITIALIZED;

            // Clear console, ready to start
            Console.Clear();
        }

        public void RenderError(string msg)
        {
            Console.Clear();
            Console.Error.WriteLine(msg);
        }

        public void RenderMessage(string message)
        {
            string msgBullet = "> ";
            string lastMsg = message;
            StringBuilder sb = new StringBuilder();

            if (messageQueue.Count == messagesMaxNum)
            {
                messageQueue.Dequeue();
            }

            if (message.Length < messagesMaxLength)
            {
                message = message
                    + BlankString(messagesMaxLength - message.Length);
            }
            else
            {
                message = message.Substring(0, messagesMaxLength);
            }

            messageQueue.Enqueue(message);

            foreach (string msg in messageQueue)
            {
                lastMsg = $"{BlankString(posMessagesLeft)}{msgBullet}{msg}{Environment.NewLine}";
                sb.Append(lastMsg);
            }

            sb.Length = sb.Length - lastMsg.Length;
            SetCursor(0, posMessagesTop);
            Console.BackgroundColor = colMessagesBg;
            Console.ForegroundColor = colMessagesFg;
            Console.Write(sb);

            Console.BackgroundColor = colLastMessageBg;
            Console.ForegroundColor = colLastMessageFg;
            Console.Write(lastMsg);
        }

        public void RenderTitle()
        {
            SetCursor(posTitleLeft, posTitleTop);
            Console.BackgroundColor = colTitleBg;
            Console.ForegroundColor = colTitleFg;
            Console.Write(" ========== Zombies VS Humans ========== ");
        }

        public void RenderLegend(int i)
        {
            SetCursor(posLegendLeft, posLegendTop);
            Console.Write($"Turn {i,4:d4}");
        }

        public void RenderWorld(IReadOnlyWorld world)
        {

            for (int y = 0; y < worldYRenderNCells; y++)
            {
                if (cache[0, y] == UNINITIALIZED)
                    SetCursor(posWorldLeft, posWorldTop + y);

                for (int x = 0; x < worldXRenderNCells; x++)
                {
                    Coord coord = new Coord(x, y);

                    if (!world.IsOccupied(coord))
                    {
                        if (cache[x, y] == EMPTY) continue;

                        if (cache[x, y] != UNINITIALIZED)
                            SetCursor(posWorldLeft + worldCellLength * x, posWorldTop + y);

                        SetDefaultColor();
                        Console.Write("... ");

                        cache[x, y] = EMPTY;

                    }
                    else
                    {
                        string agentID;
                        Agent agent = world.GetAgentAt(coord);

                        agentID = agent.ToString();

                        if (cache[x, y] == agentID) continue;

                        if (agentID.Length > 3) agentID =
                            agentID.Substring(0, 3);

                        if (cache[x, y] != UNINITIALIZED)
                            SetCursor(posWorldLeft + worldCellLength * x, posWorldTop + y);

                        SetAgentColor(agent.Kind, agent.Movement);

                        Console.Write(agentID);
                        SetDefaultColor();
                        Console.Write(" ");

                        cache[x, y] = agentID;

                    }
                }

                if (worldXRenderFog) Console.Write("~~~ ");
            }
            if (worldYRenderFog)
            {
                SetCursor(posWorldLeft, posWorldTop + worldYRenderNCells);
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
            Console.BackgroundColor = colPlayerDialogBg;
            Console.ForegroundColor = colPlayerDialogFg;

            SetCursor(posPlayerDialogLeft, posDialogTop);
            Console.Write("╔══════════════════════════╗");
            SetCursor(posPlayerDialogLeft, posDialogTop + 1);
            Console.Write($"║ Where to move {id}?       ║");
            SetCursor(posPlayerDialogLeft, posDialogTop + 2);
            Console.Write("║   Q W E          ↖ ↑ ↗   ║");
            SetCursor(posPlayerDialogLeft, posDialogTop + 3);
            Console.Write("║   A   D    or    ←   →   ║");
            SetCursor(posPlayerDialogLeft, posDialogTop + 4);
            Console.Write("║   Z X C          ↙ ↓ ↘   ║");
            SetCursor(posPlayerDialogLeft, posDialogTop + 5);
            Console.Write("║  >>>                     ║");
            SetCursor(posPlayerDialogLeft, posDialogTop + 6);
            Console.Write("╚══════════════════════════╝");
            SetCursor(posPlayerDialogLeft + 7, posDialogTop + 5);

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

        private void SetCursor(int left, int top)
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

        private string BlankString(int len) =>
            String.Format("{0," + len + "}", " ");
    }
}
