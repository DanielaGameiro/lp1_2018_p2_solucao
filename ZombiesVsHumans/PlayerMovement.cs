/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.PlayerMovement, que
/// implementa o movimento dos agentes controlado por um jogador.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    public class PlayerMovement : AbstractMovement
    {
        public PlayerMovement(IReadOnlyWorld world) : base(world) { }

        public override Coord WhereToMove(Agent agent, out string message)
        {
            Direction direction = Program.UI.InputDirection(agent.ToString());
            message = $"Player tried to move {direction}";
            return world.GetNeighbor(agent.Pos, direction);
        }
    }
}
