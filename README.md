<!--
"Proposta de resolução do 2º Projeto de Linguagens de Programação I 2018/2019"
(c) by Nuno Fachada

All documentation and text (non-source code) in "Proposta de resolução do 2º
Projeto de Linguagens de Programação I 2018/2019" is licensed under a Creative
Commons Attribution-NonCommercial-ShareAlike 4.0 International License.

All the C# and images source code is licensed under the GNU General Public
License, version 3.

You should have received a copy of both licenses along with this
work. If not, see:

* <http://creativecommons.org/licenses/by-nc-sa/4.0/>
* <https://www.gnu.org/licenses/gpl-3.0.en.html>
-->

# Zombies vs Humanos

![Zombies vs Humanos](logo.png "Logo Zombies vs Humanos")
Proposta de resolução do [2º Projeto de LP1 2018/19][enunciado].

## Sumário

Este repositório contém uma proposta de resolução do
[2º Projeto de LP1 2018/19](https://github.com/VideojogosLusofona/lp1_2018_p2),
com os seguintes conteúdos:

* Código C# para implementação adequada da solução, considerando apenas a
  matéria lecionada em LP1 (uma vez que alguns aspetos poderiam ser melhorados
  com a matéria de LP2).
* Documentação gerada em [Doxygen], disponível [aqui][docs].
* Sugestão de bom uso de Git e [boas mensagens de *commit*][commits].
* Exemplo de como elaborar algumas partes do relatório, nomeadamente:
  * Como escrever a *Arquitetura da solução* e *Referências*.
  * Como fazer um diagrama UML de classes.
  * Como fazer um fluxograma.

## Arquitetura da solução

### Funcionamento do programa

O programa deve ser invocado com as opções da linha de comandos indicadas no
[enunciado][enunciado-opcoes], seguindo depois a sequência indicada no
fluxograma apresentado na Figura 1.

![Fluxograma](fluxograma.png "Fluxograma")

**Figura 1** - Fluxograma do programa (código fonte da figura
disponível [aqui](imgsource/fluxograma.drawio), tendo a mesma sido gerada em
[Draw.io]).

O programa começa por tratar as opções da linha de comandos, e se as mesmas
forem válidas é criado o mundo de simulação, bem como os agentes que o compõem,
caso contrário, o programa termina. Após a primeira renderização, entramos no
_game loop_, no qual cada iteração do ciclo corresponde a um turno do jogo.
Em cada turno, cada agente realiza a sua ação (mover ou infetar), e caso exista
uma alteração na população, é feita uma recontagem dos agentes. A visualização
é sempre atualizada após a ação de cada agente. O _game loop_ termina quando
não existirem mais humanos ou quando tiver sido atingido o número máximo de
turnos. O programa termina com uma mensagem indicando o resultado final do
jogo.

### Design de classes

_em construção_

![Diagrama UML de classes](UML.png "Diagrama UML de classes")

**Figura 2** - Diagrama UML de classes da solução (código fonte da figura
disponível [aqui](imgsource/uml.yuml), tendo a mesma sido gerada em [yUML]).
Para simplificação do diagrama são apenas mostradas as relações de dependência
mais importantes.

### Estruturas de dados e algoritmos utilizados

_em construção_

<!--
* Fisher–Yates shuffle
* Array bi-dimensional
* Fila (para mensagens)
* Opções e algoritmo para tratamento de opções
* Algoritmo de IA dos agentes
* Cache da visualização
-->

## Referências

* [Fisher–Yates shuffle - Wikipedia](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
* _em construção_

## Metadados

* Autor: [Nuno Fachada]
* Curso:  [Licenciatura em Videojogos][lamv]
* Instituição: [Universidade Lusófona de Humanidades e Tecnologias][ULHT]

## Licenças e atribuições

* O código é disponibilizado através da licença [GPLv3].
* A documentação é disponibilizada através da licença [CC BY-NC-SA 4.0][CCBYNC].
* O [logótipo] do projeto é baseado nos ícones desenhados por [Freepik]
  disponíveis em <https://www.flaticon.com>.

[enunciado]:https://github.com/VideojogosLusofona/lp1_2018_p2
[enunciado-opcoes]:https://github.com/VideojogosLusofona/lp1_2018_p2#invoca%C3%A7%C3%A3o-do-programa
[lamv]:https://www.ulusofona.pt/licenciatura/videojogos
[Nuno Fachada]:https://github.com/fakenmc
[ULHT]:https://www.ulusofona.pt/
[GPLv3]:http://www.gnu.org/licenses/gpl.html
[CCBYNC]:https://creativecommons.org/licenses/by-nc-sa/4.0/
[logótipo]:logo.png
[Freepik]:https://www.freepik.com/home
[yUML]:https://yuml.me/
[Draw.io]:https://www.draw.io/
[Doxygen]:http://www.doxygen.nl/index.html
[docs]:https://videojogoslusofona.github.io/lp1_2018_p2_solucao/
[commits]:https://github.com/VideojogosLusofona/lp1_2018_p2_solucao/commits/master
