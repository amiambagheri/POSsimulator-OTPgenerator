using System;

namespace POS_simulator;

public class Cards
{
    public string CardNumber { get; set; } = string.Empty;
    public string Cvv2 { get; set; } = string.Empty;
    public int LatestOtp { get; set; }
    public DateTime GenerateOtpTime { get; set; }
    public DateTime ExpDate { get; set; }

}
