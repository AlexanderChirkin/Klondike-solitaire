using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    class Pile
    {
        public List<Card> Cards;

        public bool PutInPile(List<Card> cards)
        {
            if (TwoCard(Cards.Last(),cards.First()))
            {
                Cards.AddRange(cards);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Card> TakeFromPile(int n)
        {
            List<Card> list = new List<Card>();
            if (IsRight(n))
            { 
                for (int i = Cards.Count - n; i < Cards.Count; i++)
                {
                    list.Add(Cards[i]);
                    list.RemoveAt(i);
                }
            }
            return list;
        }

        private bool IsRight(int n)
        {
            if (n <= Cards.Count)
            {
                for (int i = Cards.Count - n; i < Cards.Count - 1; i++)
                {
                    if (!TwoCard(Cards[i], Cards[i + 1]))
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TwoCard(Card lower, Card upper)
        {
            return lower.Rank + 1 == upper.Rank && ((int) lower.Suit + (int) upper.Suit) % 2 == 0;
        }
    }
}
