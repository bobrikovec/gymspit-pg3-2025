using System;
using System.Collections.Generic;
using System.IO;

namespace Penezenka
{
    class Polozka
    {
        public int Castka { get; set; }
        public string Nazev { get; set; }
        public string Kategorie { get; set; }
    }

    internal class Program
    {
        static string cestaKSouboru = "data_penezenky.txt";

        //Pole s pevnou velikostí a počítadlo
        const int MAX_VELIKOST = 100;
        static Polozka[] pole = new Polozka[MAX_VELIKOST];
        static int pocetPolozek = 0; // Kolik máme reálně obsazeno, začínáme defaultně na nule

        // Hlavní metoda peněženky
        static void Main(string[] args)
        {
            NacistZeSouboru();

            bool bezi = true;
            while (bezi)
            {
                Console.Clear();
                Console.WriteLine($"=== PENĚŽENKA 2025 (Obsazeno {pocetPolozek}/{MAX_VELIKOST}) ===");
                Console.WriteLine("1. Vypsat záznamy");
                Console.WriteLine("2. Vypsat s filtrem");
                Console.WriteLine("3. Přidat záznam");
                Console.WriteLine("4. Upravit záznam");
                Console.WriteLine("5. Smazat záznam");
                Console.WriteLine("6. Uložit a konec");
                Console.Write("Vyber číslo akce: ");

                char volba = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (volba)
                {
                    case '1':
                        VypsatZaznamy("");
                        break;
                    case '2':
                        Console.Write("Zadej text pro hledání: ");
                        string filtr = Console.ReadLine();
                        VypsatZaznamy(filtr);
                        break;
                    case '3':
                        PridatZaznam();
                        break;
                    case '4':
                        UpravitZaznam();
                        break;
                    case '5':
                        SmazatZaznam();
                        break;
                    case '6':
                        UlozitDoSouboru();
                        bezi = false;
                        break;
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }

                if (bezi)
                {
                    Console.WriteLine("\nStiskni libovolnou klávesu pro návrat do menu...");
                    Console.ReadKey();
                }
            }
        }

        static void VypsatZaznamy(string filtr)
        {
            Console.WriteLine("\n--- VÝPIS POLOŽEK ---");
            Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,10} | {4,10}", "ID", "Název", "Kategorie", "Částka", "Zůstatek");
            // Formátování tabulky:
            // {0,-5} znamená: na 0. pozici dej ID, vyhraď mu 5 znaků a zarovnej doleva (mínus)
            // {3,10} znamená: na 3. pozici dej Částku, vyhraď 10 znaků a zarovnej doprava (kladné číslo)
            Console.WriteLine(new string('-', 75));     //Grafická úprava - vykreslí 75 pomlček

            int aktualniZustatek = 0;
            int soucetPrijmu = 0, pocetPrijmu = 0;
            int soucetVydaju = 0, pocetVydaju = 0;
            int maxPrijem = 0, minPrijem = int.MaxValue;    //aby hned první příjem byl zároveň i minimem (nebude menší než 0)
            int maxVydaj = int.MinValue, minVydaj = 0;

            //Slovník na kategorie, při práci s poli je lepší používat slovníky (Dictionary), protože umožňují rychlejší přístup k datům podle klíče (v našem případě kategorie)
            Dictionary<string, int> souctyKategorii = new Dictionary<string, int>();

