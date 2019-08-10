using System;

namespace ZombiesVsHumans
{
    public class World : IReadOnlyWorld
    {
        public int XDim => world.GetLength(0);
        public int YDim => world.GetLength(1);
        private Agent[,] world;

        public World(int xDim, int yDim)
        {
            world = new Agent[xDim, yDim];
        }

        public bool IsOccupied(int x, int y)
        {
            return world[x, y] != null;
        }
        public Agent GetAgentAt(int x, int y)
        {
            return world[x, y];
        }

        public void MoveAgent(Agent agent, int xDest, int yDest)
        {
            if (world[agent.X, agent.Y] != agent)
                throw new InvalidOperationException(
                    $"Tried to move agent {agent.ID} from " +
                    $"({agent.X},{agent.Y}) to ({xDest},{yDest}), " +
                    $"but source location is occupied with " +
                    world[agent.X, agent.Y] == null
                        ? "no agent"
                        : "$agent {world[agent.X, agent.Y].ID}");

            if (world[xDest, yDest] != null)
                throw new InvalidOperationException(
                    $"Tried to move agent {agent.ID} to position " +
                    $"({xDest},{yDest}) which was already occupied by agent " +
                    world[xDest, yDest].ID);

            world[xDest, yDest] = agent;
            world[agent.X, agent.Y] = null;
        }

        public void AddAgent(Agent agent)
        {
            if (world[agent.X, agent.Y] != null)
                throw new InvalidOperationException(
                    $"Tried to place agent {agent.ID} at position " +
                    $"({agent.X},{agent.Y}) which was already occupied by " +
                    $"agent {world[agent.X, agent.Y].ID}");

            world[agent.X, agent.Y] = agent;
        }
    }
}
