using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MegaChallengeWar
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            this.Cards = new List<Card>();
        }
        public Deck(List<Card> cards)
        {
            this.Cards = cards;
        }

        public static Deck BuildDeck()
        {
            Deck deck = new Deck();
            //deck.Cards = new List<Card>();

            List<Suit> suits = new List<Suit>()
            {
                new Suit { Name = "Spades", Color = "Black" },
                new Suit { Name = "Hearts", Color="Red"},
                new Suit { Name = "Clubs", Color="Black" },
                new Suit { Name = "Diamonds", Color="Red" }
            };

            foreach (var suit in suits)
            {
                deck.Cards.Add(new Card { Name = "2", Value = 2, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "3", Value = 3, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "4", Value = 4, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "5", Value = 5, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "6", Value = 6, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "7", Value = 7, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "8", Value = 8, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "9", Value = 9, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "10", Value = 10, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "Jack", Value = 11, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "Queen", Value = 12, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "King", Value = 13, Suit = suit.Name });
                deck.Cards.Add(new Card { Name = "Ace", Value = 14, Suit = suit.Name });
            }

            return ShuffleDeck(deck);
        }

        private static Deck ShuffleDeck(Deck deck)
        {
            Random random = new Random();

            deck.Cards = deck.Cards.OrderBy(c => random.Next()).ToList<Card>();

            return deck;
        }

        public string Deal(Deck deck, List<Player> players)
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < deck.Cards.Count; i++)
            {
                int index = i % players.Count;
                players[index].Cards.Add(new Card() { Name = deck.Cards[i].Name, Value = deck.Cards[i].Value, Suit = deck.Cards[i].Suit });
                output.Append($"{players[index].Name} is dealt the {deck.Cards[i].FriendlyName}<br>");
            }

            return output.ToString();
        }
    }
}