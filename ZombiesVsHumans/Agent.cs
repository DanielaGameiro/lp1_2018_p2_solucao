/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.Agent, que representa
/// um agente no jogo.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System.ComponentModel;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Classe que representa um agente.
    /// </summary>
    public class Agent
    {
        /// <summary>
        /// Propriedade auto-implementada só de leitura que representa o ID
        /// único do agente, que não muda mesmo que o agente se transforme em
        /// zombie, caso seja humano.
        /// </summary>
        /// <value>ID único do agente.</value>
        public int ID { get; }

        /// <summary>
        /// Propriedade auto-implementada que representa a posição atual do
        /// agente.
        /// </summary>
        /// <value>Posição atual do agente.</value>
        public Coord Pos { get; private set; }

        /// <summary>
        /// Propriedade auto-implementada que representa o género do agente.
        /// </summary>
        /// <value>Género do agente: zombie ou humano.</value>
        public AgentKind Kind { get; private set; }

        /// <summary>
        /// Propriedade auto-implementada que representa o tipo de movimento
        /// realizado pelo agente.
        /// </summary>
        /// <value>
        /// Tipo de movimento realizado pelo agente: "inteligência artificial"
        /// ou controlado pelo jogador.
        /// </value>
        public AgentMovement Movement { get; private set; }

        /// <summary>
        /// Variável de instância privada que contém uma referência ao mundo de
        /// simulação.
        /// </summary>
        private World world;

        /// <summary>
        /// Variável de instância privada que contém uma referência ao objeto
        /// que determina o movimento a realizar pelo agente.
        /// </summary>
        private AbstractMovement moveBehavior;

        /// <summary>
        /// Método construtor, instancia um novo agente.
        /// </summary>
        /// <param name="id">ID único do agente.</param>
        /// <param name="pos">Posição inicial do agente.</param>
        /// <param name="kind">
        /// Género inicial do agente (zombie ou humano).
        /// </param>
        /// <param name="movement">Tipo de movimento inicial do agente (por
        /// "inteligência artificial" ou controlado pelo jogador).
        /// </param>
        /// <param name="world">Referência ao mundo de simulação.</param>
        public Agent(int id, Coord pos,
            AgentKind kind, AgentMovement movement, World world)
        {
            // Género dos agentes considerados inimigos do agente a ser criado
            AgentKind enemy;
            // Indica se o agente a ser criado deve fugir (true) ou não (false)
            // dos inimigos
            bool runaway;

            // Guardar as referências passadas no construtor
            ID = id;
            Pos = pos;
            Kind = kind;
            Movement = movement;
            this.world = world;

            // Adicionar o agente ao mundo de simulação
            world.AddAgent(this);

            // Definir género dos inimigos e se o agente deve ou não fugir
            // deles, dependendo do género de agente a ser criado
            switch (kind)
            {
                case AgentKind.Zombie:
                    // Se agente for zombie os seus inimigos são humanos e
                    // não deve fugir deles (antes pelo contrário)
                    enemy = AgentKind.Human;
                    runaway = false;
                    break;
                case AgentKind.Human:
                    // Se agente for humando os seus inimigos são zombies e
                    // deve fugir deles
                    enemy = AgentKind.Zombie;
                    runaway = true;
                    break;
                default:
                    // Lançar uma exceção caso seja um género desconhecido de
                    // agente
                    throw new InvalidEnumArgumentException(
                        "Unknown agent kind");
            }

            // Definir classe concreta que define o movimento dos agentes
            switch (movement)
            {
                case AgentMovement.Player:
                    // Movimento controlado pelo jogador
                    moveBehavior = new PlayerMovement(world);
                    break;
                case AgentMovement.AI:
                    // Movimento controlado por IA
                    moveBehavior = new AIMovement(enemy, runaway, world);
                    break;
                default:
                    // Movimento desconhecido, lançar exceção
                    throw new InvalidEnumArgumentException(
                        "Unknown movement type");
            }
        }

        /// <summary>
        /// Método que executa as ações do agente para o turno atual.
        /// </summary>
        /// <param name="changePopulation">
        /// Parâmetro de saída (<c>out</c>) que deve indicar se as ações
        /// realizadas pelo agente alteraram a população de agentes (por
        /// exemplo, se algum humano foi infetado).
        /// </param>
        /// <param name="message">
        /// Parâmetro de saída (<c>out</c>) no qual deve ser colocada uma
        /// mensagem descrevendo a ação realizada pelo agente.
        /// </param>
        public void PlayTurn(out bool changePopulation, out string message)
        {
            // Obter coordenada de destino preferida, bem como uma mensagem
            // sobre esse potencial movimento
            Coord dest = moveBehavior.WhereToMove(this, out message);

            // Em princípio consideramos que não foram realizadas alterações
            // à população de agentes
            changePopulation = false;

            // O tipo de ação depende se a coordenada de destino está ocupada
            // ou não por outro agente
            if (!world.IsOccupied(dest))
            {
                // Senão estiver, simplesmente movemos o agente para lá...
                world.MoveAgent(this, dest);
                // ...atualizando a sua posição...
                Pos = dest;
                // ...bem como a mensagem, indicando que o movimento foi feito
                // com sucesso.
                message += " and succeeded";
            }
            else
            {
                // Se a coordenada estiver ocupada, obter referência ao agente
                // que a está a ocupar
                Agent other = world.GetAgentAt(dest);

                // É o próprio agente que está a ocupar a coordenada?
                if (this == other)
                {
                    // Nesse caso não é suposto o agente mover-se, pois não
                    // encontrou inimigos: basta completar a mensagem
                    message += " and failed to move";
                }
                else if (Kind == AgentKind.Zombie
                    && other.Kind == AgentKind.Human)
                {
                    // Se o agente atual for um zombie e o agente que ocupa a
                    // coordenada de destino for um humano, então vamos infetar
                    // esse humando
                    world.GetAgentAt(dest).Infect();
                    // Significa que ocorreu uma modificação na população, pelo
                    // que temos de atualizar o respetivo parâmetro out...
                    changePopulation = true;
                    // ...e completar a mensagem em conformidade
                    message += $" and infected {other}";
                }
                else
                {
                    // Caso contrário, simplesmente atualizamos a mensagem de
                    // modo a indicar que o agente não se moveu pois
                    // embateu noutro
                    message += $" but bumped into {other}";
                }
            }
        }

        /// <summary>
        /// Método privado que realizada a infeção de um agente.
        /// </summary>
        private void Infect()
        {
            // Agente passa a ser zombie e a ter movimento controlado por IA
            Kind = AgentKind.Zombie;
            Movement = AgentMovement.AI;
            moveBehavior = new AIMovement(AgentKind.Human, false, world);
        }

        /// <summary>
        /// Devolve uma string que representa o agente.
        /// </summary>
        /// <returns>Uma string que representa o agente.</returns>
        public override string ToString()
        {
            string type = (Kind == AgentKind.Zombie) ? "z" : "h";
            if (Movement == AgentMovement.Player)
                type = type.ToUpper();
            return $"{type}{ID,2:X2}";
        }
    }
}
