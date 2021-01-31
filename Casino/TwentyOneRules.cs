using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.TwentyOne
{
    class TwentyOneRules
    {
        // make a method Private when it will only be used within the class in which it was created
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>() // Dictionary is a collection of Key Value pairs
        {
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1 // later in the game we will devise some logic so that Ace can also take the value of 11
        };

        //Get all possible hand values containing Aces
        private static int[] GetAllPossibleHandValues(List<Card> Hand) //passing in the parameter list of cards
        {
            int aceCount = Hand.Count(x => x.Face == Face.Ace); //capture the hands with Aces
            int[] result = new int[aceCount + 1];// + 1 because, it can be 1 + 1, 11 + 1, 11+11
            int value = Hand.Sum(x => _cardValues[x.Face]); //take a card, look up its value in the dictionary, and sum it (lowest value with Ace = 1)
            result[0] = value;
            if (result.Length == 1) return result;
            //going through various values of Ace with a for loop
            for (int i = 1; i < result.Length; i++) //creates a second value of 10 to each Ace
            {
                value += (i * 10);
                result[i] = value;
            }
            return result;
        }

        //Now creating a method that checks for Black Jack
        public static bool CheckForBlackJack(List<Card> Hand)
        {
            //first resolve the issue of the Ace which can take 2 values (1 or 11)
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            int value = possibleValues.Max();
            if (value == 21) return true;
            else return false;

        }

        public static bool isBusted(List<Card> Hand)
        {
            //get the value of the Hand
            int value = GetAllPossibleHandValues(Hand).Min();
            if (value > 21) return true;
            else return false;

        }

        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            foreach (int value in possibleValues)
            {
                if (value > 16 && value < 22)
                {
                    return true;
                }
            }
            return false;

        }
        
        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand);
            int[] dealerResults = GetAllPossibleHandValues(DealerHand);

            int playerScore = playerResults.Where(x => x < 22).Max();
            int dealerScore = dealerResults.Where(x => x < 22).Max();

            if (playerScore > dealerScore) return true;
            else if (playerScore < dealerScore) return false;
            else return null; 
        }

    }
}
