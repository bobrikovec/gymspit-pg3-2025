using System.Collections.Generic;
using System.Linq;

namespace Lecture18;

public class Game
{
    private List<Character> characters;
    private Die attackDie;
    private Die waitDie;

    public Game(List<Character> characters, Die attackDie, Die waitDie)
    {
        this.characters = characters;
        this.attackDie = attackDie;
        this.waitDie = waitDie;
    }

    public void Run(Log log)
    {
        foreach (var c in characters) c.Reset();
        log.GameStartAll(characters);

        foreach (var c in characters)
        {
            c.Initiative = attackDie.Roll() + c.Dexterity;
        }

        characters = characters.OrderByDescending(c => c.Initiative).ToList();
        log.InitiativeOrder(characters);

        while (characters.Count(c => c.Alive) > 1)
        {
            foreach (var active in characters)
            {
                if (!active.Alive) continue;
                if (characters.Count(c => c.Alive) <= 1) break;

                log.CharacterTurn(active);
                var enemies = characters.Where(c => c != active && c.Alive).ToList();
                string action = active.Controller.ChooseAction(active, enemies);

                if (action == Controller.TURN_CHOICE_ATTACK)
                {
                    string weapon = active.Controller.ChooseWeapon(active);
                    Character target = active.Controller.ChooseTarget(active, enemies);
                    active.Attack(log, target, attackDie, weapon);
                }
                else
                {
                    active.Wait(log, waitDie);
                }

                foreach (var c in characters.Where(x => x.Alive)) log.CharacterStatus(c);
            }
        }

        log.GameOver(characters.FirstOrDefault(c => c.Alive));
    }
}