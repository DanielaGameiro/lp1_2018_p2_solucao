namespace ZombiesVsHumans
{
    public abstract class AbstractMovement : IMovement
    {
        protected readonly IReadOnlyWorld world;

        protected AbstractMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }
        public abstract void Move(Agent agent);
    }
}
