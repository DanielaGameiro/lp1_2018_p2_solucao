// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public class Agent
    {
        public int ID { get; }
        public Coord Pos { get; private set; }
        public AgentKind Kind { get; }
        public AgentMovement Movement { get; }

        private World world;

        public Agent(int id, Coord pos,
            AgentKind kind, AgentMovement movement, World world)
        {
            ID = id;
            Pos = pos;
            Kind = kind;
            Movement = movement;
            this.world = world;
            world.AddAgent(this);
        }

        public void Play()
        {

        }

        public override string ToString()
        {
            string type = (Kind == AgentKind.Zombie) ? "z" : "h";
            if (Movement == AgentMovement.Player)
                type = type.ToUpper();
            return $"{type}{ID,3:x3}";
        }
    }
}
