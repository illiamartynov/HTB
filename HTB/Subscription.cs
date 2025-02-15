﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTB
{
    public enum SubscriptionType
    {
        Free,
        Premium
    }

    public class Subscription
    {
        public static List<Subscription> Extent = new List<Subscription>();

        private int _subscriptionID;
        private DateTime _startDate;
        private DateTime _endDate;
        private SubscriptionType _type;

        [Required(ErrorMessage = "Access type is required.")]
        [JsonConverter(typeof(AccessTypeConverter))]
        public IAccessType AccessType { get; set; }

        [Required(ErrorMessage = "Subscription ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Subscription ID must be a positive integer.")]
        public int SubscriptionID
        {
            get => _subscriptionID;
            set => _subscriptionID = value;
        }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Start date must be a valid date.")]
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "End date must be a valid date.")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be after start date.")]
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        [Required(ErrorMessage = "Subscription type is required.")]
        public SubscriptionType Type
        {
            get => _type;
            set => _type = value;
        }
        
        private List<Person> _persons = new List<Person>();
        public IReadOnlyList<Person> Persons => _persons.AsReadOnly();


        public Subscription(int subscriptionID, DateTime startDate, DateTime endDate, SubscriptionType type, IAccessType accessType)
        {
            _subscriptionID = subscriptionID;
            _startDate = startDate;
            _endDate = endDate;
            _type = type;
            AccessType = accessType;
            Extent.Add(this);
        }

        public void ShowSubscriptionInfo()
        {
            Console.WriteLine($"Subscription ID: {_subscriptionID}");
            Console.WriteLine($"Type: {_type}");
            Console.WriteLine($"Access Details: {AccessType.GetAccessDescription()}");
            Console.WriteLine($"Duration: {_startDate} - {_endDate}");
        }
        
        // funcs for person connection
        public void AddPerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (Persons.Contains(person)) 
                throw new ArgumentException("The person already exists in the Subscription class", nameof(person));

            _persons.Add(person);
            person.AddSubscriptionReverse(this);
        }

        public void RemovePerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (!Persons.Contains(person))
                return;
        
            // remove Person from Subscription
            _persons.Remove(person);

            // remove Subscription from person
            person.RemoveSubscriptionReverse(this);
        }

        public void UpdatePerson(Person oldPerson, Person newPerson)
        {
            if (oldPerson == null)
                throw new ArgumentNullException(nameof(oldPerson));
            if (newPerson == null)
                throw new ArgumentNullException(nameof(newPerson));
            if (!Persons.Contains(oldPerson))
                throw new ArgumentException("The oldPerson doesn't exist in Subscription class", nameof(oldPerson));
            if (Persons.Contains(newPerson))   
                throw new ArgumentException("The person already exists in the Subscription class", nameof(newPerson));
        
            _persons.Remove(oldPerson);
            oldPerson.RemoveSubscriptionReverse(this);

            // add new person
            _persons.Add(newPerson);
            newPerson.AddSubscriptionReverse(this);
        }
    
        // Person reverse funcs
        public void AddSubscriptionReverse(Person person)
        {
            _persons.Add(person);
        }

        public void RemoveSubscriptionReverse(Person person)
        {
            _persons.Remove(person);
        }


        // extent funcs
        public static void SaveExtent(string filename = "subscription_extent.json")
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Extent, options);
            File.WriteAllText(filename, json);
        }

        public static void LoadExtent(string filename = "subscription_extent.json")
        {
            if (File.Exists(filename))
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new AccessTypeConverter());
                Extent = JsonSerializer.Deserialize<List<Subscription>>(File.ReadAllText(filename), options);
            }
        }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (comparisonProperty == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)comparisonProperty.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
