using NoireClient;
using NoireClient.Dto.Requests;
using NoireRules.Data;
using NoireRules.Logic;
using NoireRules.Models;

namespace NoireClientApp
{
    public class Program
    {
        public const string Host = "https://localhost:7298/game/";
        public static bool Recived = false;
        static void Main(string[] args)
        {

            Client client = new Client(Host);
            RequestGame response = new();

            while (true)
            {
                if (Recived == false)
                {
                    response = client.GetCurrentGame();
                    Recived = true;
                }
                var game = client.GetGame(response!.Id, response.YourRole.ToString());
                if (game != null)
                {
                    if (game.Status == "Done")
                    {
                        Console.Clear();
                        Console.WriteLine($"Игра закончена. Победил {game.Win}");
                        Console.ReadLine();
                        return;
                    }
                    else if (game.IsYourMove == true && game.Status == "Play")
                    {
                        var request = PlayGame.MoveGame(game);
                        client.SendMove(request);
                        continue;
                    }
                }
                
                PlayingField field = new()
                {
                    CardsSuspect = Converter.Flatten(game!.CardsSuspect!)
                };
                View.ShowField(field, game.PlayerName);
                Console.WriteLine("Ожидайте другого игрока...");
                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
