// License: GPLv3
// Author: Nuno Fachada

using System;
using System.ComponentModel;

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
                    $"Tried to move agent {agent} from " +
                    $"{agent.Pos} to {dest}, " +
                    $"but source location is occupied with " +
                    (!IsOccupied(agent.Pos)
                        ? "no agent" : $"agent {GetAgentAt(agent.Pos)}"));

            if (IsOccupied(dest))
                throw new InvalidOperationException(
                    $"Tried to move agent {agent} to position " +
                    $"{dest} which was already occupied by agent " +
                    GetAgentAt(dest));

            world[dest.X, dest.Y] = agent;
            world[agent.Pos.X, agent.Pos.Y] = null;
        }

        public void AddAgent(Agent agent)
        {
            if (IsOccupied(agent.Pos))
                throw new InvalidOperationException(
                    $"Tried to place agent {agent} at position " +
                    $"{agent.Pos} which was already occupied by " +
                    $"agent {GetAgentAt(agent.Pos)}");

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

        public Coord GetNeighbor(Coord pos, Direction direction)
        {
            int x, y;
            switch (direction)
            {
                case Direction.Up:
                    x = pos.X;
                    y = pos.Y > 0 ? pos.Y - 1 : YDim - 1;
                    break;
                case Direction.UpLeft:
                    x = pos.X > 0 ? pos.X - 1 : XDim - 1;
                    y = pos.Y > 0 ? pos.Y - 1 : YDim - 1;
                    break;
                case Direction.Left:
                    x = pos.X > 0 ? pos.X - 1 : XDim - 1;
                    y = pos.Y;
                    break;
                case Direction.DownLeft:
                    x = pos.X > 0 ? pos.X - 1 : XDim - 1;
                    y = pos.Y < YDim - 1 ? pos.Y + 1 : 0;
                    break;
                case Direction.Down:
                    x = pos.X;
                    y = pos.Y < YDim - 1 ? pos.Y + 1 : 0;
                    break;
                case Direction.DownRight:
                    x = pos.X < XDim - 1 ? pos.X + 1 : 0;
                    y = pos.Y < YDim - 1 ? pos.Y + 1 : 0;
                    break;
                case Direction.Right:
                    x = pos.X < XDim - 1 ? pos.X + 1 : 0;
                    y = pos.Y;
                    break;
                case Direction.UpRight:
                    x = pos.X < XDim - 1 ? pos.X + 1 : 0;
                    y = pos.Y > 0 ? pos.Y - 1 : YDim - 1;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unknown direction");
            }
            return new Coord(x, y);
        }
    }
}
