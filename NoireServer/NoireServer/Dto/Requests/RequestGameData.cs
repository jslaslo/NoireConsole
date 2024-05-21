using NoireRules.Models;

namespace NoireServer.Dto.Requests
{
    public class RequestGameData
    {        
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? PlayerName { get; set; }
        public Card[]? CardsSuspect { get; set; }
        public List<Card>? IdentityForInspector { get; set; }
        public List<Card>? DeckProof { get; set; }
        public Role YourRole { get; set; }
        public bool? IsYourMove { get; set; }
    }
}
