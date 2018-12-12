using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class GameField
    {
        public Deck Deck { get; set; }
        public Foundation[] Foundations { get; set; }
        public Pile[] Piles { get; set; }

        public delegate void WinEventHandler();

        public event WinEventHandler Win;

        public GameField()
        {
            Deck = new Deck();
            Foundations = new Foundation[4];
            for (int i = 0; i < Foundations.Length; i++)
            {
                Foundations[i] = new Foundation((Suits) i);
                Foundations[i].AddCard += Completed; //подписываемся на событие добавление карты
            }

            Piles = new Pile[7];
            for (int i = 0; i < Piles.Length; i++)
            {
                Piles[i] = new Pile();
            }

            DealCards();
        }

        public void DealCards() //раздача карт из колоды в стопки
        {
            for (int i = 0; i < Piles.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Piles[i].Cards.Push(Deck.MainDeck.Pop());
                }

                Piles[i].TurnUpperCard(); //переворачиваем нижнюю карту в каждой стопке лицом вверх
            }
        }

        private void Completed() // проверяет, разложен ли пасьянс
        {
            foreach (var foundation in Foundations)
            {
                if (foundation.PileOfCards.Count < 13)
                    return;
            }

            if (Win != null)
                Win.Invoke();
        }
    }
}
