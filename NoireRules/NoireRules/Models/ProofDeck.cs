using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NoireRules.Models
{
    public class ProofDeck
    {
        public List<Card> DeckProof { get; set; }

        public ProofDeck()
        {
            DeckProof = new List<Card>();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(DeckProof);
        }

        public static List<Card>? FromJson(string deckProof)
        {
            return JsonSerializer.Deserialize<List<Card>>(deckProof);
        }
    }
}
