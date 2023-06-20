using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame
{
    internal class cards
    {
        public enum Value
        {
            Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
            Jack, Queen, King, Ace
        }
        public enum Color
        {
            Hearts, Diamonds, Clubs, Spades
        }
        public class Card
        {
            public Value Rank { get; set; }
            public Color Suit { get; set; }

            public override string ToString()
            {
                return $"{Rank} of {Suit}";
            }


        }

    }
}
