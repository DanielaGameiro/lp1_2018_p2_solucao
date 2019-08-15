// License: GPLv3
// Author: Nuno Fachada

using System.Collections.Generic;

namespace ZombiesVsHumans
{
    public interface IUserInterface
    {
        void Initialize(int xDim, int yDim);

        void RenderError(string msg);

        void RenderStart();

        void RenderWorld(IReadOnlyWorld world);

        void RenderMessage(string msg);

        void RenderInfo(IDictionary<string, int> info);

        Direction InputDirection(string id);

        void RenderFinish();
    }
}
