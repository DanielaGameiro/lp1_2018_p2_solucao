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
    /// <summary>
    /// Interface que define de forma geral a funcionalidade de uma UI para
    /// o jogo Zombies vs Humans.
    /// </summary>
    /// <remarks>
    /// O jogo Zombie vs Humans suporta qualquer tipo de UI, gráfica ou texto,
    /// sendo suficiente que a classe que contém o código da UI implemente
    /// esta interface.
    /// </remarks>
    public interface IUserInterface
    {
        /// <summary>
        /// Inicializa e calcula todas as variáveis necessárias para desenhar
        /// a UI no ecrã, com base na dimensão do mundo.
        /// </summary>
        /// <remarks>
        /// Este método não deve apresentar nada no ecrã, apenas fazer os
        /// cálculos necessários para desenhar adequadamente a UI.
        /// </remarks>
        /// <param name="xDim">Dimensão horizontal do mundo.</param>
        /// <param name="yDim">Dimensão vertical do mundo.</param>
        void Initialize(int xDim, int yDim);

        /// <summary>
        /// Inicializa a UI, apresentado-a pela primeira vez no ecrã.
        /// </summary>
        void RenderStart();

        /// <summary>
        /// Atualiza a visualização do mundo de simulação no ecrã.
        /// </summary>
        /// <param name="world">
        /// Referência só de leitura ao objeto que define o mundo de simulação.
        /// </param>
        void RenderWorld(IReadOnlyWorld world);

        /// <summary>
        /// Apresenta uma mensagem de erro ao utilizador.
        /// </summary>
        /// <param name="msg">Mensagem de erro a apresentar.</param>
        void RenderError(string msg);

        /// <summary>
        /// Apresenta uma mensagem ao utilizador.
        /// </summary>
        /// <remarks>
        /// A mensagem a apresentar deve estar diretamente relacionada com o
        /// desenrolar normal do jogo.
        /// </remarks>
        /// <param name="msg">Mensagem a apresentar.</param>
        void RenderMessage(string msg);

        /// <summary>
        /// Apresenta e/ou atualiza o painel de informação contendo as
        /// estatísticas do jogo, por exemplo nº de turnos, nº de zombies, etc.
        /// </summary>
        /// <param name="info">
        /// Dicionário contendo a informação/estatísticas a apresentar. As
        /// chaves são strings contendo o nome da informação, os valores são
        /// os valores inteiros que correspondem à informação a apresentar.
        /// </param>
        void RenderInfo(IDictionary<string, int> info);

        /// <summary>
        /// Aguarda por input do utilizador para definir a direção na qual o
        /// agente (controlado pelo jogador) se deve mover.
        /// </summary>
        /// <param name="id">
        /// Identificar único do agente, de modo a que o
        /// jogador saiba de que agente se trata.
        /// </param>
        /// <returns>Direção para a qual o agente se deve mover.</returns>
        Direction InputDirection(string id);

        /// <summary>
        /// Finaliza e termina a UI.
        /// </summary>
        void RenderFinish();
    }
}
