/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.PlayerMovement, que
/// implementa o movimento dos agentes controlado por um jogador.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Classe que controla o movimento dos agentes controlado pelo jogador.
    /// </summary>
    public class PlayerMovement : AbstractMovement
    {
        /// <summary>
        /// Método construtor, cria uma nova instância desta classe.
        /// </summary>
        /// <param name="world">
        /// Referência só de leitura ao mundo de simulação.
        /// </param>
        public PlayerMovement(IReadOnlyWorld world) : base(world)
        {
            // Este construtor simplesmente chama o construtor da classe base.
        }

        /// <summary>
        /// Método que devolve uma coordenada escolhida pelo jogador e que
        /// corresponde a uma célula vizinha do agente que se está a mover.
        /// </summary>
        /// <param name="agent">
        /// Agente a ser movido pelo jogador.
        /// </param>
        /// <param name="message">
        /// Parâmetro de saída (<c>out</c>) onde deve ser colocada uma mensagem
        /// que sumariza o movimento que o jogador definiu.
        /// </param>
        /// <returns>
        /// A coordenada para a qual o jogador decidiu mover o agente.
        /// </returns>
        public override Coord WhereToMove(Agent agent, out string message)
        {
            // Pedir ao jogador para inserir uma direção
            Direction direction = Program.UI.InputDirection(agent.ToString());

            // Criar mensagem sobre o movimento realizado
            message = $"Player tried to move {direction}";

            // Devolver coordenada para a qual o agente se deve mover, baseada
            // na direção indicada pelo jogador
            return world.GetNeighbor(agent.Pos, direction);
        }
    }
}
