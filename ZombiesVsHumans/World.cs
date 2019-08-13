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
            coord = Normalize(coord);
            return world[coord.X, coord.Y] != null;
        }
        public Agent GetAgentAt(Coord coord)
        {
            coord = Normalize(coord);
            return world[coord.X, coord.Y];
        }

        public void MoveAgent(Agent agent, Coord dest)
        {
            dest = Normalize(dest);

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

        public Coord VectorBetween(Coord c1, Coord c2)
        {
            int x, y, xDistDirect, xDistWrapAround, yDistDirect, yDistWrapAround;

            c1 = Normalize(c1);
            c2 = Normalize(c2);

            xDistDirect = Math.Abs(c2.X - c1.X);
            xDistWrapAround = Math.Min(c2.X, c1.X) + XDim - Math.Max(c2.X, c1.X);

            x = c2.X - c1.X;
            if (xDistWrapAround < xDistDirect)
                x += -Math.Sign(x) * XDim;

            yDistDirect = Math.Abs(c2.Y - c1.Y);
            yDistWrapAround = Math.Min(c2.Y, c1.Y) + YDim - Math.Max(c2.Y, c1.Y);

            y = c2.Y - c1.Y;
            if (yDistWrapAround < yDistDirect)
                y += -Math.Sign(y) * YDim;

            return new Coord(x, y);
        }

        public Coord GetNeighbor(Coord pos, Direction direction)
        {
            int x = pos.X, y = pos.Y;
            switch (direction)
            {
                case Direction.Up:
                    y -= 1;
                    break;
                case Direction.UpLeft:
                    x -= 1;
                    y -= 1;
                    break;
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.DownLeft:
                    x -= 1;
                    y += 1;
                    break;
                case Direction.Down:
                    y += 1;
                    break;
                case Direction.DownRight:
                    x += 1;
                    y += 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
                case Direction.UpRight:
                    x += 1;
                    y -= 1;
                    break;
                case Direction.None:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unknown direction");
            }
            return Normalize(new Coord(x, y));
        }

        public Coord GetNeighbor(Coord pos, Coord directionVector)
        {
            Direction direction = default(Direction);

            directionVector = new Coord(
                Math.Sign(directionVector.X), Math.Sign(directionVector.Y));

            if (directionVector.X == 1 && directionVector.Y == 1)
                direction = Direction.DownRight;
            else if (directionVector.X == 1 && directionVector.Y == 0)
                direction = Direction.Right;
            else if (directionVector.X == 1 && directionVector.Y == -1)
                direction = Direction.UpRight;
            else if (directionVector.X == 0 && directionVector.Y == 1)
                direction = Direction.Down;
            else if (directionVector.X == 0 && directionVector.Y == 0)
                direction = Direction.None;
            else if (directionVector.X == 0 && directionVector.Y == -1)
                direction = Direction.Up;
            else if (directionVector.X == -1 && directionVector.Y == 1)
                direction = Direction.DownLeft;
            else if (directionVector.X == -1 && directionVector.Y == 0)
                direction = Direction.Left;
            else if (directionVector.X == -1 && directionVector.Y == -1)
                direction = Direction.UpLeft;

            return GetNeighbor(pos, direction);
        }

        private Coord Normalize(Coord coord)
        {
            int x = coord.X;
            int y = coord.Y;

            while (x >= XDim) x -= XDim;
            while (x < 0) x += XDim;
            while (y >= YDim) y -= YDim;
            while (y < 0) y += YDim;

            return new Coord(x, y);
        }
    }
}
