using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Casino
{
    public class Dealer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }  // dealer has a deck - it is a property
        public int Balance { get; set; }

        public void Deal(List<Card> Hand)
        {
            Hand.Add(Deck.Cards.First());
            //method - giving the dealer the ability to deal cards
            //takes for an input parameter a list of Cards, called Hand
            //gives the first item in the list and add to the "hand" which is a list
            string card = string.Format(Deck.Cards.First().ToString() + "\n");
            //logs to an external file each card that has been dealt, (append method)
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"C:\Users\catdu\OneDrive\Desktop\Basic-C-Sharp-projects\TwentyOneLog.txt", true))
                //this makes sure that the memory gets cleaned up when done - "true" is the answer to the bool in the method for append
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine(card);
            }
            Deck.Cards.RemoveAt(0); // passes in the index at which we want to remove the item
        }
    }
}
