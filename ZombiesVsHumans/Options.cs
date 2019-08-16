/// @file
/// @brief Este ficheiro contém a `struct` ZombiesVsHumans.Options, trata e
/// contém as opções inseridas pelo utilizador na linha de comandos.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System.Collections.Generic;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Esta struct trata as opções inseridas pelo utilizador na linha de
    /// comandos, disponibilizando-as depois em propriedades.
    /// </summary>
    /// <remarks>
    /// Esta struct não pode ser instanciada diretamente uma vez que os seus
    /// construtores são privados. Para criar uma instância desta struct é
    /// necessário usar o método estático <see cref="ParseArgs"/>, que serve
    /// como "fábrica" de objetos deste tipo.
    /// </remarks>
    public struct Options
    {
        /// <summary>
        /// Propriedade auto-implementada que contém a dimensão horizontal do
        /// mundo de jogo.
        /// </summary>
        /// <value>A dimensão horizontal do mundo de jogo.</value>
        public uint XDim { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém a dimensão vertical do
        /// mundo de jogo.
        /// </summary>
        /// <value>A dimensão vertical do mundo de jogo.</value>
        public uint YDim { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém o número inicial de
        /// zombies.
        /// </summary>
        /// <value>Número inicial de zombies.</value>
        public uint Zombies { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém o número inicial de
        /// humanos.
        /// </summary>
        /// <value>Número inicial de humanos.</value>
        public uint Humans { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém o número inicial de
        /// zombies controlados pelo jogador.
        /// </summary>
        /// <value>Número inicial de zombies controlados pelo jogador.</value>
        public uint PlayerZombies { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém o número inicial de
        /// humanos controlados pelo jogador.
        /// </summary>
        /// <value>Número inicial de humanos controlados pelo jogador.</value>
        public uint PlayerHumans { get; }

        /// <summary>
        /// Propriedade auto-implementada que contém o número máximo de turnos
        /// do jogo.
        /// </summary>
        /// <value>Número máximo de turnos do jogo.</value>
        public uint Turns { get; }

        /// <summary>
        /// Propriedade auto-implementada que indica se ocorreram erros no
        /// tratamento das opções da linha de comandos.
        /// </summary>
        /// <value>
        /// O seu valor é <c>true</c> caso tenham ocorrido erros no tratamento
        /// das opções da linha de comandos e <c>false</c> caso contrário.
        /// </value>
        public bool Error { get; private set; }

        /// <summary>
        /// Propriedade que expõe um enumerável mensagens de erro que possam
        /// ter ocorrido no tratamento das opções da linha de comandos.
        /// </summary>
        /// <value>
        /// Enumerável com mensagens de erro.
        /// </value>
        public IEnumerable<string> ErrorMessages => errorMessages;

        /// <summary>
        /// Variável de instância privada que suporta a propriedade
        /// <see cref="ErrorMessages"/>, ou seja, que contém a lista concreta
        /// de erros ocorridos no tratamento das opções da linha de comandos.
        /// </summary>
        private IList<string> errorMessages;

        /// <summary>
        /// Lista de opções da linha de comandos que são válidas. Usamos uma
        /// <c>IList</c> pois precisamos do método <c>Contains()</c> e de
        /// aceder a cada uma das opções com um índice.
        /// </summary>
        private static IList<string> validOptions;

        /// <summary>
        /// Construtor estático que simplesmente inicializa a variável de
        /// instância <see cref="validOptions"/>.
        /// </summary>
        static Options()
        {
            validOptions = new List<string>()
                { "-x", "-y", "-z", "-h", "-Z", "-H", "-t" };
        }

        /// <summary>
        /// Construtor privado que inicializa uma instância desta struct
        /// com valores válidos para as diferentes opções da linha de comandos.
        /// </summary>
        /// <param name="xDim">Dimensão horizontal da grelha de jogo.</param>
        /// <param name="yDim">Dimensão vertical da grelha de jogo.</param>
        /// <param name="zombies">Número total de zombies.</param>
        /// <param name="humans">Número total de humanos.</param>
        /// <param name="playerZombies">Número de zombies controlados por jogadores.</param>
        /// <param name="playerHumans">Número de humanos controlados por jogadores.</param>
        /// <param name="turns">Número máximo de turnos.</param>
        private Options(uint xDim, uint yDim, uint zombies, uint humans,
            uint playerZombies, uint playerHumans, uint turns)
        {
            // Guardar as opções válidas
            XDim = xDim;
            YDim = yDim;
            Zombies = zombies;
            Humans = humans;
            PlayerZombies = playerZombies;
            PlayerHumans = playerHumans;
            Turns = turns;
            // Especificar que não houve qualquer erro
            Error = false;
            errorMessages = new List<string>();
        }

        /// <summary>
        /// Construtor privado que inicializa uma instância desta struct
        /// assumindo que ocorreu um erro no tratamento das opções da linha de
        /// comandos.
        /// </summary>
        /// <param name="error">Mensagem do primeiro erro ocorrido.</param>
        private Options(string error)
        {
            // Inicializar todas as opções a zero, pois ocorreu um erro
            XDim = 0;
            YDim = 0;
            Zombies = 0;
            Humans = 0;
            PlayerZombies = 0;
            PlayerHumans = 0;
            Turns = 0;
            // Indicar a ocorrência desse erro
            Error = true;
            errorMessages = new List<string>() { error };
        }

        /// <summary>
        /// Método estático que trata as opções da linha de comandos,
        /// produzindo objetos do tipo <see cref="Options"/> que contêm essas
        /// mesmas opções de forma tratada.
        /// </summary>
        /// <param name="args">Opções da linha de comandos.</param>
        /// <returns>
        /// Um objeto do tipo <see cref="Options"/> que contém as opções do
        /// jogo.
        /// </returns>
        public static Options ParseArgs(string[] args)
        {
            // Opções a devolver (nota: as structs têm sempre um construtor
            // vazio que inicializa todos os seus campos com o valor por
            // omissão)
            Options optionsObject = new Options();

            // Dicionário usado para ir guardando e processando as opções
            IDictionary<string, uint> options = new Dictionary<string, uint>();

            // Se o número de opções não for válido...
            if (args.Length != 2 * validOptions.Count)
            {
                // ...devolver um objeto Options com indicação desse erro
                return new Options("Invalid number of arguments");
            }

            // Processar as opções, "saltando" por cima dos valores das mesmas
            // para já
            for (int i = 0; i < args.Length; i += 2)
            {
                // Será a opção atual válida?
                if (validOptions.Contains(args[i]))
                {
                    // Será que esta opção ainda não foi indicada?
                    if (!options.ContainsKey(args[i]))
                    {
                        // Verificar se relativamente à opção atual foi dado
                        // um inteiro sem sinal válido
                        if (uint.TryParse(args[i + 1], out uint value))
                        {
                            // Pelos vistos sim, guardar esse valor no
                            // dicionário de opções
                            options[args[i]] = value;
                        }
                        else
                        {
                            // Não foi possível converter o valor num
                            // inteiro sem sinal, indicar esse erro e sair do
                            // ciclo
                            optionsObject = new Options(
                                $"Invalid value for option {args[i]}");
                            break;
                        }
                    }
                    else
                    {
                        // Opção já tinha sido indicada, indicar esse erro e
                        // sair do ciclo
                        optionsObject = new Options(
                            $"Repeated option: {args[i]}");
                        break;
                    }
                }
                else
                {
                    // Opção desconhecida, indicar esse erro e sair do ciclo
                    optionsObject = new Options($"Unknown option: {args[i]}");
                    break;
                }
            }

            // Caso não tenha ocorrido nenhum erro...
            if (!optionsObject.Error)
            {
                // ...construir instância de Options com os valores indicados
                // pelo utilizador...
                optionsObject = new Options(
                    options[validOptions[0]],
                    options[validOptions[1]],
                    options[validOptions[2]],
                    options[validOptions[3]],
                    options[validOptions[4]],
                    options[validOptions[5]],
                    options[validOptions[6]]);

                // ...e verificar se esses valores são válidos, isto é, se
                // fazem sentido
                optionsObject.Validate();

            }

            // Devolver objeto Options com as opções devidamente analisadas
            return optionsObject;
        }

        /// <summary>
        /// Método privado que valida as opções inseridas pelo utilizador.
        /// Verifica, por exemplo, se o número indicado de agentes cabe nas
        /// dimensões especificadas para o mundo. Caso alguma das opções não
        /// seja válida, altera o estado de erro da instância para true.
        /// </summary>
        private void Validate()
        {
            // Erro caso a dimensão horizontal seja zero
            if (XDim == 0)
            {
                SetError("Horizontal dimension (x) must be > 0");
            }

            // Erro caso a dimensão vertical seja zero
            if (YDim == 0)
            {
                SetError("Vertical dimension (y) must be > 0");
            }

            // Erro caso o número de zombies seja inferior ao número de zombies
            // controlados pelo jogador
            if (Zombies < PlayerZombies)
            {
                SetError("There are more player-controlled zombies "
                    + $"({PlayerZombies}) than total zombies ({Zombies})");
            }

            // Erro caso o número de humanos seja inferior ao número de humanos
            // controlados pelo jogador
            if (Humans < PlayerHumans)
            {
                SetError("There are more player-controlled humans "
                    + $"({PlayerHumans}) than total humans ({Humans})");
            }

            // Erro caso o número total de agentes não caiba nas dimensões do
            // mundo
            if (Zombies + Humans > XDim * YDim)
            {
                SetError($"Too many agents ({Zombies + Humans}) for the "
                    + $"game world (which contains {XDim * YDim} cells)");
            }
        }

        /// <summary>
        /// Método privado que estabelece o estado de erro das opções,
        /// adicionando uma mensagem de erro à lista de mensagens de erro
        /// </summary>
        /// <param name="msg"></param>
        private void SetError(string msg)
        {
            Error = true;
            errorMessages.Add(msg);
        }
    }
}