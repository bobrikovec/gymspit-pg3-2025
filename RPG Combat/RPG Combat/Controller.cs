using System.Collections.Generic;

namespace Lecture18;

public interface Controller
{
    public const string TURN_CHOICE_ATTACK = "attack";
    public const string TURN_CHOICE_WAIT = "wait";

    string ChooseAction(Character character, List<Character> aliveEnemies);
    string ChooseWeapon(Character character);
    Character ChooseTarget(Character character, List<Character> aliveEnemies);
}