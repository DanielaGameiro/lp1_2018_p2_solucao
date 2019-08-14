// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public interface IUserInterface
    {
        void Initialize(int xDim, int yDim);

        void RenderError(string msg);

        void RenderTitle();
        void RenderWorldNeighborhood(IReadOnlyWorld world, Coord coord);
        void RenderWorld(IReadOnlyWorld world);

        void RenderMessage(string msg);

        void RenderLegend(int i);

        Direction InputDirection(string id);
    }
}
