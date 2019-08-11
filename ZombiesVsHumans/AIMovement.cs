// License: GPLv3
// Author: Nuno Fachada
using System;

namespace ZombiesVsHumans
{
    public class AIMovement : AbstractMovement
    {
        private AgentKind enemy;
        private bool runAway;
        public AIMovement(AgentKind target, bool runAway, IReadOnlyWorld world)
            : base(world)
        {
            this.enemy = target;
            this.runAway = runAway;
        }

        public override bool WhereToMove(Agent agent, out Coord dest)
        {
            bool foundEnemy = false;
            bool willMove = false;
            int maxRadius = Math.Max(world.XDim, world.YDim) / 2;
            Coord vector = default(Coord);

            dest = agent.Pos;

            for (int r = 1; r <= maxRadius && !foundEnemy; r++)
            {
                for (int x = -r; x <= r && !foundEnemy; x++)
                {
                    for (int y = -r; y <= r && !foundEnemy; y++)
                    {
                        Coord currentPos = new Coord(x, y);

                        if (currentPos.Equals(agent.Pos)) continue;

                        if (world.IsOccupied(currentPos)
                            && world.GetAgentAt(currentPos).Kind == enemy)
                        {

                            vector = runAway
                                ? world.VectorBetween(currentPos, agent.Pos)
                                : world.VectorBetween(agent.Pos, currentPos);

                            foundEnemy = true;
                        }
                    }
                }
            }

            if (foundEnemy)
            {
                dest = world.GetNeighbor(agent.Pos, vector);
                if (!world.IsOccupied(dest))
                {
                    willMove = true;
                }
                else
                {
                    // If vector is diagonal, try to move just Up, Down, Right
                    // or left
                    if (vector.X != 0 && vector.Y != 0)
                    {
                        // Try to move right or left
                        Coord xVector = new Coord(vector.X, 0);
                        dest = world.GetNeighbor(agent.Pos, xVector);

                        if (!world.IsOccupied(dest))
                        {
                            willMove = true;
                        }
                        else
                        {
                            // Try to move up or down
                            Coord yVector = new Coord(0, vector.Y);
                            dest = world.GetNeighbor(agent.Pos, yVector);

                            if (!world.IsOccupied(dest))
                            {
                                willMove = true;
                            }
                        }
                    }
                }
            }

            return willMove;
        }
    }
}
