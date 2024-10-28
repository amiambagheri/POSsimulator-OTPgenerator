using System;

namespace POS_simulator;

public class Cards
{
    public string CardNumber { get; set; } = string.Empty;
    public string CVV2 { get; set; } = string.Empty;
    public DateTime ExpDate { get; set; }
}
