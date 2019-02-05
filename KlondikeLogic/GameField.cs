using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public CardOnUsersHand CardOnUsersHand { get; set; }

        public delegate void WinEventHandler();

        public event WinEventHandler Win;

        public GameField()
        {
            Deck = new Deck();
            Foundations = new Foundation[4];
            for (int i = 0; i < Foundations.Length; i++)
            {
                Foundations[i] = new Foundation((Suits) i);
                Foundations[i].FullFoundation += CheckComplete;
            }

            Piles = new Pile[7];
            for (int i = 0; i < Piles.Length; i++)
            {
                Piles[i] = new Pile();
            }

            CardOnUsersHand = new CardOnUsersHand();
            DealCards();
        }

        /// <summary>
        /// Раздача карт из колоды в стопки
        /// </summary>
        public void DealCards()
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

        public void GiveBackCards()
        {
            CardOnUsersHand.GiveBackCards();
        }

        public void FoldCard()
        {
            Deck.FoldCard();
        }

        public void TakeFromDeck()
        {
            if (!CardOnUsersHand.Flag)
            {
                List<Card> card;
                if (Deck.GetCard(out card))
                {
                    CardOnUsersHand.UsersCards = card;
                    CardOnUsersHand.DGiveBackCards = Deck.PutCardsWithoutCheck;
                    CardOnUsersHand.FoldLastCard = null;
                }
            }
        }

        public void TakeFromFoundation(int index)
        {
            if (!CardOnUsersHand.Flag)
            {
                List<Card> card;
                if (Foundations[index].GetCard(out card))
                {
                    CardOnUsersHand.UsersCards = card;
                    CardOnUsersHand.DGiveBackCards = Foundations[index].PutCardsWithoutCheck;
                    CardOnUsersHand.FoldLastCard = null;
                }
            }
        }

        public void TakeFromPile(int pilesIndex, int number)
        {
            if (!CardOnUsersHand.Flag)
            {
                List<Card> cards;
                if (Piles[pilesIndex].TakeFromPile(number, out cards))
                {
                    CardOnUsersHand.UsersCards = cards;
                    CardOnUsersHand.DGiveBackCards = Piles[pilesIndex].PutCardsWithoutCheck;
                    CardOnUsersHand.FoldLastCard = Piles[pilesIndex].TurnUpperCard;
                }
            }
        }

        public void PutToFoundation(int foundationsIndex)
        {
            if (CardOnUsersHand.Flag && CardOnUsersHand.UsersCards.Count == 1 &&
                Foundations[foundationsIndex].PutCard(CardOnUsersHand.UsersCards))
            {
                CardOnUsersHand.Flag = false;
                if (CardOnUsersHand.FoldLastCard != null)
                    CardOnUsersHand.FoldLastCard();
            }
            else
            {
                CardOnUsersHand.GiveBackCards();
            }
        }

        public void PutToPile(int pilesIndex)
        {
            if (CardOnUsersHand.Flag &&
                Piles[pilesIndex].PutCards(CardOnUsersHand.UsersCards))
            {
                CardOnUsersHand.Flag = false;
                if (CardOnUsersHand.FoldLastCard != null)
                    CardOnUsersHand.FoldLastCard();
            }
            else
            {
                CardOnUsersHand.GiveBackCards();
            }
        }

        private void CheckComplete() // проверяет, разложен ли пасьянс
        {
            foreach (var foundation in Foundations)
            {
                if (foundation.PileOfCards.Count < Enum.GetNames(typeof(Ranks)).Length)
                    return;
            }

            if (Win != null)
                Win.Invoke();
        }
    }
}
