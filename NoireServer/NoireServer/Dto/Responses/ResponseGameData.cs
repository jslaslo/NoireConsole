using Newtonsoft.Json;
using NoireRules.Models;
using NoireServer.Database.Model;

namespace NoireServer.Dto.Responses
{
    public class ResponseGameData
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? PlayerName { get; set; } = "";
        public Card[]? CardsSuspect { get; set; }
        public string? Bandit { get; set; }
        public string? Inspector { get; set; }
        public List<Card>? IdentityForInspector { get; set; }
        public List<Card>? DeckProof { get; set; }
        public Role? YourRole { get; set; }
        public bool? IsYourMove { get; set; }
        public string? Win { get; set; } = "";

        public ResponseGameData(GameData game)
        {
            Id = game.Id;
            Status = game.Status;
            CardsSuspect = PlayingField.FromJson(game.PlayingField!);
            Bandit = game.Bandit;
            Inspector = game.Inspector;
            DeckProof = ProofDeck.FromJson(game.ProofDeck!);
            Win = game.Win;
        }
        public ResponseGameData(GameData game, PlayerData player)
        {
            PlayerName = player.Name != null ? player.Name : "";
            Id = game.Id;
            Status = game.Status;
            CardsSuspect = PlayingField.FromJson(game.PlayingField!);
            Bandit = game.Bandit!;
            Inspector = game.Inspector!;
            DeckProof = ProofDeck.FromJson(game.ProofDeck!);
            YourRole = player.YourRole;
            IsYourMove = player.IsYourMove;
            IdentityForInspector = JsonConvert.DeserializeObject<List<Card>>(player.IdentityForInspector!)!;
            Win = game.Win;
        }
    }
}
