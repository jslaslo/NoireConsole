using NoireServer.Database.Model;
using NoireServer.Database.Models.Contexts;
using System.Data;
using NoireRules.Models;
using NoireServer.Dto.Responses;
using NoireServer.Dto.Requests;
using NoireRules.Logic;
using NoireRules.Data;
using NoireRules.Models.Players;
using Newtonsoft.Json;

namespace NoireServer
{
    public class Logic
    {
        private readonly NoireContext db;
        private readonly List<Role> AllRoles;

        public Logic()
        {
            AllRoles = Enum.GetValues(typeof(Role)).Cast<Role>().ToList();
            AllRoles.Remove(Role.Civilian);
            db = new();
        }

        public ResponseGameData? GetCurrentGame()
        {
            GameData? game = db.Games
                .Where(g => g.Status == "Wait")
                .OrderBy(g => g.Id)
                .FirstOrDefault()!;
            if (game == null)
            {
                return CreateNewGame()!;
            }

            var player = AddPlayerGame(game);

            db.Update(game);
            db.SaveChanges();

            return new ResponseGameData(game, player);
        }

        public ResponseGameData? GetPlayGame(int id, string nameRole)
        {
            Role role = Enum.Parse<Role>(nameRole);
            var game = db.Games.Find(id)!;
            var player = db.Players.Where(p => p.GameId == id && p.YourRole == role)
                                   .FirstOrDefault();
            if (game != null || player != null)
            {
                return new ResponseGameData(game!, player!);
            }
            else
            {
                return null;
            }
        }

        public GameData GetGame(int id)        
        {
            return db.Games.Find(id)!;
        }

        public ResponseGameData? GetMove(string stringJson)
        {            
            var request = JsonConvert.DeserializeObject<RequestGameData>(stringJson);
            GameData game = GetGame(request!.Id);

            if (game == null)
            {
                return new ResponseGameData(game!);
            }

            var players = db.Players
                            .Where(p => p.GameId == request.Id).ToList();


            var player = players.First(p => p.GameId == request.Id
                                    && p.YourRole == request.YourRole);

            if (CheckWin(game, players, request))
            {
                db.Update(player);
                db.Update(game);
                db.SaveChanges();
                return new ResponseGameData(game, player);
            }

            game.PlayingField = JsonConvert.SerializeObject(request.CardsSuspect);
            game.ProofDeck = JsonConvert.SerializeObject(request.DeckProof);
            var inspector = players.Find(p => p.YourRole == Role.Inspector);

            if (inspector!.Name != "")
            {
                ChangePlayerData(game, player, request);
            }

            СhangeMove(players);
            ChangeSecretIdentity(request, player);

            db.Update(player);
            db.Update(game);
            db.SaveChanges();

            return new ResponseGameData(game, player);
        }

        private static void СhangeMove(List<PlayerData> players)
        {
            foreach (var player in players)
            {
                if (player.IsYourMove == true)
                {
                    player.IsYourMove = false;
                    continue;
                }
                else
                {
                    player.IsYourMove = true;
                    continue;
                }
            }
        }

        private PlayerData AddPlayerGame(GameData game)
        {
            var player = CreatingPlayerData(game);
            player!.IsYourMove = GetFirstMove(player);
            if (player.YourRole == Role.Inspector)
            {
                game.Inspector = Role.Inspector.ToString();
            }
            else
            {
                game.Bandit = Role.Bandit.ToString();
            }
            game.Status = "Play";
            db.Players.Add(player);
            db.Update(game);
            db.SaveChanges();
            return player;
        }

        private ResponseGameData? CreateNewGame()
        {
            PlayingField playingField = DeckCreator.CreateDeck();
            ProofDeck proofDeck = DeckCreator.CreateDeckProof(playingField);

            var game = new GameData()
            {
                PlayingField = playingField.ToJson(),
                ProofDeck = proofDeck.ToJson(),
                Status = "Wait",
            };
            var player = CreatingPlayerData(game);

            if (player!.YourRole == Role.Bandit)
            {
                game.Bandit = Role.Bandit.ToString();
            }
            else
            {
                game.Inspector = Role.Inspector.ToString();
            }

            player.IsYourMove = GetFirstMove(player);

            /*if (game.Bandit == "Bandit")
            {*/
                db.Players.Add(player);
                db.Games.Add(game);
                db.SaveChanges();
            /*}*/
            return new ResponseGameData(game, player);
        }

