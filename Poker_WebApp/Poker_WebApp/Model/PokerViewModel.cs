using Poker_WebApp.Service;
using Poker_WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using static Poker_WebApp.Enums.Poker;
using static Poker_WebApp.Model.PokerViewModel;

namespace Poker_WebApp.Model
{
    public class PokerViewModel
    {
        public class CardStrength
        {
            public string Description { get; set; }
            public float Strength { get; set; }
        }
    }

    public class PokerPlayer
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public CardStrength CardStrength { get; set; }

        public void CandOnHand(Card card)
        {
            if (Hand == null)
            {
                Hand = new List<Card>();
            }

            Hand.Add(card);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine(Name + ": " );
            foreach (var card in Hand)
            {
                s.Append(card + ",");
            }
            s.AppendLine("(" + CardStrength.Description + ")");
            s.AppendLine();

            return s.ToString();
        }
    }
    public class Card
    {
        public Suit Suit { get; set; }
        public CardValue Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Value.Description(), Suit.Description());
        }
    }


    public class PokerGame
    {
        private PokerDeck PokerDeck { get; set; }
        private List<PokerPlayer> Players { get; set; }

        private static readonly string defaultNumberOfCardsPerPerson = ConfigurationManager.AppSettings["NumberOfCardsPerPerson"].ToString();

        public PokerGame(int defaultNumberOfPlayers)
        {
            CreateGamePlayers(defaultNumberOfPlayers);
        }

        public void InitializeDeck()
        {
            PokerDeck = new PokerDeck();
            PokerDeck.RandomCardPicker();
            SettleCard();
        }

        public void InitializeGame()
        {
            Players.ForEach(p => p.CardStrength = PokerService.DetermineCardStrength(p.Hand));
        }

        private void CreateGamePlayers(int defaultNumberOfPlayers)
        {
            Players = new List<PokerPlayer>();

            for (var i = 1; i <= defaultNumberOfPlayers; i++)
            {

                Players.Add(new PokerPlayer
                {

                    Name = string.Format("Poker Player {0}", i)
                });


            }
        }

        protected void SettleCard()
        {

            for (var i = 1; i <= int.Parse(defaultNumberOfCardsPerPerson); i++)
            {
                foreach (var player in Players)
                {
                    player.CandOnHand(PokerDeck.SubmitCard());
                }
            }
        }

        public string Hands()
        {
            var sb = new StringBuilder();
            foreach (var player in Players)
            {
                sb.Append(player);
            }

            return sb.ToString();
        }

        public string Winner()
        {
            var winner = Players
                .OrderByDescending(p => p.CardStrength.Strength)
                .First();

            var tiedGamePlayers = Players.Where(p => p.CardStrength == winner.CardStrength).ToList();

            if (tiedGamePlayers.Count() > 1)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Tie between ", tiedGamePlayers.Count());
                for (var i = 0; i < tiedGamePlayers.Count(); i++)
                {
                    sb.Append(i == tiedGamePlayers.Count - 1 ? " & " : ", ");
                    sb.AppendFormat(tiedGamePlayers[i].Name);
                }
                return sb.ToString();
            }

            return string.Format("Given the following players and their hands, the winner is: {0} with a {1}", winner.Name, winner.CardStrength.Description);
        }
    }

    public class PokerDeck
    {
        private List<Card> Cards { get; set; }

        public PokerDeck()
        {
            BuildCard();
        }

        public int CardOnHandCount {
            get { return Cards.Count; }
        }

        public Card SubmitCard()
        {
           
            var card = Cards.First();
            Cards.Remove(card);

            return card;
        }

        public void RandomCardPicker()
        {
            var randomCardPicker = new Random();
            for (var randomCard = 0; randomCard < Cards.Count; randomCard++)
            {
                var randomCardVal = randomCardPicker.Next(randomCard, Cards.Count);
                var tempVar = Cards[randomCard];
                Cards[randomCard] = Cards[randomCardVal];
                Cards[randomCardVal] = tempVar;
            }
        }

        private void BuildCard()
        {
            Cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    Cards.Add(new Card
                    {
                        Value = value,
                        Suit = suit
                    });
                }
            }
        }
    }




}
