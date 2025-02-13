using System;
using System.Collections.Generic;
using System.Linq;

namespace fiszkii
{
    public static class TrybNauki
    {
        public static void RozpocznijNauke()
        {
            if (ZarzadzanieFiszkami.Fiszki.Count == 0)
            {
                Console.WriteLine("Brak fiszek do nauki. Dodaj fiszki najpierw.");
                Console.ReadLine();
                return;
            }
            Console.Clear();
            Console.WriteLine("Wybierz tryb nauki:");
            Console.WriteLine("1. Łatwy");
            Console.WriteLine("2. Trudny");
            string wyborTrybu = Console.ReadLine();

            // Losowe przetasowanie fiszek
            List<Flashcard> listaTreningowa = ZarzadzanieFiszkami.Fiszki.OrderBy(x => Guid.NewGuid()).ToList();
            int wynik = 0;

            foreach (var fiszka in listaTreningowa)
            {
                Console.Clear();
                Console.WriteLine($"Opis: {fiszka.Description}");

                if (wyborTrybu == "1")
                {
                    // Tryb łatwy: 2 próby z podpowiedzią po pierwszej nieudanej próbie
                    Console.Write($"Podaj tłumaczenie dla słowa '{fiszka.Word}': ");
                    string odpowiedz = Console.ReadLine();
                    if (SprawdzOdpowiedzLatwa(odpowiedz, fiszka.Translations))
                    {
                        Console.WriteLine("Dobrze!");
                        wynik++;
                    }
                    else
                    { 
                        string podpowiedz = string.Join(", ", fiszka.Translations.Select(t => !string.IsNullOrEmpty(t) ? t[0].ToString() : "")); // Podpowiedź: pierwsze litery tłumaczeń
                        Console.WriteLine($"Niepoprawnie. Podpowiedź - pierwsze litery tłumaczeń: {podpowiedz}");
                        Console.Write("Spróbuj ponownie: ");
                        string drugaOdpowiedz = Console.ReadLine();
                        if (SprawdzOdpowiedzLatwa(drugaOdpowiedz, fiszka.Translations))
                        {
                            Console.WriteLine("Dobrze!");
                            wynik++;
                        }
                        else
                        {
                            Console.WriteLine($"Źle! Poprawne odpowiedzi: {string.Join(", ", fiszka.Translations)}");
                        }
                    }
                }
                else if (wyborTrybu == "2")
                {
                    // Tryb trudny: użytkownik musi podać wszystkie tłumaczenia (kolejność nie ma znaczenia)
                    Console.Write($"Podaj tłumaczenia dla słowa '{fiszka.Word}' (oddzielone przecinkami): ");
                    string odpowiedz = Console.ReadLine();
                    List<string> odpowiedziUzytkownika = odpowiedz
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim().ToLower())
                        .ToList();

                    HashSet<string> poprawneOdpowiedzi = new HashSet<string>(fiszka.Translations.Select(t => t.ToLower()));
                    HashSet<string> odpowiedziUzytk = new HashSet<string>(odpowiedziUzytkownika);

                    if (poprawneOdpowiedzi.SetEquals(odpowiedziUzytk))
                    {
                        Console.WriteLine("Dobrze!");
                        wynik++;
                    }
                    else
                    {
                        Console.WriteLine($"Źle! Poprawne odpowiedzi: {string.Join(", ", fiszka.Translations)}");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór trybu.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Naciśnij Enter, aby przejść do następnej fiszki...");
                Console.ReadLine();
            }

            Console.WriteLine($"Koniec nauki! Twój wynik: {wynik}/{listaTreningowa.Count}");
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu...");
            Console.ReadLine();
        }

        // Sprawdza odpowiedź w trybie łatwym (ignorując wielkość liter)
        private static bool SprawdzOdpowiedzLatwa(string odpowiedz, List<string> poprawneTlumaczenia)
        {
            string odp = odpowiedz.Trim().ToLower();
            foreach (var tlumaczenie in poprawneTlumaczenia)
            {
                if (odp == tlumaczenie.ToLower())
                    return true;
            }
            return false;
        }
    }
}
