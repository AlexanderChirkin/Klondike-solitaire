using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KlondikeLogic;

namespace KlondikeWindowsForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private GameField Game { get; set; }
        private Draw Drawer { get; set; }

        private int MouseDouwnX { get; set; }
        private int MouseDouwnY { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewGame();
            Redraw();
        }

        private void NewGame()
        {
            Game = new GameField();
            Game.Win += ShowMessageWin;
            Drawer = new Draw(new Bitmap(pictureBoxField.Width, pictureBoxField.Height));
            Drawer.Game = Game;
        }

        public void ShowMessageWin()
        {
            Redraw();
            if (MessageBox.Show("Поздравляем! Вы победили! Начать заново?", "Сообщение", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                NewGame();
                Redraw();
            }
        }

        private void Redraw()
        {
            Drawer.DrawAllObjects();
            pictureBoxField.Image = Drawer.Field;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (Game != null && this.WindowState != FormWindowState.Minimized&& pictureBoxField.Height!=0)
            {
                Drawer.Field = (new Bitmap(pictureBoxField.Width, pictureBoxField.Height));
                Redraw();
            }
        }

        private void pictureBoxField_MouseClick(object sender, MouseEventArgs e)
        {
            if (Drawer.OnDeck(e.X, e.Y))
            {
                Game.Deck.FoldCard();
                Redraw();
            }
        }

        private void pictureBoxField_MouseUp(object sender, MouseEventArgs e)
        {
            int indexOfFoundation;
            int indexOfPileTo;
            int indexOfPileFrom;
            int indexOfCardInPile;
            if (Drawer.OnFoundation(e.X, e.Y, out indexOfFoundation)) // переносим в дом
            {
                if (Drawer.OnDumpedDeck(MouseDouwnX, MouseDouwnY)) //из колоды
                {
                    Card card;
                    if (Game.Deck.CheckTake(out card) && Game.Foundations[indexOfFoundation].CheckPut(card))
                    {
                        card = Game.Deck.TakeFromDeck();
                        Game.Foundations[indexOfFoundation].PutInFoundation(card);
                    }
                }
                else if (Drawer.OnPile(MouseDouwnX, MouseDouwnY, out indexOfPileFrom, out indexOfCardInPile)) //из стопки
                {
                    if ((Game.Piles[indexOfPileFrom].Cards.Count - indexOfCardInPile) == 1)
                    {
                        if (Game.Piles[indexOfPileFrom].CheckTake(1))
                        {
                            if (Game.Foundations[indexOfFoundation].CheckPut(Game.Piles[indexOfPileFrom].Cards.Peek()))
                            {
                                Card fromList = Game.Piles[indexOfPileFrom].TakeFromPile(1).Pop();
                                Game.Foundations[indexOfFoundation]
                                    .PutInFoundation(fromList);
                            }
                        }
                    }
                }
            }
            else if (Drawer.OnPile(e.X, e.Y, out indexOfPileTo, out indexOfCardInPile)) // переносим в стопку
            {
                if (Drawer.OnDumpedDeck(MouseDouwnX, MouseDouwnY)) // из колоды
                {
                    Card card;
                    if (Game.Deck.CheckTake(out card))
                    {
                        if (Game.Piles[indexOfPileTo].CheckPut(card, 1))
                        {
                            card = Game.Deck.TakeFromDeck();
                            Stack<Card> cards = new Stack<Card>();
                            cards.Push(card);
                            Game.Piles[indexOfPileTo].PutInPile(cards);
                        }
                    }
                }
                else if (Drawer.OnFoundation(MouseDouwnX, MouseDouwnY, out indexOfFoundation)) // из дома
                {
                    Card card;
                    if (Game.Foundations[indexOfFoundation].CheckGet(out card))
                    {
                        if (Game.Piles[indexOfPileTo].CheckPut(card, 1))
                        {
                            card = Game.Foundations[indexOfFoundation].GetFromFoundation();
                            Stack<Card> cards = new Stack<Card>();
                            cards.Push(card);
                            Game.Piles[indexOfPileTo].PutInPile(cards);
                        }
                    }
                }
                else if (Drawer.OnPile(MouseDouwnX, MouseDouwnY, out indexOfPileFrom, out indexOfCardInPile)) // из другой стопки
                {
                    if (Game.Piles[indexOfPileFrom]
                        .CheckTake((Game.Piles[indexOfPileFrom].Cards.Count - indexOfCardInPile)))
                    {
                        if (Game.Piles[indexOfPileTo].CheckPut(Game.Piles[indexOfPileFrom].Cards.Peek(),
                            Game.Piles[indexOfPileFrom].Cards.Count - indexOfCardInPile))
                        {
                            Game.Piles[indexOfPileTo].PutInPile(Game.Piles[indexOfPileFrom]
                                .TakeFromPile(Game.Piles[indexOfPileFrom].Cards.Count - indexOfCardInPile));
                        }

                    }
                }
            }
            Redraw();
        }

        private void pictureBoxField_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDouwnX = e.X;
            MouseDouwnY = e.Y;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            Redraw();
        }
    }
}
