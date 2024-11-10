namespace HTB;

using System;

public class Payment
{
    private int _paymentID;
    private float _amount;
    private DateTime _paymentDate;
    private string _paymentMethod;
    private static string _currency = "USD";

    public int PaymentID
    {
        get => _paymentID;
        set => _paymentID = value;
    }

    public float Amount
    {
        get => _amount;
        set => _amount = value;
    }

    public DateTime PaymentDate
    {
        get => _paymentDate;
        set => _paymentDate = value;
    }

    public string PaymentMethod
    {
        get => _paymentMethod;
        set => _paymentMethod = value;
    }

    public static string Currency
    {
        get => _currency;
        set => _currency = value;
    }

    public Payment(int paymentID, float amount, DateTime paymentDate, string paymentMethod, string currency = "USD")
    {
        _paymentID = paymentID;
        _amount = amount;
        _paymentDate = paymentDate;
        _paymentMethod = paymentMethod;
        _currency = currency;
    }

    public void ProcessPayment()
    {
        Console.WriteLine($"Processed payment of {_amount} in {_currency} (converted to USD).");
    }

    public void ViewPaymentDetails()
    {
        Console.WriteLine($"PaymentID: {_paymentID}, Amount: {_amount} in {_currency}, Method: {_paymentMethod}, Date: {_paymentDate}");
    }
}