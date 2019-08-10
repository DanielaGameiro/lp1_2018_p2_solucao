// License: GPLv3
// Author: Nuno Fachada
using System;
using System.Collections.Generic;

namespace ZombiesVsHumans
{
    public class Game
    {

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
            // First render
            Program.UI.RenderWorld(world);

            for (int i = 0; i < agents.Length; i++)
            {
                for (int j = i + 1; j < agents.Length; j++)
                {

                    Console.WriteLine(
                        $"Distance between {agents[i].ID} and {agents[j].ID} is {world.DistanceBetween(agents[i].Pos, agents[j].Pos)}");
                }
            }

            // Game loop
            for (int i = 0; i < options.Turns; i++)
            {
                // Shuffle agent list
                Shuffle();

                // Cycle through agents and make them play
                foreach (Agent agent in agents)
                {
                    agent.Play();
                }

                // Render at end of turn
                Program.UI.RenderWorld(world);
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
