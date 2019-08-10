// License: GPLv3
// Author: Nuno Fachada

namespace ZombiesVsHumans
{
    public interface IUserInterface
    {
         void ShowError(string msg);

         void RenderWorld(IReadOnlyWorld world);

         Direction InputDirection();
    }
}
