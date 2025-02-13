using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fiszkii
{
    public static class ZarzadzanieFiszkami
    {
        public static List<Flashcard> Fiszki = new List<Flashcard>();

        private static string sciezkaPliku = "fiszki.txt"; 
        public static void WczytajFiszki() // Wczytuje fiszki z pliku
        {
            if (File.Exists(sciezkaPliku))
            {
                string[] linie = File.ReadAllLines(sciezkaPliku);
                foreach (string linia in linie)
                {
                    if (string.IsNullOrWhiteSpace(linia))
                        continue;

                    string[] czesci = linia.Split('|');
                    if (czesci.Length >= 4)
                    {
                        string slowo = czesci[0];
                        string opis = czesci[1];
                        string poziom = czesci[2];
                        List<string> tlumaczenia = czesci[3]
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList();

                        Fiszki.Add(new Flashcard
                        {
                            Word = slowo,
                            Description = opis,
                            Difficulty = poziom,
                            Translations = tlumaczenia
                        });
                    }
                }
            }
        }
        public static void WyswietlFiszki()
        {
            Console.Clear();
            Console.WriteLine("Lista fiszek:");
            foreach (var fiszka in Fiszki)
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine($"Słowo: {fiszka.Word}");
                Console.WriteLine($"Opis: {fiszka.Description}");
                Console.WriteLine($"Poziom trudności: {fiszka.Difficulty}");
                Console.WriteLine($"Tłumaczenia: {string.Join(", ", fiszka.Translations)}");
                Console.WriteLine();
            }
            Console.WriteLine("Naciśnij Enter, aby wrócić do menu...");
            Console.ReadLine();
        }
        public static void DodajFiszke() // Dodaje nową fiszkę i zapisuje ją do pliku
        {
            Console.Clear();
            Console.Write("Podaj słowo: ");
            string slowo = Console.ReadLine();

            Console.Write("Podaj opis fiszki: ");
            string opis = Console.ReadLine();

            Console.Write("Podaj poziom trudności (1 - łatwy, 2 - trudny): ");
            string wybor = Console.ReadLine();
            string poziom = "";
            if (wybor == "1")
                poziom = "łatwy";
            else if (wybor == "2")
                poziom = "trudny";
            else
            {
                poziom = "łatwy";
                Console.WriteLine("Nieprawidłowy wybór, ustawiono poziom 'łatwy'.");
            }

            Console.Write("Podaj tłumaczenia (oddzielone przecinkami): ");
            string tlumaczeniaInput = Console.ReadLine();
            List<string> tlumaczenia = tlumaczeniaInput
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

            Flashcard nowaFiszka = new Flashcard
            {
                Word = slowo,
                Description = opis,
                Difficulty = poziom,
                Translations = tlumaczenia
            };

            Fiszki.Add(nowaFiszka);

            string liniaDoPliku = $"{slowo}|{opis}|{poziom}|{string.Join(", ", tlumaczenia)}";
            File.AppendAllText(sciezkaPliku, liniaDoPliku + Environment.NewLine);

            Console.WriteLine("Fiszka dodana! Naciśnij Enter, aby kontynuować...");
            Console.ReadLine();
        }
    }
}
