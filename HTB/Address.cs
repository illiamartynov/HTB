using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTB
{
    public class Address
    {
        private static List<Address> _addresses = new List<Address>();
        public static IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
        public string Country { get; private set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string City { get; private set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(200, ErrorMessage = "Street name cannot exceed 200 characters.")]
        public string Street { get; private set; }

        [Required(ErrorMessage = "Number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number must be a positive integer.")]
        public int Number { get; private set; }
        
        public Person _person { get; private set; }
        
        private Address(string country, string city, string street, int number)
        {
            Country = country;
            City = city;
            Street = street;
            Number = number;
            _addresses.Add(this);
        }

        // Создание нового адреса
        public static Address AddAddress(string country, string city, string street, int number)
        {
            return new Address(country, city, street, number);
        }


        // Удаление адреса
        public void RemoveAddress()
        {
            _addresses.Remove(this);
        }

        // Обновление адреса
        public void UpdateAddress(string? country = null, string? city = null, string? street = null, int? number = null)
        {
            if (!string.IsNullOrEmpty(country)) Country = country;
            if (!string.IsNullOrEmpty(city)) City = city;
            if (!string.IsNullOrEmpty(street)) Street = street;
            if (number.HasValue && number.Value > 0) Number = number.Value;
        }

        // funcs for person connection
        public void AddPerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (_person != person) 
                throw new ArgumentException("The person already exists in the Address class", nameof(person));

            _person = null;
            person.AddAddressReverse(this);
        }

        public void RemovePerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (_person != person)
                throw new ArgumentException("Isn't the right person", nameof(person));
        
            // remove Person from Address
            _person = null;

            // remove Address from person
            person.RemoveAddressReverse(this);
        }

        public void UpdatePerson(Person oldPerson, Person newPerson)
        {
            if (oldPerson == null)
                throw new ArgumentNullException(nameof(oldPerson));
            if (newPerson == null)
                throw new ArgumentNullException(nameof(newPerson));
            if (_person != oldPerson)
                throw new ArgumentException("The oldPerson doesn't exist in Address class", nameof(oldPerson));
            if (_person == newPerson)  
                throw new ArgumentException("The person already exists in the Address class", nameof(newPerson));
        
            _person = null;
            oldPerson.RemoveAddressReverse(this);

            // add new person
            _person = newPerson;
            newPerson.AddAddressReverse(this);
        }
    
        // Person reverse funcs
        public void AddAddressReverse(Person person)
        {
            _person = person;
        }

        public void RemoveAddressReverse(Person person)
        {
            _person = person;
        }
        
        // other funcs
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
}
