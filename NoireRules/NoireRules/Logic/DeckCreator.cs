using NoireRules.Data;
using NoireRules.Models;

namespace NoireRules.Logic
{
    public static class DeckCreator
    {
        public static PlayingField CreateDeck()
        {
            PlayingField playingField = new();
            Card[,] cards = new Card[5, 5];
            NamesData names = new();
            Random random = new();
            for (int i = 0; i < cards.GetLength(1); i++)
            {
                for (int j = 0; j < cards.GetLength(0); j++)
                {
                    int namedСharacter = random.Next(0, names.NamesList.Count);
                    cards[i, j] = new Card(names.NamesList[namedСharacter], Role.Civilian, State.Alive)
                    {
                        PointY = i,
                        PointX = j
                    };
                    names.NamesList.Remove(names.NamesList[namedСharacter]);
                }
            }
            playingField.CardsSuspect = cards;
            return playingField;
        }

        public static ProofDeck CreateDeckProof(PlayingField playingField)
        {
            ProofDeck proofDeck = new();
            for (int i = 0; i < playingField.CardsSuspect!.GetLength(0); i++)
            {
                for (int j = 0; j < playingField.CardsSuspect.GetLength(1); j++)
                {
                    proofDeck.DeckProof.Add(playingField.CardsSuspect[i, j]);
                }
            }
            RefreshDeckProof(proofDeck.DeckProof);
            return proofDeck;
        }
        private static void RefreshDeckProof(List<Card> cards)
        {

            Random rand = new();
            for (int i = 0; i < cards.Count - 1; i++)
            {
                var tmp = cards[0];
                cards.RemoveAt(0);
                cards.Insert(rand.Next(cards.Count), tmp);
            }
        }
    }
}
