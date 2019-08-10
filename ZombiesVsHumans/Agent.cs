// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Agent
    {
        public AgentKind Kind { get; }
        public AgentMovement Movement { get; }

        private int x, y;

        public Agent(int x, int y, AgentKind kind, AgentMovement movement)
        {
            this.x = x;
            this.y = y;
            Kind = kind;
            Movement = movement;
        }

    }
}