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
    public class Pile
    {
        public Stack<Card> Cards;

        public Pile()
        {
            Cards = new Stack<Card>();
        }

        public void PutInPile(Stack<Card> cards)
        {
            // если (в стопке ничего нет и первая карта в той стопке которую хотим положить король) или (верхняя карта в стопке и нижняя карта, которую хотим положить подходят)   
            if ((Cards.Count == 0 && cards.Peek().Ranks == Ranks.King) || TwoCardsIsRight(Cards.First(), cards.First()))
            {
                while (cards.Count > 0) //пока не положим в стопку все карты
                {
                    Cards.Push(cards.Pop());
                }
            }
        }

        //проверяет можно ли положить в стопку другую стопку из number карт с последней картой lastCard
        public bool CheckPut(Card lastCard, int number)
        {
            if (Cards.Count > 0 && Cards.First().Ranks == lastCard.Ranks + number)
            {
                if (number % 2 == 0)
                {
                    if (((int) lastCard.Suits + (int) Cards.First().Suits) % 2 == 0)
                        return true;
                }
                else if (((int) lastCard.Suits + (int) Cards.First().Suits) % 2 == 1)
                        return true;
            }
            else if (Cards.Count==0&&(int)lastCard.Ranks + number == 13) // если это король
            {
                return true;
            }

            return false;
        }

        public Stack<Card> TakeFromPile(int n)
        {
            Stack<Card> cards = new Stack<Card>();
            if (CheckTake(n))
            { 
                for (int i = 0; i<n; i++)
                {
                   cards.Push(Cards.Pop());
                }
            }
            TurnUpperCard();
            return cards;
        }

        public bool CheckTake(int n) //проверяет правильно ли сложены n последних карт в стопке
        {
            List<Card> cards = Cards.ToList();
            if (n <= cards.Count)
            {
                for (int i = 0; i < n-1; i++)
                {
                    if (!TwoCardsIsRight(cards[i+1], cards[i ]))
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TwoCardsIsRight(Card lower, Card upper) // проверяет правильно ли сложены две карты(разного цвета, ранг одной на 1 меньше ранга другой)
        {
            return lower.Ranks - 1 == upper.Ranks && ((int) lower.Suits + (int) upper.Suits) % 2 == 1;
        }

        public void TurnUpperCard()  // переворачивает последнюю карту в стопке лицом вверх
        {
            if (Cards.Count>0)
            Cards.Peek().FaceUp = true;
        }
    }
}
