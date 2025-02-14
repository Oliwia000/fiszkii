using System;

namespace Fiszki
{
    public static class MenuProgram
    {
        public static void Uruchom()
        {
            ZarzadzanieFiszkami.LoadFlashcards();      // Wczytanie fiszek z pliku(plik zostanie utworzony, jeśli nie istnieje)

            bool kontynuuj = true;
            while (kontynuuj)
            {
                Console.Clear();
                Console.WriteLine("==== MENU ====");
                Console.WriteLine("1. Pokaż fiszki");
                Console.WriteLine("2. Dodaj fiszkę");
                Console.WriteLine("3. Rozpocznij naukę");
                Console.WriteLine("4. Wyjdź");
                Console.Write("Wybierz opcję: ");
                string wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        ZarzadzanieFiszkami.DisplayFlashcards();
                        break;
                    case "2":
                        ZarzadzanieFiszkami.AddFlashcard();
                        break;
                    case "3":
                        TrybNauki.StartTraining();
                        break;
                    case "4":
                        kontynuuj = false;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór! Naciśnij Enter, aby spróbować ponownie.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
