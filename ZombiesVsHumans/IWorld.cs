namespace ZombiesVsHumans
{
    public interface IReadOnlyWorld
    {
        int XDim { get; }
        int YDim { get; }
        bool IsOccupied(int x, int y);
        Agent GetAgentAt(int x, int y);
    }
}
