using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Foundation
    {
        public Suits Suits { get; set; }
        public Stack<Card> PileOfCards { get; set; }

        public delegate void AddCardEventHandler();

        public event AddCardEventHandler AddCard;

        public Foundation(Suits suit)
        {
            this.Suits = suit;
            this.PileOfCards = new Stack<Card>();
        }

        public void PutInFoundation(Card card) // кладёт карту в дом
        {
            if (PileOfCards.Count > 0)
            {
                if (card.Suits == Suits && PileOfCards.Peek().Ranks + 1 == card.Ranks)
                {
                    PushToStack(card);
                }
            }
            else if (card.Suits == Suits && card.Ranks == Ranks.Ace)
            {
                PushToStack(card);
            }
        }

        private void PushToStack(Card card)
        {
            PileOfCards.Push(card);
            if (AddCard != null)
                AddCard.Invoke();
        }

        public bool CheckPut(Card card) //проверяет можно ли положить карту в дом
        {
            if (PileOfCards.Count > 0)
            {
                //если дом не пуст
                return (card.Suits == Suits && PileOfCards.Peek().Ranks + 1 == card.Ranks);
            }
            else
            {
                return (card.Suits == Suits && card.Ranks == Ranks.Ace);
            }
        }

        public Card GetFromFoundation()
        {
            Card returnedCard = null;
            if (PileOfCards.Count > 0)
            {
                returnedCard = PileOfCards.Pop();
            }

            return returnedCard;
        }

        public bool CheckGet(out Card card) // проверяет можно ли взять карту из дома
        {
            card = null;
            if (PileOfCards.Count > 0)
            {
                card = PileOfCards.Peek();
                return true;
            }

            return false;
        }
    }
}
