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

        private GameField Game;
        private Draw Drawer;

        private void Form1_Load(object sender, EventArgs e)
        {
            Game = new GameField();
            Drawer = new Draw(new Bitmap( pictureBoxField.Width,pictureBoxField.Height));
            Drawer.Game = Game;
            Drawer.DrawDeck();

            pictureBoxField.Image = Drawer.Field;
        }

        private void Redraw()
        {
            Drawer.DrawDeck();

            pictureBoxField.Image = Drawer.Field;
        }

        private void pictureBoxField_MouseMove(object sender, MouseEventArgs e)
        {
            Redraw();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (Game != null)
            {
                Drawer.Field = (new Bitmap(pictureBoxField.Width, pictureBoxField.Height));
                Redraw();
            }
        }
    }
}
