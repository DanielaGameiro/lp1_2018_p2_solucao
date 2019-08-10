// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class PlayerMovement : IMovement
    {
        public void Move(Agent agent)
        {
            Direction direction;
            do
            {
                direction = Program.UI.InputDirection();
            } while (true); // Check if position available
        }
    }
}
