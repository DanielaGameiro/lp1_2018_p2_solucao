/// @file
/// @brief Este ficheiro contém a classe ZombiesVsHumans.World, que
/// implementa o mundo de jogo concreto.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

using System;
using System.ComponentModel;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Mundo de simulação.
    /// </summary>
    public class World : IReadOnlyWorld
    {
        /// <summary>
        /// Propriedade que representa a dimensão horizontal do mundo de
        /// simulação.
        /// </summary>
        /// <remarks>
        /// Na prática o valor desta propriedade é obtido através da dimensão 0
        /// do array bi-dimensional que internamente representa o mundo de
        /// simulação.
        /// </remarks>
        /// <value>Dimensão horizontal do mundo de simulação.</value>
        public int XDim => world.GetLength(0);

        /// <summary>
        /// Propriedade que representa a dimensão vertical do mundo de
        /// simulação.
        /// </summary>
        /// <remarks>
        /// Na prática o valor desta propriedade é obtido através da dimensão 1
        /// do array bi-dimensional que internamente representa o mundo de
        /// simulação.
        /// </remarks>
        /// <value>Dimensão vertical do mundo de simulação.</value>
        public int YDim => world.GetLength(1);

        /// <summary>
        /// Variável de instância privada que contém uma referência ao array
        /// bi-dimensional que internamente representa o mundo de simulação.
        /// </summary>
        private Agent[,] world;

        /// <summary>
        /// Cria uma nova instância do mundo de simulação.
        /// </summary>
        /// <param name="xDim">Dimensão horizontal do mundo.</param>
        /// <param name="yDim">Dimensão vertical do mundo.</param>
        public World(int xDim, int yDim)
        {
            // Internamente o mundo é representado por um array bi-dimensional
            // de agentes, instância que é criada aqui
            world = new Agent[xDim, yDim];
        }

        // ///////////////////////////////////////// //
        // Implementação da interface IReadOnlyWorld //
        // ///////////////////////////////////////// //

        /// <summary>
        /// Método que indica se existe um agente na posição indicada no
        /// parâmetro <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">Posição no mundo de simulação.</param>
        /// <returns>
        /// <c>true</c> se existir um agente na posição dada em
        /// <paramref name="coord"/>, <c>false</c> caso contrário.
        /// </returns>
        public bool IsOccupied(Coord coord)
        {
            // Normalizar coordenada (ou seja, garantir que a mesma está
            // contida nas dimensões do mundo)
            coord = Normalize(coord);

            // Devolver true senão estiver nada na coordenada,
            // devolver false caso contrário
            return world[coord.X, coord.Y] != null;
        }

        /// <summary>
        /// Método que devolve o agente na posição indicada no parâmetro
        /// <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">Posição no mundo de simulação.</param>
        /// <returns>
        /// Agente na posição indicada no parâmetro <paramref name="coord"/>.
        /// </returns>
        public Agent GetAgentAt(Coord coord)
        {
            // Normalizar coordenada (ou seja, garantir que a mesma está
            // contida nas dimensões do mundo)
            coord = Normalize(coord);

            // Devolver o agente na posição indicada
            return world[coord.X, coord.Y];
        }

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
        public Coord VectorBetween(Coord c1, Coord c2)
        {
            // Variáveis necessárias
            int x, y, xDistDirect, xDistWrapAround, yDistDirect, yDistWrapAround;

            // Normalizar coordenadas (ou seja, garantir que as mesmas estão
            // contidas nas dimensões do mundo)
            c1 = Normalize(c1);
            c2 = Normalize(c2);

            // Calcular distância horizontal direta entre as coordenadas, ou
            // seja sem dar a volta pelos lados
            xDistDirect = Math.Abs(c2.X - c1.X);

            // Calcular distância horizontal entre as coordenadas dando a volta
            // pelos lados
            xDistWrapAround = Math.Min(c2.X, c1.X) + XDim - Math.Max(c2.X, c1.X);

            // Calcular componente horizontal do vetor
            x = c2.X - c1.X;
            // Se a distância dando a volta for menor, temos de a ajustar...
            if (xDistWrapAround < xDistDirect)
                // ...adicionando ou subtraindo a dimensão horizontal do mundo
                x += -Math.Sign(x) * XDim;

            // Calcular distância vertical direta entre as coordenadas, ou seja
            // sem dar a volta pelo topo e/ou base
            yDistDirect = Math.Abs(c2.Y - c1.Y);

            // Calcular distância vertical entre as coordenadas dando a volta
            // pelo topo e/ou base
            yDistWrapAround = Math.Min(c2.Y, c1.Y) + YDim - Math.Max(c2.Y, c1.Y);

            // Calcular componente vertical do vetor
            y = c2.Y - c1.Y;
            // Se a distância dando a volta for menor, temos de a ajustar...
            if (yDistWrapAround < yDistDirect)
                // ...adicionando ou subtraindo a dimensão vertical do mundo
                y += -Math.Sign(y) * YDim;

            // Devolver vetor entre c1 e c2
            return new Coord(x, y);
        }

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
        public Coord GetNeighbor(Coord pos, Direction direction)
        {
            // x e y começam por ser a posição atual
            int x = pos.X, y = pos.Y;

            // Verificar que direção se trata e ajustar x e y em conformidade
            switch (direction)
            {
                case Direction.Up:
                    y -= 1;
                    break;
                case Direction.UpLeft:
                    x -= 1;
                    y -= 1;
                    break;
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.DownLeft:
                    x -= 1;
                    y += 1;
                    break;
                case Direction.Down:
                    y += 1;
                    break;
                case Direction.DownRight:
                    x += 1;
                    y += 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
                case Direction.UpRight:
                    x += 1;
                    y -= 1;
                    break;
                case Direction.None:
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unknown direction");
            }

            // Devolver posição vizinha, devidamente normalizada
            return Normalize(new Coord(x, y));
        }

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
        public Coord GetNeighbor(Coord pos, Coord directionVector)
        {
            // Variável que irá conter a direção representada pelo vetor
            Direction direction = default(Direction);

            // Converter valores do vetor em -1, 0 ou 1, obtendo apenas a sua
            // direção ignorando o seu comprimento
            directionVector = new Coord(
                Math.Sign(directionVector.X), Math.Sign(directionVector.Y));

            // Determinar qual a direção representada pelo vetor
            if (directionVector.X == 1 && directionVector.Y == 1)
                direction = Direction.DownRight;
            else if (directionVector.X == 1 && directionVector.Y == 0)
                direction = Direction.Right;
            else if (directionVector.X == 1 && directionVector.Y == -1)
                direction = Direction.UpRight;
            else if (directionVector.X == 0 && directionVector.Y == 1)
                direction = Direction.Down;
            else if (directionVector.X == 0 && directionVector.Y == 0)
                direction = Direction.None;
            else if (directionVector.X == 0 && directionVector.Y == -1)
                direction = Direction.Up;
            else if (directionVector.X == -1 && directionVector.Y == 1)
                direction = Direction.DownLeft;
            else if (directionVector.X == -1 && directionVector.Y == 0)
                direction = Direction.Left;
            else if (directionVector.X == -1 && directionVector.Y == -1)
                direction = Direction.UpLeft;

            // Devolver posição vizinha usando o overload deste método que
            // aceita direções em vez de vetores
            return GetNeighbor(pos, direction);
        }

        // ///////////////////////////////////////////////////// //
        // Métodos que permitem alteração dos conteúdos do mundo //
        // ///////////////////////////////////////////////////// //

        /// <summary>
        /// Move agente, indicado no parâmetro <paramref name="agent"/>, para
        /// a posição indicada no parâmetro <paramref name="dest"/>.
        /// </summary>
        /// <remarks>
        /// Este método não verifica se o destino está ocupado. Desta forma, o
        /// código que chama este método deve verificar essa situação usando o
        /// método <see cref="IsOccupied()"/>.
        /// </remarks>
        /// <param name="agent">Agente a mover.</param>
        /// <param name="dest">Posição para onde mover o agente.</param>
        public void MoveAgent(Agent agent, Coord dest)
        {
            // Normalizar coordenada (ou seja, garantir que a mesma está
            // contida nas dimensões do mundo)
            dest = Normalize(dest);

            // Colocar agente na posição destino
            world[dest.X, dest.Y] = agent;

            // Remover agente da posição origem
            world[agent.Pos.X, agent.Pos.Y] = null;
        }

        /// <summary>
        /// Adiciona agente ao mundo de simulação.
        /// </summary>
        /// <remarks>
        /// Este método não verifica se o destino está ocupado. Desta forma, o
        /// código que chama este método deve verificar essa situação usando o
        /// método <see cref="IsOccupied()"/>.
        /// </remarks>
        /// <param name="agent">
        /// Agente a adicionar ao mundo de simulação.
        /// </param>
        public void AddAgent(Agent agent)
        {
            // Colocar agente no seu destino
            world[agent.Pos.X, agent.Pos.Y] = agent;
        }

        /// <summary>
        /// Normaliza coordenada, garantido que a mesma está dentro dos limites
        /// do mundo.
        /// </summary>
        /// <param name="coord">Coordenada a normalizar.</param>
        /// <returns>Coordenada normalizada.</returns>
        private Coord Normalize(Coord coord)
        {
            // Valores x e y inicialmente colocados com os valores originais
            int x = coord.X;
            int y = coord.Y;

            // Garantir que dimensão horizontal está dentro das dimensões do
            // mundo
            while (x >= XDim) x -= XDim;
            while (x < 0) x += XDim;
            // Garantir que dimensão vertical está dentro das dimensões do
            // mundo
            while (y >= YDim) y -= YDim;
            while (y < 0) y += YDim;

            // Devolver coordenada normalizada.
            return new Coord(x, y);
        }
    }
}
