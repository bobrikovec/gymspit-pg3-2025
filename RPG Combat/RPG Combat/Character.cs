using System;

namespace Lecture18;

public class Character
{
    private Controller controller;
    private string name;
    private int hitPoints;
    private int maxHitPoints;
    private int attackBonus;
    private int armorClass;
    private Die weaponDie;

    public int Dexterity { get; private set; }
    public int Initiative { get; set; }
    public int Position { get; set; }

    public Character(Controller controller, string name, int maxHitPoints, int attackBonus, int armorClass, Die weaponDie, int dexterity)
    {
        this.controller = controller;
        this.name = name;
        this.maxHitPoints = maxHitPoints;
        this.attackBonus = attackBonus;
        this.armorClass = armorClass;
        this.weaponDie = weaponDie;
        this.Dexterity = dexterity;
        Reset();
    }

    public Controller Controller => controller;
    public string Name => name;
    public double HealthRatio => (double)hitPoints / maxHitPoints;
    public bool Alive => hitPoints > 0;

    public void Reset()
    {
        hitPoints = maxHitPoints;
        Position = 0;
    }

    public void Attack(Log log, Character target, Die attackDie, string weapon)
    {
        if (weapon == "sword")
        {
            Position = Math.Max(0, Position - 6);
            log.CharacterMoves(this, "Sword", Position);
        }
        else if (weapon == "bow")
        {
            Position += 6;
            log.CharacterMoves(this, "Bow", Position);
        }

        int distance = Math.Abs(Position - target.Position);
        bool halfDamage = false;

        if (weapon == "sword" && distance > 6)
        {
            log.OutOfRange(this, target, distance, "Sword");
            return;
        }
        if (weapon == "bow" && distance > 36)
        {
            log.OutOfRange(this, target, distance, "Bow");
            return;
        }
        if (weapon == "bow" && distance > 18)
        {
            halfDamage = true;
        }

        int attackRoll = attackDie.Roll() + attackBonus;
        log.CharacterAttack(this, target, attackRoll);
        target.Hit(log, this, attackRoll, halfDamage);
    }

    public void Hit(Log log, Character source, int attackRoll, bool halfDamage)
    {
        if (attackRoll < armorClass)
        {
            log.AttackMiss(source, this);
            return;
        }

        int damageRoll = source.RollDamage();
        if (halfDamage)
        {
            damageRoll = Math.Max(1, damageRoll / 2);
            log.HalfDamageLog();
        }

        log.AttackHit(source, this, damageRoll);
        hitPoints -= damageRoll;
    }

    public void Wait(Log log, Die waitDie)
    {
        log.CharacterWait(this, waitDie.Roll());
    }

    public int RollDamage()
    {
        return weaponDie.Roll() + attackBonus;
    }
}