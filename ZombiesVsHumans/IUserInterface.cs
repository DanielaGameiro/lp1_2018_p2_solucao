// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public interface IUserInterface
    {
         void RenderError(string msg);

         void RenderWorld(IReadOnlyWorld world);

         void RenderMessage(string msg);

         Direction InputDirection();
    }
}
