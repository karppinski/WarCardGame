using static WarCardGame.cards;
using static WarCardGame.Playerr;



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
                    Card Temp = player.Hand.Dequeue();
                    table.Add(player , Temp);
                }

                foreach(var Pair in table)
                {
                    Console.WriteLine("Player : {0} used {1} ",Pair.Key, Pair.Value );
                }
                table = table.OrderByDescending(pair => pair.Value.Rank)
                       .ToDictionary(pair => pair.Key, pair => pair.Value);
                //table = table.OrderByDescending(Card => Card.Value.Rank).ToDictionary<Player, Card>;
                
                


                for (int i = 1; i < NrOfPlayers-1; i+= 2) // check if values are not the same and if they are start the war
                {
                    if (table.ElementAt(i).Value.Rank == (table.ElementAt(i + 1).Value.Rank))
                    {
                        int FirstPlayerIndex = i-1;
                        int SecondPlayerIndex = i ;

                        int FirstPlayer = (int.Parse(table.ElementAt(FirstPlayerIndex).Key.Name.Substring(6)) -1 ) % NrOfPlayers;
                        int SecondPlayer = (int.Parse(table.ElementAt(SecondPlayerIndex).Key.Name.Substring(6)) -1) % NrOfPlayers;
                       
                        
                        /*FirstPlayer -= NrOfPlayers % 4;
                        SecondPlayer -= NrOfPlayers % 4;*/

                        bool again = true;
                        if (playerList[FirstPlayer].Hand.Count < 1 )
                        {
                            Console.WriteLine("You don't have engouh cards to continue, opponent won ! ");
                            Card card = playerList[FirstPlayer].Hand.Dequeue();
                            playerList[SecondPlayer].Hand.Enqueue(card);


                            again = false;

                        }
                        else if (playerList[SecondPlayer].Hand.Count < 1)
                        {
                            Console.WriteLine("You don't have engouh cards to continue, opponent won ! ");

                            Card card = playerList[SecondPlayer].Hand.Dequeue();
                            playerList[FirstPlayer].Hand.Enqueue(card);

                            again = false;
                        }
                        
                        while (again = true)
                        {
                            Console.WriteLine("This is a war between {0} and {1} !", table.ElementAt(i - 1).Key.Name, table.ElementAt(i).Key.Name);
                            Console.WriteLine();

                            Dictionary<Player, Card> warTableBlank = new Dictionary<Player, Card>();
                            Dictionary<Player, Card> warTable = new Dictionary<Player, Card>();

                            Player player1 = playerList[FirstPlayer];
                            Player player2 = playerList[SecondPlayer];


                            Card Temp1 = player1.Hand.Dequeue();
                            Card Temp2 = player1.Hand.Dequeue();
                            Card Temp3 = player2.Hand.Dequeue();
                            Card Temp4 = player2.Hand.Dequeue();// tutaj empty ?


                            Player FirstPlayerBlank = new Player(player1.Name + " blank ");
                            Player SecondPlayerBlank = new Player(player2.Name + " blank ");


                            warTableBlank.Add(FirstPlayerBlank, Temp1);
                            Console.WriteLine("Blank card player 1 : {0}", Temp1);
                            warTable.Add(playerList[FirstPlayer], Temp2);
                            Console.WriteLine("War card player 1 : {0}", Temp2);

                            warTableBlank.Add(SecondPlayerBlank, Temp3);
                            Console.WriteLine("Blank card player 2 : {0}", Temp3);
                            warTable.Add(playerList[SecondPlayer], Temp4);
                            Console.WriteLine("War card player 2 : {0}", Temp4);

                            warTable = warTable.OrderByDescending(pair => pair.Value.Rank)
                           .ToDictionary(pair => pair.Key, pair => pair.Value);

                            if(warTable.Count >= 2 && warTable.ElementAt(0).Value.Rank == (warTable.ElementAt(1).Value.Rank))
                            { 
                                Console.WriteLine("Another war !");
                                continue;
                            }

                            again = false;

                            Player warWinner = warTable.First().Key;
                            Console.WriteLine("Player  {0} won ", warTable.First().Key);
                            foreach (var Pair in warTable)
                            {
                                Card card = Pair.Value;
                                warWinner.Hand.Enqueue(card);
                            }
                            foreach (var Pair in warTableBlank)
                            {
                                Card card = Pair.Value;
                                warWinner.Hand.Enqueue(card);
                            }

                            break;

                        }

                        Console.ReadKey();
                        break;

                    }
                }
                


             Console.WriteLine("{0} won with: {1}",table.First().Key.Name, table.First().Value);

                Player winner = table.First().Key;
                foreach(var Pair in table)
                {
                    Card card = Pair.Value;
                    winner.Hand.Enqueue(card);
                }



                Thread.Sleep(100);
                table.Clear();

                List<Player> loosers = new List<Player>();

                foreach (Player player1 in playerList)
                {
                    if (player1.Hand.Count <= 0)
                    {
                        loosers.Add(player1);
                        Console.WriteLine("{0} lost !", player1.Name);
                        NrOfPlayers--;
                        Console.ReadKey();
                    }
                }
                playerList.RemoveAll(p => p.Hand.Count <= 0);
         

                if (playerList.Count == 1 || playerList.Any(Player => Player.Hand.Count == 52))

                    {
                    Console.WriteLine("Game is over! {0} won !", playerList[0].Name);
                    Console.ReadKey();
                    PlayAgain = false;
                    }
                Console.Clear();

                static void War()
                {

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



        

    }
}