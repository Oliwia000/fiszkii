using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiszki
{
    public class Fiszka
    {
        // Wersja w języku początkowym (np. PL) – zwykle jedno słowo lub wyrażenie
        public List<string> WersjeJezyka1 { get; set; }

        // Wersja w drugim języku (np. ENG) – tłumaczenie lub tłumaczenia (alternatywy)
        public List<string> WersjeJezyka2 { get; set; }

        // Dodatkowe pola
        public string Opis { get; set; }
        public string Poziom { get; set; }

        // Zwraca pytanie do wyświetlenia (słowo lub wyrażenie) w zależności od kierunku nauki
        public string PobierzPytanie(string kierunek)
        {
            if (kierunek == "PL -> ENG")
                return string.Join(" | ", WersjeJezyka1);
            else
                return string.Join(" | ", WersjeJezyka2);
        }

        // Zwraca listę poprawnych tłumaczeń (odpowiedzi) w zależności od kierunku nauki
        public List<string> PobierzOdpowiedzi(string kierunek)
        {
            if (kierunek == "PL -> ENG")
                return WersjeJezyka2.Select(s => s.ToLower()).ToList();
            else
                return WersjeJezyka1.Select(s => s.ToLower()).ToList();
        }

        // Generuje podpowiedź – pierwsza litera pierwszej odpowiedzi, a reszta znaków zastąpiona '_'
        public string PobierzPodpowiedz(string kierunek)
        {
            var odpowiedzi = PobierzOdpowiedzi(kierunek);
            if (odpowiedzi.Count > 0)
            {
                string odp = odpowiedzi[0];
                if (!string.IsNullOrEmpty(odp))
                    return odp[0] + new string('_', odp.Length - 1);
            }
            return "";
        }
    }
}
