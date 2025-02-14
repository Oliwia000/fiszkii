using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiszki
{
  public class Fiszka
    {
        public List<string> WersjeJezyka1 { get; set; }

        public List<string> WersjeJezyka2 { get; set; }

        public string Opis { get; set; }
        public string Poziom { get; set; }

        public string PobierzPytanie(string kierunek)
        {
            if (kierunek == "PL -> ENG")
                return string.Join(" | ", WersjeJezyka1);
            else
                return string.Join(" | ", WersjeJezyka2);
        }

        public List<string> PobierzOdpowiedzi(string kierunek)
        {
            if (kierunek == "PL -> ENG")
                return WersjeJezyka2.Select(s => s.ToLower()).ToList();
            else
                return WersjeJezyka1.Select(s => s.ToLower()).ToList();
        }

        // Generuje podpowiedź 
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
