using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.PropertyGridInternal;
using KlondikeWindowsForms.Properties;
using KlondikeLogic;


namespace KlondikeLogic
{
    class Draw
    {
        private Bitmap field;

        public Bitmap Field
        {
            get { return field; }
            set
            {
                field = value;
                Drawer = Graphics.FromImage(field);
                CalculateCoordinates();
            }
        }

        public GameField Game { get; set; }
        private Graphics Drawer { get; set; }
        private int CardWidth { get; set; }
        private int CardHigth { get; set; }
        private int FoundationsY { get; set; }
        private int PilesY { get; set; }
        private int[] PilesX { get; set; }
        private int[] CardInPileY { get; set; }

        public Draw(Bitmap field)
        {
            this.Field = field;
        }

        public void DrawAllObjects()
        {
            Drawer.Clear(Color.Green);
            DrawDeck();
            DrawFoundations();
            DrawPiles();
        }

        private void CalculateCoordinates()
        {

            CardWidth = field.Width / 11;
            CardHigth = CardWidth * 19 / 14; // 19:14 - пропорции карты
            FoundationsY = CardHigth / 5;
            PilesY = CardHigth * 7 / 5;
            PilesX = new int[7];
            for (int i = 0; i < 7; i++)
                PilesX[i] = CardWidth * 1 / 2 + CardWidth * 3 / 2 * i;
            CardInPileY = new int[19];
            for (int i = 0; i < 19; i++)
                CardInPileY[i] = PilesY + CardHigth / 6 * i;
        }

        private void DrawDeck()
        {
            Image cardOfMainDeck =
                Game.Deck.MainDeck.Count > 0 ? GetImageCard(Game.Deck.MainDeck.Peek()) : GetEmptyImage();
            Drawer.DrawImage(cardOfMainDeck, new Rectangle(PilesX[0], FoundationsY, CardWidth, CardHigth));
            Image cardOfDumpedDeck = Game.Deck.DumpedDeck.Count > 0
                ? GetImageCard(Game.Deck.DumpedDeck.Peek())
                : GetEmptyImage();
            Drawer.DrawImage(cardOfDumpedDeck, new Rectangle(PilesX[1], FoundationsY, CardWidth, CardHigth));
        }

        private Image GetEmptyImage()
        {
            Bitmap bitmap = new Bitmap(CardWidth, CardHigth);
            Graphics G = Graphics.FromImage(bitmap);
            G.Clear(Color.Gray);
            return bitmap;
        }

        private void DrawFoundations()
        {
            int i = 3;
            Image card;
            foreach (var foundation in Game.Foundations)
            {
                card = foundation.PileOfCards.Count > 0
                    ? GetImageCard(foundation.PileOfCards.Peek())
                    : GetImageSuit(foundation.Suits);
                Drawer.DrawImage(card,
                    new Rectangle(PilesX[i], FoundationsY, CardWidth, CardHigth));
                i++;
            }
        }

        private Image GetImageSuit(Suits suit)
        {
            if (suit == Suits.Clubs)
                return Resources.CLUBS;
            else if (suit == Suits.Diamonds)
                return Resources.DIAMONDS;
            else if (suit == Suits.Hearts)
                return Resources.HEARTS;
            else return Resources.SPADES;
        }

        private void DrawPiles()
        {
            int i = 0;
            Image imgCard;
            foreach (var pile in Game.Piles)
            {
                if (pile.Cards.Count > 0)
                {
                    int j = 0;
                    Card[] list = new Card[pile.Cards.Count];
                    list = pile.Cards.ToArray();
                    for (int k = list.Length - 1; k >= 0; k--)
                    {
                        imgCard = GetImageCard(list[k]);
                        Drawer.DrawImage(imgCard, new Rectangle(PilesX[i], CardInPileY[j], CardWidth, CardHigth));
                        j++;
                    }
                }
                else
                {
                    Drawer.DrawImage(GetEmptyImage(), new Rectangle(PilesX[i], PilesY, CardWidth, CardHigth));
                }

                i++;
            }
        }

        // проверяет попадает ли точка (x;y) в колоду
        public bool OnDeck(int x, int y)
        {
            return PilesX[0] <= x && x <= PilesX[0] + CardWidth && FoundationsY <= y &&
                   y <= FoundationsY + CardHigth;
        }

        // проверяет попадает ли точка (x;y) в сброшенную колоду
        public bool OnDumpedDeck(int x, int y)
        {
            return PilesX[1] <= x && x <= PilesX[1] + CardWidth && FoundationsY <= y &&
                   y <= FoundationsY + CardHigth;
        }

        // проверяет попадает ли точка (x;y) в дом, если да, то index-индекс дома, куда попала точка
        public bool OnFoundation(int x, int y, out int index)
        {
            index = -1;
            if (PilesX[3] <= x && x <= PilesX[3] + CardWidth && FoundationsY <= y &&
                y <= FoundationsY + CardHigth)
                index = 0;
            else if (PilesX[4] <= x && x <= PilesX[4] + CardWidth && FoundationsY <= y &&
                     y <= FoundationsY + CardHigth)
                index = 1;
            else if (PilesX[5] <= x && x <= PilesX[5] + CardWidth && FoundationsY <= y &&
                     y <= FoundationsY + CardHigth)
                index = 2;
            else if (PilesX[6] <= x && x <= PilesX[6] + CardWidth && FoundationsY <= y &&
                     y <= FoundationsY + CardHigth)
                index = 3;
            return index != -1;
        }

