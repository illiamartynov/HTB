public class Address
{
    private static List<Address> _addresses = new List<Address>();

    private string _country;
    private string _city;
    private string _street;
    private int _number;

    public static IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public string Country
    {
        get => _country;
        set => _country = value;
    }

    public string City
    {
        get => _city;
        set => _city = value;
    }

    public string Street
    {
        get => _street;
        set => _street = value;
    }

    public int Number
    {
        get => _number;
        set => _number = value;
    }

    public Address() { }

    public Address(string country, string city, string street, int number)
    {
        _country = country;
        _city = city;
        _street = street;
        _number = number;
        _addresses.Add(this);
    }

    public void ViewAddress()
    {
        Console.WriteLine($"{_street} {_number}, {_city}, {_country}");
    }

    public static void ViewAllAddresses()
    {
        Console.WriteLine("All addresses:");
        foreach (var address in _addresses)
        {
            address.ViewAddress();
        }
    }

    public static void ClearAddresses()
    {
        _addresses.Clear();
    }
}