using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fiszki
{
    class Program
    {
        static List<Fiszka> fiszki = new List<Fiszka>();
        static int punkty = 0;
        static string jezykPoczatkowy;
        static string tryb;
        const string PlikFiszki = "fiszki.txt";

        static void Main()
        {
            SprawdzPlik(); // Tworzy plik, jeśli nie istnieje
            WczytajFiszki();
            Console.WriteLine("Wybierz tryb: [PL -> ENG] lub [ENG -> PL]");
            jezykPoczatkowy = Console.ReadLine().ToUpper();
            Console.WriteLine("Wybierz tryb: łatwy (L) czy trudny (T)?");
            tryb = Console.ReadLine().ToUpper();
            Console.WriteLine("Ile fiszek chcesz przećwiczyć? (1-10)");
            int liczbaFIszek = int.Parse(Console.ReadLine());
            Nauka(liczbaFIszek);
            Console.WriteLine($"Twój wynik: {punkty}/{liczbaFIszek}");
        }

        static void SprawdzPlik()
        {
            if (!File.Exists(PlikFiszki))
            {
                File.WriteAllText(PlikFiszki, "PL | ENG\n"); // Pierwsza linia informacyjna
            }
        }

        static void WczytajFiszki()
        {
            string[] linie = File.ReadAllLines(PlikFiszki);
            foreach (var linia in linie.Skip(1))
            {
                var dane = linia.Split('|');
                fiszki.Add(new Fiszka(dane[0], dane[1], dane[2], dane[3].Split(',')));
            }
        }

        static void DodajFiszke(string polski, string angielski, string poziom, string tlumaczenia)
        {
            string nowaLinia = $"{polski}|{angielski}|{poziom}|{tlumaczenia}\n";
            File.AppendAllText(PlikFiszki, nowaLinia);
        }

        static void Nauka(int liczbaFIszek)
        {
            var losoweFiszki = fiszki.OrderBy(x => Guid.NewGuid()).Take(liczbaFIszek).ToList();
            foreach (var fiszka in losoweFiszki)
            {
                Console.WriteLine(jezykPoczatkowy == "PL -> ENG" ? fiszka.Polski : fiszka.Angielski);
                if (tryb == "L")
                {
                    Console.WriteLine("Podpowiedź: " + fiszka.Podpowiedz());
                }
                string odpowiedz = Console.ReadLine().ToLower();
                if (fiszka.CzyPoprawnaOdpowiedz(odpowiedz, jezykPoczatkowy))
                {
                    Console.WriteLine("Dobrze!");
                    punkty++;
                }
                else
                {
                    Console.WriteLine($"Źle! Poprawne odpowiedzi: {string.Join(", ", fiszka.Tlumaczenia)}");
                }
            }
        }
    } 

    class Fiszka
    {
        public string Polski { get; }
        public string Angielski { get; }
        public string PoziomTrudnosci { get; }
        public string[] Tlumaczenia { get; }

        public Fiszka(string polski, string angielski, string poziom, string[] tlumaczenia)
        {
            Polski = polski;
            Angielski = angielski;
            PoziomTrudnosci = poziom;
            Tlumaczenia = tlumaczenia;
        }

        public bool CzyPoprawnaOdpowiedz(string odp, string jezyk)
        {
            return jezyk == "PL -> ENG" ? Tlumaczenia.Contains(odp) : Polski.Equals(odp, StringComparison.OrdinalIgnoreCase);
        }

        public string Podpowiedz()
        {
            string slowo = Polski.Length > Angielski.Length ? Polski : Angielski;
            char[] podpowiedz = slowo.ToCharArray();
            for (int i = 1; i < podpowiedz.Length - 1; i++)
            {
                podpowiedz[i] = '_';
            }
            return new string(podpowiedz);
        }
    }
}


