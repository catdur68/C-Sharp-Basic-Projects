using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Casino;
using Casino.TwentyOne;
using System.Data.SqlClient;

namespace TwentyOne.cs
{
    class Program
    {
        static void Main(string[] args)
        {
                     
            
            //Step 1
            Console.WriteLine("Welcome! What is your name?");
            string playerName = Console.ReadLine();
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadExceptions();
                foreach (var exception in Exceptions)
                {
                    Console.WriteLine(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();

                }
                Console.Read();
                return;
            }


            //handling of potential user input error 
            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("How much money do you want to play with today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals.");
            }

            Console.WriteLine("Are you ready to play a game of 21, {0}?", playerName);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                Player player = new Player(playerName, bank);
                //Step 2 - create a constructor in the Player class
                //Step 3 - continue with setting the game
                Game game = new TwentyOneGame(); // polymorphism happening here
                game += player; //adding a player to the game
                player.isActivelyPlaying = true; //allows for a while loop to start and stop the game
                while (player.isActivelyPlaying && player.Balance >0)
                {
                    try
                    {
                        //Start the game
                        game.Play(); //this is only one game - after that game it goes to ask the player if wants to play another game
                        //going to Step 4 - in the TwentyOneGame class
                    }
                    catch (FraudException ex) //specific exception (always to be treated before generic exception)
                    {
                        Console.WriteLine(ex.Message);
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex) //generic exception
                    {
                        Console.WriteLine("An error occured. Please contact your system administrator ");
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return; //ends the game (this is optional: without this line the player would be asked for a new bet)
                    }


                }
                game -= player; //outside of the while loop (player.isActivelyPlaying is now set to false)
                Console.WriteLine("Thank you for playing. See you again soon.");
            }
            Console.WriteLine("Feel free to watch."); //if the answer is not "yes"




            Console.Read();
        }

        //update dba with a log of all exceptions encountered
        private static void UpdateDbWithException(Exception ex)//taking the argument type Exception called ex
        {
            //connection string
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES 
                (@ExceptionType, @ExceptionMessage, @TimeStamp)" ;

            //this next line of code guarantees that the connection closes after use, good for controlling memory usage
            using (SqlConnection connection = new SqlConnection(connectionString)) //use the click action refactoring short cut
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                //add datatype
                command.Parameters.Add("@ExceptionType", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", System.Data.SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message; //Message is already a string
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                command.ExecuteNonQuery(); //nonQuery because this is an insert statement
                connection.Close();

            }
        }
        private static List<ExceptionEntity> ReadException()
        {
            //connection string
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            string queryString = @"SELECT Id, ExceptionType, ExceptionMessage, TimeStamp FROM Exceptions ";

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader(); 

                while (reader.Read())
                {
                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    Exceptions.Add(exception);


                }


                connection.Close();
            }
            return Exceptions;

        }
    }
}

////once the method originally created within the main program has been moved to its class (see below for original script)
////we just need to call the method, as follow: see line 18


////public static Deck Shuffle(Deck deck, out int timesShuffled, int times = 1)
////{
////    timesShuffled = 0;
////    for (int i = 0; i < times; i++)
////    {
////        timesShuffled++;
////        //taking a random card from the deck and putting it in another list, building a new deck of shuffled cards 1 by 1
////        List<Card> TempList = new List<Card>();
////        Random random = new Random();

////        while (deck.Cards.Count > 0)
////        {
////            int randomIndex = random.Next(0, deck.Cards.Count);
////            //create random index between 0 and 52 (52 cards in a deck)
////            TempList.Add(deck.Cards[randomIndex]);
////            deck.Cards.RemoveAt(randomIndex);
////        }

////        deck.Cards = TempList;
////    }

////    return deck;
////}
////public static Deck Shuffle(Deck deck, int times)
////{
////    for (int i = 0; i < times; i++)
////    {
////        deck = Shuffle(deck);
////    }
////    return deck;
////}

////deck = Shuffle(deck); //calling the function Shufflle defined below



