// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) { }

        public override string Message { get; protected set; }

        public override Coord WhereToMove(Agent agent)
        {
            Direction direction = Program.UI.InputDirection(agent.ToString());
            Message = $"Player tried to move {direction}";
            return world.GetNeighbor(agent.Pos, direction);
        }
    }
}
