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

    public int PaymentID => _paymentID;
    public float Amount => _amount;
    public DateTime PaymentDate => _paymentDate;
    public string PaymentMethod => _paymentMethod;
    public static string Currency => _currency;

    public Person Owner { get; private set; }

    private Payment(int paymentID, float amount, DateTime paymentDate, string paymentMethod, Person owner, string currency = "USD")
    {
        _paymentID = paymentID;
        _amount = amount;
        _paymentDate = paymentDate;
        _paymentMethod = paymentMethod;
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        _currency = currency;
    }

    // Фабричный метод для создания платежа
    public static Payment Create(int paymentID, float amount, DateTime paymentDate, string paymentMethod, Person owner, string currency = "USD")
    {
        return new Payment(paymentID, amount, paymentDate, paymentMethod, owner, currency);
    }

    // Обновление данных платежа
    public void Update(float? newAmount = null, DateTime? newDate = null, string? newMethod = null, string? newCurrency = null)
    {
        if (newAmount.HasValue)
            _amount = newAmount.Value;
        if (newDate.HasValue)
            _paymentDate = newDate.Value;
        if (!string.IsNullOrEmpty(newMethod))
            _paymentMethod = newMethod;
        if (!string.IsNullOrEmpty(newCurrency))
            _currency = newCurrency;
    }

    // Удаление связи с владельцем
    public void UnassignOwner()
    {
        Owner = null;
    }

    // Удаление платежа
    public static void Delete(Payment payment)
    {
        payment.UnassignOwner();
    }

    public void ViewPaymentDetails()
    {
        Console.WriteLine($"PaymentID: {_paymentID}, Amount: {_amount} in {_currency}, Method: {_paymentMethod}, Date: {_paymentDate}, Owner: {Owner?.Name}");
    }
    // Payment
    public void AssignOwner(Person owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

}
