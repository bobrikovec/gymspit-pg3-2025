using System;
using System.Collections.Generic;

namespace Lecture18;

public class Player : Controller
{
    private TextReader input;
    private TextWriter? prompt;

    public Player(TextReader input, TextWriter? prompt = null)
    {
        this.input = input;
        this.prompt = prompt;
    }

    public string ChooseAction(Character character, List<Character> aliveEnemies)
    {
        while (true)
        {
            prompt?.WriteLine("\nAction: (A)ttack, (W)ait");
            string choice = input.ReadLine()?.ToLower();
            if (choice == "a" || choice == "attack") return Controller.TURN_CHOICE_ATTACK;
            if (choice == "w" || choice == "wait") return Controller.TURN_CHOICE_WAIT;
        }
    }

    public string ChooseWeapon(Character character)
    {
        while (true)
        {
            prompt?.WriteLine("Weapon: (1) Sword (Range 0-6), (2) Bow (Range 0-36)");
            string choice = input.ReadLine();
            if (choice == "1") return "sword";
            if (choice == "2") return "bow";
        }
    }

    public Character ChooseTarget(Character character, List<Character> aliveEnemies)
    {
        while (true)
        {
            prompt?.WriteLine("Target:");
            for (int i = 0; i < aliveEnemies.Count; i++)
            {
                int dist = Math.Abs(character.Position - aliveEnemies[i].Position);
                prompt.WriteLine($"{i}: {aliveEnemies[i].Name} (Dist: {dist})");
            }
            if (int.TryParse(input.ReadLine(), out int c) && c >= 0 && c < aliveEnemies.Count) return aliveEnemies[c];
        }
    }
}