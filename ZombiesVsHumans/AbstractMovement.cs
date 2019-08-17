/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.AbstractMovement, que
/// define de forma abstrata o movimento dos agentes.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Classe que define de forma abstrata o movimento realizado pelos agentes.
    /// </summary>
    /// <remarks>
    /// Tipos de movimento específicos devem ser implementados em classes que
    /// estendem esta classe.
    /// </remarks>
    public abstract class AbstractMovement
    {
        /// <summary>
        /// Referência, numa perspetiva só de leitura, ao mundo de simulação.
        /// </summary>
        protected readonly IReadOnlyWorld world;

        /// <summary>
        /// Construtor, que simplesmente aceita e guarda uma referência, só de
        /// leitura, ao mundo de simulação.
        /// </summary>
        /// <param name="world">
        /// Referência, só de leitura, ao mundo de simulação.
        /// </param>
        protected AbstractMovement(IReadOnlyWorld world)
        {
            this.world = world;
        }

        /// <summary>
        /// Método abstrato que devolve a coordenada para a qual o agente se
        /// deve mover. As sub-classes desta classe devem oferecer uma
        /// implementação concreta deste método.
        /// </summary>
        /// <param name="agent">Agente que se quer mover.</param>
        /// <param name="message">
        /// Parâmetro de saída (<c>out</c>) onde deve ser colocada uma mensagem
        /// que sumariza o movimento que deve ser realizado pelo agente.
        /// </param>
        /// <returns>A coordenada para a qual o agente se deve mover.</returns>
        public abstract Coord WhereToMove(Agent agent, out string message);
    }
}
