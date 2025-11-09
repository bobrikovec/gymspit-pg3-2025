using System;       //nechal jsem si poradit od Chata a pak jsem mu říkal řádek po řádku co dělá a on mě doplňoval, komentáře píšu jako vysvětlení k funkcím, jak jsem je pochopil 

namespace KalkulackaFunkce
{
    class Program
    {
        static void Main(string[] args)     //static používáme protože main metoda je statická a nepatří k žádnému objektu //string[] args je pole řetězců které může obsahovat argumenty příkazové řádky ale my je nepoužíváme
        {
            Console.WriteLine("=== Konzolová kalkulačka ===");

            while (true)        //cyklus který běží dokud ho nepřerušíme breakem
            {
                PrintMenu();
                char operation = ReadOperation();       //stanovujeme proměnnou operation která bude obsahovat znak operace kterou uživatel zadá

                if (operation == 'k')  // konec
                {
                    Console.WriteLine("Program ukončen.");
                    break;      //přerušíme cyklus a tím i program
                }

                Console.WriteLine("Zadej první číslo:");
                double a = ReadDouble();        //desetinné proměnná a

                Console.WriteLine("Zadej druhé číslo:");
                bool mustBeNonZero = operation == '/'; // pokud dělíme, nesmí být nula
                double b = ReadDouble(mustBeNonZero); // desetinné proměnná b u které je bool (true/false) podle toho jestli dělíme nebo ne 

                double result = Compute(operation, a, b); //výpočet výsledku podle zadané operace a čísel a a b
                PrintResult(operation, a, b, result);       //musíme proměnné zapisovat v pořadí, jak jsme je nadefinovali nad tím
            }
        }

        static void PrintMenu()     //vyhodí menu s možnostmi operací //void zadáváme když si metoda nic nevrací
        {
            Console.WriteLine("\nVyber operaci:");
            Console.WriteLine("+ : sčítání");
            Console.WriteLine("- : odčítání");
            Console.WriteLine("* : násobení");
            Console.WriteLine("/ : dělení");
            Console.WriteLine("k : konec");
        }

        static char ReadOperation()
        {
            while (true)
            {
                Console.Write("Zadej operaci: ");
                string input = Console.ReadLine()?.Trim().ToLower();        //textový input si program přečte z konzole, odstraní bílé znaky na začátku a konci (Trim) a převede na malá písmena (ToLower)

                if (input == "+" || input == "-" || input == "*" || input == "/" || input == "k")   //cyklicky kontrolujeme jestli je zadaná operace platná
                {
                    return input[0];
                }

                Console.WriteLine("Neplatná operace, zkus to znovu.");      //není platná (nebyl zadán žádný z povolených znaků)
            }
        }

        static double ReadDouble(bool nonZero = false)      //metoda pro čtení desetinného čísla, s volitelným nonZero který určuje jestli číslo nesmí být nula //stačí to jednou, i když máme dvě čísla - u prvního je jen ReadDouble() bez parametru a u druhého je ReadDouble(true)
        {
            while (true)
            {
                Console.Write("Zadej číslo: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double value))       //pokud se podaří převést vstup na double, uloží ho do proměnné value
                {
                    if (nonZero && value == 0)      //výrok, který kontroluje jestli nonZero je true a zároveň jestli je hodnota zadaného čísla nula
                    {
                        Console.WriteLine("Číslo nesmí být nula (dělení nulou není povoleno).");
                        continue;           //pokračuje na začátek cyklu
                    }

                    return value;
                }

                Console.WriteLine("Neplatný vstup, zkus to znovu.");
            }
        }

        static double Compute(char operation, double operand1, double operand2)     //funkce Compute která provádí výpočet na základě zadané operace a dvou operandů (číslo se kterým operujeme)
        {
            switch (operation)      //přepínač který kontroluje hodnotu operation
            {
                case '+': return operand1 + operand2;       //"pokud je operace +, vrať součet operand1 a operand2" atd...
                case '-': return operand1 - operand2;
                case '*': return operand1 * operand2;
                case '/': return operand1 / operand2;
                default:        //pokud operace není žádná z výše uvedených
                    Console.WriteLine("Neznámá operace.");
                    return double.NaN;      //vrací "Not a Number" (není číslo)
            }
        }

        static void PrintResult(char operation, double operand1, double operand2, double result)        //PrintResult vypíše kompletní výsledek operace v desetinných číslech a zapíše to jako celý příklad, ne jen výsledek
        {
            Console.WriteLine($"\nVýsledek: {operand1} {operation} {operand2} = {result}\n");       //$ umožňuje vkládání proměnných přímo do řetězce, n dělá nový řádek, tady už to píšeme ve správněm pořadí, jak to chceme přečíst
        }
    }
}

