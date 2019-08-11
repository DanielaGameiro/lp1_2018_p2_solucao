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

        public override Coord WhereToMove(Agent agent)
        {
            bool foundEnemy = false;
            int maxRadius = Math.Max(world.XDim, world.YDim) / 2;
            Coord vector = default(Coord);

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

            return foundEnemy ? world.GetNeighbor(agent.Pos, vector) : agent.Pos;
        }
    }
}
