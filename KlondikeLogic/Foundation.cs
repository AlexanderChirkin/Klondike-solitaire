using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    class Foundation
    {
        public Suit Suit { get; private set; }
        public Queue<Card> HeapCards { get; set; }

        public bool PutInFoundation(Card card)
        {
            if (card.Suit == Suit && HeapCards.Peek().Rank + 1 == card.Rank)
            {
                HeapCards.Enqueue(card);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
