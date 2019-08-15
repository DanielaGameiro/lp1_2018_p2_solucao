// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public abstract class AbstractMovement
    {
        protected readonly IReadOnlyWorld world;

        protected AbstractMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }
        public abstract Coord WhereToMove(Agent agent, out string message);
    }
}
