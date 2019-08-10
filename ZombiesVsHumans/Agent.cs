// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Agent
    {
        public int ID { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public AgentKind Kind { get; }
        public AgentMovement Movement { get; }

        private World world;

        public Agent(int id, int x, int y,
            AgentKind kind, AgentMovement movement, World world)
        {
            ID = id;
            X = x;
            Y = y;
            Kind = kind;
            Movement = movement;
            this.world = world;
            world.AddAgent(this);
        }

        public void Play()
        {

        }

    }
}
