// License: GPLv3
// Author: Nuno Fachada
using System.ComponentModel;

namespace ZombiesVsHumans
{
    public class Agent
    {
        public int ID { get; }
        public Coord Pos { get; private set; }
        public AgentKind Kind { get; private set; }
        public AgentMovement Movement { get; private set; }

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

        public void PlayTurn()
        {
            Coord dest = moveBehavior.WhereToMove(this);

            if (!world.IsOccupied(dest))
            {
                world.MoveAgent(this, dest);
                Pos = dest;
            }
            else
            {
                if (Kind == AgentKind.Zombie)
                {
                    world.GetAgentAt(dest).TryInfect();
                }
            }
        }

        private void TryInfect()
        {
            if (Kind == AgentKind.Human)
            {
                Kind = AgentKind.Zombie;
                moveBehavior = new AIMovement(AgentKind.Human, false, world);
            }
        }

        public override string ToString()
        {
            string type = (Kind == AgentKind.Zombie) ? "z" : "h";
            if (Movement == AgentMovement.Player)
                type = type.ToUpper();
            return $"{type}{ID,2:x2}";
        }
    }
}
