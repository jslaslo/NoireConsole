using NoireRules.Models;
using NoireRules.Models.Players;

namespace NoireRules.Logic
{
    public static class SecretIdentitiesCreator
    {
        public static Bandit IdentifyBandit(PlayingField deck, ProofDeck deckProof)
        {

            var card = deckProof.DeckProof[0];

            deckProof.DeckProof.Remove(card);

            deck.CardsSuspect![card.PointY, card.PointX] = new Bandit(card.Name, Role.Bandit, State.Alive)
            {
                PointX = card.PointX,
                PointY = card.PointY
            };

            return (Bandit)deck.CardsSuspect[card.PointY, card.PointX];
        }
        public static Bandit IdentifyBandit(PlayingField playingField, string name, ProofDeck deckProof)
        {
            var player = PlayerFinder.GetPlayer(playingField, name);

            playingField.CardsSuspect![player!.PointY, player.PointX] = new Bandit(name, Role.Bandit, State.Alive)
            {
                PointX = player.PointX,
                PointY = player.PointY
            };
            deckProof.DeckProof.Remove(player);
            return (Bandit)playingField.CardsSuspect[player.PointY, player.PointX]; ;
        }

        public static List<Card> IdentifyInspector(ProofDeck deckProof)
        {
            List<Card> cardsInspector = new();
            for (int i = 0; i < 4; i++)
            {
                cardsInspector.Add(deckProof.DeckProof[0]);
                deckProof.DeckProof.Remove(deckProof.DeckProof[0]);
            }
            return cardsInspector;
        }
        public static Inspector IdentifyInspector(PlayingField playingField, string name, List<Card> secretIdentities)
        {
            var player = PlayerFinder.GetPlayer(playingField, name);
            playingField.CardsSuspect![player!.PointY, player.PointX] = new Inspector(name, Role.Inspector, State.Alive, secretIdentities)
            {
                PointX = player.PointX,
                PointY = player.PointY
            };
            return (Inspector)playingField.CardsSuspect[player.PointY, player.PointX];
        }
    }
}
