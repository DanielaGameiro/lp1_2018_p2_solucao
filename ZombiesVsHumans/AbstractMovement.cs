/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.AbstractMovement, que
/// define de forma abstrata o movimento dos agentes.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    public abstract class AbstractMovement
    {
        protected readonly IReadOnlyWorld world;

        protected AbstractMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }
        public abstract Coord WhereToMove(Agent agent, out string message);
    }
}
