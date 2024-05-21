using System.ComponentModel.DataAnnotations;

namespace NoireServer.Database.Model
{
    public class GameData
    {
        [Key] public int Id { get; set; }
        public string? Status { get; set; }
        public string? PlayingField { get; set; }
        public string? Bandit { get; set; }
        public string? Inspector { get; set; }
        public string? ProofDeck { get; set; }
        public string? Win { get; set; }
    }
}
