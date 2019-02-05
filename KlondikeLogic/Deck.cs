using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Deck
    {
        public Stack<Card> MainDeck { get; set; } // стопка колоды, которая лежит рубашкой вверх - основная колода

        public Stack<Card>
            DumpedDeck
        { get; set; } // стопка, в которую сбрасываются карты из колоды - сброшенная колода

        public Deck()
        {
            this.MainDeck = new Stack<Card>();
            this.DumpedDeck = new Stack<Card>();
            CreateDeck();
            Shuffle();
        }

        /// <summary>
        /// Сбрасывает одну карту из основной колоды в сброшенную. Если основная колода пуста перекладывает все карты из сброшенной калоды в основную.
        /// </summary>
        public void FoldCard()
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

        /// <summary>
        /// Снятие одной карты из сброшенной колоды
        /// </summary>
        /// <param name="card">Снятая карта(лист из одной карты). Если сброшенная колода пуста, будет равно null</param>
        /// <returns>True, если снятие карты прошло успешно; false, если карта не снята (сброшенная колода пуста)</returns>
        public bool GetCard(out List<Card> card)
        {
            if (DumpedDeck.Count > 0)
            {
                card = new List<Card>();
                card.Add(DumpedDeck.Pop());
                return true;
            }
            else
            {
                card = null;
                return false;
            }
        }

        public void PutCardsWithoutCheck(List<Card> cards)
        {
            if (cards.Count == 1)
                DumpedDeck.Push(cards[0]);
        }

        private void CreateDeck()
        {
            for (int i = 0; i < Enum.GetNames(typeof(Ranks)).Length; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(Suits)).Length; j++)
                {
                    MainDeck.Push(new Card((Suits)j, (Ranks)i));
                }
            }
        }

        private void Shuffle() // перемешивает карты в MainDeck
        {
            Random rnd = new Random();
            List<Card> list = new List<Card>();
            list = MainDeck.OrderBy(item => rnd.Next()).ToList();
            MainDeck.Clear();
            foreach (var card in list)
            {
                MainDeck.Push(card);
            }
        }
    }
}
