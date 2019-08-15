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
        private int posInfoLeft;
        private int posInfoTop;
        private int posDialogTop;
        private int posMessagesTop;
        private int posWorldTop;
        private int worldXRenderNCells;
        private int worldYRenderNCells;
        private int worldXRenderLength;
        private int worldYRenderLength;
        private bool worldXRenderFog;
        private bool worldYRenderFog;
        private Queue<string> messageQueue;
        private string[,] cache;

        private bool worldRendered;

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
        private readonly ConsoleColor colInfoBg = ConsoleColor.DarkGray;
        private readonly ConsoleColor colInfoFg = ConsoleColor.White;

        private readonly int worldXRenderNCellsMax = 30;
        private readonly int worldYRenderNCellsMax = 30;
        private readonly int worldCellLength = 4;
        private readonly int worldMinHeight = 4;
        private readonly int posTitleTop = 1;
        private readonly int posTitleLeft = 1;
        private readonly int posWorldTopFromTitle = 2;
        private readonly int posWorldLeft = 1;
        private readonly int posLegendTop = 3;
        private readonly int posLegendLeftFromWorld = 3;
        private readonly int posInfoTopFromWorld = 1;
        private readonly int posInfoLeftFromMessages = 3;
        private readonly int posPlayerDialogLeft = 10;
        private readonly int posPlayerDialogTopFromWorld = 3;
        private readonly int playerDialogWidth = 35;
        private readonly int playerDialogHeight = 10;
        private readonly int posMessagesLeft = 2;
        private readonly int posMessagesTopFromWorld = 1;
        private readonly int messagesMaxNum = 11;
        private readonly int messagesMaxLength = 55;
        private string msgBullet = "> ";

        public ConsoleUserInterface()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            worldRendered = false;
            messageQueue = new Queue<string>(messagesMaxNum);
        }

        public void Initialize(int xDim, int yDim)
        {
            // Variables which define world size in console characters
            int worldLength;
            int worldHeight;

            // Maximum length of messages
            int messagesCompleteLength;

            // Determine distance of world from top
            posWorldTop = posTitleTop + posWorldTopFromTitle;

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
            worldHeight = Math.Max(
                worldYRenderNCells + (worldYRenderFog ? 1 : 0),
                worldMinHeight);

            // Determine complete length of messages
            messagesCompleteLength =
                posMessagesLeft + msgBullet.Length + messagesMaxLength;

            // Determine left position of legend
            posLegendLeft = worldLength + posLegendLeftFromWorld;

            // Determine left position of info
            posInfoLeft = messagesCompleteLength + posInfoLeftFromMessages;

            // Determine top position of info
            posInfoTop =
                posWorldTop + worldHeight + posInfoTopFromWorld;

            // Determine top position of player dialog
            posDialogTop =
                posWorldTop + worldHeight + posPlayerDialogTopFromWorld;

            // Determine position of information messages
            posMessagesTop =
                posWorldTop + worldHeight + posMessagesTopFromWorld;

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
                lastMsg =
                    $"{BlankString(posMessagesLeft)}{msgBullet}{msg}{Environment.NewLine}";
                sb.Append(lastMsg);
            }

            sb.Length = sb.Length - lastMsg.Length;
            SetCursor(0, posMessagesTop);
            SetColor(colMessagesFg, colMessagesBg);
            Console.Write(sb);

            SetColor(colLastMessageFg, colLastMessageBg);
            Console.Write(lastMsg);
        }

        public void RenderStart()
        {
            SetColor(colTitleFg, colTitleBg);
            SetCursor(posTitleLeft, posTitleTop);
            Console.Write(" ========== Zombies VS Humans ========== ");

            SetCursor(posLegendLeft, posLegendTop);
            SetColor(colAIZombieFg, colAIZombieBg);
            Console.Write("zXX");
            SetDefaultColor();
            Console.Write(" Zombie (AI)");

            SetCursor(posLegendLeft, posLegendTop + 1);
            SetColor(colAIHumanFg, colAIHumanBg);
            Console.Write("hXX");
            SetDefaultColor();
            Console.Write(" Human (AI)");

            SetCursor(posLegendLeft, posLegendTop + 2);
            SetColor(colPlayerZombieFg, colPlayerZombieBg);
            Console.Write("ZXX");
            SetDefaultColor();
            Console.Write(" Zombie (player)");

            SetCursor(posLegendLeft, posLegendTop + 3);
            SetColor(colPlayerHumanFg, colPlayerHumanBg);
            Console.Write("HXX");
            SetDefaultColor();
            Console.Write(" Human (player)");
        }

        public void RenderInfo(IDictionary<string, int> info)
        {
            int pos = 1;

            SetColor(colInfoFg, colInfoBg);
            SetCursor(posInfoLeft, posInfoTop);
            Console.WriteLine("╔════════════════════════╗");
            foreach (KeyValuePair<string, int> kv in info)
            {
                SetCursor(posInfoLeft, posInfoTop + pos);
                Console.Write($"║ {kv.Value,5} {kv.Key,-16} ║");
                pos++;
            }
            SetCursor(posInfoLeft, posInfoTop + pos);
            Console.Write("╚════════════════════════╝");
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
                            SetCursor(
                                posWorldLeft + worldCellLength * x,
                                posWorldTop + y);

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
                            SetCursor(
                                posWorldLeft + worldCellLength * x,
                                posWorldTop + y);

                        SetAgentColor(agent.Kind, agent.Movement);

                        Console.Write(agentID);
                        SetDefaultColor();
                        Console.Write(" ");

                        cache[x, y] = agentID;

                    }
                }

                if (worldXRenderFog && !worldRendered)
                {
                    SetCursor(
                        posWorldLeft + worldCellLength * worldXRenderNCells,
                        posWorldTop + y);
                    Console.Write("~~~ ");
                }
            }
            if (worldYRenderFog && !worldRendered)
            {
                string fogLine = new StringBuilder().Insert(
                    0, "~~~ ",
                    worldXRenderNCells + (worldXRenderFog ? 1 : 0)).ToString();

                SetCursor(posWorldLeft, posWorldTop + worldYRenderNCells);
                Console.Write(fogLine);
            }

            worldRendered = true;

        }

        public Direction InputDirection(string id)
        {
            Direction dir = Direction.None;

            SetColor(colPlayerDialogFg, colPlayerDialogBg);

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

            Console.CursorVisible = true;
            while (dir == Direction.None)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.D8:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Console.WriteLine(Direction.Up);
                        dir = Direction.Up;
                        break;
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.D7:
                    case ConsoleKey.Home:
                    case ConsoleKey.Q:
                        Console.WriteLine(Direction.UpLeft);
                        dir = Direction.UpLeft;
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.D4:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Console.WriteLine(Direction.Left);
                        dir = Direction.Left;
                        break;
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                    case ConsoleKey.End:
                    case ConsoleKey.Z:
                        Console.WriteLine(Direction.DownLeft);
                        dir = Direction.DownLeft;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.X:
                        Console.WriteLine(Direction.Down);
                        dir = Direction.Down;
                        break;
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:
                    case ConsoleKey.PageDown:
                    case ConsoleKey.C:
                        Console.WriteLine(Direction.DownRight);
                        dir = Direction.DownRight;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.D6:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Console.WriteLine(Direction.Right);
                        dir = Direction.Right;
                        break;
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.D9:
                    case ConsoleKey.PageUp:
                    case ConsoleKey.E:
                        Console.WriteLine(Direction.UpRight);
                        dir = Direction.UpRight;
                        break;
                }
            }
            Console.CursorVisible = false;
            return dir;
        }

        public void RenderFinish()
        {
            Console.CursorVisible = true;
            SetDefaultColor();
            Console.WriteLine();
        }
        private void SetCursor(int left, int top)
        {
            Console.CursorTop = top;
            Console.CursorLeft = left;
        }

        private void SetDefaultColor()
        {
            SetColor(colDefaultFg, colDefaultBg);
        }

        private void SetColor(ConsoleColor fgColor, ConsoleColor bgColor)
        {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
        }

        private void SetAgentColor(AgentKind kind, AgentMovement mov)
        {
            if (kind == AgentKind.Zombie && mov == AgentMovement.AI)
            {
                SetColor(colAIZombieFg, colAIZombieBg);
            }
            else if (kind == AgentKind.Zombie && mov == AgentMovement.Player)
            {
                SetColor(colPlayerZombieFg, colPlayerZombieBg);
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.AI)
            {
                SetColor(colAIHumanFg, colAIHumanBg);
            }
            else if (kind == AgentKind.Human && mov == AgentMovement.Player)
            {
                SetColor(colPlayerHumanFg, colPlayerHumanBg);
            }
        }

        private string BlankString(int len) =>
            String.Format("{0," + len + "}", " ");
    }
}
