using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Deck
    {
        public Stack<Card> MainDeck { get; set; } // стопка колоды, которая лежит рубашкой вверх
        public Stack<Card> DumpedDeck { get; set; } // стопка, в которую сбрасываются карты из колоды

        public Deck()
        {
            this.MainDeck = new Stack<Card>();
            this.DumpedDeck = new Stack<Card>();
            CreateDeck();
            Shuffle();
        }

        private void CreateDeck()
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    MainDeck.Push(new Card((Suits) j, (Ranks) i));
                }
            }
        }

        private void Shuffle()
        {
            int length = MainDeck.Count;
            Card[] deck = new Card[length];
            for (int i = 0; i < length; i++) // выталкиваем все карты из стека в массив
            {
                deck[i] = MainDeck.Pop();
            }

            Random rnd = new Random();
            for (int i = deck.Length - 1; i >= 1; i--) // перемешиваем массив
            {
                int j = rnd.Next(i + 1);
                var tmp = deck[j];
                deck[j] = deck[i];
                deck[i] = tmp;
            }

            foreach (var card in deck) // загоняем обратно с стек
            {
                MainDeck.Push(card);
            }
        }

        public void FoldCard() // сбрасывает одну карту
        {
            if (MainDeck.Count > 0) // если в колоде есть карты, то сбрасываем одну
            {
                Card temp = MainDeck.Pop();
                temp.FaceUp = true;
                DumpedDeck.Push(temp);
            }
            else // иначе(колода пуста) перемещаем все сброшенные карты назад в колоду
            {
                Card temp;
                while (DumpedDeck.Count > 0)
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

        public bool CheckTake(out Card card) //проверяет можно ли взять карту из колоды
        {
            card = null;
            if (DumpedDeck.Count > 0)
            {
                card = DumpedDeck.Peek();
                return true;
            }
            return false;
        }
    }
}
