namespace WarCardGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<Player> playerList = new List<Player>();
            int NrOfPlayers;
            Console.WriteLine("How much players is going to play? : ");
            NrOfPlayers = int.Parse(Console.ReadLine());

            for (int i = 0; i < NrOfPlayers; i++) 
            {
                Player player = new Player("Player " + (i+1));
                playerList.Add(player);
            }
            
            List<Card> cards = new List<Card>();


            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    Card card = new Card();
                    card.Suit = color;
                    card.Rank = value;
                    cards.Add(card);
                }
            }
           
            Stack<Card> shuffled = Shuffle(cards);



             static Stack<Card> Shuffle(List<Card> cards)
            {
                var random = new Random();
                for (int i = cards.Count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    var temp = cards[i];
                    cards[i] = cards[j];
                    cards[j] = temp;
                }

                var deck = new Stack<Card>(cards);
                return deck;
            }

            int tempo = 0;

            while(shuffled.Count > 0) 
            {
                playerList[tempo % NrOfPlayers].Hand.Enqueue(shuffled.Pop());
                tempo++;
            }

            bool PlayAgain = true;

            while (PlayAgain == true)
            {
                Dictionary<Player, Card> table = new Dictionary<Player, Card>();

                foreach (Player player in playerList)
                {
                    Card Temp = player.Hand.Dequeue();// usuwnie playeera po zzerowaniu stacku 
                    table.Add(player , Temp);
                }

                foreach(var Pair in table)
                {
                    Console.WriteLine("Player : {0} used {1} ",Pair.Key, Pair.Value );
                }
                table = table.OrderByDescending(entry => entry.Value.Rank)
                       .ToDictionary(entry => entry.Key, entry => entry.Value);
                //table = table.OrderByDescending(Card => Card.Value.Rank).ToDictionary<Player, Card>;

                foreach (var Pair in table)
                {
                    Console.WriteLine("Player : {0}  dropped {1}", Pair.Key, Pair.Value);
                }

                Console.ReadKey();

             Console.WriteLine($"{table.First().Key.Name} won with: {table.First().Value}");

                Player winner = table.First().Key;
                foreach(var Pair in table)
                {
                    Card card = Pair.Value;
                    winner.Hand.Enqueue(card);
                }



                Thread.Sleep(1000);
                table.Clear();






                if (playerList.Count == 1 || playerList.Any(Player => Player.Hand.Count == 52))

                    {
                    PlayAgain = false;
                    }
            }
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*checking hand
            * 
            * 
            * foreach(Player player1 in playerList) 
            {
                Console.WriteLine(player1.Name + " has on hand");
                foreach (Card card in player1.Hand)
                {
                    Console.WriteLine(card + ", ");
                }

            }
            



            /* checking stack

          foreach (Card ewe in shuffled)
          {
              Console.WriteLine(ewe.ToString());
          }
          Console.WriteLine("po szuflu");
          Console.ReadKey();*/

           
            /*while (shuffled.Count > 0)
            {
                Console.WriteLine("Player threw : {0} ", shuffled.Pop());

            }
            Console.ReadKey();
            /* sprawdzanie listy
             * 
            foreach (Card card in cards)
            {
                Console.WriteLine(card.ToString());
            }
            Console.ReadKey();
            Console.WriteLine();*/

            
           
        }


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