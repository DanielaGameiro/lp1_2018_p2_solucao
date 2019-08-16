/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.AIMovement, que
/// implementa o movimento por "inteligência artificial" dos agentes.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Define o movimento por "inteligência artificial" dos agentes.
    /// </summary>
    public class AIMovement : AbstractMovement
    {
        /// <summary>
        /// Género de agente considerado inimigo.
        /// </summary>
        private AgentKind enemy;

        /// <summary>
        /// Indica se agent deve fugir ou perseguir os agentes considerados
        /// inimigos.
        /// </summary>
        private bool runAway;

        public AIMovement(AgentKind target, bool runAway, IReadOnlyWorld world)
            : base(world)
        {
            this.enemy = target;
            this.runAway = runAway;
        }

        public override Coord WhereToMove(Agent agent, out string message)
        {
            Agent target = null;
            bool foundEnemy = false;
            int maxRadius = Math.Max(world.XDim, world.YDim) / 2;
            Coord vector = default(Coord);
            Coord currentPos;

            for (int r = 1; r <= maxRadius && !foundEnemy; r++)
            {
                for (int dx = -r; dx <= r && !foundEnemy; dx++)
                {
                    for (int dy = -r; dy <= r && !foundEnemy; dy++)
                    {
                        if (Math.Max(Math.Abs(dx), Math.Abs(dy)) != r) continue;

                        currentPos =
                            new Coord(agent.Pos.X + dx, agent.Pos.Y + dy);

                        if (world.IsOccupied(currentPos))
                        {
                            target = world.GetAgentAt(currentPos);

                            if (target.Kind == enemy)
                            {

                                vector = runAway
                                    ? world.VectorBetween(currentPos, agent.Pos)
                                    : world.VectorBetween(agent.Pos, currentPos);

                                foundEnemy = true;
                            }
                        }
                    }
                }
            }

            if (foundEnemy)
            {
                string way = runAway ? "runaway from" : "move towards";
                message = $"{agent} tried to {way} {target}";
                return world.GetNeighbor(agent.Pos, vector);
            }

            message = $"{agent} didn't find enemies";
            return agent.Pos;
        }
    }
}
