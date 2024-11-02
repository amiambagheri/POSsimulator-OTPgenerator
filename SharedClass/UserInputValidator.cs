using System.Globalization;
using System.Text.RegularExpressions;





namespace SharedClass
{

    public partial class UserInputValidator
    {
        public int UserCardChose { get; private set; }
        public string CardNumber { get; private set; } = string.Empty;
        public string Cvv2 { get; private set; } = string.Empty;
        public DateTime ExpDate { get; private set; }

        public void RequestInput(string prompt, Func<string, bool> validationMethod)
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


        public bool UserCardChoice(string input)
        {

            if (int.TryParse(input, out int choice) && choice > 0 && choice < 10)
            {
                UserCardChose = choice;
                return true;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a valid number.");
                return false;
            }
        }

        public bool InputCardNumber(string input)
        {
            var matchCardNumber = CardNumPat().Match(input);
            if (matchCardNumber.Success)
            {
                CardNumber = TrimPat().Replace(matchCardNumber.Value, "");
                return true;
            }

            Console.WriteLine("Invalid card number. Please enter a valid card number.");
            return false;
        }

        public bool InputCvv2(string input)
        {
            var matchedCvv2 = Cvv2Pat().Match(input);
            if (matchedCvv2.Success)
            {
                Cvv2 = matchedCvv2.Value;
                return true;
            }

            Console.WriteLine("Invalid CVV2. Please enter a valid CVV2.");
            return false;
        }

        public bool InputExpDate(string input)
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
                    ExpDate = expDate;
                    return true;
                }
            }

            Console.WriteLine("Invalid expiry date. Please enter a valid date in the format year/month.");
            return false;
        }

    }



    partial class UserInputValidator
    {
        [GeneratedRegex(@"^[456]\d{3}(\d{12}|([ ]\d{4}){3})$")]
        private static partial Regex CardNumPat();
    }
    partial class UserInputValidator
    {
        [GeneratedRegex(@"\D")]
        private static partial Regex TrimPat();
    }
    partial class UserInputValidator
    {
        [GeneratedRegex(@"^[1-9]\d{2,3}$")]
        private static partial Regex Cvv2Pat();
    }
    partial class UserInputValidator
    {
        // year/month
        [GeneratedRegex(@"^\d\d\/(0[1-9]|1[012])$")]
        private static partial Regex ExpDtaePat();
    }

}
