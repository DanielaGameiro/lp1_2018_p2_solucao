// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Agent
    {
        public AgentKind Kind { get; }
        public AgentMovement Movement { get; }

        private int x, y;

        private Agent[,] world;

        public Agent(
            int x, int y, AgentKind kind, AgentMovement movement, Agent[,] world)
        {
            this.x = x;
            this.y = y;
            Kind = kind;
            Movement = movement;
            this.world = world;
            world[x, y] = this;
        }

        public void Play()
        {

        }

    }
}