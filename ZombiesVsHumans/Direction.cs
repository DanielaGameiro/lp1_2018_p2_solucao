/// @file
/// @brief Este ficheiro contém a enumeração ZombiesVsHumans.Direction, que
/// define as direções para as quais os agentes se podem mover.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Direção para onde o agente se poderá mover.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Agente não se move.
        /// </summary>
        None,
        /// <summary>
        /// Agente move-se para cima.
        /// </summary>
        Up,
        /// <summary>
        /// Agente move-se para cima e para a esquerda.
        /// </summary>
        UpLeft,
        /// <summary>
        /// Agente move-se para a esquerda.
        /// </summary>
        Left,
        /// <summary>
        /// Agente move-se para baixo e para a esquerda.
        /// </summary>
        DownLeft,
        /// <summary>
        /// Agente move-se para baixo.
        /// </summary>
        Down,
        /// <summary>
        /// Agente move-se para baixo e para a direita.
        /// </summary>
        DownRight,
        /// <summary>
        /// Agente move-se para a direita.
        /// </summary>
        Right,
        /// <summary>
        /// Agente move-se para cima e para a direita.
        /// </summary>
        UpRight
    }
}
