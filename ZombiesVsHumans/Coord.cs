/// @file
/// @brief Este ficheiro contém a `struct` ZombiesVsHumans.Coord, que
/// define uma posição no mundo.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Define uma posição no mundo do jogo.
    /// </summary>
    public struct Coord
    {
        /// <summary>
        /// Propriedade auto-implementada que indica uma posição horizontal no
        /// mundo de jogo.
        /// </summary>
        /// <value>Posição horizontal no mundo de jogo.</value>
        public int X { get; }
        /// <summary>
        /// Propriedade auto-implementada que indica uma posição vertical no
        /// mundo de jogo.
        /// </summary>
        /// <value>Posição vertical no mundo de jogo.</value>
        public int Y { get; }

        /// <summary>
        /// Cria uma nova posição no mundo de jogo.
        /// </summary>
        /// <param name="x">Posição horizontal no mundo de jogo.</param>
        /// <param name="y">Posição vertical no mundo de jogo.</param>
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Devolve a posição devidamente formatada.
        /// </summary>
        /// <returns>A posição devidamente formatada.</returns>
        public override string ToString() => $"({X},{Y})";

    }
}
