using NoireRules.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoireRules.Abstractions
{
    public interface IRole
    {
        public abstract Dictionary<int, string> Action { get; set; }
        public abstract bool FirstAction(PlayingField playingField);
        public abstract bool SecondAction(PlayingField playingField, ProofDeck proofDeck);
    }
}
