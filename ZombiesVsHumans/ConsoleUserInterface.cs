/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.ConsoleUserInterface,
/// que implementa uma UI em consola.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System;
using System.Text;
using System.Collections.Generic;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Classe que implementa uma UI em modo de consola para o jogo Zombies
    /// vs Humans.
    /// </summary>
    public class ConsoleUserInterface : IUserInterface
    {

        // //////////////////////////////////////////////////////////////// //
        // Constantes que não devem ser modificadas, utilizadas pela cache. //
        // //////////////////////////////////////////////////////////////// //

        /// <summary>
        /// Constante que representa, na cache, um espaço sem agentes.
        /// </summary>
        private const string EMPTY = null;

        /// <summary>
        /// Constante que representa um espaço não inicializado na cache, ou
        /// seja, ainda não mostrado no ecrã.
        /// </summary>
        private const string UNINITIALIZED = " ";

        // ///////////////////////////////////////////////////////////////// //
        // Variáveis de instância só de leitura que podem ser alteradas      //
        // manualmente p/ personalizar a UI.                                 //
        // Idealmente os seus valores deveriam ser carregados a partir de um //
        // ficheiro de configuração.
        // ///////////////////////////////////////////////////////////////// //

        /// <summary>
        /// Cor de fundo por omissão.
        /// </summary>
        private readonly ConsoleColor colDefaultBg = Console.BackgroundColor;

        /// <summary>
        /// Cor de primeiro plano por omissão.
        /// </summary>
        private readonly ConsoleColor colDefaultFg = Console.ForegroundColor;

        /// <summary>
        /// Cor de fundo dos zombies controlados pela IA.
        /// </summary>
        private readonly ConsoleColor colAIZombieBg = ConsoleColor.Black;

        /// <summary>
        /// Cor de primeiro plano dos zombies controlados pela IA.
        /// </summary>
        private readonly ConsoleColor colAIZombieFg = ConsoleColor.Red;

        /// <summary>
        /// Cor de fundo dos zombies controlados pelo jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerZombieBg = ConsoleColor.DarkMagenta;

        /// <summary>
        /// Cor de primeiro plano dos zombies controlados pelo jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerZombieFg = ConsoleColor.DarkRed;

        /// <summary>
        /// Cor de fundo dos humanos controlados pela IA.
        /// </summary>
        private readonly ConsoleColor colAIHumanBg = ConsoleColor.Black;

        /// <summary>
        /// Cor de primeiro plano dos humanos controlados pela IA.
        /// </summary>
        private readonly ConsoleColor colAIHumanFg = ConsoleColor.Green;

        /// <summary>
        /// Cor de fundo dos humanos controlados pelo jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerHumanBg = ConsoleColor.DarkMagenta;

        /// <summary>
        /// Cor de primeiro plano dos humanos controlados pelo jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerHumanFg = ConsoleColor.DarkGreen;

        /// <summary>
        /// Cor de fundo do título.
        /// </summary>
        private readonly ConsoleColor colTitleBg = ConsoleColor.Yellow;

        /// <summary>
        /// Cor de primeiro plano do título.
        /// </summary>
        private readonly ConsoleColor colTitleFg = ConsoleColor.Black;

        /// <summary>
        /// Cor de fundo das mensagens.
        /// </summary>
        private readonly ConsoleColor colMessagesBg = Console.BackgroundColor;

        /// <summary>
        /// Cor de primeiro plano das mensagens.
        /// </summary>
        private readonly ConsoleColor colMessagesFg = ConsoleColor.Gray;

        /// <summary>
        /// Cor de fundo da mensagem mais recente.
        /// </summary>
        private readonly ConsoleColor colLastMessageBg = ConsoleColor.DarkGray;

        /// <summary>
        /// Cor de primeiro plano da mensagem mais recente.
        /// </summary>
        private readonly ConsoleColor colLastMessageFg = ConsoleColor.White;

        /// <summary>
        /// Cor de fundo da janela de diálogo que pede a direção ao jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerDialogBg = ConsoleColor.DarkGreen;

        /// <summary>
        /// Cor de primeiro plano da janela de diálogo que pede a direção ao
        /// jogador.
        /// </summary>
        private readonly ConsoleColor colPlayerDialogFg = ConsoleColor.White;

        /// <summary>
        /// Cor de fundo do painel de informação/estatísticas.
        /// </summary>
        private readonly ConsoleColor colInfoBg = ConsoleColor.DarkGray;

        /// <summary>
        /// Cor de primeiro plano do painel de informação/estatísticas.
        /// </summary>
        private readonly ConsoleColor colInfoFg = ConsoleColor.White;

        /// <summary>
        /// Número máximo de células a mostrar na horizontal.
        /// </summary>
        private readonly int worldXRenderNCellsMax = 30;

        /// <summary>
        /// Número máximo de células a mostrar na vertical.
        /// </summary>
        private readonly int worldYRenderNCellsMax = 30;

        /// <summary>
        /// Número de carateres (dimensão horizontal) de cada célula do mundo.
        /// </summary>
        private readonly int worldCellLength = 4;

        /// <summary>
        /// Altura mínima (em carateres) reservada no ecrã para mostrar o mundo.
        /// </summary>
        private readonly int worldMinHeight = 4;

        /// <summary>
        /// Distância vertical (em carateres) entre o título e o topo da
        /// consola.
        /// </summary>
        private readonly int posTitleTop = 1;

        /// <summary>
        /// Distância horizontal (em carateres) entre título e o lado esquerdo
        /// da consola.
        /// </summary>
        private readonly int posTitleLeft = 1;

        /// <summary>
        /// Distância vertical (em carateres) entre o mundo e o título.
        /// </summary>
        private readonly int posWorldTopFromTitle = 2;

        /// <summary>
        /// Distância horizontal (em carateres) entre o mundo e o lado esquerdo
        /// da consola.
        /// </summary>
        private readonly int posWorldLeft = 1;

        /// <summary>
        /// Distância vertical (em carateres) entre a legenda e o topo da
        /// consola.
        /// </summary>
        private readonly int posLegendTop = 3;

        /// <summary>
        /// Distância horizontal (em carateres) entre a legenda e o mundo.
        /// </summary>
        private readonly int posLegendLeftFromWorld = 3;

        /// <summary>
        /// Distância vertical (em carateres) entre o painel de informação e
        /// o mundo.
        /// </summary>
        private readonly int posInfoTopFromWorld = 1;

        /// <summary>
        /// Distância horizontal (em carateres) entre o painel de informação
        /// e o painel de mensagens.
        /// </summary>
        private readonly int posInfoLeftFromMessages = 3;

        /// <summary>
        /// Distância horizontal (em carateres) entre o diálogo do jogador e
        /// o lado esquerdo da consola.
        /// </summary>
        private readonly int posPlayerDialogLeft = 10;

        /// <summary>
        /// Distância vertical (em carateres) entre o diálogo do jogador e o
        /// mundo.
        /// </summary>
        private readonly int posPlayerDialogTopFromWorld = 3;

        /// <summary>
        /// Distância horizontal (em carateres) entre o painel de mensagens e
        /// o lado esquerdo da consola.
        /// </summary>
        private readonly int posMessagesLeft = 2;

        /// <summary>
        /// Distância vertical (em carateres) entre o painel de mensagens e o
        /// mundo.
        /// </summary>
        private readonly int posMessagesTopFromWorld = 1;

        /// <summary>
        /// Número máximo de mensagens a mostrar no painel de mensagens.
        /// </summary>
        private readonly int messagesMaxNum = 11;

        /// <summary>
        /// Dimensão máxima (em carateres) das mensagens.
        /// </summary>
        private readonly int messagesMaxLength = 55;

        /// <summary>
        /// String que marca o início de cada mensagem.
        /// </summary>
        private readonly string msgBullet = "> ";

        // ////////////////////////////////////////////////////////////////// //
        // Variáveis de instância cujo valor depende das variáveis só de      //
        // leitura definidas em cima.                                         //
        // ////////////////////////////////////////////////////////////////// //

        /// <summary>
        /// Distância horizontal (em carateres) entre a legenda e o lado
        /// esquerdo da consola.
        /// </summary>
        private int posLegendLeft;

        /// <summary>
        /// Distância horizontal (em carateres) entre o painel de informação
        /// e o lado esquerdo da consola.
        /// </summary>
        private int posInfoLeft;

        /// <summary>
        /// Distância vertical (em carateres) entre o painel de informação e o
        /// topo da consola.
        /// </summary>
        private int posInfoTop;

        /// <summary>
        /// Distância vertical (em carateres) entre o diálogo do jogador e o
        /// topo da consola.
        /// </summary>
        private int posDialogTop;

        /// <summary>
        /// Distância vertical (em carateres) entre o painel de mensagens e o
        /// topo da consola.
        /// </summary>
        private int posMessagesTop;

        /// <summary>
        /// Distância vertical (em carateres) entre o mundo e o topo da consola.
        /// </summary>
        private int posWorldTop;

        /// <summary>
        /// Número de células do mundo a mostrar na horizontal.
        /// </summary>
        private int worldXRenderNCells;

        /// <summary>
        /// Número de células do mundo a mostrar na vertical.
        /// </summary>
        private int worldYRenderNCells;

        /// <summary>
        /// Mostrar nevoeiro horizontal? Isto ocorre se a dimensão horizontal
        /// do mundo for maior que <see cref="worldXRenderNCellsMax"/>.
        /// </summary>
        private bool worldXRenderFog;

        /// <summary>
        /// Mostrar nevoeiro vertical? Isto ocorre se a dimensão vertical
        /// do mundo for maior que <see cref="worldYRenderNCellsMax"/>.
        /// </summary>
        private bool worldYRenderFog;

        /// <summary>
        /// Fila de mensagens a mostrar.
        /// </summary>
        private Queue<string> messageQueue;

        /// <summary>
        /// Cache do mundo que permite evitarmos renderizar posições do mundo
        /// cujos conteúdos não foram alterados.
        /// </summary>
        private string[,] cache;

        /// <summary>
        /// Booleano que indica que se o mundo já foi alguma vez renderizado.
        /// </summary>
        private bool worldRendered;

        /// <summary>
        /// Construtor, cria uma nova instância da UI em consola.
        /// </summary>
        public ConsoleUserInterface()
        {
            // Especificar codificação UTF-8, de modo a podermos mostrar uma
            // maior gama de carateres
            Console.OutputEncoding = Encoding.UTF8;

            // Não mostrar cursos a piscar, tornando a visualização mais
            // agradável
            Console.CursorVisible = false;

            // Indicar que o mundo ainda não foi renderizado
            worldRendered = false;
            messageQueue = new Queue<string>(messagesMaxNum);
        }

        /// <summary>
        /// Inicializa e calcula todas as variáveis necessárias para desenhar
        /// a UI no ecrã, com base na dimensão do mundo.
        /// </summary>
        /// <remarks>
        /// Este método respeita o que está definido em
        /// <see cref="IUserInterface.Initialize(int, int)"/>.
        /// </remarks>
        /// <param name="xDim">Dimensão horizontal do mundo.</param>
        /// <param name="yDim">Dimensão vertical do mundo.</param>
        public void Initialize(int xDim, int yDim)
        {
            // Variáveis que definem o tamanho do mundo (em carateres)
            int worldLength;
            int worldHeight;

            // Tamanho completo das mensagens, incluindo o marcador inicial
            int messagesCompleteLength;

            // Calcular a distância vertical entre o mundo e o topo da consola
            posWorldTop = posTitleTop + posWorldTopFromTitle;

            // Calcular o número máximo de células do mundo a mostrar no ecrã
            worldXRenderNCells = Math.Min(xDim, worldXRenderNCellsMax);
            worldYRenderNCells = Math.Min(yDim, worldYRenderNCellsMax);

            // Decidir se vamos mostrar o nevoeiro na horizontal e/ou vertical
            worldXRenderFog = xDim > worldXRenderNCellsMax;
            worldYRenderFog = yDim > worldYRenderNCellsMax;

            // Calcular dimensão horizontal do mundo em carateres
            worldLength = posWorldLeft +
                worldCellLength *
                (worldXRenderNCells + (worldXRenderFog ? 1 : 0));

            // Calcular dimensão vertical do mundo em carateres
            worldHeight = Math.Max(
                worldYRenderNCells + (worldYRenderFog ? 1 : 0),
                worldMinHeight);

            // Calcular tamanho total das mensagens, incluindo marcador inicial
            messagesCompleteLength =
                posMessagesLeft + msgBullet.Length + messagesMaxLength;

            // Calcular distância horizontal (em carateres) entre a legenda e o
            // lado esquerdo da consola
            posLegendLeft = worldLength + posLegendLeftFromWorld;

            // Calcular distância horizontal (em carateres) entre o painel de
            // informação e o lado esquerdo da consola
            posInfoLeft = messagesCompleteLength + posInfoLeftFromMessages;

            // Calcular distância vertical (em carateres) entre o painel de
            // informação e o topo da consola
            posInfoTop =
                posWorldTop + worldHeight + posInfoTopFromWorld;

            // Calcular distância vertical (em carateres) entre o diálogo do
            // jogador e o topo da consola
            posDialogTop =
                posWorldTop + worldHeight + posPlayerDialogTopFromWorld;

            // Calcular distância vertical (em carateres) entre o painel de
            // mensagens e o topo da consola
            posMessagesTop =
                posWorldTop + worldHeight + posMessagesTopFromWorld;

            // Criar cache de visualização, que irá conter os IDs dos agentes
            // já renderizados, bem como as posições vazias
            cache = new string[xDim, yDim];
            // Inicialmente, todos o elementos da cache não estão inicializados
            // (só estarão inicializados após a primeira renderização)
            for (int i = 0; i < xDim; i++)
                for (int j = 0; j < yDim; j++)
                    cache[i, j] = UNINITIALIZED;
        }

        /// <summary>
        /// Mostrar uma mensagem de erro.
        /// </summary>
        /// <param name="msg">Mensagem de erro a mostrar.</param>
        public void RenderError(string msg)
        {
            // Limpar consola
            Console.Clear();

            // Mostrar mensagem de erro no output específico para erros
            // (Console.Error)
            Console.Error.WriteLine(msg);
        }


        /// <summary>
        /// Apresenta uma mensagem ao utilizador.
        /// </summary>
        /// <param name="message">Mensagem a apresentar.</param>
        public void RenderMessage(string message)
        {
            // Variável onde guardar a última mensagem, que será mostrada com
            // cores diferentes
            string lastMsg = null;

            // Um string builder, que permite construir strings de forma
            // mais eficiente (comparativamente a concatenar strings)
            // Vai ser usado para construir todas as mensagens a imprimir de
            // uma só vez
            StringBuilder sb = new StringBuilder();

            // Se a fila de mensagens estiver cheia...
            if (messageQueue.Count == messagesMaxNum)
            {
                // ...remover mensagem mais antiga da fila
                messageQueue.Dequeue();
            }

            // O tamanho da mensagem é superior ao máximo?
            if (message.Length < messagesMaxLength)
            {
                // Se não for, colocar espaços no fim de modo a que a string
                // fique com o tamanho máximo
                // Isto permite mostrar a cor de fundo das mensagens de forma
                // consistente
                message = message
                    + BlankString(messagesMaxLength - message.Length);
            }
            else
            {
                // Se for, encurtar a mensagem de modo a que fique com o
                // tamanho máximo
                message = message.Substring(0, messagesMaxLength);
            }

            // Colocar mensagem na fila
            messageQueue.Enqueue(message);

            // Construir string contendo as mensagens a mostrar
            foreach (string msg in messageQueue)
            {
                // Construir mensagem atual, com espaçamento à esquerda,
                // marcador de início de mensagem, a mensagem propriamente dita
                // e a nova linha ("\n" em Linux/Mac, "\r\n" em Windows)
                lastMsg =
                    $"{BlankString(posMessagesLeft)}{msgBullet}{msg}{Environment.NewLine}";

                // Adicionar mensagem atual ao string builder, que produzirá
                // todas as mensagens a mostrar de forma mais eficiente
                sb.Append(lastMsg);
            }

            // Remover última mensagem do string builder, pois esta será
            // mostrada com cores diferentes
            sb.Length = sb.Length - lastMsg.Length;

            // Posicionar o cursor para mostrar o painel de mensagens
            SetCursor(0, posMessagesTop);

            // Definir cores das mensagens (exceto da última)
            SetColor(colMessagesFg, colMessagesBg);

            // Mostrar mensagens (exceto a última)
            // Notar que o string builder é automaticamente convertido em
            // string através do seu método ToString()
            Console.Write(sb);

            // Definir a cor para a última mensagem e mostrá-la
            SetColor(colLastMessageFg, colLastMessageBg);
            Console.Write(lastMsg);
        }

        public void RenderStart()
        {
            // Limpar consola, prontos para começar
            Console.Clear();

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
