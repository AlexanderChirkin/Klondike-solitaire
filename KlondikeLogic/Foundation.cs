using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Foundation
    {
        public Suits Suits { get; private set; }
        public Stack<Card> HeapCards { get; set; }

        public Foundation(Suits suit)
        {
            this.Suits = suit;
            HeapCards = new Stack<Card>();
        }

        public bool PutInFoundation(Card card)
        {
            if (card.Suits == Suits && HeapCards.Peek().Ranks + 1 == card.Ranks)
            {
                HeapCards.Push(card);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Card GetFromFoundation()
        {
            Card returnedCard = null;
            if (HeapCards.Count > 0)
            {
                returnedCard = HeapCards.Pop();
            }
            return returnedCard;
        }
    }
}
