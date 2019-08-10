namespace ZombiesVsHumans
{
    public class AIMovement : IMovement
    {
        private AgentKind target;
        private bool runAway;
        private IReadOnlyWorld world;
        public AIMovement(AgentKind target, bool runAway, IReadOnlyWorld world)
        {
            this.target = target;
            this.runAway = runAway;
            this.world = world;
        }

        public void Move(Agent agent)
        {

        }
    }
}
