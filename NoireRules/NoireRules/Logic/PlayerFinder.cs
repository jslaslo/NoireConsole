using NoireRules.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoireRules.Logic
{
    public static class PlayerFinder
    {
        public static Card? GetPlayer(PlayingField deck, Role role)
        {
            for (int i = 0; i < deck.CardsSuspect!.GetLength(0); i++)
            {
                for (int j = 0; j < deck.CardsSuspect.GetLength(1); j++)
                {
                    if (deck.CardsSuspect[i, j].Role == role)
                    {
                        return deck.CardsSuspect[i, j];
                    }
                }
            }
            return null;
        }
        public static Card? GetPlayer(PlayingField deck, string? foundName)
        {
            for (int i = 0; i < deck.CardsSuspect!.GetLength(0); i++)
            {
                for (int j = 0; j < deck.CardsSuspect.GetLength(1); j++)
                {
                    if (deck.CardsSuspect[i, j].Name == foundName)
                    {
                        return deck.CardsSuspect[i, j];
                    }
                }
            }
            return null;
        }
    }
}
