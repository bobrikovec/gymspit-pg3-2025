//Napiš program, který se zeptá na barvu, vlastnost, zvíře, činnost a vypíše dohromady větu
using System;
Console.WriteLine("Zadej barvu:");
string color = Console.ReadLine();
Console.WriteLine("Zadej vlastnost:");
string adjective = Console.ReadLine();
Console.WriteLine("Zadej zvíře:");
string animal = Console.ReadLine();
Console.WriteLine("Zadej činnost:");
string activity = Console.ReadLine();
Console.WriteLine($"Bylo nebylo, v jedné {color} zemi žil {adjective} {animal}, který rád {activity}.");
