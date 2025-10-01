
using System.ComponentModel.Design;

Console.WriteLine("Napiš číslo: ");
double num1 = double.Parse(Console.ReadLine());     //tady jsem si nechal poradit, že to musí být s double.Parse, protože Console.ReadLine() vrací string
Console.WriteLine("Napiš druhé číslo: ");
double num2 = double.Parse(Console.ReadLine());

string inputKey;                                      //dal jsem sem string místo char, aby to šlo porovnat s 'konec' (všude tedy bude "" místo '')
while (true) {                                         //tady mě zachránil Adam Svoboda, že to může být while(true) a break na konci
    Console.Write("Jakou chceš provést operaci? Zadej znak nebo 'konec' pro ukončení: ");
    inputKey = Console.ReadLine();                    //teď jelikož je to ve stringu a ne v char, nemusí to být .KeyChar
    Console.WriteLine();                             //prý to bude přehlednější

    if (inputKey == "konec")                  //chat mi poradil, aby to bylo na začátku cyklu, aby se to hned ukončilo (taky že else if nemůže být za else)
    {
        Console.WriteLine("Konec programu.");
        break;
    }
    else if (inputKey == "+") 
        Console.WriteLine($"Výsledek: {num1} + {num2} = {num1 + num2}");
    else if (inputKey == "-") 
        Console.WriteLine($"Výsledek: {num1} - {num2} = {num1 - num2}");
    else if (inputKey == "*") 
        Console.WriteLine($"Výsledek: {num1} * {num2} = {num1 * num2}");
    else if (inputKey == "/") {
        if (num2 != 0)
            Console.WriteLine($"Výsledek: {num1} / {num2} = {num1 / num2}");        
        else
            Console.WriteLine("Chyba: Dělení nulou není povoleno.");
     
    } else 
        Console.WriteLine("Neplatná operace. Zadej +, -, * nebo /.");
   
}
