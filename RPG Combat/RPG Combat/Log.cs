using System.IO;
using System.Collections.Generic;

namespace Lecture18;

public class Log
{
    private TextWriter writer;
    public Log(TextWriter writer) => this.writer = writer;

    public void GameStartAll(List<Character> c) => writer.WriteLine("--- BATTLE ROYALE START ---");
    public void InitiativeOrder(List<Character> c)
    {
        foreach (var x in c) writer.WriteLine($"{x.Name} Initiative: {x.Initiative}");
    }
    public void CharacterTurn(Character c) => writer.WriteLine($"\n> {c.Name} (Pos: {c.Position})");
    public void CharacterStatus(Character c) => writer.WriteLine($"{c.Name}: {c.HealthRatio:P0} HP");
    public void CharacterAttack(Character s, Character t, int r) => writer.WriteLine($"{s.Name} attacks {t.Name} (roll {r})");
    public void CharacterWait(Character c, int r) => writer.WriteLine($"{c.Name} waits (heals slightly)");
    public void CharacterDoesNothing(Character c) => writer.WriteLine($"{c.Name} stands still...");
    public void AttackMiss(Character s, Character t) => writer.WriteLine("Miss!");
    public void AttackHit(Character s, Character t, int d) => writer.WriteLine($"Hit for {d} damage!");
    public void CharacterMoves(Character c, string w, int p) => writer.WriteLine($"{c.Name} uses {w}, new Pos: {p}");
    public void OutOfRange(Character s, Character t, int d, string w) => writer.WriteLine($"{w} out of range! (Dist: {d})");
    public void HalfDamageLog() => writer.WriteLine("Long distance - Half Damage!");
    public void GameOver(Character? w) => writer.WriteLine($"\nWINNER: {w?.Name ?? "None"}");
}