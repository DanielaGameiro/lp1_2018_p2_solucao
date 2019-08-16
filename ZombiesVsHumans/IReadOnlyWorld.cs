/// @file
/// @brief Este ficheiro contém a interface ZombiesVsHumans.IReadOnlyWorld,
/// que apresenta uma visão só de leitura do mundo de jogo.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    public interface IReadOnlyWorld
    {
        int XDim { get; }
        int YDim { get; }
        bool IsOccupied(Coord coord);
        Agent GetAgentAt(Coord coord);
        Coord VectorBetween(Coord c1, Coord c2);
        Coord GetNeighbor(Coord pos, Direction direction);
        Coord GetNeighbor(Coord pos, Coord directionVector);
    }
}
