/// @file
/// @brief Este ficheiro contém a enumeração ZombiesVsHumans.AgentMovement, que
/// define os diferentes tipos de movimento que os agentes podem realizar.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Tipo de movimento realizado pelo agente.
    /// </summary>
    public enum AgentMovement
    {
        /// <summary>
        /// Movimento controlado por "inteligência artificial".
        /// </summary>
        AI,
        /// <summary>
        /// Movimento controlado por um jogador.
        /// </summary>
        Player
    }
}