        private void ChangePlayerData(GameData game, PlayerData player, RequestGameData request)
        {

            var field = PlayingField.FromJson(game.PlayingField!);

            PlayingField readyField = new()
            {
                CardsSuspect = Converter.Flatten(field!)
            };
            ProofDeck proofDeck = new()
            {
                DeckProof = ProofDeck.FromJson(game.ProofDeck!)!
            };
            var role = GetRole(game);
            if (player.Name != request.PlayerName)
            {
                player.Name = request.PlayerName;

                if (role == Role.Bandit)
                {
                    player.Name = SecretIdentitiesCreator.IdentifyBandit(readyField, request.PlayerName!, proofDeck).Name;
                    player.YourRole = Role.Bandit;
                    player.Game = game;
                }
                else
                {
                    player.Name = SecretIdentitiesCreator.IdentifyInspector(readyField, request.PlayerName!, request.IdentityForInspector!).Name;
                    player.IdentityForInspector = JsonConvert.SerializeObject(SecretIdentitiesCreator.IdentifyInspector(proofDeck));
                    player.YourRole = Role.Inspector;
                    player.Game = game;
                }
            }
        }

        private PlayerData? CreatingPlayerData(GameData game)
        {
            PlayerData player = new();
            var field = PlayingField.FromJson(game.PlayingField!);

            PlayingField readyField = new()
            {
                CardsSuspect = Converter.Flatten(field!)
            };
            ProofDeck proofDeck = new()
            {
                DeckProof = ProofDeck.FromJson(game.ProofDeck!)!
            };
            var role = GetRole(game);

            if (role == Role.Bandit)
            {
                player.Name = SecretIdentitiesCreator.IdentifyBandit(readyField, proofDeck).Name;
                player.YourRole = Role.Bandit;
                player.Game = game;

                player.IsYourMove = GetFirstMove(player);
                game.ProofDeck = proofDeck.ToJson();

                return player;
            }
            else if (role == Role.Inspector)
            {
                player.IdentityForInspector = JsonConvert.SerializeObject(SecretIdentitiesCreator.IdentifyInspector(proofDeck));
                player.YourRole = Role.Inspector;
                player.Game = game;

                player.IsYourMove = GetFirstMove(player);
                game.ProofDeck = proofDeck.ToJson();
                return player;
            }
            return player;
        }

        private Role GetRole(GameData game)
        {
            if (game.Inspector == null && game.Bandit == null)
            {
                var role = AllRoles[new Random().Next(AllRoles.Count)];
                return role;
            }
            else if (game.Inspector != null)
            {
                return Role.Bandit;
            }
            return Role.Inspector;
        }

        private static bool GetFirstMove(PlayerData player)
        {
            if (player.YourRole == Role.Bandit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ChangeSecretIdentity(RequestGameData request, PlayerData playerData)
        {
            if (request.YourRole == Role.Bandit)
            {
                var player = request.CardsSuspect!.First(p => p.Name == playerData.Name);
                player = new Bandit(player.Name, player.Role, player.State)
                {
                    PointX = player.PointX,
                    PointY = player.PointY
                };
                playerData.Name = player.Name;
                for (int i = 0; i < request.CardsSuspect!.Length - 1; i++)
                {
                    if (request.CardsSuspect[i].Name == player.Name)
                    {
                        request.CardsSuspect[i] = player;
                        return;
                    }
                }
            }
            else if (request.YourRole == Role.Inspector)
            {
                var player = request.CardsSuspect!.First(p => p.Name == request.PlayerName);
                player = new Inspector(player.Name, player.Role, player.State, request.IdentityForInspector!)
                {
                    PointX = player.PointX,
                    PointY = player.PointY
                };
                playerData.Name = player.Name;
                playerData.IdentityForInspector = JsonConvert.SerializeObject(request.IdentityForInspector);
                for (int i = 0; i < request.CardsSuspect!.Length - 1; i++)
                {
                    if (request.CardsSuspect[i].Name == player.Name)
                    {
                        request.CardsSuspect[i] = player;
                    }
                }
            }
        }

        private static bool CheckWin(GameData game, List<PlayerData> players, RequestGameData request)
        {
            PlayingField readyField = new()
            {
                CardsSuspect = Converter.Flatten(request.CardsSuspect!)
            };
            var opponent = players.First(p => p.Name != request.PlayerName);
            var cardOpponent = PlayerFinder.GetPlayer(readyField, opponent.Name!);
            if (cardOpponent == null) { return false; }
            if (cardOpponent != null && cardOpponent.State == State.Killed
             || cardOpponent!.State == State.Arrested)
            {
                game.Win = $"{request.YourRole}";
                game.Status = "Done";
                return true;
            }
            return false;
        }
    }
}
