// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) {}

        public override bool WhereToMove(Agent agent, out Coord dest)
        {
            // TODO: Check if there is any place to move and return false if
            // that's not the case
            do
            {
                Direction direction =
                    Program.UI.InputDirection(agent.ToString());
                dest = world.GetNeighbor(agent.Pos, direction);
                if (world.IsOccupied(dest))
                {
                    Program.UI.RenderMessage(
                        $"Can't move to {direction} because it's occupied " +
                        $"with {world.GetAgentAt(dest)}");
                }
                else
                {
                    break;
                }
            } while (true);
            return true;
        }
    }
}