        // проверяет попадает ли точка (x;y) в карту в стопке, если да, то pileIndex -индекс стопки на столе, cardIndex - индекс карты в стопке 
        public bool OnPile(int x, int y, out int pileIndex, out int cardIndex)
        {
            pileIndex = -1;
            cardIndex = -1;
            for (int i = 0; i < PilesX.Length; i++)
            {
                if (PilesX[i] <= x && x <= PilesX[i] + CardWidth)
                {
                    pileIndex = i;
                    int j;
                    for (j = Game.Piles[i].Cards.Count - 1; j >= 0; j--)
                    {
                        if (CardInPileY[j] <= y &&
                            y <= CardInPileY[j] + CardHigth)
                        {
                            cardIndex = j;
                            break;
                        }
                    }

                    if (j == -1)
                    {
                        if (CardInPileY[0] <= y &&
                            y <= CardInPileY[0] + CardHigth)
                        {
                            cardIndex = 0;
                        }
                    }

                    break;
                }
            }

            return pileIndex != -1 && cardIndex != -1;
        }

        private Image GetImageCard(Card card)
        {
            if (card.FaceUp == false)
                return Resources.Back;
            else if (card.Suits == Suits.Diamonds)
            {
                switch (card.Ranks)
                {
                    case Ranks.Ace:
                        return Resources.DA;
                    case Ranks.Two:
                        return Resources.D2;
                    case Ranks.Three:
                        return Resources.D3;
                    case Ranks.Four:
                        return Resources.D4;
                    case Ranks.Five:
                        return Resources.D5;
                    case Ranks.Six:
                        return Resources.D6;
                    case Ranks.Seven:
                        return Resources.D7;
                    case Ranks.Eight:
                        return Resources.D8;
                    case Ranks.Nine:
                        return Resources.D9;
                    case Ranks.Ten:
                        return Resources.D10;
                    case Ranks.Jack:
                        return Resources.DJ;
                    case Ranks.Queen:
                        return Resources.DQ;
                    case Ranks.King:
                        return Resources.DK;
                }
            }
            else if (card.Suits == Suits.Clubs)
            {
                switch (card.Ranks)
                {
                    case Ranks.Ace:
                        return Resources.CA;
                    case Ranks.Two:
                        return Resources.C2;
                    case Ranks.Three:
                        return Resources.C3;
                    case Ranks.Four:
                        return Resources.C4;
                    case Ranks.Five:
                        return Resources.C5;
                    case Ranks.Six:
                        return Resources.C6;
                    case Ranks.Seven:
                        return Resources.C7;
                    case Ranks.Eight:
                        return Resources.C8;
                    case Ranks.Nine:
                        return Resources.C9;
                    case Ranks.Ten:
                        return Resources.C10;
                    case Ranks.Jack:
                        return Resources.CJ;
                    case Ranks.Queen:
                        return Resources.CQ;
                    case Ranks.King:
                        return Resources.CK;
                }
            }
            else if (card.Suits == Suits.Hearts)
            {
                switch (card.Ranks)
                {
                    case Ranks.Ace:
                        return Resources.HA;
                    case Ranks.Two:
                        return Resources.H2;
                    case Ranks.Three:
                        return Resources.H3;
                    case Ranks.Four:
                        return Resources.H4;
                    case Ranks.Five:
                        return Resources.H5;
                    case Ranks.Six:
                        return Resources.H6;
                    case Ranks.Seven:
                        return Resources.H7;
                    case Ranks.Eight:
                        return Resources.H8;
                    case Ranks.Nine:
                        return Resources.H9;
                    case Ranks.Ten:
                        return Resources.H10;
                    case Ranks.Jack:
                        return Resources.HJ;
                    case Ranks.Queen:
                        return Resources.HQ;
                    case Ranks.King:
                        return Resources.HK;
                }
            }
            else if (card.Suits == Suits.Spades)
            {
                switch (card.Ranks)
                {
                    case Ranks.Ace:
                        return Resources.SA;
                    case Ranks.Two:
                        return Resources.S2;
                    case Ranks.Three:
                        return Resources.S3;
                    case Ranks.Four:
                        return Resources.S4;
                    case Ranks.Five:
                        return Resources.S5;
                    case Ranks.Six:
                        return Resources.S6;
                    case Ranks.Seven:
                        return Resources.S7;
                    case Ranks.Eight:
                        return Resources.S8;
                    case Ranks.Nine:
                        return Resources.S9;
                    case Ranks.Ten:
                        return Resources.S10;
                    case Ranks.Jack:
                        return Resources.SJ;
                    case Ranks.Queen:
                        return Resources.SQ;
                    case Ranks.King:
                        return Resources.SK;
                }
            }

            return null;
        }

    }
}
