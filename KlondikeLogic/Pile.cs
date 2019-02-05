using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    /// <summary>
    /// Стопка карт на столе
    /// </summary>
    public class Pile
    {
        public Stack<Card> Cards { get; set; }

        public Pile()
        {
            Cards = new Stack<Card>();
        }

        /// <summary>
        /// Кладёт лист карт в стопку, если карты подходят
        /// </summary>
        /// <param name="cards">Лист карт, который будет положен(не положен)</param>
        /// <returns>True, если карты положены в стопку; false, если карты не положены (не подошли)</returns>
        public bool PutCards(List<Card> cards)
        {
            if (cards.Count > 0)
            {
                if (Cards.Count == 0)
                {
                    if (cards.Last().Rank == Ranks.King)
                    {
                        PutCardsWithoutCheck(cards);
                        return true;
                    }
                }
                else if (RightOrder(Cards.Peek(), cards.Last()))
                {
                    PutCardsWithoutCheck(cards);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Кладёт карты в стопку не проверяя подходят они или нет
        /// </summary>
        /// <param name="cards"> Лист карт, которые будут положены в стопку. Карты будут положены в обратном порядке</param>
        public void PutCardsWithoutCheck(List<Card> cards)
        {
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                Cards.Push(cards[i]);
            }
        }

        /// <summary>
        /// // Удаляет из стопки и возвращает лист из n карт, если их можно взять
        /// </summary>
        /// <param name="n"></param>
        /// <param name="ReturnedCards"></param>
        /// <returns></returns>
        public bool TakeFromPile(int n, out List<Card> ReturnedCards)
        {
            ReturnedCards = new List<Card>();
            if (CheckTake(n))
            {
                for (int i = 0; i < n; i++)
                {
                    ReturnedCards.Add(Cards.Pop());
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///  Переворачивает последнюю карту в стопке лицом вверх
        /// </summary>
        public void TurnUpperCard()
        {
            if (Cards.Count > 0)
                Cards.Peek().FaceUp = true;
        }

        private bool CheckTake(int n) //проверяет правильно ли сложены n последних карт в стопке
        {
            List<Card> listCards = Cards.ToList();
            if (n <= listCards.Count)
            {
                for (int i = 0; i < n - 1; i++)
                {
                    if (!RightOrder(listCards[i + 1], listCards[i]))
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool
            RightOrder(Card lower,
                Card upper) // проверяет правильно ли сложены две карты(разного цвета, ранг одной на 1 меньше ранга другой)
        {
            return lower.Rank - 1 == upper.Rank && ((int) lower.Suit + (int) upper.Suit) % 2 == 1;
        }

    }
}
