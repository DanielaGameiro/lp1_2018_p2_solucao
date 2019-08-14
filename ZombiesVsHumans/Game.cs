// License: GPLv3
// Author: Nuno Fachada
using System;
using System.Threading;

namespace ZombiesVsHumans
{
    public class Game
    {
        public const int agentActionDelay = 150;
        public const int turnActionDelay = 300;

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
            Program.UI.Initialize(world.XDim, world.YDim);

            // First render
            Program.UI.RenderTitle();
            Program.UI.RenderWorld(world);
            Program.UI.RenderLegend(0);

            // Game loop
            for (int i = 0; i < options.Turns; i++)
            {
                Program.UI.RenderLegend(i + 1);

                // Shuffle agent list
                Shuffle();

                // Cycle through agents and make them play
                foreach (Agent agent in agents)
                {
                    //Thread.Sleep(agentActionDelay);
                    //Program.UI.RenderMessage($"To move: {agent}");
                    //Console.ReadKey();

                    agent.PlayTurn();
                    Program.UI.RenderMessage(agent.Message);

                    // Render after agent movement
                    Program.UI.RenderWorldNeighborhood(world, agent.Pos);
                }

                // Render at end of turn
                //Program.UI.RenderWorld(world);
                //Thread.Sleep(turnActionDelay);
            }
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
    }
}
