namespace HTB;

using System;

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

    // funcs for person connection
    public void AddPerson(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (Owner != person) 
            throw new ArgumentException("The person already exists in the Profile class", nameof(person));

        Owner = null;
        person.AddPaymentReverse(this);
    }

    public void RemovePerson(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));
        if (Owner != person)
            throw new ArgumentException("Isn't the right person", nameof(person));
    
        // remove Person from Payment
        Owner = null;

        // remove Payment from person
        person.RemovePaymentReverse(this);
    }

    public void UpdatePerson(Person oldPerson, Person newPerson)
    {
        if (oldPerson == null)
            throw new ArgumentNullException(nameof(oldPerson));
        if (newPerson == null)
            throw new ArgumentNullException(nameof(newPerson));
        if (Owner != oldPerson)
            throw new ArgumentException("The oldPerson doesn't exist in Payment class", nameof(oldPerson));
        if (Owner == newPerson)  
            throw new ArgumentException("The person already exists in the Payment class", nameof(newPerson));
    
        Owner = null;
        oldPerson.RemovePaymentReverse(this);

        // add new person
        Owner = newPerson;
        newPerson.AddPaymentReverse(this);
    }

    // Person reverse funcs
    public void AddPaymentReverse(Person person)
    {
        Owner = person;
    }

    public void RemovePaymentReverse(Person person)
    {
        Owner = person;
    }

    public void ViewPaymentDetails()
    {
        Console.WriteLine($"PaymentID: {_paymentID}, Amount: {_amount} in {_currency}, Method: {_paymentMethod}, Date: {_paymentDate}, Owner: {Owner?.Name}");
    }
}
