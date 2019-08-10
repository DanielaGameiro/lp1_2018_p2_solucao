// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) {}

        public override void Move(Agent agent)
        {
            Coord dest;
            do
            {
                Direction direction = Program.UI.InputDirection();
                dest = world.GetNeighbor(agent.Pos, direction);
                if (world.IsOccupied(dest))
                {
                    Program.UI.RenderMessage(
                        $"Can't move to {direction} because it's occupied " +
                        $"with {agent}");
                }
            } while (true); // Check if position available
        }
    }
}
