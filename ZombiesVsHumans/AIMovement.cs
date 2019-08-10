namespace ZombiesVsHumans
{
    public class AIMovement : IMovement
    {
        private AgentKind target;
        private bool runAway;
        public AIMovement(AgentKind target, bool runAway)
        {
            this.target = target;
            this.runAway = runAway;
        }

        public void Move()
        {

        }
    }
}
