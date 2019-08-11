// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) { }

        public override Coord WhereToMove(Agent agent)
        {
            Direction direction = Program.UI.InputDirection(agent.ToString());
            return world.GetNeighbor(agent.Pos, direction);
        }
    }
}
