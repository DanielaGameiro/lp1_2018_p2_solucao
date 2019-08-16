/// @file
/// @brief Este ficheiro contém a interface ZombiesVsHumans.IUserInterface,
/// que representa de forma abstrata uma UI para o jogo.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

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
