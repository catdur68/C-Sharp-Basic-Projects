using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Deck
    {
        //creating a constructor (always on top)
        //constructor = a way to assign (default) values immediately to an object upon instantiation
        //must name it exactly like the class - here "Deck"
        public Deck()
        {
            Cards = new List<Card>();//this is the first thing the constructor does: 
            for (int i = 0; i < 13; i++) // loops trhough each of the Faces
            {
                for (int j = 0; j < 4; j++) // nested for loop - loops through each Suit
                {
                    Card card = new Card(); // after each loop, create a new card
                    //assigning a Face value to the card
                    card.Face = (Face)i; // casting to (Face = datatype) - j is an int - at j=0, value in the enum = Two
                    //assigning a Suit value to the card
                    card.Suit = (Suit)j;
                    //then add this card to the list of cards
                    Cards.Add(card);
                }
            }

            

        }
        public List<Card> Cards { get; set; } //datatype = Card (from the class model Card)


        //After moving the method from the main program
            //static is gone
        public void Shuffle(int times = 1)  // Deck deck, out int timesShuffled, is gone
        {
            //timesShuffled = 0; is gone
            for (int i = 0; i < times; i++)
            {
                //timesShuffled++; is gone
                //taking a random card from the deck and putting it in another list, building a new deck of shuffled cards 1 by 1
                List<Card> TempList = new List<Card>();
                Random random = new Random();

                while (Cards.Count > 0) //deck.Cards.Count is gone
                {
                    int randomIndex = random.Next(0, Cards.Count); //deck.Cards.Count is gone
                    //create random index between 0 and 52 (52 cards in a deck)
                    TempList.Add(Cards[randomIndex]);//deck.Cards.Count is gone
                    Cards.RemoveAt(randomIndex);//deck.Cards.RemoveAt is gone
                }

                this.Cards = TempList;//deck.Cards is gone, this is optional
            }

            //return deck; not needed because it is within its class - the void in the statement means we don't need a return
        }
    }
}
//            //instantiate its property "Cards" which is a list
//            //////////////////////////////////////////
//            ///IF WE WERE TO CREATE ONE CARD AT A TIME
//            ///                                  /////
//            ///Card cardOne = new Card();
//            ///cardOne.Suit = "Hearts";
//            ///cardOne.Face = "Two";
//            ///Cards.Add(cardOne);
//            ///////////////////////////////////////////
//            ///but with a nested for loop, we can create a list of Face and a list of Suit
//            //Cards = new List<Card>(); //the datatype for Cards is defined in its class
//            // Cards is a list (plural) of Card (which is a class), the two are related and defined in Card.cs
//            List<string> Suits = new List<string>() { "Clubs", "Hearts", "Diamonds", "Spades" };
//List<string> Faces = new List<string>()
//            {
//                "Two", "Three", "Four", "Five", "Six", "Seven",
//                "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace"
//            };

//foreach (string face in Faces)
//{
//    foreach (string suit in Suits)
//    {
//        //During each Face loop... for example foreach Two...
//        //first you instantiate an object card
//        Card card = new Card();
//        //Second you set its properties
//        card.Suit = suit;
//        card.Face = face;
//        //third you add the template object to the list
//        Cards.Add(card); // add the instantiation of the face-suit combination to the list of Cards
//                         //as initialized before the loop 

//        //this loop will repeat 4 times before going to the next face (like Three)
//    }
//}//to verify this worked - code a writeLine in the program.cs
