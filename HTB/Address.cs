namespace HTB;

using System;


public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }

    
    public Address(string country, string city, string street, int number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }

    
    public void ViewAddress()
    {
        Console.WriteLine($"{Street} {Number}, {City}, {Country}");
    }
}
