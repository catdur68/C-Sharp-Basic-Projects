using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Interfaces;

namespace Casino.TwentyOne
{
    public class TwentyOneGame : Game, IWalkAway
        //the class TwentyOneGame inherits from the super class Game
        //while a class can inherit from only 1 class, it can inherite from multiple interfaces 
    {
        public TwentyOneDealer Dealer { get; set; }

        //this is going to be the method that is specific to the TwentyOneGame class
        //this method would not work for the superclass Game
        public override void Play() //adding override satisfies the contract to use this method from the base class Game
        {
            //Step 4 - initiating the dealer 
            Dealer = new TwentyOneDealer();
            foreach (Player player in Players) //there is more than one player in the list Players
                //Recall: Players is a property of the Game class, and is a list of players
            {
                player.Hand = new List<Card>(); //resets the hand of the player to no cards in hand
                player.Stay = false; 
            }
            Dealer.Hand = new List<Card>(); // we also want the dealer to get a new list of cards
            Dealer.Stay = false;
            Dealer.Deck = new Deck(); //set a new deck of cards for the dealer
            Dealer.Deck.Shuffle();
            //Console.WriteLine("Place your bet.");

            //Step 5 - initializing the bets
            foreach (Player player in Players)
            {
                //handling of potential user input error
                bool validAnswer = false;
                int bet = 0;
                while (!validAnswer)
                {
                    Console.WriteLine("Place your bet.");
                    validAnswer = int.TryParse(Console.ReadLine(), out bet);
                    //bet = Convert.ToInt32(Console.ReadLine());
                    if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals.");
                }
                if (bet < 0)
                {
                    throw new FraudException("Security! Kick this person out.");
                }
                
                bool succesfullyBet = player.Bet(bet); //where Bet is the method found in Player class, bet is the argument just obtained in previous line
                if (!succesfullyBet) //if successfullyBet is false
                {
                    return; //normally in a void method we don't return anything. Here the return means end this method
                    //the program will then return to the beginning a while loop in the main program, which will restart the Play() method
                }
                Bets[player] = bet; //Bets[player] where Bets represents the Dictionary, with player as key, and bet the value
                //the dictionary setting can be found in the Game class
            }

            //Step 6 - Distributing the cards to players and dealer (2 rounds of 1 card each)
            for (int i=0; i<2; i++)
            {
                Console.WriteLine("Dealing...");
                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name); //not WriteLine, allows the next thing to render on same line
                    Dealer.Deal(player.Hand); //calling the function Deal() from Dealer class
                    //checking for a win immediately upon the dealing is over. Win = ace + Head
                    if (i==1)
                    {
                        bool blackJack = TwentyOneRules.CheckForBlackJack(player.Hand);
                        if (blackJack)
                        {
                            Console.WriteLine("Blackjack! {0} wins {1}", player.Name, Bets[player]);
                            player.Balance += Convert.ToInt32((Bets[player]* 1.5) + Bets[player]); //blackjack rule win = 1.5bet + original bet
                            //Bets.Remove(player);
                            return; //end this game round
                        }
                    }
                }
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = TwentyOneRules.CheckForBlackJack(Dealer.Hand); //static class, so need to preceed method with the class name
                    {
                        if (blackJack)
                        {
                            Console.WriteLine("Dealer has BlackJack! Everyone loses.");
                            foreach (KeyValuePair<Player, int> entry in Bets)
                            {
                                Dealer.Balance += entry.Value;
                            }
                            return;

                        }
                    }
                }
                foreach (Player player in Players)
                {
                    while (!player.Stay) //while player stay is no
                    {
                        Console.WriteLine("Your cards are: ");
                        foreach (Card card in player.Hand)
                        {
                            Console.Write("{0} ", card.ToString());
                        }
                        Console.WriteLine("\n\nHit or Stay?");
                        string answer = Console.ReadLine().ToLower();
                        if (answer == "stay")
                        {
                            player.Stay = true;
                            break; //break the loop 
                        }
                        else if (answer == "hit")
                        {
                            Dealer.Deal(player.Hand);
                        }
                        bool busted = TwentyOneRules.isBusted(player.Hand); //returns true or false
                        if (busted)
                        {
                            Dealer.Balance += Bets[player];
                            Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);
                            Console.WriteLine("Do you want to play again?");
                            answer = Console.ReadLine().ToLower();
                            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
                            {
                                player.isActivelyPlaying = true;
                                return;
                            }
                            else
                            {
                                player.isActivelyPlaying = false;
                                return;
                            }
                        }

                    }
                }
                Dealer.isBusted = TwentyOneRules.isBusted(Dealer.Hand);
                Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
                while (!Dealer.Stay && !Dealer.isBusted)
                {
                    Console.WriteLine("Dealer is hitting...");
                    Dealer.Deal(Dealer.Hand);
                    Dealer.isBusted = TwentyOneRules.isBusted(Dealer.Hand);
                    Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
                }
                if (Dealer.Stay)
                {
                    Console.WriteLine("Dealer is staying.");
                }
                if (Dealer.isBusted)
                {
                    Console.WriteLine("Dealer busted.");
                    //players win money
                    foreach (KeyValuePair<Player, int> entry in Bets)
                    {
                        Console.WriteLine("{0} won {1}. ", entry.Key.Name, entry.Value);
                        Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);
                        //where produces a list
                        //player name is equal to the Key of the dictionary
                        //take the first name
                        //then take their balance
                        //and add to the balance the value of the winnings which is the value in the dictionary(the bet) * 2
                        Dealer.Balance -= entry.Value;
                    }
                    return; //ends the round
                }
                //we now examine the situation when both player and dealer stay
                //we want to compare the value of the player's hand to the value of the dealer's hand
                //possible outcomes: <, >, =
                foreach (Player player in Players)
                {
                    //this is the case of a bool that can deal with more than true or false
                    //and considers a null - more like "cannot say true and cannot say false"
                    //the way to do this is bool? which takes a bool datatype 
                    bool? playerWon = TwentyOneRules.CompareHands(player.Hand, Dealer.Hand);
                    if (playerWon == null)
                    {
                        Console.WriteLine("Push! No One wins.");
                        player.Balance += Bets[player];
                        
                    }
                    else if(playerWon == true)
                    {
                        Console.WriteLine("{0} wins {1}", player.Name, Bets[player]);
                        player.Balance += (Bets[player] * 2);
                        Dealer.Balance -= Bets[player];
                        Console.WriteLine("Balance {0}: {1}", player.Name, player.Balance);
                    }
                    else
                    {
                        Console.WriteLine("Dealer wins {0}", Bets[player]);
                        
                        Dealer.Balance += Bets[player];
                        
                    }
                    Console.WriteLine("Play again?");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
                    {
                        player.isActivelyPlaying = true;
                    }
                    else
                    {
                        player.isActivelyPlaying = false;
                    }
                }
                


            }


            //for now we don't want to call it, so we write this code
            //throw new NotImplementedException();
        }
        public override void ListPlayers()
        {
            
            Console.WriteLine("Players for 21 Game"); //here we are overriding the ListPlayer() method as defined in the super class Game
            base.ListPlayers();
        }
        //implementation of the IWalkAway 
        public void WalkAway(Player player)
        {
            throw new NotImplementedException();

        }

    }
}
