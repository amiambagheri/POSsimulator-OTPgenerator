using System;
using System.Text.RegularExpressions;

namespace POS_simulator;

public class Validator
{

    public bool IsValidateAmount(uint inputAmount)
    {
        if (inputAmount == 0 || inputAmount > 1E9)
        {
            Console.WriteLine("Please ensure that the purchase amount input is between 0 to 1,000,000,000.");
            return false;

        }
        else return true;
    }

    public bool IsValidateCardNumber(string cardNumber)
    {
        int digitSum = 0;
        for (int i = 0; i < cardNumber.Length; i++)
        {
            int digit = (int)char.GetNumericValue(cardNumber[i]);

            if (i % 2 == 0)
            {
                digitSum += (digit * 2 > 9) ? (digit * 2) - 9 : (digit * 2);
            }
            else
            {
                digitSum += digit;
            }
        }

        if (digitSum % 10 != 0)
        {
            Console.WriteLine("is not valid, please enter valid card number");
            return false;
        }
        else return true;


    }

    public bool IsValidateCvv2(string cvv2)
    {
        return true;
    }

    public bool IsValidateExpDate(DateTime expDate)
    {
        if (expDate < DateTime.Now)
        {
            Console.WriteLine("yor card is expire");
            return false;
        }
        else return true;
    }
    public bool IsValidateOtp(string otp)
    {
        return true;
        // time is 2 min 
        // otp is for this card
    }
    public bool IsValidateTransaction()
    {
        return true;
    }
}

