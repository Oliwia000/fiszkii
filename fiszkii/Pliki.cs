using System.Collections.Generic;

namespace fiszkii
{
    public class Flashcard
    {
        public string Word { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public List<string> Translations { get; set; }
    }
}
