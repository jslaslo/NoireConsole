using NoireRules.Data;
using System.Text.Json;

namespace NoireRules.Models
{
    public class PlayingField
    {
        public Card[,]? CardsSuspect { get; set; }

        public string ToJson()
        {
            var tmp = Converter.Expand(CardsSuspect!);
            return JsonSerializer.Serialize(tmp);
        }

        public static Card[]? FromJson(string field)
        {
            return JsonSerializer.Deserialize<Card[]>(field);
        }
    }
}