/////////////////////////////////////////////////////////////////////////////////
//// public static Deck Shuffle(Deck deck, int times = 1)
////public = accessible to other parts of the program
////static = we don't need to have an object program instantiating to call this function
////Deck = type of data that the function is returning
////Shuffle = name of the function
//// (Deck deck) arguments = parameter of type Deck variable named deck

////{
////for (int i = 0; i < times; i++)
////{
////    //taking a random card from the deck and putting it in another list, building a new deck of shuffled cards 1 by 1
////    List<Card> TempList = new List<Card>();
////    Random random = new Random();

////   // while (deck.Cards.Count > 0)
////    {
////        int randomIndex = random.Next(0, deck.Cards.Count);
////        //create random index between 0 and 52 (52 cards in a deck)
////        TempList.Add(deck.Cards[randomIndex]);
////        deck.Cards.RemoveAt(randomIndex);

////    }
////    deck.Cards = TempList;
////}

////return deck; // the deck is now replenished 

////}

////option resuse this function to shuffle the deck multiple times
////public static Deck Shuffle(Deck deck, int times)
////{
////    for (int = 0; int<times; int++)
////    {
////        deck = Shuffle(deck);
////    }
////}



////////////////////////////////////////////////////////////////////////////////
////THIS WOULD CREATE A DECK OF CARDS ONE CARD AT A TIME!
////deck.Cards = new List<Card>(); // instantiating a new empty list of cards
////                               //deck has the property, Cards, and Cards is going to be a list here
////                               //Cards is the name of the list
////                               //Card is the datatype for the list (from model/class Card.cs)

//////now we can populate the list with a card, but first we instantiate a card
//////instantiating the objet cardOne based on the model (class) card
////// cardOne has a datatype of Card
////Card cardOne = new Card(); //this line alone has no value, it is an empty object
//////assigning value to the object
////cardOne.Face = "Queen";
////cardOne.Suit = "Spades";

////now we add this card to the deck
////deck.Cards.Add(cardOne);
////////////////////////////////////////////////////////////////////////////////////
////Console.WriteLine(cardOne.Face + " of " + cardOne.Suit);

////second way: making use of classes and polymorphism
//Game game = new TwentyOneGame(); //polymorphism
//game.Players = new List<Player>(); //initializing the list - a list must always be intialized
//Player player = new Player(); //creates a player
//player.Name = "Jesse"; // setting a player name to Jesse
//                       //adding player to the game TwentyOne
//game = game + player;
//game = game - player;


////instantiating the object "deck of cards" and assigning it variable "deck"
//Deck deck = new Deck(); // datatype = Deck from the model (class) Deck
//                        //int timesShuffled = 0;
//                        //deck = Shuffle(deck, out timesShuffled, 3); no longer needed like that because method was moved to its class
//deck.Shuffle(3);

////deck = Shuffle(deck: deck, times: 3);

////BECAUSE WE DON'T WANT TO DO THIS 52 TIMES, WE'LL CREATE A DECK CONSTRUCTOR IN THE DECK CLASS
////as of now with a constructor that has instantiated only 1 card, we could check that card with
////Console.WriteLine(deck.Cards[0].Face + " of " + deck.Cards[0].Suit);
////
////after the deck constructor was defined, we can verify its results by

//foreach (Card card in deck.Cards) //Card is the class, Cards is the property of deck
//                                  //cards is defined in the deck constructor as a list
//{
//    Console.WriteLine(card.Face + " of " + card.Suit);
//}
//Console.WriteLine(deck.Cards.Count); //Cards is a property of deck, which has a datatype of list, and
//                                     //list has a property called Count
//                                     //Console.WriteLine("Times deck was shuffled:{0} ", timesShuffled); no longer needed as the method was moved to its own class

////instantiate a TwentyOneGame
////first simple method not using classes and polymorphism
////TwentyOneGame game = new TwentyOneGame();
////now we have access to the properties of the generic game object - inheriting from superclass Game, giving access to the superclass properties
////game.Dealer = "jesse";
////game.Players = new List<string> { "Bob", "Eric", "Michael"};//initializing with values
////game.ListPlayers(); //calling the superclass method associated with the object game which is going to write the values found in the list
////game.Play(); //this is calling the Play() method from the TwentyOneGame class - remove the throw NotImplemented exception to activate the method


