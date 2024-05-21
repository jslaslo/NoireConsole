using NoireRules.Models;

namespace NoireClient.Dto.Requests
{
    public class RequestGame
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? PlayerName { get; set; }
        public Card[]? CardsSuspect { get; set; }
        public Role? Bandit { get; set; }
        public Role? Inspector { get; set; }
        public List<Card>? IdentityForInspector { get; set; }
        public List<Card>? DeckProof { get; set; }
        public Role? YourRole { get; set; }
        public bool? IsYourMove { get; set; }
        public string? Win { get; set; }
    }
}
