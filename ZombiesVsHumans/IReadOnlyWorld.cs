// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public interface IReadOnlyWorld
    {
        int XDim { get; }
        int YDim { get; }
        bool IsOccupied(Coord coord);
        Agent GetAgentAt(Coord coord);
        int DistanceBetween(Coord c1, Coord c2);
        Coord GetNeighbor(Coord pos, Direction direction);
        Direction DirectionFromTo(Coord c1, Coord c2);
    }
}
