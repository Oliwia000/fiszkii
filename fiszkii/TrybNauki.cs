using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiszki
{
    public static class TrybNauki
    {
        public static void StartTraining()
        {
            if (!ZarzadzanieFiszkami.Fiszki.Any())
            {
                Console.WriteLine("Brak fiszek do nauki. Dodaj fiszki i spróbuj ponownie.");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("==== Tryb Nauki ====");

            // Wybór kierunku
            Console.WriteLine("Wybierz kierunek nauki:");
            Console.WriteLine("1. " + ZarzadzanieFiszkami.Lang1Name + " -> " + ZarzadzanieFiszkami.Lang2Name);
            Console.WriteLine("2. " + ZarzadzanieFiszkami.Lang2Name + " -> " + ZarzadzanieFiszkami.Lang1Name);
            Console.Write("Wybór (1 lub 2): ");
            bool fromLang1 = Console.ReadLine() == "1";

            // Wybór poziomu trudności
            Console.WriteLine("\nWybierz tryb:");
            Console.WriteLine("1. Łatwy (podpowiedzi)");
            Console.WriteLine("2. Trudny (wszystkie odpowiedzi muszą być poprawne)");
            Console.Write("Wybór (1 lub 2): ");
            bool isEasy = Console.ReadLine() == "1";

            // Pobieranie fiszek odpowiedniego poziomu
            List<Fiszka> fiszkiDoNauki = ZarzadzanieFiszkami.Fiszki
                .Where(f => f.Poziom.Trim().ToLower() == (isEasy ? "łatwy" : "trudny"))
                .ToList();

            if (!fiszkiDoNauki.Any())
            {
                Console.WriteLine($"Brak fiszek na poziomie {(isEasy ? "łatwym" : "trudnym")}.");
                Console.ReadLine();
                return;
            }

            // Wybór liczby fiszek
            Console.Write($"\nIle fiszek chcesz przećwiczyć? (1-{fiszkiDoNauki.Count}): ");
            int liczbaFiszek = PobierzLiczbe(1, fiszkiDoNauki.Count);

            // Losowe tasowanie fiszek
            Random rnd = new Random();
            List<Fiszka> wybraneFiszki = fiszkiDoNauki.OrderBy(x => rnd.Next()).Take(liczbaFiszek).ToList();

            int score = 0;
            foreach (var fiszka in wybraneFiszki)
            {
                Console.Clear();
                string pytanie = fromLang1 ? fiszka.Slowo : fiszka.Tlumaczenie;
                List<string> poprawneOdpowiedzi = (fromLang1 ? fiszka.Tlumaczenia : new List<string> { fiszka.Slowo })
                    .Select(s => s.ToLower().Trim()).ToList();

                Console.WriteLine($"Przetłumacz: {pytanie}");

                if (isEasy) // Tryb łatwy – podpowiedź
                {
                    Console.WriteLine($"Podpowiedź: {poprawneOdpowiedzi.First()[0]}{new string('_', poprawneOdpowiedzi.First().Length - 1)}");
                }

                Console.Write("Twoja odpowiedź: ");
                string odpowiedz = Console.ReadLine().Trim().ToLower();

                bool poprawna = isEasy 
                    ? poprawneOdpowiedzi.Contains(odpowiedz) 
                    : poprawneOdpowiedzi.All(odp => odpowiedz.Contains(odp));

                if (poprawna)
                {
                    Console.WriteLine("✅ Dobrze!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"❌ Błąd! Poprawne odpowiedzi: {string.Join(", ", poprawneOdpowiedzi)}");
                }

                Console.WriteLine("Naciśnij Enter, aby przejść dalej...");
                Console.ReadLine();
            }

            // Wynik końcowy
            Console.Clear();
            double procent = ((double)score / liczbaFiszek) * 100;
            Console.WriteLine("==== KONIEC NAUKI ====");
            Console.WriteLine($"Twój wynik: {score}/{liczbaFiszek} ({procent:F0}%)");
            Console.WriteLine("Naciśnij Enter, aby wrócić do menu...");
            Console.ReadLine();
        }

        private static int PobierzLiczbe(int min, int max)
        {
            int liczba;
            while (!int.TryParse(Console.ReadLine(), out liczba) || liczba < min || liczba > max)
            {
                Console.Write($"Wpisz liczbę od {min} do {max}: ");
            }
            return liczba;
        }
    }
}

}
