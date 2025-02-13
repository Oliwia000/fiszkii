using System;

namespace fiszkii
{
    public class MenuProgramu
    {
        public void Uruchom()
        {
            ZarzadzanieFiszkami.WczytajFiszki();

            bool kontynuuj = true;
            while (kontynuuj)
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Pokaż fiszki");
                Console.WriteLine("2. Dodaj fiszkę");
                Console.WriteLine("3. Rozpocznij naukę");
                Console.WriteLine("4. Wyjdź");
                string wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        ZarzadzanieFiszkami.WyswietlFiszki();
                        break;
                    case "2":
                        ZarzadzanieFiszkami.DodajFiszke();
                        break;
                    case "3":
                        TrybNauki.RozpocznijNauke();
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
