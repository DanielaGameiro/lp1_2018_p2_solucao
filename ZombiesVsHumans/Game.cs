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
    public class Game
    {
        private int agentActionDelay = 0;
        private int turnActionDelay = 300;

        private Options options;
        private IReadOnlyWorld world;
        private Agent[] agents;
        private Random rand;

        public Game(Options options)
        {
            this.options = options;
            world = new World((int)options.XDim, (int)options.YDim);
            agents = new Agent[options.Zombies + options.Humans];
            rand = new Random();

            for (uint i = 0; i < options.Zombies; i++)
            {

                if (i < options.PlayerZombies)
                {
                    // Create player-controlled zombie
                    NewAgent(AgentKind.Zombie, AgentMovement.Player, (int)i);
                }
                else
                {
                    // Create AI zombie
                    NewAgent(AgentKind.Zombie, AgentMovement.AI, (int)i);
                }
            }

            for (uint i = 0; i < options.Humans; i++)
            {

                if (i < options.PlayerHumans)
                {
                    // Create player-controlled human
                    NewAgent(AgentKind.Human, AgentMovement.Player,
                        (int)(options.Zombies + i));
                }
                else
                {
                    // Create AI human
                    NewAgent(AgentKind.Human, AgentMovement.AI,
                        (int)(options.Zombies + i));
                }
            }
        }

        public void Play()
        {
            string turnKey = "Turn";
            string totalZombiesKey = "Total Zombies";
            string totalHumansKey = "Total Humans";
            string playerZombiesKey = "Player Zombies";
            string playerHumansKey = "Player Humans";

            int totalZombiesValue = (int)options.Zombies;
            int totalHumansValue = (int)options.Humans;
            int playerZombiesValue = (int)options.PlayerZombies;
            int playerHumansValue = (int)options.PlayerHumans;

            IDictionary<string, int> info = new Dictionary<string, int>()
            {
                { turnKey, 0 },
                { totalZombiesKey, totalZombiesValue },
                { totalHumansKey, totalHumansValue },
                { playerZombiesKey, playerZombiesValue },
                { playerHumansKey, playerHumansValue }
            };

            Program.UI.Initialize(world.XDim, world.YDim);

            // First render
            Program.UI.RenderStart();
            Program.UI.RenderWorld(world);
            Program.UI.RenderInfo(info);

            // Game loop
            for (int i = 0; i < options.Turns && totalHumansValue > 0; i++)
            {
                info[turnKey] = i + 1;
                Program.UI.RenderInfo(info);
                Program.UI.RenderMessage($"Starting turn {i + 1}");

                // Shuffle agent list
                Shuffle();

                // Cycle through agents and make them play
                foreach (Agent agent in agents)
                {
                    bool changePopulation;
                    string agentMessage;

                    if (agentActionDelay > 0) Thread.Sleep(agentActionDelay);

                    agent.PlayTurn(out changePopulation, out agentMessage);

                    if (changePopulation)
                    {
                        ReCountAgents(
                            out totalZombiesValue, out totalHumansValue,
                            out playerZombiesValue, out playerHumansValue);
                        info[totalZombiesKey] = totalZombiesValue;
                        info[totalHumansKey] = totalHumansValue;
                        info[playerZombiesKey] = playerZombiesValue;
                        info[playerHumansKey] = playerHumansValue;
                        Program.UI.RenderInfo(info);
                    }

                    Program.UI.RenderMessage(agentMessage);

                    // Render after agent movement
                    Program.UI.RenderWorld(world);
                }

                if (turnActionDelay > 0) Thread.Sleep(turnActionDelay);
            }

            if (totalHumansValue > 0)
                Program.UI.RenderMessage(
                    $"Humans survived after {options.Turns} turns!");
            else
                Program.UI.RenderMessage(
                    $"No humans left! Zombies won after {info[turnKey]} turns!");

            Program.UI.RenderFinish();

        }

        private void NewAgent(AgentKind kind, AgentMovement movement, int id)
        {
            Coord pos;
            Agent agent;

            do
            {
                pos = new Coord(
                    rand.Next((int)options.XDim),
                    rand.Next((int)options.YDim));
            } while (world.IsOccupied(pos));

            agent = new Agent(id, pos, kind, movement, (World)world);
            agents[id] = agent;
        }

        /// <summary>
        /// Shuffle agent list using Fisher–Yates shuffle.
        /// </summary>
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

        private void ReCountAgents(
            out int totalZombies, out int totalHumans,
            out int playerZombies, out int playerHumans)
        {
            totalZombies = 0;
            totalHumans = 0;
            playerZombies = 0;
            playerHumans = 0;
            foreach (Agent ag in agents)
            {
                if (ag.Kind == AgentKind.Zombie)
                {
                    totalZombies++;
                    if (ag.Movement == AgentMovement.Player)
                    {
                        playerZombies++;
                    }
                }
                else
                {
                    totalHumans++;
                    if (ag.Movement == AgentMovement.Player)
                    {
                        playerHumans++;
                    }
                }
            }
        }
    }
}
