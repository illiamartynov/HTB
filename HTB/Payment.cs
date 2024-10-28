namespace HTB;

using System;

public class Payment
{
    public int PaymentID { get; set; }
    public float Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    
    public static string Currency { get; set; } = "USD";

    public Payment(int paymentID, float amount, DateTime paymentDate, string paymentMethod, string currency = "USD")
    {
        PaymentID = paymentID;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
        Currency = currency;
    }

    public void ProcessPayment()
    {
        Console.WriteLine($"Processed payment of {Amount} in {Currency} (converted to USD).");
    }

    public void ViewPaymentDetails()
    {
        Console.WriteLine($"PaymentID: {PaymentID}, Amount: {Amount} in {Currency}, Method: {PaymentMethod}, Date: {PaymentDate}");
    }
}