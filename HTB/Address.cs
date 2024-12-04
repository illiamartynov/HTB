using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HTB
{
    public class Address
    {
        private static List<Address> _addresses = new List<Address>();

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

        private List<Person> _persons = new List<Person>();

        public IReadOnlyList<Person> Persons => _persons.AsReadOnly();
        public static IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

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
            foreach (var person in _persons.ToList()) // Удаляем связь с каждым объектом Person
            {
                person.RemoveAddress();
            }
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

        // Добавление объекта Person
        public void AddPerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");

            if (!_persons.Contains(person))
                _persons.Add(person);

            if (person.Address != this)
                person.AssignAddress(this); // Устанавливаем связь в обратном направлении
        }


        // Удаление объекта Person
        public void RemovePerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");

            if (!_persons.Contains(person))
                return; // Если объект Person не связан с этим адресом, выходим из метода

            _persons.Remove(person);

            if (person.Address == this) // Удаляем связь только если адрес совпадает
                person.RemoveAddress();
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
}
