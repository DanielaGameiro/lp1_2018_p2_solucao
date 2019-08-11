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
            int maxRadius = Math.Max(world.XDim, world.YDim) / 2;
            Direction direction = default(Direction);

            for (int r = 1; r < maxRadius && !foundEnemy; r++)
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
                            direction = runAway
                                ? world.DirectionFromTo(currentPos, agent.Pos)
                                : world.DirectionFromTo(agent.Pos, currentPos);
                            foundEnemy = true;
                        }
                    }
                }
            }

            dest = foundEnemy
                ? world.GetNeighbor(agent.Pos, direction)
                : agent.Pos;

            return foundEnemy & !world.IsOccupied(dest);
        }
    }
}
