using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    /// <summary>
    /// Карты, которые пользователь в данный момент перемещает
    /// </summary>
    public class CardOnUsersHand
    {
        private List<Card> usersCards;

        public List<Card> UsersCards
        {
            get { return usersCards; }
            set
            {
                usersCards = value;
                Flag = true;
            }
        }

        public bool Flag { get; set; } // показывает есть ли карты у пользователя

        public delegate void ReturnCards(List<Card> cards);

        public delegate void FoldCard();

        public FoldCard FoldLastCard;
        public ReturnCards DGiveBackCards;

        public CardOnUsersHand()
        {
            Flag = false;
        }

        public void GiveBackCards()
        {
            if (Flag && DGiveBackCards != null)
                DGiveBackCards(UsersCards);
            Flag = false;
        }
    }
}
