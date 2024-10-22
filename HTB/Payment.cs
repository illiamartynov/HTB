namespace HTB;

using System;

public class Payment
{
    public int PaymentID { get; set; }
    public float Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public static string Currency { get; set; } = "USD"; 

    
    public Payment(int paymentID, float amount, DateTime paymentDate, string paymentMethod)
    {
        PaymentID = paymentID;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
    }

    
    public void ProcessPayment()
    {
        Console.WriteLine($"Processed payment of {Amount} {Currency}.");
    }

    
    public void ViewPaymentDetails()
    {
        Console.WriteLine($"PaymentID: {PaymentID}, Amount: {Amount}, Method: {PaymentMethod}, Date: {PaymentDate}");
    }
}
