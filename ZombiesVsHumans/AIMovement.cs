/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.AIMovement, que
/// implementa o movimento por "inteligência artificial" dos agentes.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Define o movimento por "inteligência artificial" dos agentes.
    /// </summary>
    public class AIMovement : AbstractMovement
    {
        /// <summary>
        /// Género de agente considerado inimigo.
        /// </summary>
        private AgentKind enemy;

        /// <summary>
        /// Indica se agent deve fugir ou perseguir os agentes considerados
        /// inimigos.
        /// </summary>
        private bool runAway;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="enemy">Agentes inimigos, a perseguir ou evitar.</param>
        /// <param name="runAway">
        /// Indica se os agentes inimigos devem ser evitados (<c>true</c>)
        /// ou perseguidos (<c>false</c>).</param>
        /// <param name="world">
        /// Referência, só de leitura, ao mundo de simulação.
        /// </param>
        public AIMovement(AgentKind enemy, bool runAway, IReadOnlyWorld world)
            : base(world)
        {
            this.enemy = enemy;
            this.runAway = runAway;
        }

        /// <summary>
        /// Método que implementa o movimento por "inteligência artificial" dos
        /// agentes.
        /// </summary>
        /// <remarks>
        /// Este método é uma sobreposição concreta do método
        /// <see cref="AbstractMovement.WhereToMove(Agent, out string)"/>.
        /// </remarks>
        /// <param name="agent">Agente que se quer mover.</param>
        /// <param name="message">
        /// Parâmetro de saída (<c>out</c>) onde deve ser colocada uma mensagem
        /// que sumariza o movimento que deve ser realizado pelo agente.
        /// </param>
        /// <returns>A coordenada para a qual o agente se deve mover.</returns>
        public override Coord WhereToMove(Agent agent, out string message)
        {
            // Agente alvo, a perseguir ou evitar
            Agent target = null;

            // Variável que indica se o alvo/inimigo foi encontrado
            bool foundEnemy = false;

            // Raio máximo de procura, igual à maior dimensão do mundo (x ou y)
            // a dividir por 2
            int maxRadius = Math.Max(world.XDim, world.YDim) / 2;

            // Vetor entre o agente atual e um possível alvo/inimigo
            Coord vector = default(Coord);

            // Variável auxiliar para a procura de alvos/inimigos
            Coord currentPos;

            // Percorrer os raios desde 1 até ao raio máximo
            // Ciclo pode parar antecipadamente caso inimigo seja encontrado
            for (int r = 1; r <= maxRadius && !foundEnemy; r++)
            {
                // Percorrer as coordenadas horizontais dentro do raio atual
                for (int dx = -r; dx <= r && !foundEnemy; dx++)
                {
                    // Percorrer as coordenadas verticais dentro do raio atual
                    for (int dy = -r; dy <= r && !foundEnemy; dy++)
                    {
                        // Caso a coordenada (x,y) atual não esteja no raio
                        // atual (porque está num raio menor), significa que já
                        // procuramos nesta posição e podemos passar para a
                        // próxima
                        if (Math.Max(Math.Abs(dx), Math.Abs(dy)) != r) continue;

                        // Criar uma instância para a coordenada (x,y) atual
                        currentPos =
                            new Coord(agent.Pos.X + dx, agent.Pos.Y + dy);

                        // Está alguém na coordenada atual?
                        if (world.IsOccupied(currentPos))
                        {
                            // Se sim, obter esse "alguém"
                            target = world.GetAgentAt(currentPos);

                            // Esse "alguém" é inimigo?
                            if (target.Kind == enemy)
                            {
                                // Se sim, vetor de movimento entre agente a
                                // mover-se e o seu inimigo
                                vector = runAway
                                    // Se for para fugir, vetor é entre inimigo
                                    // a agente a mover-se
                                    ? world.VectorBetween(currentPos, agent.Pos)
                                    // Se for para perseguir, vetor é entre
                                    // agente a mover-se e inimigo
                                    : world.VectorBetween(agent.Pos, currentPos);

                                // Indicar que foi encontrado inimigo de modo
                                // a sairmos dos ciclos
                                foundEnemy = true;
                            }
                        }
                    }
                }
            }

            // Inimigo foi encontrado?
            if (foundEnemy)
            {
                // Se sim, atualizar mensagem com essa indicação...
                string way = runAway ? "runaway from" : "move towards";
                message = $"{agent} tried to {way} {target}";
                // ...e devolver a posição vizinha determinada pela direção do
                // vetor calculado anteriormente
                return world.GetNeighbor(agent.Pos, vector);
            }

            // Caso inimigo não tenha sido encontrado, atualizar mensagem com
            // essa indicação e devolver posição onde o agente se encontra
            // atualmente (indicando que não há movimento a ser feito)
            message = $"{agent} didn't find enemies";
            return agent.Pos;
        }
    }
}
