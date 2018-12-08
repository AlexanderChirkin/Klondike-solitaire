using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlondikeLogic
{
    public class Card
    {
        public Suits Suits { get; private set; }
        public Ranks Ranks { get; private set; }
        public bool FaceUp { get; set; }

        public Card(Suits suits, Ranks ranks, bool faceUp = false)
        {
            this.Suits = suits;
            this.Ranks = ranks;
            this.FaceUp = faceUp;
        }
    }
}