            //Iterujeme jen do pocetPolozek, ne přes celé pole (zbytek je null)
            for (int i = 0; i < pocetPolozek; i++) //prochází se celé pole od indexu 0 do posledního obsazeného, v každém dalším kroku se i zvětší o 1
            {
                Polozka p = pole[i];        //získáme aktuální položku z pole s indexem i, na začátku kódu je k Položce řazena částka, název a kategorie
                aktualniZustatek += p.Castka;

                if (p.Castka > 0)   //kladná částka se kategorizuje jako příjem
                {
                    soucetPrijmu += p.Castka;
                    pocetPrijmu++;
                    if (p.Castka > maxPrijem) maxPrijem = p.Castka; //pokud částka přesahuje dosavadní maximum, je nové maximum právě tato částka
                    if (p.Castka < minPrijem) minPrijem = p.Castka; //naopak
                }
                else if (p.Castka < 0) //stejný princip pro výdaje (záporné částky)
                {
                    soucetVydaju += p.Castka;
                    pocetVydaju++;
                    if (p.Castka < minVydaj) minVydaj = p.Castka;
                    if (p.Castka > maxVydaj) maxVydaj = p.Castka;
                }

                if (souctyKategorii.ContainsKey(p.Kategorie))   //Je to jako šanon – když už existuje, přičte částku ke stávající, když ne, založím ho
                    souctyKategorii[p.Kategorie] += p.Castka;   //ConatinsKey zkontroluje, jestli už v slovníku existuje klíč se zadaným názvem kategorie
                else
                    souctyKategorii[p.Kategorie] = p.Castka;

                //uživatel může zadat filtr, podle kterého se budou zobrazovat jen položky obsahující tento text v názvu, pokud je filtr prázdný nebo null, zobrazí se všechny položky (výrok NEBO - stačí aby jedna podmínka byla pravdivá)
                bool zobrazit = string.IsNullOrEmpty(filtr) || p.Nazev.ToLower().Contains(filtr.ToLower());

                if (zobrazit)
                {
                    Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,10} | {4,10}",
                        i + 1,
                        //zkrácení zápisu if else pomocí operátoru ?: (ternární operátor), pokud je délka názvu větší (?) než 20 (definovaná šířka sloupce), vypíšeme prvních 17 znaků + "...", jinak (:) celý název
                        p.Nazev.Length > 20 ? p.Nazev.Substring(0, 17) + "..." : p.Nazev,
                        p.Kategorie,
                        p.Castka,
                        aktualniZustatek);
                }
            }

            Console.WriteLine(new string('-', 75));
            // Ošetření, abychom nevypisovali min/max hodnoty, když žádné nejsou
            if (pocetPrijmu > 0)
                Console.WriteLine($"Příjmy: {pocetPrijmu} ks, Celkem: {soucetPrijmu}, Max: {maxPrijem}, Min: {minPrijem}"); //$ značí, že jde o chytrý řetězec s interpolací, kde můžeme přímo vkládat proměnné do textu
            else
                Console.WriteLine("Příjmy: 0 ks");

            if (pocetVydaju > 0)
                Console.WriteLine($"Výdaje: {pocetVydaju} ks, Celkem: {soucetVydaju}, Max: {maxVydaj}, Min: {minVydaj}");
            else
                Console.WriteLine("Výdaje: 0 ks");

            Console.WriteLine("\nSoučty dle kategorií:");
            foreach (var kvp in souctyKategorii) //procházíme všechny položky ve slovníku, kde každá položka je pár klíč-hodnota (KeyValuePair), kvp.Key je kategorie, kvp.Value je součet částek v této kategorii
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        static void PridatZaznam()
        {
            Console.WriteLine("\n--- PŘIDAT ZÁZNAM ---");

            //Kontrola kapacity pole, podobný princip jako u Twitteru, kde se kontroluje, jestli uživatel může přidat nový post
            if (pocetPolozek >= MAX_VELIKOST)
            {
                Console.WriteLine("Chyba: Peněženka je plná!");
                return;
            }

            int castka = NacistCislo("Zadej částku (kladná=příjem, záporná=výdaj): ");
            Console.Write("Zadej název: ");
            string nazev = Console.ReadLine();
            Console.Write("Zadej kategorii: ");
            string kategorie = Console.ReadLine();

            //Přidání do pole na první volné místo
            pole[pocetPolozek] = new Polozka { Castka = castka, Nazev = nazev, Kategorie = kategorie };
            pocetPolozek++; // Zvýšíme počítadlo

            Console.WriteLine("Záznam přidán.");
        }

        static void UpravitZaznam()
        {
            Console.WriteLine("\n--- UPRAVIT ZÁZNAM ---");
            int id = NacistCislo("Zadej ID řádku k úpravě: ");

            //Kontrola proti pocetPolozek, podobně jako bylo u Twitteru, kde se kontroluje, jestli post s daným indexem existuje
            if (id < 1 || id > pocetPolozek)
            {
                Console.WriteLine("Chyba: Záznam s tímto ID neexistuje.");
                return;
            }

            int index = id - 1;
            Console.WriteLine($"Upravuješ: {pole[index].Nazev} ({pole[index].Castka})");

            pole[index].Castka = NacistCislo("Zadej novou částku: ");
            Console.Write("Zadej nový název: ");
            pole[index].Nazev = Console.ReadLine();
            Console.Write("Zadej novou kategorii: ");
            pole[index].Kategorie = Console.ReadLine();

            Console.WriteLine("Záznam byl upraven.");
        }

        static void SmazatZaznam()
        {
            Console.WriteLine("\n--- SMAZAT ZÁZNAM ---");
            int id = NacistCislo("Zadej ID řádku ke smazání: ");

            if (id < 1 || id > pocetPolozek)        //kontrola platnosti zadaného ID v poli/rozsahu
            {
                Console.WriteLine("Chyba: Mimo rozsah.");
                return;
            }

            int indexKeSmazani = id - 1; //převedení ID na index pole (ID začíná od 1, pole se čísluje od 0)

            // Vezmeme prvek za smazaným a dáme ho na jeho místo, a takhle až do konce
            for (int i = indexKeSmazani; i < pocetPolozek - 1; i++)     //v cyklu kontrolujeme proměnnou i jako indexKeSmazani, po každém cyklu se i zvětší o 1, max na předposlední prvek
            {
                pole[i] = pole[i + 1];      //mazanou položku přepíšeme položkou, která je vpravo od ní
            }

            pole[pocetPolozek - 1] = null;      // Poslední prvek (teď už duplicitní) vymažeme a snížíme počet o 1
            pocetPolozek--;

            Console.WriteLine("Záznam smazán.");
        }

        static int NacistCislo(string vyzva)    //definujeme metodu pro bezpečné načtení čísla s opakováním při neplatném vstupu, vyzva je text, který se zobrazí uživateli podle volané funkce (např. PridatZaznam)
        {
            int vysledek;
            while (true) //cyklus vlastně skončí jakmile uživatel zadá platné číslo
            {
                Console.Write(vyzva);
                //int.TryParse se pokusí převést vstup na číslo, pokud se to povede, vrátí true a číslo je v proměnné vysledek (out)
                if (int.TryParse(Console.ReadLine(), out vysledek)) //ReadLine čte odpově´d uživatele na výzvu 
                {
                    return vysledek;        //konec cyklu
                }
                Console.WriteLine("To není platné číslo, zkus to znovu.");
            }
        }

        static void UlozitDoSouboru()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(cestaKSouboru)) //pokud soubor neexistuje, vytvoří se nový
                {
                    //Ukládáme jen platné položky
                    for (int i = 0; i < pocetPolozek; i++)
                    {
                        Polozka p = pole[i];
                        sw.WriteLine($"{p.Castka}|{p.Nazev}|{p.Kategorie}");
                    }
                }
                Console.WriteLine("Uloženo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při ukládání: {ex.Message}");
            }
        }

        static void NacistZeSouboru()
        {
            if (!File.Exists(cestaKSouboru)) return;

            try
            {
                using (StreamReader sr = new StreamReader(cestaKSouboru))
                {
                    string radek;
                    // Resetujeme pole před načtením
                    pocetPolozek = 0;

                    while ((radek = sr.ReadLine()) != null && pocetPolozek < MAX_VELIKOST)
                    {
                        string[] casti = radek.Split('|');
                        if (casti.Length >= 3)
                        {
                            Polozka p = new Polozka();
                            int c;
                            if (int.TryParse(casti[0], out c)) p.Castka = c;
                            p.Nazev = casti[1];
                            p.Kategorie = casti[2];

                            // Plnění pole
                            pole[pocetPolozek] = p;
                            pocetPolozek++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při načítání: {ex.Message}");
            }
        }
    }
}