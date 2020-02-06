using Poker_WebApp.Model;
using System.Collections.Generic;
using System.Linq;
using static Poker_WebApp.Enums.Poker;
using static Poker_WebApp.Model.PokerViewModel;

namespace Poker_WebApp.Service
{
    public class PokerService
    {
        public static CardStrength DetermineCardStrength(IList<Card> cards)
        {
            #region  default variable
            var pokerDescription = string.Empty;
            var cardStength = new CardStrength();
            var cardValuelist = cards.Select(c => (int)c.Value).ToList();
            var category = PokerHand.NotSet;
            var first = 0;
            var second = 0;
            var third = 0;
            var third_2 = 0;
            var third_3 = 0;
            var highCard = cardValuelist.Max();
            var vals = cardValuelist.GroupBy(x => x).ToList();
            var pairVal = vals.Any(g => g.Count() > 1);
            #endregion

            #region Poker Hand (Pair)
            if (pairVal)
            {
                #region threeOfAKind
                var threeOfAKind = vals.FirstOrDefault(g => g.Count() == 3);
                var pairs = vals.Where(g => g.Count() == 2).ToList();

                if (threeOfAKind != null && pairs.Any())
                {
                    category = PokerHand.FullHouse;
                    first = threeOfAKind.Key;
                    second = pairs.First().Key;
                    pokerDescription = string.Format("FullHouse, {0}s over {1}s", (CardValue)threeOfAKind.Key, (CardValue)pairs.First().Key);
                }
                else if (threeOfAKind != null)
                {
                    category = PokerHand.ThreeOfAKind;
                    first = threeOfAKind.Key;
                    second = cardValuelist.Where(x => x != threeOfAKind.Key).Max();
                    third = cardValuelist.Where(x => x != threeOfAKind.Key).Min();
                    pokerDescription = string.Format("Set of {0}s", (CardValue)threeOfAKind.Key);
                }
                #endregion
                #region TwoPair
                else if (pairs.Count() == 2)
                {
                    category = PokerHand.TwoPair;
                    first = pairs.Select(p => p.Key).Max();
                    second = pairs.Select(p => p.Key).Min();
                    third = vals.First(g => g.Count() == 1).Key;
                    pokerDescription = string.Format("Two Pair, {0}s over {1}s", (CardValue)first, (CardValue)second);
                }
                #endregion
                #region OnePair
                else if (pairs.Count == 1)
                {
                    category = PokerHand.OnePair;
                    first = pairs.First().Key;
                    var otherCards = vals.Where(g => g.Count() == 1).Select(g => g.Key).OrderBy(x => x).ToList();
                    second = otherCards[0];
                    third = otherCards[1];
                    third_2 = otherCards[2];
                    pokerDescription = string.Format("Pair of {0}s", (CardValue)first);
                }
                #endregion
            }
            #endregion
            #region Poker Hand HighCard (NotSet)
            if (category == PokerHand.NotSet)
            {
                #region HighCard
                category = PokerHand.HighCard;
                var orderedCards = cardValuelist.OrderByDescending(x => x).ToList();
                first = orderedCards[0];
                second = orderedCards[1];
                third = orderedCards[2];
                third_2 = orderedCards[3];
                third_3 = orderedCards[4];
                pokerDescription = (CardValue)first + " HighCard";
                #endregion
            }
            #endregion

            var result = ((int)category * 10000000000) + (first * 100000000) + (second * 1000000) +  (third * 10000) + (third_2 * 100) +  third_3;

            cardStength.Description = pokerDescription;
            cardStength.Strength = result;

            return cardStength;
        }
    }
}
