using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Player
    {

        //step 2 - creating a constructor for Player
        public Player (string name, int beginningBalance)
        {
            Hand = new List<Card>(); //empty list
            Balance = beginningBalance;
            Name = name;

        }

        private List<Card> _hand = new List<Card>();
        public List<Card> Hand { get { return _hand; } set { _hand = value; } }
        public int Balance { get; set; }
        public string Name { get; set; }
        public bool isActivelyPlaying { get; set; }
        public bool Stay { get; set; }

        //bet function which is specific to a player
        public bool Bet(int amount)
        {
            if (Balance -  amount < 0)
            {
                Console.WriteLine("You do not have enough to place a bet that size.");
                return false; //meaning the bet did not work
            }
            else
            {
                Balance -= amount; //updates balance after the bet
                return true;
            }
        }



        //overloading operator
        public static Game operator+ (Game game, Player player)
        //Game is the return type
        //inside parathensis is what is being added together
        //it is returning a game, because we are affecting the Game object
        {
            game.Players.Add(player);  
            //game is passed in as a parameter
            // in other words, it takes a game, adds a player to it
            return game; // and returns a game
        }
        public static Game operator- (Game game, Player player)
        //removing a player from a game
        {
            game.Players.Remove(player);
            return game; // and returns a game
        }

    }
}
