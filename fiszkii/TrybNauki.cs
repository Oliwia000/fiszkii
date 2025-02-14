using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiszki
{
    public static class TrybNauki
    {
        public static void StartTraining()
        {
            var fiszki = ZarzadzanieFiszkami.Fiszki;
            if (fiszki.Count == 0)
            {
                Console.WriteLine("Brak fiszek.");
                return;
            }

            Console.WriteLine("Wybierz kierunek nauki:");
            Console.WriteLine("1. PL -> ENG");
            Console.WriteLine("2. ENG -> PL");
            int wybor = int.Parse(Console.ReadLine());
            bool fromLang1 = wybor == 1;

            Console.WriteLine("\nWybierz tryb:");
            Console.WriteLine("1. Łatwy (podpowiedzi)");
            Console.WriteLine("2. Trudny (wszystkie odpowiedzi muszą być poprawne)");
            bool isEasy = Console.ReadLine() == "1";

            var fiszkiDoNauki = fiszki.Where(f => f.Poziom.ToLower() == (isEasy ? "łatwy" : "trudny")).ToList();
            if (fiszkiDoNauki.Count == 0)
            {
                Console.WriteLine("Brak fiszek na wybranym poziomie trudności.");
                return;
            }

            Console.WriteLine("\nIle fiszek chcesz przećwiczyć?");
            int liczbaFiszek = int.Parse(Console.ReadLine());
            if (liczbaFiszek < 1 || liczbaFiszek > fiszkiDoNauki.Count)
            {
                Console.WriteLine("Nieprawidłowa liczba fiszek.");
                return;
            }

            var random = new Random();
            var wybraneFiszki = fiszkiDoNauki.OrderBy(x => random.Next()).Take(liczbaFiszek).ToList();
            int score = 0;

            foreach (var fiszka in wybraneFiszki)
            {
                Console.Clear();
                string pytanie = fromLang1 ? fiszka.Slowo : fiszka.Tlumaczenia[0]; // Wybór odpowiedniego pytania
                Console.WriteLine($"Przetłumacz: {pytanie}");

                List<string> poprawneOdpowiedzi = fromLang1 ? fiszka.Tlumaczenia : new List<string> { fiszka.Slowo };

                if (isEasy)
                {
                    Console.WriteLine($"Podpowiedź: {poprawneOdpowiedzi[0][0]}{new string('_', poprawneOdpowiedzi[0].Length - 1)}");
                }

                Console.Write("Twoja odpowiedź: ");
                string odpowiedz = Console.ReadLine().Trim().ToLower();

                if (poprawneOdpowiedzi.Any(o => o.ToLower() == odpowiedz))
                {
                    Console.WriteLine("✅ Dobrze!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"❌ Błąd! Poprawna odpowiedź: {string.Join(", ", poprawneOdpowiedzi)}");
                }

                Console.WriteLine("Naciśnij Enter, aby przejść dalej...");
                Console.ReadLine();
            }

            double procent = (double)score / liczbaFiszek * 100;
            Console.Clear();
            Console.WriteLine($"Twój wynik: {score}/{liczbaFiszek} ({procent:F0}%)");
        }
    }
}
