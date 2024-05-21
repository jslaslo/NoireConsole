using NoireRules.Abstractions;
using NoireRules.Logic;

namespace NoireRules.Models.Players
{
    public class Inspector : Card, IRole
    {
        public List<Card> SecretIdentites { get; set; }
        public Dictionary<int, string> Action { get; set; } = new Dictionary<int, string>
        {
            {0,"Обвинить" },
            {1,"Оправдать" }
        };

        public Inspector(string name, Role role, State state, List<Card> secretIdentities) : base(name, role, state)
        {
            SecretIdentites = secretIdentities;
        }
        public bool FirstAction(PlayingField deck)
        {
            Console.WriteLine("Кого хотите обвинить");
            string? nameAccused = Console.ReadLine();
            var listSuspects = FindingSuspects(deck.CardsSuspect![PointY, PointX], deck);
            Card? suspect = listSuspects.FirstOrDefault(x => x.Name!.ToLower() == nameAccused?.ToLower());
            
            if (suspect != null && suspect.State == State.Alive)
            {
                if (suspect.Role == Role.Bandit)
                {
                    suspect.State = State.Arrested;
                    Console.WriteLine($"{deck.CardsSuspect[suspect.PointY, suspect.PointX]} оказался бандитом!");
                    Console.WriteLine("Нажмите enter для продолжения...");
                    Console.ReadLine();
                    return true;
                }
                Console.WriteLine($"{deck.CardsSuspect[suspect.PointY, suspect.PointX]} является мирным жителем!");
                Console.WriteLine("Нажмите enter для продолжения...");
                Console.ReadLine();
                return true;
            }
            else
            {
                Console.WriteLine("Жертва должна быть поблизости с Вами...");
                Console.WriteLine("Нажмите enter для продолжения...");
                Console.ReadLine();
                return false;
            }
        }

        public bool SecondAction(PlayingField deck, ProofDeck deckProof)
        {
            if (deckProof.DeckProof.Count > 0)
            {
                var foundPerson = deckProof.DeckProof[0];
                SecretIdentites.Add(foundPerson);
                Console.Write($"Ваши помощники: {String.Join(',', SecretIdentites)}" +
                            "\nВыберите(введите имя) одного помощника, для разведки: ");
                string name = Console.ReadLine()!;
                if (name != string.Empty)
                {
                    
                    if (foundPerson != null)
                    {
                        Card choice = SecretIdentites.FirstOrDefault(c => c.Name.ToLower() == name.ToLower())!;
                        Card? choiceOnDeck = PlayerFinder.GetPlayer(deck, choice.Name);
                        

                        if (choice != null && choiceOnDeck!.State == State.Alive)
                        {
                            SecretIdentites.Remove(choice);
                            deckProof.DeckProof.RemoveAt(0);
                            return Interrogation(choice, deck, Role.Bandit);
                        }
                        else
                        {
                            SecretIdentites.Remove(foundPerson);
                            Console.WriteLine("Данную личность нельзя оправдать...");
                            Console.WriteLine("Нажмите enter для продолжения...");
                            Console.ReadLine();
                            return true;
                        }
                    }
                    
                }
            }
            return false;
        }
    }
}
