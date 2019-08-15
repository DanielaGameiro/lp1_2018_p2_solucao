// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) { }

        public override Coord WhereToMove(Agent agent, out string message)
        {
            Direction direction = Program.UI.InputDirection(agent.ToString());
            message = $"Player tried to move {direction}";
            return world.GetNeighbor(agent.Pos, direction);
        }
    }
}
