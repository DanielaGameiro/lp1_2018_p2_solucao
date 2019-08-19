/// @file
/// @brief Este ficheiro contém a interface ZombiesVsHumans.IReadOnlyWorld,
/// que apresenta uma visão só de leitura do mundo de jogo.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Interface que oferece uma visão só de leitura do mundo de simulação.
    /// </summary>
    /// <remarks>
    /// A maioria das classes que possuem uma referência ao mundo de simulação
    /// devem apenas ter esta visão só de leitura do mundo, uma vez que não
    /// precisam de o modificar.
    /// </remarks>
    public interface IReadOnlyWorld
    {
        /// <summary>
        /// Propriedade que representa a dimensão horizontal do mundo.
        /// </summary>
        /// <value>Dimensão horizontal do mundo.</value>
        int XDim { get; }

        /// <summary>
        /// Propriedade que representa a dimensão vertical do mundo.
        /// </summary>
        /// <value>Dimensão vertical do mundo.</value>
        int YDim { get; }

        /// <summary>
        /// Método que indica se existe um agente na posição indicada no
        /// parâmetro <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">Posição no mundo de simulação.</param>
        /// <returns>
        /// <c>true</c> se existir um agente na posição dada em
        /// <paramref name="coord"/>, <c>false</c> caso contrário.
        /// </returns>
        bool IsOccupied(Coord coord);

        /// <summary>
        /// Método que devolve o agente na posição indicada no parâmetro
        /// <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">Posição no mundo de simulação.</param>
        /// <returns>
        /// Agente na posição indicada no parâmetro <paramref name="coord"/>.
        /// </returns>
        Agent GetAgentAt(Coord coord);

        /// <summary>
        /// Método que devolve um vetor entre as posições <paramref name="c1"/>
        /// e <paramref name="c2"/>.
        /// </summary>
        /// <param name="c1">Primeira posição no mundo de simulação.</param>
        /// <param name="c2">Segunda posição no mundo de simulação.</param>
        /// <returns>
        /// Vetor entre as posições <paramref name="c1"/> e
        /// <paramref name="c2"/>.
        /// </returns>
        Coord VectorBetween(Coord c1, Coord c2);

        /// <summary>
        /// Método que devolve a posição vizinha da posição dada no parâmetro
        /// <paramref name="pos"/> indo na direção indicada no parâmetro
        /// <paramref name="direction"/>.
        /// </summary>
        /// <param name="pos">Posição no mundo de simulação.</param>
        /// <param name="direction">Direção.</param>
        /// <returns>
        /// Posição vizinha da posição dada no parâmetro <paramref name="pos"/>
        /// indo na direção indicada no parâmetro <paramref name="direction"/>.
        /// </returns>
        Coord GetNeighbor(Coord pos, Direction direction);

        /// <summary>
        /// Método que devolve a posição vizinha da posição dada no parâmetro
        /// <paramref name="pos"/> indo na direção do vetor passado no
        /// parâmetro <paramref name="directionVector"/>.
        /// </summary>
        /// <param name="pos">Posição no mundo de simulação.</param>
        /// <param name="directionVector">Vetor que define a direção.</param>
        /// <returns>
        /// Posição vizinha da posição dada no parâmetro <paramref name="pos"/>
        /// indo na direção do vetor passado no parâmetro
        /// <paramref name="directionVector"/>.
        /// </returns>
        Coord GetNeighbor(Coord pos, Coord directionVector);
    }
}
