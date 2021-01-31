using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    //public means it is accessible to other parts of the program
    public struct Card
    {
        //public Card() // this is a constructor (a function)
        //assigning default values to the object upon instantiation
        //if we don't assign values in the program.cs, these will be the values by default
        //{//this is an example of instantiating 1 card
        //    Suit = "Spades";
        //    Face = "Two";
        //}
        
        
            //set object properties - 2 properties each being of an enum class datatype (see below)
        public Suit Suit { get; set; } // you can set this property or get it
        public Face Face { get; set; }

        public override string ToString()
        {
            return string.Format("{0} of {1}", Face, Suit);//custom ToString method to print to console
        }


    }

    public enum Suit
    {
        Spades,
        Diamonds,
        Hearts,
        Clubs
    }

    public enum Face
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King, 
        Ace
    }




    // in program you will need to write:
    // Card card = new Card();
    // card.Suit = Suit.Clubs;


}
