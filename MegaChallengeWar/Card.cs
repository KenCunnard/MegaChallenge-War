using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MegaChallengeWar
{
    public class Card
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Suit { get; set; }

        public string FriendlyName
        {
            get { return $"{this.Name} of {this.Suit}"; }
        }
    }
}