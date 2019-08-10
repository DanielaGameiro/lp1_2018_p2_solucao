// License: GPLv3
// Author: Nuno Fachada

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

        public bool IsOccupied(Coord coord)
        {
            return world[coord.X, coord.Y] != null;
        }
        public Agent GetAgentAt(Coord coord)
        {
            return world[coord.X, coord.Y];
        }

        public void MoveAgent(Agent agent, Coord dest)
        {
            if (world[agent.Pos.X, agent.Pos.Y] != agent)
                throw new InvalidOperationException(
                    $"Tried to move agent {agent.ID} from " +
                    $"{agent.Pos} to {dest}, " +
                    $"but source location is occupied with " +
                    (!IsOccupied(agent.Pos)
                        ? "no agent" : $"agent {GetAgentAt(agent.Pos).ID}"));

            if (IsOccupied(dest))
                throw new InvalidOperationException(
                    $"Tried to move agent {agent.ID} to position " +
                    $"{dest} which was already occupied by agent " +
                    GetAgentAt(dest).ID);

            world[dest.X, dest.Y] = agent;
            world[agent.Pos.X, agent.Pos.Y] = null;
        }

        public void AddAgent(Agent agent)
        {
            if (IsOccupied(agent.Pos))
                throw new InvalidOperationException(
                    $"Tried to place agent {agent.ID} at position " +
                    $"{agent.Pos} which was already occupied by " +
                    $"agent {GetAgentAt(agent.Pos).ID}");

            world[agent.Pos.X, agent.Pos.Y] = agent;
        }

        public int DistanceBetween(Coord c1, Coord c2)
        {

            // Determine minimum horizontal distance
            int minXDist = Math.Min(
                // Is it direct?
                Math.Abs(c1.X - c2.X),
                // Or wrap around?
                Math.Min(c1.X, c2.X) + XDim - Math.Max(c1.X, c2.X)
            );

            // Determine minimum vertical distance
            int minYDist = Math.Min(
                // Is is direct?
                Math.Abs(c1.Y - c2.Y),
                // Or wrap around?
                Math.Min(c1.Y, c2.Y) + YDim - Math.Max(c1.Y, c2.Y)
            );

            // The distance between coordinates is the largest between the
            // minimum horizontal and vertical distances
            return Math.Max(minXDist, minYDist);
        }
    }
}
