public class Address
{
    public static List<Address> Addresses = new List<Address>(); 

    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }

    public Address() { }

    public Address(string country, string city, string street, int number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;
        Addresses.Add(this);  
    }

    public void ViewAddress()
    {
        Console.WriteLine($"{Street} {Number}, {City}, {Country}");
    }

    public static void ViewAllAddresses()
    {
        Console.WriteLine("All addresses:");
        foreach (var address in Addresses)
        {
            address.ViewAddress();
        }
    }
}