// License: GPLv3
// Author: Nuno Fachada
using System.ComponentModel;

namespace ZombiesVsHumans
{
    public class Agent
    {
        public int ID { get; }
        public Coord Pos { get; private set; }
        public AgentKind Kind { get; }
        public AgentMovement Movement { get; }

        private World world;
        private AbstractMovement moveBehavior;

        public Agent(int id, Coord pos,
            AgentKind kind, AgentMovement movement, World world)
        {
            AgentKind enemy;
            bool runaway;

            ID = id;
            Pos = pos;
            Kind = kind;
            Movement = movement;
            this.world = world;
            world.AddAgent(this);

            switch (kind)
            {
                case AgentKind.Zombie:
                    enemy = AgentKind.Human;
                    runaway = false;
                    break;
                case AgentKind.Human:
                    enemy = AgentKind.Zombie;
                    runaway = true;
                    break;
                default:
                    throw new InvalidEnumArgumentException(
                        "Unknown agent kind");
            }

            switch (movement)
            {
                case AgentMovement.Player:
                    moveBehavior = new PlayerMovement(world);
                    break;
                case AgentMovement.AI:
                    moveBehavior = new AIMovement(enemy, runaway, world);
                    break;
                default:
                    throw new InvalidEnumArgumentException(
                        "Unknown movement type");
            }

        }

        public void Move()
        {
            if (moveBehavior.WhereToMove(this, out Coord dest))
            {
                world.MoveAgent(this, dest);
                Pos = dest;
            }
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
