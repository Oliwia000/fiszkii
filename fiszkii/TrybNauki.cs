using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiszki
{
    public static class TrybNauki
    {
        public static void StartTraining()
        {
            if (ZarzadzanieFiszkami.Fiszki.Count == 0)
            {
                Console.WriteLine("Brak fiszek do nauki. Najpierw dodaj kilka fiszek.");
                Console.ReadLine();
                return;
            }
            Console.Clear();
            Console.WriteLine("==== Tryb nauki ====");

            Console.WriteLine("Wybierz kierunek nauki:");
            Console.WriteLine("1. " + ZarzadzanieFiszkami.Lang1Name + " -> " + ZarzadzanieFiszkami.Lang2Name);
            Console.WriteLine("2. " + ZarzadzanieFiszkami.Lang2Name + " -> " + ZarzadzanieFiszkami.Lang1Name);
            Console.Write("Wybór (1 lub 2): ");
            string kierunekInput = Console.ReadLine();
            string kierunek = (kierunekInput == "1")
                ? ZarzadzanieFiszkami.Lang1Name + " -> " + ZarzadzanieFiszkami.Lang2Name
                : ZarzadzanieFiszkami.Lang2Name + " -> " + ZarzadzanieFiszkami.Lang1Name;
           
            Console.WriteLine("\nWybierz tryb trudności:");   // Wybór poziomu trudności
            Console.WriteLine("1. Łatwy (jedna próba, podpowiedź po nieudanej próbie)");
            Console.WriteLine("2. Trudny (wymagane podanie wszystkich tłumaczeń)");
            Console.Write("Wybór (1 lub 2): ");
            string trybInput = Console.ReadLine();
            bool isEasy = (trybInput == "1");

            List<Fiszka> filteredCards = ZarzadzanieFiszkami.Fiszki // Pobieranie listy fiszek tylko dla wybranego poziomu trudności
                .Where(f => string.Equals(f.Poziom.Trim(), isEasy ? "łatwy" : "trudny", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filteredCards.Count == 0)
            {
                Console.WriteLine("Brak fiszek o poziomie " + (isEasy ? "łatwy" : "trudny") + ".");
                Console.WriteLine("Naciśnij Enter, aby powrócić do menu...");
                Console.ReadLine();
                return;
            }
            int total = filteredCards.Count; // Wybór liczby fiszek do przećwiczenia
            int count;
            do
            {
                Console.Write("\nIle fiszek chcesz przećwiczyć? (1 - " + total + "): ");
            } while (!int.TryParse(Console.ReadLine(), out count) || count < 1 || count > total);

            Random rnd = new Random();  // Losowe tasowanie tylko z wybranego poziomu trudności
            List<Fiszka> trainingCards = filteredCards.OrderBy(x => rnd.Next()).Take(count).ToList();

            int score = 0;
            foreach (var card in trainingCards)
            {
                Console.Clear();
                Console.WriteLine("Przetłumacz: " + card.PobierzPytanie(kierunek));
                if (isEasy)
                {
                       // Tryb łatwy – jedna próba + podpowiedź
                    Console.WriteLine("Podpowiedź: " + card.PobierzPodpowiedz(kierunek));
                    Console.Write("Podaj tłumaczenie: ");
                    string answer = Console.ReadLine().Trim().ToLower();

                    if (card.PobierzOdpowiedzi(kierunek).Contains(answer))
                    {
                        Console.WriteLine("Dobrze!");
                        score++;
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawnie. Poprawne odpowiedzi: " +
                            string.Join(", ", card.PobierzOdpowiedzi(kierunek)));
                        Console.Write("Spróbuj ponownie: ");
                        string secondAttempt = Console.ReadLine().Trim().ToLower();
                        if (card.PobierzOdpowiedzi(kierunek).Contains(secondAttempt))
                        {
                            Console.WriteLine("Dobrze!");
                            score++;
                        }
                        else
                        {
                            Console.WriteLine("Źle! Poprawne odpowiedzi: " +
                                string.Join(", ", card.PobierzOdpowiedzi(kierunek)));
                        }
                    } }
                else
                {
                    // Tryb trudny – użytkownik musi podać wszystkie tłumaczenia
                    Console.WriteLine("Podaj wszystkie tłumaczenia, oddzielone przecinkami:");
                    Console.Write("Tłumaczenia dla '" + card.PobierzPytanie(kierunek) + "': ");
                    string input = Console.ReadLine();
                    var userAnswers = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(s => s.Trim().ToLower()).ToList();
                    var correctAnswers = card.PobierzOdpowiedzi(kierunek);

                    if (new HashSet<string>(userAnswers).SetEquals(correctAnswers))
                    {
                        Console.WriteLine("Dobrze!");
                        score++;
                    }
                    else
                    {
                        Console.WriteLine("Źle! Poprawne odpowiedzi: " + string.Join(", ", correctAnswers));
                    } }
                Console.WriteLine("\nNaciśnij Enter, aby przejść do następnej fiszki...");
                Console.ReadLine();
            }
            Console.Clear();
            double percentage = ((double)score / count) * 100;
            Console.WriteLine("==== KONIEC NAUKI ====");
            Console.WriteLine("Twój wynik: " + score + "/" + count + " (" + percentage.ToString("F0") + "%)");
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu...");
            Console.ReadLine();
        }
    }
}
