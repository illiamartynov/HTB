using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Address
{
    private static List<Address> _addresses = new List<Address>();

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    [StringLength(200, ErrorMessage = "Street name cannot exceed 200 characters.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Number is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Number must be a positive integer.")]
    public int Number { get; set; }

    public static IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public Address() { }

    public Address(string country, string city, string street, int number)
    {
        Country = country;
        City = city;
        Street = street;
        Number = number;

        _addresses.Add(this);
    }

    public void ViewAddress()
    {
        Console.WriteLine($"{Street} {Number}, {City}, {Country}");
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