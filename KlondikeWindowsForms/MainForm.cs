using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            FlagMouseDown = false;
        }

        private GameField Game { get; set; }
        private Draw Drawer { get; set; }

        private int MouseDouwnX { get; set; }
        private int MouseDouwnY { get; set; }
        private bool FlagMouseDown { get; set; }

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
            if (Game != null && this.WindowState != FormWindowState.Minimized && pictureBoxField.Height != 0)
            {
                Drawer.Field = (new Bitmap(pictureBoxField.Width, pictureBoxField.Height));
                Redraw();
            }
        }

        private void pictureBoxField_MouseClick(object sender, MouseEventArgs e)
        {
            if (Drawer.OnDeck(e.X, e.Y))
            {
                Game.FoldCard();
                Redraw();
            }
        }

        private void pictureBoxField_MouseUp(object sender, MouseEventArgs e)
        {
            int MouseUpX = e.X;
            int MouseUpY = e.Y;
            int indexOfFoundation;
            int indexOfPileFrom;
            int indexOfCardInPile;
            if (Drawer.OnPile(MouseUpX, MouseUpY, out indexOfPileFrom, out indexOfCardInPile)) //в стопку
            {
                Game.PutToPile(indexOfPileFrom);
            }
            else if (Drawer.OnFoundation(MouseUpX, MouseUpY, out indexOfFoundation)) // в дом
            {
                Game.PutToFoundation(indexOfFoundation);
            }
            else
            {
                Game.GiveBackCards();
            }

            FlagMouseDown = false;
            Redraw();
        }

        private void pictureBoxField_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDouwnX = e.X;
            MouseDouwnY = e.Y;
            FlagMouseDown = true;
            Drawer.MouseX = MouseDouwnX;
            Drawer.MouseY = MouseDouwnY;
            int indexOfFoundation;
            int indexOfPileFrom;
            int indexOfCardInPile;
            if (Drawer.OnDumpedDeck(MouseDouwnX, MouseDouwnY)) //из колоды
            {
                Game.TakeFromDeck();
            }
            else if (Drawer.OnPile(MouseDouwnX, MouseDouwnY, out indexOfPileFrom, out indexOfCardInPile)) //из стопки
            {
                Game.TakeFromPile(indexOfPileFrom, Game.Piles[indexOfPileFrom].Cards.Count - indexOfCardInPile);
            }
            else if (Drawer.OnFoundation(MouseDouwnX, MouseDouwnY, out indexOfFoundation)) // из дома
            {
                Game.TakeFromFoundation(indexOfFoundation);
            }

            Redraw();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            Redraw();
        }

        private void pictureBoxField_MouseMove(object sender, MouseEventArgs e)
        {
            if (FlagMouseDown)
            {
                Drawer.MouseX = e.X;
                Drawer.MouseY = e.Y;
                Redraw();
            }
        }
    }
}
