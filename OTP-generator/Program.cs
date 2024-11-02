using OTP_generator;
using SharedClass;


Otp otp = new();
UserInputValidator userInputValidator = new();


userInputValidator.RequestInput("1: One time pass\n2: Add new card\n3: Remove card\n", input =>
{
    switch (input)
    {
        case "1" or "3":
            if (otp.DisplaySuggestion())
            {
                userInputValidator.RequestInput("Please choose one of cards", userInputValidator.UserCardChoice);
                if (input == "1") otp.GenerateOtp(userInputValidator.UserCardChose);
                else otp.RemoveCardNumber(userInputValidator.UserCardChose);

                userInputValidator.RequestInput("If you want keep going type \"Y\"", input => input == "y");
                return false;
            }


            Console.WriteLine("There is no existing card for suggestions.");
            userInputValidator.RequestInput("If you want adding a card type \"Y\"", input => input == "y");
            return false;
        case "2":
            // Validating and setting the card number
            userInputValidator.RequestInput("Please enter card number", userInputValidator.InputCardNumber);
            otp.SetCardNumber(userInputValidator.CardNumber);
            // Validating and setting the CVV2
            userInputValidator.RequestInput("Please enter CVV2", userInputValidator.InputCvv2);
            otp.SetCvv2(userInputValidator.Cvv2);
            // Validating and setting the expiry date
            userInputValidator.RequestInput("Please enter expiry date (year/month)", userInputValidator.InputExpDate);
            otp.SetExpDate(userInputValidator.ExpDate);
            otp.SaveCards();

            userInputValidator.RequestInput("If you want keep going type \"Y\"", input => input == "y");
            return false;

        default:
            {
                Console.WriteLine("Invalid number. Please enter a valid number.");
                return false;
            }

    }
});





