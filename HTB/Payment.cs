namespace HTB;

using System;
using System.ComponentModel.DataAnnotations;

public class Payment
{
    private int _paymentID;
    private float _amount;
    private DateTime _paymentDate;
    private string _paymentMethod;
    private static string _currency = "USD";

    [Required(ErrorMessage = "Payment ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Payment ID must be a positive integer.")]
    public int PaymentID
    {
        get => _paymentID;
        set => _paymentID = value;
    }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, float.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public float Amount
    {
        get => _amount;
        set => _amount = value;
    }

    [Required(ErrorMessage = "Payment date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Payment date must be a valid date.")]
    public DateTime PaymentDate
    {
        get => _paymentDate;
        set => _paymentDate = value;
    }

    [Required(ErrorMessage = "Payment method is required.")]
    [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
    public string PaymentMethod
    {
        get => _paymentMethod;
        set => _paymentMethod = value;
    }

    [Required(ErrorMessage = "Currency is required.")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
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
