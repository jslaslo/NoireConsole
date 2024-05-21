using NoireRules.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NoireRules.Models.Players
{
    public class Bandit : Card, IRole
    {
        public Dictionary<int, string> Action { get; set; } = new Dictionary<int, string>
        {
            {0,"Убить" },
            {1,"Замаскироваться" }
        };

        public Bandit(string name, Role role, State state) : base(name, role, state)
        {
        }

        public bool FirstAction(PlayingField deck)
        {
            Console.WriteLine("Кого хотите убить");
            string? nameVictim = Console.ReadLine();
            var listSuspects = FindingSuspects(deck.CardsSuspect![PointY, PointX], deck);
            var suspect = listSuspects.FirstOrDefault(x => x.Name!.ToLower() == nameVictim?.ToLower());
            if (suspect != null)
            {
                if (suspect.State == State.Alive)
                {
                    deck.CardsSuspect[suspect.PointY, suspect.PointX].State = State.Killed;
                    Console.WriteLine($"{deck.CardsSuspect[suspect.PointY, suspect.PointX].Name} убит");
                    Console.WriteLine("Нажмите enter для продолжения...");
                    Console.ReadLine();
                    return true;
                }
                else if (suspect.State == State.Acquitted)
                {
                    deck.CardsSuspect[suspect.PointY, suspect.PointX].State = State.Killed;
                    Console.WriteLine($"{deck.CardsSuspect[suspect.PointY, suspect.PointX].Name} убит");
                    Console.WriteLine("Перед убиством Вы провели допрос: ");

                    return Interrogation(suspect, deck, Role.Inspector);
                }
                else
                {
                    Console.WriteLine("Жерта уже мертва");
                    Console.WriteLine("Нажмите enter для продолжения...");
                    Console.ReadLine();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Выбранная жертва не может быть убита...");
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
                deckProof.DeckProof.RemoveAt(0);
                if (foundPerson != null)
                {
                    deck.CardsSuspect![PointY, PointX] = new Card(Name, Role.Civilian, State.Alive)
                    {
                        State = State.Acquitted
                    };
                    deck.CardsSuspect[foundPerson.PointY, foundPerson.PointX] = new Bandit(foundPerson.Name!, Role.Bandit, State.Alive)
                    {
                        PointX = foundPerson.PointX,
                        PointY = foundPerson.PointY
                    };

                    Name = foundPerson.Name;
                    PointX = foundPerson.PointX;
                    PointY = foundPerson.PointY;

                    Console.WriteLine($"Успешная смена личности.\nВаша личность: {foundPerson}");
                    Console.WriteLine("Нажмите enter для продолжения...");
                    Console.ReadLine();
                    return true;
                }
                Console.WriteLine("Сменить личность не удалось");
                Console.WriteLine("Нажмите enter для продолжения...");
                Console.ReadLine();
                return false;
            }
            else
            {
                Console.WriteLine("Сменить личность не удалось");
                Console.WriteLine("Нажмите enter для продолжения...");
                Console.ReadLine();
                return false;
            }
        }
    }
}
