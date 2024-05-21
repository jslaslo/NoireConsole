using NoireClient.Dto.Requests;
using NoireRules.Models;

namespace NoireClient.Dto.Responses
{
    public class ResponseGame
    {
        public int Id { get; set; }
        public string? PlayerName { get; set; }
        public Card[]? CardsSuspect { get; set; }
        public List<Card>? IdentityForInspector { get; set; } = new();
        public List<Card>? DeckProof { get; set; }
        public Role? YourRole { get; set; }
        public bool? IsYourMove { get; set; }
        public string? PlayerStatus { get; set; }

        public ResponseGame(RequestGame game, Card[] cardsSuspect, List<Card> deckProof, Card player)
        {
            Id = game.Id;
            PlayerName = player.Name;
            CardsSuspect = cardsSuspect;
            IdentityForInspector = game.IdentityForInspector;
            DeckProof = deckProof;
            YourRole = game.YourRole;
            IsYourMove = game.IsYourMove;
        }
        public ResponseGame(RequestGame game, Card[] cardsSuspect, List<Card> deckProof)
        {
            Id = game.Id;
            CardsSuspect = cardsSuspect;
            DeckProof = deckProof;
            YourRole = game.YourRole;
            IsYourMove = game.IsYourMove;
        }
    }
}
