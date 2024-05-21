using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoireRules.Models
{
    public class Card
    {
        public int PointX { get; set; }
        public int PointY { get; set; }
        public string Name { get; private protected set; }
        public Role Role { get; protected set; }
        public State State { get; set; }

        public Card(string name, Role role, State state)
        {
            Name = name;
            Role = role;
            State = state;
        }

        public override string ToString()
        {
            
            if (State == State.Killed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return Name;
            }
            if (State == State.Acquitted)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                return Name;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                return Name;
            }

        }

        private static protected List<Card> FindingSuspects(Card player, PlayingField deck)
        {
            List<Card> listSuspects = new();
            int rowLimit = deck.CardsSuspect!.GetLength(0) - 1;
            int columnLimit = deck.CardsSuspect.GetLength(1) - 1;

            if (rowLimit > 0)
            {
                for (int i = Math.Max(0, player!.PointY - 1); i <= Math.Min(player.PointY + 1, rowLimit); i++)
                {
                    for (int j = Math.Max(0, player.PointX - 1); j <= Math.Min(player.PointX + 1, columnLimit); j++)
                    {
                        if (i != player.PointY || j != player.PointX)
                        {
                            listSuspects.Add(deck.CardsSuspect[i, j]);
                        }
                    }
                }
            }
            return listSuspects;
        }

        private static protected bool Interrogation(Card identity, PlayingField deck, Role role)
        {
            var listIdentitys = FindingSuspects(identity, deck);
            var oneIdentity = listIdentitys.FirstOrDefault(c => c.Role == role);
            if (oneIdentity != null)
            {
                if (role == Role.Bandit)
                {
                    deck.CardsSuspect![identity.PointY, identity.PointX].State = State.Acquitted;
                }
                Console.WriteLine($"{role} находится рядом с жертвой");
                Console.WriteLine("Нажмите enter для продолжения...");
                Console.ReadLine();
                return true;
            }
            if (role == Role.Bandit)
            {
                deck.CardsSuspect![identity.PointY, identity.PointX].State = State.Acquitted;
            }
            Console.WriteLine($"{role} рядом с жертвой нет");
            Console.WriteLine("Нажмите enter для продолжения...");
            Console.ReadLine();
            return true;
        }
    }
}
