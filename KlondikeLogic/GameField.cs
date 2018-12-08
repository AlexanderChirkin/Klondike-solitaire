using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class GameField
    {
        public Deck Deck { get; set; }
        public Foundation[] Foundations { get; set; }
        public Pile[] Piles { get; set; }

        private const int NUMBER_OF_PILES = 7;

        public GameField()
        {
            Deck = new Deck();
            int numberOfSuit = Enum.GetNames(typeof(Suits)).Length;
            Foundations = new Foundation[numberOfSuit];
            for (int i = 0; i < numberOfSuit; i++)
            {
                Foundations[i] = new Foundation((Suits) i);
            }

            Piles = new Pile[NUMBER_OF_PILES];
            for (int i = 0; i < NUMBER_OF_PILES; i++)
            {
                Piles[i] = new Pile();
            }
        }
    }
}
