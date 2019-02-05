using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    /// <summary>
    /// Дом, то место куда нужно сложить карты
    /// </summary>
    public class Foundation
    {
        public Suits Suit { get; private set; } // масть карт, которые будут сложены в этом доме
        public Stack<Card> PileOfCards { get; set; } //стопка карт, которые лежат в этом доме

        public delegate void AddCardEventHandler();

        public event AddCardEventHandler FullFoundation;

        public Foundation(Suits suit)
        {
            this.Suit = suit;
            this.PileOfCards = new Stack<Card>();
        }

        /// <summary>
        /// Кладёт карту в дом, если карта подходит
        /// </summary>
        /// <param name="card">Карта, которую кладём</param>
        /// <returns>True, если карта положена в дом; false, если карта не положена в дом (не подходит масть или достоинство)</returns>
        public bool PutCard(List<Card> card)
        {
            if (card.Count == 1)
            {
                if ((card[0].Suit == Suit) && ((PileOfCards.Count > 0 && PileOfCards.Peek().Rank + 1 == card[0].Rank) ||
                                               (PileOfCards.Count == 0 && card[0].Suit == Suit &&
                                                card[0].Rank == Ranks.Ace)))
                {
                    PushToStack(card[0]);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Кладёт карту в дом, не проверяя подходит она или нет
        /// </summary>
        /// <param name="card"></param>
        public void PutCardsWithoutCheck(List<Card> cards)
        {
            if (cards.Count == 1)
                PushToStack(cards[0]);
        }

        /// <summary>
        /// Получает верхнюю карту из дома
        /// </summary>
        /// <param name="card">возвращаемая карта</param>
        /// <returns>True, если карта возвращена успешно; false, если карта не возвращена (дом пуст)</returns>
        public bool GetCard(out List<Card> card)
        {
            card = new List<Card>();
            if (PileOfCards.Count > 0)
            {
                card.Add(PileOfCards.Pop());
                return true;
            }

            return false;
        }

        //кладёт карту в лом и, если дом полон вызывается событие
        private void PushToStack(Card card)
        {
            PileOfCards.Push(card);
            if (PileOfCards.Count == Enum.GetNames(typeof(Ranks)).Length)
                if (FullFoundation != null)
                    FullFoundation.Invoke();
        }
    }
}
