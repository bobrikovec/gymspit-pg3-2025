using System;
using System.Collections.Generic;

namespace Lecture18;

public class AI : Controller
{
    private Random random;
    public AI(Random random) => this.random = random;

    public string ChooseAction(Character character, List<Character> aliveEnemies)
        => random.Next(3) == 0 ? Controller.TURN_CHOICE_WAIT : Controller.TURN_CHOICE_ATTACK;

    public string ChooseWeapon(Character character)
        => random.Next(2) == 0 ? "sword" : "bow";

    public Character ChooseTarget(Character character, List<Character> aliveEnemies)
        => aliveEnemies[random.Next(aliveEnemies.Count)];
}