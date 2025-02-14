using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fiszki
{
    public static class ZarzadzanieFiszkami
    {
        public static List<Fiszka> Fiszki = new List<Fiszka>();
        // Nazwy języków – pobierane z pierwszej linii pliku
        public static string Lang1Name = "PL";
        public static string Lang2Name = "ENG";
        private static string filePath = "fiszki.txt";

        public static void LoadFlashcards()
        {
            Fiszki.Clear();
            if (!File.Exists(filePath))
            {
                // Jeśli plik nie istnieje, tworzymy go z nagłówkiem oraz przykładowymi fiszkami
                List<string> linesToWrite = new List<string>();
                linesToWrite.Add("PL - ENG"); // nagłówek

                // Przykładowa fiszka 1:
                linesToWrite.Add("jabłko");                         // słowo
                linesToWrite.Add("Owoc czerwony, słodki");           // opis
                linesToWrite.Add("łatwy");                           // poziom
                linesToWrite.Add("apple");                           // tłumaczenie

                // Przykładowa fiszka 2:
                linesToWrite.Add("samochód");
                linesToWrite.Add("Pojazd czterokołowy");
                linesToWrite.Add("trudny");
                linesToWrite.Add("car, automobile");

                // Przykładowa fiszka 3:
                linesToWrite.Add("Czyny mówią głośniej niż słowa");
                linesToWrite.Add("Przysłowie motywacyjne");
                linesToWrite.Add("łatwy");
                linesToWrite.Add("Actions speak louder than words");

                File.WriteAllLines(filePath, linesToWrite);
            }
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                // Pierwsza linia to nagłówek z informacją o językach, np. "PL - ENG"
                string[] langs = lines[0].Split('-');
                if (langs.Length >= 2)
                {
                    Lang1Name = langs[0].Trim();
                    Lang2Name = langs[1].Trim();
                }
            }
            // Każda fiszka zajmuje 4 linie, zaczynając od indeksu 1
            for (int i = 1; i < lines.Length; i += 4)
            {
                if (i + 3 >= lines.Length)
                    break; // niekompletny wpis

                string slowo = lines[i].Trim();
                string opis = lines[i + 1].Trim();
                string poziom = lines[i + 2].Trim();
                string tlumaczenie = lines[i + 3].Trim();

                Fiszka card = new Fiszka
                {
                    WersjeJezyka1 = new List<string> { slowo },
                    // Jeśli jest więcej niż jedno tłumaczenie, rozdzielamy je przecinkiem
                    WersjeJezyka2 = tlumaczenie.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(s => s.Trim()).ToList(),
                    Opis = opis,
                    Poziom = poziom
                };
                Fiszki.Add(card);
            }
        }

        public static void DisplayFlashcards()
        {
            Console.Clear();
            Console.WriteLine("==== Lista fiszek ====");
            Console.WriteLine("Języki: " + Lang1Name + " - " + Lang2Name);
            Console.WriteLine("---------------------------------");
            foreach (var card in Fiszki)
            {
                Console.WriteLine("Słowo: " + card.WersjeJezyka1[0]);
                Console.WriteLine("Opis: " + card.Opis);
                Console.WriteLine("Poziom: " + card.Poziom);
                Console.WriteLine("Tłumaczenia: " + string.Join(", ", card.WersjeJezyka2));
                Console.WriteLine("---------------------------------");
            }
            Console.WriteLine("Naciśnij Enter, aby wrócić do menu...");
            Console.ReadLine();
        }

        // Metoda AddFlashcard() z dodanym sprawdzaniem pustych pól
        public static void AddFlashcard()
        {
            Console.Clear();
            Console.WriteLine("==== Dodaj nową fiszkę ====");

            Console.Write("Podaj słowo: ");
            string slowo = Console.ReadLine().Trim();

            Console.Write("Podaj opis fiszki: ");
            string opis = Console.ReadLine().Trim();

            Console.Write("Podaj poziom trudności (np. łatwy/trudny): ");
            string poziom = Console.ReadLine().Trim();

            Console.Write("Podaj tłumaczenie (w przypadku wielu tłumaczeń oddziel przecinkami): ");
            string tlumaczenie = Console.ReadLine().Trim();

            // Sprawdzamy, czy żadne pole nie jest puste
            if (string.IsNullOrWhiteSpace(slowo) ||
                string.IsNullOrWhiteSpace(opis) ||
                string.IsNullOrWhiteSpace(poziom) ||
                string.IsNullOrWhiteSpace(tlumaczenie))
            {
                Console.WriteLine("Nie można dodać pustej fiszki. Upewnij się, że wszystkie pola są wypełnione.");
                Console.WriteLine("Naciśnij Enter, aby powrócić do menu...");
                Console.ReadLine();
                return;
            }

            // Zapisujemy nową fiszkę w pliku w formacie 4-liniowym
            string flashcardData = slowo + Environment.NewLine +
                                   opis + Environment.NewLine +
                                   poziom + Environment.NewLine +
                                   tlumaczenie + Environment.NewLine;
            File.AppendAllText(filePath, flashcardData);

            // Dodajemy fiszkę do listy
            Fiszka nowaFiszka = new Fiszka
            {
                WersjeJezyka1 = new List<string> { slowo },
                WersjeJezyka2 = tlumaczenie.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(s => s.Trim()).ToList(),
                Opis = opis,
                Poziom = poziom
            };
            Fiszki.Add(nowaFiszka);

            Console.WriteLine("Fiszka dodana! Naciśnij Enter, aby kontynuować...");
            Console.ReadLine();
        }
    }
}
