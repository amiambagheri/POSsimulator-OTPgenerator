using System;
using System.Text.Json;

namespace POS_simulator;

public class Transaction
{
    public uint PurchaseAmount { get; set; }
    public Currency Currency;
    public string CardNumber { get; set; } = string.Empty;
    public Status Status;
    public DateTime TransactionTime { get; set; }
}
