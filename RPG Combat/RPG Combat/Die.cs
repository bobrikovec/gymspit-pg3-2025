namespace Lecture18;

public class Die
{
    Random random;
    int sides;

    public Die(Random random, int sides)
    {
        this.random = random;
        this.sides = sides;
    }

    public int Roll() => random.Next(sides) + 1;
}