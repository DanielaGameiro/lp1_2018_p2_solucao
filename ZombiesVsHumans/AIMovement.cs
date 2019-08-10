// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class AIMovement : AbstractMovement
    {
        private AgentKind target;
        private bool runAway;
        public AIMovement(AgentKind target, bool runAway, IReadOnlyWorld world)
            : base(world)
        {
            this.target = target;
            this.runAway = runAway;
        }

        public override void Move(Agent agent)
        {

        }
    }
}
