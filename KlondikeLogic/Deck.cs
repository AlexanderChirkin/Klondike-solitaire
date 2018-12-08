using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Deck
    {
        public Stack<Card> MainDeck { get; set; }
        public Stack<Card> DumpedDeck { get; set; }

        public Deck()
        {
            this.MainDeck = new Stack<Card>();
            this.DumpedDeck = new Stack<Card>();
            FillDeck();
            Shuffle();
        }

        private void FillDeck()
        {
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    MainDeck.Push(new Card((Suits)j, (Ranks)i));
                }
            }
        }

        private void Shuffle()
        {
            int length = MainDeck.Count;
            Card[] deck = new Card[length];
            for (int i = 0; i < length; i++)
            {
                deck[i] = MainDeck.Pop();
            }

            Random rnd = new Random();
            //shuffle

            foreach (var card in deck)
            {
                MainDeck.Push(card);
            }
        }

        public void Turn()
        {
            if (MainDeck.Count > 0)
            {
                Card temp = MainDeck.Pop();
                temp.FaceUp = true;
                DumpedDeck.Push(temp);
            }
            else
            {
                Card temp;
                for (int i = 0; i < DumpedDeck.Count; i++)
                {
                    temp = DumpedDeck.Pop();
                    temp.FaceUp = false;
                    MainDeck.Push(temp);
                }
            }
        }

        public Card TakeFromDeck()
        {
            Card card = null;
            if (DumpedDeck.Count > 0)
                card = DumpedDeck.Pop();
            return card;
        }
    }
}
