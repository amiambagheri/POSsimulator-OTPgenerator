using System;
using System.Runtime;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace POS_simulator;

public class Pos
{
    static readonly Validator validator = new();
    static private TransactionsFile transactionsData;
    static private CardsFile cardsData;

    static private uint _purchaseAmount;
    static private string _cardNumber = string.Empty;
    static private string _cvv2 = string.Empty;
    static private string _otp = string.Empty;
    static private Status _status;
    static private DateTime _expDate;

    static private string _transactionFilePath = @"D:\c# project\POSsimulator-OTPgenerator\Transactions";
    static private string _transactionFileName = "Transactions.json";
    static private string _cardsFilePath = @"D:\c# project\POSsimulator-OTPgenerator\Cards";
    static private string _cardsFileName = "Cards.json";

    public bool SetPurchaseAmount(uint inputAmount)
    {
        if (validator.IsValidateAmount(inputAmount))
        {
            _purchaseAmount = inputAmount;
            return true;
        }
        else return false;
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
    public bool SetOtp(string otp)
    {
        if (validator.IsValidateOtp(otp))
        {
            _otp = otp;
            return true;
        }
        else return false;
    }

    public void SetStatus()
    {
        if (validator.IsValidateTransaction())
        {
            _status = Status.successful;
            Console.Write("Transaction is ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Status.successful);
            Console.ResetColor();
            Console.WriteLine(".");
        }
        else
        {
            _status = Status.Failed;
            Console.Write("Transaction is ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Status.Failed);
            Console.ResetColor();
            Console.WriteLine(".");
        }

        SaveTransaction();
    }
    private static bool ReadTransaction()
    {
        if (File.Exists(_transactionFilePath + @"\" + _transactionFileName))
        {
            var jsonString = File.ReadAllText(_transactionFilePath + @"\" + _transactionFileName);
            transactionsData = JsonSerializer.Deserialize<TransactionsFile>(jsonString);
            return true;
        }
        else return false;
    }
    public void SaveTransaction()
    {
        if (!ReadTransaction())
        {
            if (!Directory.Exists(_transactionFilePath)) Directory.CreateDirectory(_transactionFilePath);
            var transactionFile = File.Create(_transactionFilePath + @"\" + _transactionFileName);
            transactionFile.Close();
            transactionsData = new TransactionsFile { TransactionsList = [] };

        }

        Transaction newTransaction = new()
        {
            PurchaseAmount = _purchaseAmount,
            CardNumber = _cardNumber,
            Currency = Currency.IRRial,
            TransactionTime = DateTime.Now,
            Status = _status
        };

        transactionsData.TransactionsList.Add(newTransaction);

        var updatedJson = JsonSerializer.Serialize(transactionsData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_transactionFilePath + @"\" + _transactionFileName, updatedJson);

        SaveCards();
    }
    private static bool ProcessCardsData()
    {
        if (!Directory.Exists(_cardsFilePath)) Directory.CreateDirectory(_cardsFilePath);
        if (File.Exists(_cardsFilePath + @"\" + _cardsFileName))
        {
            var jsonString = File.ReadAllText(_cardsFilePath + @"\" + _cardsFileName);
            cardsData = JsonSerializer.Deserialize<CardsFile>(jsonString);
            return true;
        }
        else
        {
            var cardsFile = File.Create(_cardsFilePath + @"\" + _cardsFileName);
            cardsFile.Close();
            cardsData = new CardsFile { CardsList = [] };
            return false;
        }
    }
    private static void SaveCards()
    {

        ProcessCardsData();

        var isExistCard = cardsData.CardsList.Any(c => c.CardNumber == _cardNumber);
        if (!isExistCard)
        {
            Cards newCard = new()
            {
                CardNumber = _cardNumber,
                CVV2 = _cvv2,
                ExpDate = _expDate
            };

            cardsData.CardsList.Add(newCard);
            var updatedJson = JsonSerializer.Serialize(cardsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_cardsFilePath + @"\" + _cardsFileName, updatedJson);
        }
    }

    public void DisplayTransactions(int numberOfTransaction)
    {
        if (ReadTransaction())
        {
            var transactionCount = transactionsData.TransactionsList.Count;
            if (transactionCount > 0)
            {
                if (numberOfTransaction > transactionCount)
                {
                    numberOfTransaction = transactionCount;
                    Console.WriteLine($"just exist {transactionCount} transaction :");
                }
                Console.WriteLine("----------------");
                for (int i = transactionCount - 1; i > transactionCount - 1 - numberOfTransaction; i--)
                {
                    Console.WriteLine(transactionsData.TransactionsList[i].PurchaseAmount + " " + transactionsData.TransactionsList[i].Currency);
                    Console.WriteLine(transactionsData.TransactionsList[i].CardNumber);
                    Console.WriteLine(transactionsData.TransactionsList[i].TransactionTime);
                    if (transactionsData.TransactionsList[i].Status == Status.successful) Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(transactionsData.TransactionsList[i].Status);
                    Console.ResetColor();
                    Console.WriteLine("----------------");
                }
            }
            else Console.WriteLine("no transaction exist");
        }
    }

}
