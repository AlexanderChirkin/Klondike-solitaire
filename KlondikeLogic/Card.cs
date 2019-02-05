using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Card
    {
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }
        public bool FaceUp { get; set; }

        public Card(Suits suit, Ranks rank, bool faceUp = false)
        {
            this.Suit = suit;
            this.Rank = rank;
            this.FaceUp = faceUp;
        }
    }
}
