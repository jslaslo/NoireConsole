using NoireRules.Models;

namespace NoireRules.Logic
{
    public static class View
    {
        public static void ShowField(PlayingField deck, string? namePerson)
        {
            for (int i = 0; i < deck.CardsSuspect!.GetLength(0); i++)
            {
                for (int j = 0; j < deck.CardsSuspect.GetLength(1); j++)
                {
                    if (deck.CardsSuspect[i, j].Name == namePerson)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(deck.CardsSuspect[i, j].Name.PadRight(15));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(deck.CardsSuspect[i, j].ToString().PadRight(15));
                    }
                }
                Console.WriteLine("\n");
            }
        }

        public static void ShowPlayer(PlayingField deck, Role role)
        {
            var player = PlayerFinder.GetPlayer(deck, role);
            Console.WriteLine($"Ход {player} ({player?.Role})");
        }
    }
}
