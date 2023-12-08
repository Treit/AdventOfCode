using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    enum HandStrength : long
    {
        Unknown = -1,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    internal class Hand : IComparable<Hand>
    {
        static readonly Dictionary<char, int> CardValues
            = new Dictionary<char, int>
            {
                { 'A', 13 },
                { 'K', 12 },
                { 'Q', 11 },
                { 'J', 10 },
                { 'T', 9 },
                { '9', 8 },
                { '8', 7 },
                { '7', 6 },
                { '6', 5 },
                { '5', 4 },
                { '4', 3 },
                { '3', 2 },
                { '2', 1 },
            };

        public string Cards { get; private set; }

        public HandStrength Strength { get; private set; }

        public Hand(string handDescription)
        {
            Cards = handDescription;
            Strength = GetStrength(Cards);
        }

        HandStrength GetStrength(string cards)
        {
            var counts = cards
                .OrderBy(x => x)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Count());

            var n = counts.Count();
            var strength = (n, counts.First()) switch
            {
                (5, _) => HandStrength.HighCard,
                (4, _) => HandStrength.OnePair,
                (3, 2) => HandStrength.TwoPair,
                (3, 3) => HandStrength.ThreeOfAKind,
                (2, 3) => HandStrength.FullHouse,
                (2, 4) => HandStrength.FourOfAKind,
                (1, _) => HandStrength.FiveOfAKind,
                _ => HandStrength.Unknown
            };

            return strength;
        }

        public int CompareTo(Hand other)
        {
            var result = Strength.CompareTo(other.Strength);
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                result = CardValues[Cards[i]].CompareTo(CardValues[other.Cards[i]]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }
    }

    internal class WildcardHand : IComparable<WildcardHand>
    {
        static readonly Dictionary<char, int> CardValues
            = new Dictionary<char, int>
            {
                { 'A', 13 },
                { 'K', 12 },
                { 'Q', 11 },
                { 'J', 0 }, // Joker, worth the least
                { 'T', 9 },
                { '9', 8 },
                { '8', 7 },
                { '7', 6 },
                { '6', 5 },
                { '5', 4 },
                { '4', 3 },
                { '3', 2 },
                { '2', 1 },
            };

        public string Cards { get; private set; }

        public string EffectiveCards { get; private set; }

        public HandStrength Strength { get; private set; }

        public WildcardHand(string handDescription)
        {
            Cards = handDescription;
            EffectiveCards = GetEffectiveCards(Cards);
            Strength = GetStrength(EffectiveCards);
        }

        HandStrength GetStrength(string cards)
        {
            var counts = cards
                .OrderBy(x => x)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Count());

            var n = counts.Count();
            var strength = (n, counts.First()) switch
            {
                (5, _) => HandStrength.HighCard,
                (4, _) => HandStrength.OnePair,
                (3, 2) => HandStrength.TwoPair,
                (3, 3) => HandStrength.ThreeOfAKind,
                (2, 3) => HandStrength.FullHouse,
                (2, 4) => HandStrength.FourOfAKind,
                (1, _) => HandStrength.FiveOfAKind,
                _ => HandStrength.Unknown
            };

            return strength;
        }

        public int CompareTo(WildcardHand other)
        {
            var result = Strength.CompareTo(other.Strength);
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                result = CardValues[Cards[i]].CompareTo(CardValues[other.Cards[i]]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        private string GetEffectiveCards(string cards)
        {
            if (cards.IndexOf('J') == -1)
            {
                return cards;
            }

            cards = new string(cards.OrderBy(x => x).ToArray());
            var highestValue = cards.ToCharArray().OrderByDescending(x => CardValues[x]).First();
            var newValue = highestValue;
            var countOfHighest = cards.Count(x => x == highestValue);

            foreach (var c in cards.Where(x => x != highestValue && x != 'J'))
            {
                var tempCount = cards.Count(x => x == c);

                if (tempCount > countOfHighest)
                {
                    countOfHighest = tempCount;
                    newValue = c;
                }
            }

            cards = cards.Replace('J', newValue);
            return cards;
        }
    }
}
