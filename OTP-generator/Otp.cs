using System.Text.Json;
using POS_simulator;
using SharedClass;



namespace OTP_generator;

public class Otp
{
    public int MyProperty { get; set; }
    Validator validator = new();
    FileAddress fileAddress = new();
    CardsFile cardsData;

    static private string _cardNumber = string.Empty;
    static private string _cvv2 = string.Empty;
    static private DateTime _expDate;



    private bool ProcessCardsData()
    {
        if (!Directory.Exists(fileAddress._cardsFilePath)) Directory.CreateDirectory(fileAddress._cardsFilePath);
        if (File.Exists(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName))
        {
            var jsonString = File.ReadAllText(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName);
            cardsData = JsonSerializer.Deserialize<CardsFile>(jsonString);
            return true;
        }
        else
        {
            var cardsFile = File.Create(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName);
            cardsFile.Close();
            cardsData = new CardsFile { CardsList = [] };
            return false;
        }
    }
    public bool DisplaySuggestion()
    {
        if (ProcessCardsData())
        {
            for (var i = 0; i < cardsData.CardsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {cardsData.CardsList[i].CardNumber}");
            }
            return true;
        }
        return false;
    }
    public void GenerateOtp(int choice)
    {
        ProcessCardsData();
        Random random = new();
        var newOtp = random.Next(100000, 999999);
        cardsData.CardsList[choice - 1].LatestOtp = newOtp;
        cardsData.CardsList[choice - 1].GenerateOtpTime = DateTime.Now;
        var updatedJson = JsonSerializer.Serialize(cardsData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName, updatedJson);
        Console.WriteLine(newOtp);
    }

    public bool SetCardNumber(string cardNumber)
    {

        if (validator.IsValidateCardNumber(cardNumber))
        {
            _cardNumber = cardNumber;
            return true;
        }
        else return false;
    }

    public bool SetCvv2(string cvv2)
    {
        if (validator.IsValidateCvv2(cvv2))
        {
            _cvv2 = cvv2;
            return true;
        }
        else return false;
    }

    public bool SetExpDate(DateTime expDate)
    {

        if (validator.IsValidateExpDate(expDate))
        {
            _expDate = expDate;
            return true;
        }
        else return false;
    }
    public void SaveCards()
    {

        ProcessCardsData();

        var isExistCard = cardsData.CardsList.Any(c => c.CardNumber == _cardNumber);
        if (!isExistCard)
        {
            Cards newCard = new()
            {
                CardNumber = _cardNumber,
                Cvv2 = _cvv2,
                ExpDate = _expDate
            };

            cardsData.CardsList.Add(newCard);
            var updatedJson = JsonSerializer.Serialize(cardsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName, updatedJson);
        }
    }

    public void RemoveCardNumber(int choice)
    {
        cardsData.CardsList.RemoveAt(choice - 1);
        var updatedJson = JsonSerializer.Serialize(cardsData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileAddress._cardsFilePath + @"\" + fileAddress._cardsFileName, updatedJson);

    }


}
