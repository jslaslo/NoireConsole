using Newtonsoft.Json;
using NoireClient.Dto.Responses;
using NoireClient.Dto.Requests;
using NoireRules.Abstractions;
using NoireRules.Data;
using NoireRules.Logic;
using NoireRules.Models;
using NoireRules.Models.Players;

namespace NoireClient
{
    public class PlayGame
    {
        public static bool IsFirstMove { get; set; } = true;

        public static string MoveGame(RequestGame game)
        {
            var role = (Role)game.YourRole!;
            IRole player;
            ResponseGame? response = null;
            var deck = new PlayingField()
            {
                CardsSuspect = Converter.Flatten(game.CardsSuspect!)
            };
            var deckProof = new ProofDeck()
            {
                DeckProof = game.DeckProof!
            };
            View.ShowField(deck, game.PlayerName);

            if (role == Role.Bandit && IsFirstMove == true)
            {
                bool rightAction = false;
                SecretIdentitiesCreator.IdentifyBandit(deck, game.PlayerName!, deckProof);
                player = (IRole)PlayerFinder.GetPlayer(deck, role)!;
                while (rightAction == false)
                {
                    View.ShowPlayer(deck, role);
                    rightAction = player.FirstAction(deck);
                }
                IsFirstMove = false;
                response = new ResponseGame(game, Converter.Expand(deck.CardsSuspect), deckProof.DeckProof);
                Console.Clear();
            }
            else if (role == Role.Inspector && IsFirstMove == true)
            {
                var cardsInspector = game.IdentityForInspector;
                Card? choice = null;
                Card? choiceOnDeck;
                while (true)
                {
                    Console.Write("Выберите тайную личность из предложенных ниже имен:\n"
                        + String.Join(", ", cardsInspector!) + ";"
                        + "\nЧтобы выбрать тайную личность введите имя: ");
                    string name = "";
                    
                    while (choice == null)
                    {
                        name = Console.ReadLine()!;
                        choice = cardsInspector!.FirstOrDefault(c => c.Name.ToLower() == name.ToLower())!;
                    }                    
                    choiceOnDeck = PlayerFinder.GetPlayer(deck, choice.Name);

                    if (choice != null && choiceOnDeck != null && choiceOnDeck!.State != State.Killed)
                    {
                        cardsInspector!.Remove(choice);
                        deck.CardsSuspect[choice.PointY, choice.PointX] = new Inspector(choice.Name!, Role.Inspector, choice.State, cardsInspector)
                        {
                            PointX = choice.PointX,
                            PointY = choice.PointY
                        };
                        Console.WriteLine($"Ваша тайная личность: {choice.Name}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вы ввели некорректное имя, повторите попытку...");
                        Console.ReadLine();
                    }
                }
                var inspector = (Inspector)PlayerFinder.GetPlayer(deck, role)!;
                inspector.SecretIdentites = cardsInspector;
                IsFirstMove = false;
                response = new ResponseGame(game, Converter.Expand(deck.CardsSuspect), deckProof.DeckProof, inspector);
                Console.Clear();
            }
            else
            {
                bool rightAction = false;
                if (role == Role.Inspector)
                {
                    player = SecretIdentitiesCreator.IdentifyInspector(deck, game.PlayerName!, game.IdentityForInspector!);
                }
                else
                {
                    var proofDeck = new ProofDeck()
                    {
                        DeckProof = game.DeckProof!
                    };
                    player = SecretIdentitiesCreator.IdentifyBandit(deck, game.PlayerName!, proofDeck);
                }
                View.ShowPlayer(deck, role);
                Console.WriteLine($"Какой ход хотите сделать?" +
                    $"\n1. Сдвинуть игровое поле;" +
                    $"\n2. Совершить действие {player.Action[0]}" +
                    $"\n3. Совершить действие {player.Action[1]}");

                string? choice = null;
                bool flag = false;
                while (flag == false)
                {
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Mover.Move(deck);
                            Console.Clear();
                            response = ResponseMove(game, game.YourRole, player, deckProof, deck);
                            flag = true;
                            return JsonConvert.SerializeObject(response);

                        case "2":
                            while (rightAction == false)
                            {
                                rightAction = player.FirstAction(deck);
                            }
                            Console.Clear();

                            response = ResponseMove(game, game.YourRole, player, deckProof, deck);
                            flag = true;
                            return JsonConvert.SerializeObject(response);

                        case "3":
                            while (rightAction == false)
                            {
                                rightAction = player.SecondAction(deck, deckProof);
                            }
                            Console.Clear();

                            response = ResponseMove(game, game.YourRole, player, deckProof, deck);
                            flag = true;
                            return JsonConvert.SerializeObject(response);
                        default:
                            Console.WriteLine("Повторите попытку");
                            flag = false;
                            break;
                    }
                }
                
            }
            Console.Clear();
            return JsonConvert.SerializeObject(response);
        }

        private static ResponseGame ResponseMove(RequestGame game, Role? role, IRole player, ProofDeck deckProof, PlayingField deck)
        {
            ResponseGame response;
            if (role == Role.Inspector)
            {
                response = new ResponseGame(game, Converter.Expand(deck.CardsSuspect!), deckProof.DeckProof, (Inspector)player);
            }
            else
            {
                response = new ResponseGame(game, Converter.Expand(deck.CardsSuspect!), deckProof.DeckProof, (Bandit)player);
            }
            return response;
        }
    }
}

