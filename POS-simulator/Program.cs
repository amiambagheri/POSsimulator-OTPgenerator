using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using POS_simulator;

Pos pos = new();


void RequestInput(string prompt, Func<string, bool> validationMethod)
{
    bool isValid = false;
    while (!isValid)
    {
        Console.WriteLine(prompt + " or type 'EXIT'.");
        string input = Console.ReadLine() ?? string.Empty;
        if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);
        isValid = validationMethod(input);
    }
}

// Main menue
RequestInput("1: buying\n2: show transactions\n", input =>
{
    switch (input)
    {
        case "2":
            RequestInput("Please enter how many recent transactions you want to view, between 0 and 20.", input =>
                    {
                        if (int.TryParse(input, out int choice) && choice <= 20)
                        {
                            pos.DisplayTransactions(choice);
                            return true;
                        }
                        Console.WriteLine("Invalid number. Please enter a valid number between 0 and 20.");
                        return false;
                    });

            RequestInput("If you want keep going type \"Y\"", input => input == "y");
            return false;
        case "1":
            // Validating and setting the purchase amount
            RequestInput("Please enter purchase amount", input =>
            {
                var matchAmount = AmountPat().Match(input);
                if (matchAmount.Success)
                {
                    var trimedAmount = TrimPat().Replace(input, "");

                    if (uint.TryParse(trimedAmount, out uint amount) && amount <= 1_000_000_000)
                        return pos.SetPurchaseAmount(amount);
                }


                Console.WriteLine("Invalid amount. Please enter a valid number between 0 and 1,000,000,000.");
                return false;
            });

            // Validating and setting the card number
            RequestInput("Please enter card number", input =>
            {
                var matchCardNumber = CardNumPat().Match(input);
                if (matchCardNumber.Success)
                {
                    string trimmedCardNumber = TrimPat().Replace(matchCardNumber.Value, "");
                    return pos.SetCardNumber(trimmedCardNumber);
                }

                Console.WriteLine("Invalid card number. Please enter a valid card number.");
                return false;
            });

            // Validating and setting the CVV2
            RequestInput("Please enter CVV2", input =>
            {
                var matchedCvv2 = Cvv2Pat().Match(input);
                if (matchedCvv2.Success)
                    return pos.SetCvv2(matchedCvv2.Value);

                Console.WriteLine("Invalid CVV2. Please enter a valid CVV2.");
                return false;
            });

            // Validating and setting the expiry date
            RequestInput("Please enter expiry date (year/month)", input =>
            {
                var matchedExpDate = ExpDtaePat().Match(input);
                if (matchedExpDate.Success)
                {
                    string[] dateParts = matchedExpDate.Value.Split('/');
                    if (int.TryParse(dateParts[0], out int year) && int.TryParse(dateParts[1], out int month))
                    {
                        year += 1400;
                        month += 1;
                        PersianCalendar persianCalendar = new();
                        DateTime expDate = persianCalendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
                        return pos.SetExpDate(expDate);
                    }
                }

                Console.WriteLine("Invalid expiry date. Please enter a valid date in the format year/month.");
                return false;
            });

            // Validating and setting the OTP
            RequestInput("Please enter 6-digit One Time Pass (OTP)", input =>
            {
                var matchedOtp = OtpPat().Match(input);
                if (matchedOtp.Success)
                    return pos.SetOtp(matchedOtp.Value);

                Console.WriteLine("Invalid OTP. Please enter a valid 6-digit OTP.");
                return false;
            });

            pos.SetStatus();
            RequestInput("If you want keep going type \"Y\"", input => input == "y");
            return false;
        default:
            Console.WriteLine("Invalid number. Please enter a valid number.");
            return false;

    }
});



partial class Program
{
    [GeneratedRegex(@"^[1-9](?:[,]?\d){0,9}$")]
    private static partial Regex AmountPat();
}
partial class Program
{
    [GeneratedRegex(@"^[456]\d{3}(\d{12}|([ ]\d{4}){3})$")]
    private static partial Regex CardNumPat();
}

partial class Program
{
    [GeneratedRegex(@"\D")]
    private static partial Regex TrimPat();
}
partial class Program
{
    [GeneratedRegex(@"^[1-9]\d{2,3}$")]
    private static partial Regex Cvv2Pat();
}
partial class Program
{
    // year/month
    [GeneratedRegex(@"^\d\d\/(0[1-9]|1[012])$")]
    private static partial Regex ExpDtaePat();
}
partial class Program
{
    [GeneratedRegex(@"^[1-9]\d{5}$")]
    private static partial Regex OtpPat();
}
