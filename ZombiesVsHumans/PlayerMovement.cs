// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : IMovement
    {
        private IReadOnlyWorld world;

        public PlayerMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }
        public void Move(Agent agent)
        {
            //Coord dest;
            do
            {
                Direction direction = Program.UI.InputDirection();
                //dest = agent.Pos.GetNeighbor(direction);
                //if (world.IsOccupied(dest))
                {
                    Program.UI.RenderMessage($"Can't move {direction}");
                }
            } while (true); // Check if position available
        }
    }
}
