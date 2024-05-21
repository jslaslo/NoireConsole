using NoireRules.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoireRules.Data
{
    public class Converter
    {
        public static Card[] Expand(Card[,] cards)
        {
            Card[] newCards = new Card[cards.Length];
            int z = 0;
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                for (int j = 0; j < cards.GetLength(1); j++)
                {
                    newCards[z] = cards[i, j];
                    z++;
                }
            }
            return newCards;
        }
        public static Card[,] Flatten(Card[] cards)
        {
            int count = cards.Length / 5;
            Card[,] newCards = new Card[count, count];
            int z = 0;
            for (int i = 0; i < newCards.GetLength(0); i++)
            {
                for (int j = 0; j < newCards.GetLength(1); j++)
                {
                    newCards[i, j] = cards[z++];
                }
            }
            return newCards;
        }
    }
}
