using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WarCardGame.cards;

namespace WarCardGame
{
    internal class Playerr
    {
        public class Player
        {
            public Player(string name)
            {
                Name = name;
                Hand = new Queue<Card>();
            }

            public string Name { get; set; }
            public Queue<Card> Hand { get; set; }

            public override string ToString()
            {
                return Name;
            }

        }
    }
}
