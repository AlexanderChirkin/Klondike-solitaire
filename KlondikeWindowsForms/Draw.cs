using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
            get { return field;}
            set
            {
                field = value;
                Drawer = Graphics.FromImage(field);
                cardWidth = field.Width / 11;
                cardHigth = cardWidth * 19 / 14;
            }
        }
        private Graphics Drawer { get; set; }
        public GameField Game { get; set; }
        private int cardWidth { get; set; }
        private int cardHigth { get; set; }
        private int Y { get; set; }
        public Draw(Bitmap field)
        {
            this.Field = field;
        }

        public void DrawDeck()
        {
            Image cardOfMainDeck;
            if (Game.Deck.MainDeck.Count > 0)
                //cardOfMainDeck=...
                ;
            else
            cardOfMainDeck = Resources.Back;
                Drawer.DrawImage(cardOfMainDeck, new Rectangle(Field.Width / 22, cardHigth / 3, cardWidth, cardHigth));
            Image cardOfDumpedDeck=Game.Deck.DumpedDeck.Count>0? /*  */:/*пустая картинка*/ ; 
                Drawer.DrawImage(cardOfDumpedDeck, new Rectangle(cardWidth * 11 / 6, cardHigth / 3, cardWidth, cardHigth));
        }

        public void DrawFoundations()
        {
            int i=0;
            Image card;
            foreach (var foundation in Game.Foundations)
            {
                card = foundation.HeapCards.Count>0?:/* */:/* */;
                Drawer.DrawImage(card, new Rectangle(cardWidth * 9 / 2+cardWidth*4/3*i, cardHigth / 3, cardWidth, cardHigth));
                i++;
            }
        }

        public void DrawPiles()
        {
            int i = 0;
            Image imgCard;
            foreach (var pile in Game.Piles)
            {
                if (pile.Cards.Count > 0)
                {
                    int j = 0;
                    foreach (var card in pile.Cards)
                    {
                        imgCard =  /*   */;
                        Drawer.DrawImage(imgCard, new Rectangle(cardWidth * 1 / 2 + cardWidth * 4 / 3 * i, cardHigth * 2 / 3 + cardHigth+cardHigth/5*j, cardWidth, cardHigth));
                        j++;
                    }
                }
                else
                {
                    imgCard =  /*   */;
                    Drawer.DrawImage(imgCard, new Rectangle(cardWidth * 1 / 2 + cardWidth * 4 / 3 * i, cardHigth *2 / 3+cardHigth, cardWidth, cardHigth));
                }
                i++;
            }
        }

    }
}
