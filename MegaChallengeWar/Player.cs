using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MegaChallengeWar
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; }

        public Player()
        {
        }
        public Player(string name)
        {
            this.Name = name;
            this.Cards = new List<Card> { };
        }
        public Player(string name, List<Card> cards)
        {
            this.Name = name;
            this.Cards = cards;
        }
    }
}