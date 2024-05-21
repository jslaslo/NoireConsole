using Newtonsoft.Json;
using NoireClient.Dto.Requests;

namespace NoireClient
{
    public class Client
    {
        public string Host { get; private set; }
        public int GameId { get; private set; }

        public Client(string host)
        {
            Host = host;
        }

        public RequestGame GetGame(int? id, string? role)
        {
            var response = CallServer("getplaygame/" + id + "/" + role).Result;
            RequestGame game = JsonConvert.DeserializeObject<RequestGame>(response.Content.ReadAsStringAsync().Result)!;

            return game;
        }

        public RequestGame SendMove(string move)
        {
            var response = CallServer("getmove/" + move).Result;

            return JsonConvert.DeserializeObject<RequestGame>(response.Content.ReadAsStringAsync().Result)!;
        }

        public RequestGame GetCurrentGame()
        {
            var response = CallServer("getcurrentgame").Result;
            RequestGame game = JsonConvert.DeserializeObject<RequestGame>(response.Content.ReadAsStringAsync().Result)!;
            GameId = game.Id!;

            return game;
        }

        private async Task<HttpResponseMessage> CallServer(string param = "")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Uri uri = new Uri(Host + param);
                    return await client.GetAsync(uri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null!;
            }
        }
    }
}
