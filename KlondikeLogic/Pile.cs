using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Pile
    {
        public Stack<Card> Cards;

        public Pile()
        {
            Cards = new Stack<Card>();
        }

        public bool PutInPile(Stack<Card> cards)
        {
            if (TwoCard(Cards.Last(),cards.First()))
            {
               while (cards.Count>0)
                {
                    Cards.Push(cards.Pop());
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Stack<Card> TakeFromPile(int n)
        {
            Stack<Card> list = new Stack<Card>();
            if (IsRight(n))
            { 
                for (int i = 0; i<n; i++)
                {
                   list.Push(Cards.Pop());
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
                 //   if (!TwoCard(Cards[i], Cards[i + 1]))
                  //  return false;
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
            return lower.Ranks + 1 == upper.Ranks && ((int) lower.Suits + (int) upper.Suits) % 2 == 0;
        }

        private void TernUpperCard()
        {
            Cards.Peek().FaceUp = true;
        }
    }
}
