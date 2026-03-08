using Lecture18;
using System.Collections.Generic;
using System.Numerics;

Random random = new Random();
Log log = new Log(Console.Out);

Controller ai = new AI(random);
// Poslední parametr je Dexterity (Obratnost pro iniciativu)
Character c3PO = new Character(ai, "C-3PO", 15, 2, 12, new Die(random, 8), 2);
Character r2D2 = new Character(ai, "R2-D2", 10, 0, 14, new Die(random, 6), 5);
Character luke = new Character(new Player(Console.In, Console.Out), "Luke", 20, 0, 10, new Die(random, 4), 8);

List<Character> roster = new List<Character> { c3PO, r2D2, luke };

Game game = new Game(roster, new Die(random, 20), new Die(random, 6));
game.Run(log);