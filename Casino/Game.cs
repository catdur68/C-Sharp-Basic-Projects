using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public abstract class Game //this is the super class - The TwentyOneGame class inherits from this class
    {
        private List<Player> _players = new List<Player>();
        private Dictionary<Player, int> _bets = new Dictionary<Player, int>();
        //make the template of the class as generic as possible so it can be reused, and customized during instantiation
        
        public List<Player> Players { get { return _players; } set { _players = value; } } //replaced string with Player, since the Player class now exists
        public string Name { get; set; }
        //public string Dealer { get; set; }
        public Dictionary<Player, int> Bets { get { return _bets; } set { _bets = value; } } //property Bets = a player, an amount

        public abstract void Play(); //every class that inherits this base class MUST IMPLEMENT this method

        public virtual void ListPlayers() // this method is called in the TwentyOneGame class where it is further customized (overriden)
        {
            foreach (Player player in Players) //replaced string with Player, since the Player class now exists
            {
                Console.WriteLine(player.Name); 
            }
        }
    }
}
