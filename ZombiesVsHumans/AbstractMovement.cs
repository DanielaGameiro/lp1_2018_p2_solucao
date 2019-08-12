// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public abstract class AbstractMovement
    {
        protected readonly IReadOnlyWorld world;

        public abstract string Message { get; protected set; }

        protected AbstractMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }
        public abstract Coord WhereToMove(Agent agent);
    }
}
