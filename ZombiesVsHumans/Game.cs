/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.Game, que implementa
/// o jogo propriamente dito.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System;
using System.Threading;
using System.Collections.Generic;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Classe responsável por gerir o andamento do jogo.
    /// </summary>
    public class Game
    {

        // ///////////////////////////////////////////////////////////////// //
        // Variáveis de instância que podem ser alteradas manualmente para   //
        // personalizar a o jogo. Idealmente os seus valores deveriam ser    //
        // carregados a partir de um ficheiro de configuração.               //
        // ///////////////////////////////////////////////////////////////// //

        /// <summary>
        /// Número de milissegundos entre as ações de cada agente.
        /// </summary>
        private int agentActionDelay = 0;

        /// <summary>
        /// Número de milissegundos entre cada turno.
        /// </summary>
        private int turnActionDelay = 300;

        // ///////////////////////////////////////////////////////////////// //
        // Outras variáveis de instância, não devem ser modificadas.         //
        // ///////////////////////////////////////////////////////////////// //

        /// <summary>
        /// Opções do jogo.
        /// </summary>
        /// <remarks>
        /// Estas opções foram inicialmente passadas na linha de comandos, mas
        /// estão disponíveis neste objeto já tratadas.
        /// </remarks>
        private Options options;

        /// <summary>
        /// Referência só de leitura ao mundo do jogo.
        /// </summary>
        private IReadOnlyWorld world;

        /// <summary>
        /// Array contendo todos os agentes existentes na simulação.
        /// </summary>
        /// <remarks>
        /// Como o número total de agentes nunca muda, podemos usar um array de
        /// tamanho fixo, mais simples, em vez de uma lista, mais complexa.
        /// </remarks>
        private Agent[] agents;

        /// <summary>
        /// Gerador de números aleatórios.
        /// </summary>
        private Random rand;

        /// <summary>
        /// Construtor, cria uma nova instância desta classe.
        /// </summary>
        /// <param name="options">Opções do jogo.</param>
        public Game(Options options)
        {
            // Guardar as opções do jogo numa variável de instância
            this.options = options;

            // Criar o mundo de jogo com o tamanho indicado nas opções do jogo
            world = new World((int)options.XDim, (int)options.YDim);

            // Instanciar o array de agentes com o tamanho exacto para todos
            // os agentes
            agents = new Agent[options.Zombies + options.Humans];

            // Instanciar o gerador de números aleatórios
            rand = new Random();

            // Criar os zombies
            for (uint i = 0; i < options.Zombies; i++)
            {
                if (i < options.PlayerZombies)
                {
                    // Criar novo zombie controlado pelo jogador
                    NewAgent(AgentKind.Zombie, AgentMovement.Player, (int)i);
                }
                else
                {
                    // Criar novo zombie controlado pela IA
                    NewAgent(AgentKind.Zombie, AgentMovement.AI, (int)i);
                }
            }

            // Criar os humanos
            for (uint i = 0; i < options.Humans; i++)
            {
                if (i < options.PlayerHumans)
                {
                    // Criar novo humano controlado pelo jogador
                    NewAgent(AgentKind.Human, AgentMovement.Player,
                        (int)(options.Zombies + i));
                }
                else
                {
                    // Criar novo humano controlado pela IA
                    NewAgent(AgentKind.Human, AgentMovement.AI,
                        (int)(options.Zombies + i));
                }
            }
        }

        /// <summary>
        /// Começar o jogo.
        /// </summary>
        public void Play()
        {
            // Chaves (nome) da informação estatística sobre o jogo
            string turnKey = "Turn";
            string totalZombiesKey = "Total Zombies";
            string totalHumansKey = "Total Humans";
            string playerZombiesKey = "Player Zombies";
            string playerHumansKey = "Player Humans";

            // Valores da informação estatística sobre o jogo
            int totalZombiesValue = (int)options.Zombies;
            int totalHumansValue = (int)options.Humans;
            int playerZombiesValue = (int)options.PlayerZombies;
            int playerHumansValue = (int)options.PlayerHumans;

            // Criar um dicionário com as chaves-valores da informação
            // estatística sobre o jogo
            IDictionary<string, int> info = new Dictionary<string, int>()
            {
                { turnKey, 0 },
                { totalZombiesKey, totalZombiesValue },
                { totalHumansKey, totalHumansValue },
                { playerZombiesKey, playerZombiesValue },
                { playerHumansKey, playerHumansValue }
            };

            // Inicializar a interface gráfica tendo em conta o tamanho do
            // mundo de jogo
            Program.UI.Initialize(world.XDim, world.YDim);

            // Inicializar renderização
            Program.UI.RenderStart();

            // Renderizar mundo de jogo pela primeira vez
            Program.UI.RenderWorld(world);

            // Renderizar informação estatística sobre o jogo pela primeira vez
            Program.UI.RenderInfo(info);

            // Game loop:
            // - Ciclo principal do jogo, cada iteração corresponde a um turno
            // - Ciclo continua enquanto não tiver sido atingindo o número
            //   máximo de turnos e enquanto existirem humanos no jogo
            for (int i = 0; i < options.Turns && totalHumansValue > 0; i++)
            {
                // Atualizar turno no dicionário que contém a info estatística
                info[turnKey] = i + 1;

                // Atualizar a informação estatística na UI
                Program.UI.RenderInfo(info);

                // Mostrar mensagem com indicação de novo turno
                Program.UI.RenderMessage($"Starting turn {i + 1}");

                // Embaralhar array de agentes
                Shuffle();

                // Percorrer array de agentes de modo a que eles possam
                // realizar a sua ação no turno atual
                foreach (Agent agent in agents)
                {
                    // Variável que indica se houve uma alteração na composição
                    // da população (isto é, se um humano se transformou em
                    // zombie)
                    bool changePopulation;

                    // Mensagem devolvida pelo agente após ter realizado a sua
                    // ação neste turno
                    string agentMessage;

                    // Se for necessário esperar entre a ação de cada agente,
                    // então vamos fazê-lo agora
                    if (agentActionDelay > 0) Thread.Sleep(agentActionDelay);

                    // Pedir ao agente atual para realizar a sua ação
                    agent.PlayTurn(out changePopulation, out agentMessage);

                    // Houve uma alteração na população devido à ação do agente?
                    if (changePopulation)
                    {
                        // Se sim, recontar número de agentes...
                        ReCountAgents(
                            out totalZombiesValue, out totalHumansValue,
                            out playerZombiesValue, out playerHumansValue);
                        // ...atualizar dicionário com informação estatística...
                        info[totalZombiesKey] = totalZombiesValue;
                        info[totalHumansKey] = totalHumansValue;
                        info[playerZombiesKey] = playerZombiesValue;
                        info[playerHumansKey] = playerHumansValue;
                        // ...e atualizar UI com essa nova informação
                        Program.UI.RenderInfo(info);
                    }

                    // Mostrar mensagem devolvida pelo agente relativa à sua
                    // ação
                    Program.UI.RenderMessage(agentMessage);

                    // Atualizar mundo de simulação na UI após a ação do agente
                    // atual
                    Program.UI.RenderWorld(world);
                }

                // Se for necessário esperar entre turnos, então vamos fazê-lo
                // agora
                if (turnActionDelay > 0) Thread.Sleep(turnActionDelay);
            }

            // Se chegarmos aqui significa que o jogo terminou, sendo
            // necessário mostrar a mensagem de vitória adequada
            if (totalHumansValue > 0)
                // Humanos sobreviveram!
                Program.UI.RenderMessage(
                    $"Humans survived after {options.Turns} turns!");
            else
                // Zombies infetaram todos os humanos!
                Program.UI.RenderMessage(
                    $"No humans left! Zombies won after {info[turnKey]} turns!");

            // Finalizar UI/renderização
            Program.UI.RenderFinish();
        }

        /// <summary>
        /// Método privado que cria um novo agente no mundo de jogo.
        /// </summary>
        /// <param name="kind">Género do agente (zombie ou humano).</param>
        /// <param name="movement">Tipo de movimento (jogador ou IA).</param>
        /// <param name="id">ID único do agente.</param>
        private void NewAgent(AgentKind kind, AgentMovement movement, int id)
        {
            // Posição inicial do agente
            Coord pos;
            // Referência ao agente
            Agent agent;

            // Escolher um local aleatório desocupado para colocar o agente
            do
            {
                pos = new Coord(
                    rand.Next((int)options.XDim),
                    rand.Next((int)options.YDim));
                // Este ciclo continua até que um local desocupado tenha sido
                // encontrado
            } while (world.IsOccupied(pos));

            // Instanciar novo agente
            agent = new Agent(id, pos, kind, movement, (World)world);

            // Guardar agente no array de agentes
            agents[id] = agent;
        }

        /// <summary>
        /// Método que embaralha a lista de agentes usando o algoritmo de
        /// Fisher–Yates.
        /// </summary>
        /// <remarks>
        /// Referência: [Wikipédia](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
        /// </remarks>
        private void Shuffle()
        {
            for (int i = agents.Length - 1; i >= 1; i--)
            {
                Agent aux;
                int j = rand.Next(i + 1);
                aux = agents[j];
                agents[j] = agents[i];
                agents[i] = aux;
            }
        }

        /// <summary>
        /// Realiza contagem dos tipos de agente no jogo.
        /// </summary>
        /// <param name="totalZombies">
        /// Parâmetro de saida onde colocar o número total de zombies.
        /// </param>
        /// <param name="totalHumans">
        /// Parâmetro de saida onde colocar o número total de humanos.
        /// </param>
        /// <param name="playerZombies">
        /// Parâmetro de saida onde colocar o número de zombies controlados
        /// pelo jogador.
        /// </param>
        /// <param name="playerHumans">
        /// Parâmetro de saida onde colocar o número de humanos controlados
        /// pelo jogador.
        /// </param>
        private void ReCountAgents(
            out int totalZombies, out int totalHumans,
            out int playerZombies, out int playerHumans)
        {
            // Valores iniciais
            totalZombies = 0;
            totalHumans = 0;
            playerZombies = 0;
            playerHumans = 0;

            // Percorrer array de agentes
            foreach (Agent ag in agents)
            {
                // Agente é zombie ou humano?
                if (ag.Kind == AgentKind.Zombie)
                {
                    // Agente é zombie, incrementar número total de zombies
                    totalZombies++;

                    // Zombie é controlado pelo jogador?
                    if (ag.Movement == AgentMovement.Player)
                    {
                        // Se sim, incrementar número de zombies controlados
                        // pelo jogador
                        playerZombies++;
                    }
                }
                else
                {
                    // Agente é humano, incrementar número total de humanos
                    totalHumans++;

                    // Humano é controlado pelo jogador?
                    if (ag.Movement == AgentMovement.Player)
                    {
                        // Se sim, incrementar número de humanos controlados
                        // pelo jogador
                        playerHumans++;
                    }
                }
            }
        }
    }
}
