using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MegaChallengeWar.Exceptions;

namespace MegaChallengeWar
{
    public class Game
    {
        public List<Player> Players { get; set; }

        public Deck Deck { get; set; }

        public Game(List<Player> players)
        {
            this.Players = players;
            this.Deck = Deck.BuildDeck();
        }

        public Game(List<Player> players, Deck deck)
        {
            this.Players = players;
            this.Deck = deck;
        }

        public string PlayWar()
        {
            StringBuilder output = new StringBuilder();

            output.Append(dealCards());

            output.Append("<h3>Begin Battle ...</h3>");

            // Limit the number of rounds to prevent endless games
            int roundLimit = 20;

            // TODO: Fix the check for card count to include all players
            while (roundLimit-- > 0 && this.Players[0].Cards.Count > 0)
            {
                output.Append(setupBattle());
            }

            output.Append(determineWinner());

            return output.ToString();
        }

        private string dealCards()
        {
            StringBuilder output = new StringBuilder();

            output.Append("<h3>Dealing Cards ...</h3>");

            output.Append(Deck.Deal(this.Deck, this.Players));

            return output.ToString();
        }

        private string setupBattle()
        {
            StringBuilder output = new StringBuilder();

            Dictionary<Player, Card> battleCards = new Dictionary<Player, Card>();

            foreach (var player in Players)
            {
                try
                {
                    battleCards.Add(player, getNextCard(player));
                }
                catch (OutOfCardsException)
                {
                    return "Oops. I ran out of cards to deal. This is a bug that will eventually be fixed. The game scores below will not be correct.<br>";
                }
                catch (Exception)
                {
                    continue;
                }
            }

            output.Append("<p>");

            output.Append(startBattle(battleCards));

            output.Append("</p>");

            return output.ToString();
        }

        private Card getNextCard(Player player)
        {
            if (player.Cards.Count <= 0)
            {
                throw new OutOfCardsException();
            }

            Card card = player.Cards.First<Card>();

            player.Cards.RemoveAt(0);

            return card;
        }

        private string startBattle(Dictionary<Player, Card> battleCards)
        {
            Dictionary<Player, List<Card>> bountyCards = new Dictionary<Player, List<Card>>();

            return scoreBattle(battleCards, bountyCards);
        }

        // TODO: Break the scoreBattle method down into smaller methods
        private string scoreBattle(Dictionary<Player, Card> battleCards, Dictionary<Player, List<Card>> bountyCards)
        {
            StringBuilder output = new StringBuilder();

            int currentHighestCardValue = 0;

            Dictionary<Player, Card> winners = new Dictionary<Player, Card>();

            Player winner = new Player() { };

            Card winningCard = new Card() { };

            output.Append($"<strong>Battle Cards: {String.Join(" versus ", battleCards.Select(p => p.Key.Name + "'s " + p.Value.FriendlyName).ToArray())}</strong><br>");

            foreach (var card in battleCards)
            {
                if (!bountyCards.ContainsKey(card.Key))
                {
                    bountyCards.Add(card.Key, new List<Card> { });
                }
                
                bountyCards[card.Key].Add(card.Value);

                if (card.Value.Value > currentHighestCardValue)
                {
                    currentHighestCardValue = card.Value.Value;

                    winner = card.Key;

                    winningCard = card.Value;
                }
                // TODO: Make work with more than 2 players
                // Currently, when two battle cards match, any further battle cards will be ignored
                // and all players participate in the war
                else if (card.Value.Value == currentHighestCardValue)
                {
                    // We have a tie!
                    output.Append("****** WAR ******<br>");

                    bountyCards = getBountyCards(battleCards.Keys.ToList(), bountyCards);

                    battleCards = getBattleCards(battleCards.Keys.ToList(), bountyCards);

                    // Recursion here to handle further ties
                    output.Append(scoreBattle(battleCards, bountyCards));

                    return output.ToString();
                }
            }
           
            output.Append("Bounty ...<br>");

            // This is ugly, and needs refactored
            foreach (var playerCards in bountyCards.Values)
            {
                foreach (var card in playerCards)
                {
                    output.Append($"&nbsp;&nbsp;{card.FriendlyName}<br>");

                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (Players[i].Name == winner.Name)
                        {
                            Players[i].Cards.Add(card);
                        }
                    }
                }
            }

            output.Append($"<strong>{winner.Name} wins with {winningCard.FriendlyName}!</strong>");
     
            return output.ToString();
        }

        private string determineWinner()
        {
            StringBuilder output = new StringBuilder();

            List<Player> winners = Players.OrderByDescending(p => p.Cards.Count).ToList();

            output.Append("*****************<br>");

            output.Append("<h3><strong>And the winner is ...</strong></h2>");

            output.Append($"<p><strong>{winners[0].Name} wins with {winners[0].Cards.Count} cards!</strong></p>");

            output.Append("<p>All the players' scores:<br>");

            foreach (var player in winners)
            {
                output.Append($"{player.Name}: {player.Cards.Count}<br>");
            }

            output.Append("</p>");

            return output.ToString();
        }


        const int numberOfBountyCardsToDeal = 2;

        private Dictionary<Player, List<Card>> getBountyCards(List<Player> players, Dictionary<Player, List<Card>> bountyCards)
        {
            for (int i = 0; i < numberOfBountyCardsToDeal; i++)
            {
                foreach (var player in players)
                {
                    if (!bountyCards.ContainsKey(player))
                    {
                        bountyCards.Add(player, new List<Card>() { });
                    }

                    try
                    {
                        bountyCards[player].Add(getNextCard(player));
                    }
                    catch (OutOfCardsException)
                    {
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return bountyCards;
        }

        private Dictionary<Player, Card> getBattleCards(List<Player> players, Dictionary<Player, List<Card>> bountyCards)
        {
            Dictionary<Player, Card> battleCards = new Dictionary<Player, Card>();

            foreach (var player in players)
            {
                try
                {
                    battleCards.Add(player, getNextCard(player));
                }
                catch (OutOfCardsException)
                {
                    continue;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return battleCards;
        }
    }
}