/// @file
/// Este ficheiro contém a classe `Program`, que por sua vez contém o método
/// `Main()`. Assim sendo, o programa começa aqui.
///
/// @author Nuno Fachada
/// @date 2019
/// @copyright [GPLv3](http://www.gnu.org/licenses/gpl.html)

namespace ZombiesVsHumans
{
    /// <summary>
    /// Esta classe contém o método <see cref="Main()"/>, pelo que o programa
    /// começa aqui.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Uma referência <c>static</c> e globalmente acessível ao objeto que
        /// implementa a interface de utilizador (UI).
        /// </summary>
        /// <remarks>
        /// Este tipo de acesso global a uma instância única de uma classe é
        /// geralmente realizada usando o Singleton design pattern, a ser
        /// discutido em LP2-
        /// </remarks>
        /// <value>O objeto que implementa a interface de utilizador.</value>
        public static IUserInterface UI { get; private set; }

        /// <summary>
        /// O programa começa aqui.
        /// </summary>
        /// <param name="args">
        /// Opções na linha de comando, que são as seguintes:
        /// <list type="bullet">
        /// <item>
        /// <term><c>-x</c></term>
        /// <description>Dimensão horizontal da grelha de jogo.</description>
        /// </item>
        /// <item>
        /// <term><c>-y</c></term>
        /// <description>Dimensão vertical da grelha de jogo.</description>
        /// </item>
        /// <item>
        /// <term><c>-z</c></term>
        /// <description>Número total de zombies.</description>
        /// </item>
        /// <item>
        /// <term><c>-h</c></term>
        /// <description>Número total de humanos.</description>
        /// </item>
        /// <item>
        /// <term><c>-Z</c></term>
        /// <description>Número de zombies controlados por jogadores.</description>
        /// </item>
        /// <item>
        /// <term><c>-H</c></term>
        /// <description>Número de humanos controlados por jogadores.</description>
        /// </item>
        /// <item>
        /// <term><c>-t</c></term>
        /// <description>Número máximo de turns.</description>
        /// </item>
        /// </list>
        /// <example>
        /// Um exemplo de utilização:
        /// <c>dotnet run -- -x 20 -y 20 -z 10 -h 30 -Z 1 -H 2 -t 1000</c>
        /// </example>
        /// </param>
        private static void Main(string[] args)
        {
            // Instanciar uma UI para consola. Se quisermos outro tipo de UI,
            // por exemplo uma interface gráfica, bastaria instanciar outro
            // tipo de objeto, desde que a sua classe implemente a interface
            // IUserInterface.
            UI = new ConsoleUserInterface();

            // Tratar as opções da linha de comando, criando um objeto do tipo
            // Options, que contém essas mesmas opções já tratadas.
            Options options = Options.ParseArgs(args);

            // Foram encontrados erros no tratamento das opções da linha de
            // comandos?
            if (options.Error)
            {
                // Em caso afirmativo, mostrar todos esses erros
                foreach (string error in options.ErrorMessages)
                    UI.RenderError("ERROR: " + error);
            }
            else
            {
                // Caso contrário, criar um novo jogo...
                Game game = new Game(options);
                // ...e jogá-lo
                game.Play();
            }
        }
    }
}
