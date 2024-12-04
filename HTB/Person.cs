using System.Text.Json.Serialization;

namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

public class Person
{
    private static List<Person> _extent = new List<Person>();
    public static IReadOnlyList<Person> Extent => _extent.AsReadOnly();

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; private set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; private set; }

    [JsonIgnore]
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    private string _password;

    public string Password
    {
        get => "****";
        private set => _password = HashPassword(value);
    }

    [Required(ErrorMessage = "Registration date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Registration date must be a valid date.")]
    public DateTime RegistrationDate { get; private set; }

    [Required(ErrorMessage = "Birth date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Birth date must be a valid date.")]
    public DateTime BirthDate { get; private set; }

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; private set; }

    [Range(0, int.MaxValue, ErrorMessage = "Balance cannot be negative.")]
    public int Balance { get; private set; }

    [Required(ErrorMessage = "User profile is required.")]
    public Profile UserProfile { get; private set; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
    public int Age { get; private set; }

    [Required(ErrorMessage = "Address is required.")]
    public Address Address { get; private set; }

    [Required(ErrorMessage = "Rank is required.")]
    public Rank Rank { get; private set; }

    [Required(ErrorMessage = "Completeness level is required.")]
    public CompletenessLevel CompletenessLevel { get; private set; }

    public IReadOnlyList<Certificate> Certificates => _certificates.AsReadOnly();

    private List<Certificate> _certificates = new List<Certificate>();

    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    private List<Payment> _payments = new List<Payment>();

    [Required(ErrorMessage = "Leaderboard is required.")]
    public Leaderboard Leaderboard { get; private set; }

    public IReadOnlyList<Person> ReferredUsers => _referredUsers.AsReadOnly();

    private List<Person> _referredUsers = new List<Person>();

    [Required(ErrorMessage = "Subscription is required.")]
    public Subscription Subscription { get; private set; }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    private List<Course> _courses = new List<Course>();

    public IReadOnlyList<Challenge> Challenges => _challenges.AsReadOnly();

    private List<Challenge> _challenges = new List<Challenge>();

    // Добавлен пустой конструктор для десериализации
    public Person()
    {
    }

    // Оригинальный конструктор для создания объектов
    public Person(
        string email,
        string name,
        string password,
        DateTime registrationDate,
        DateTime birthDate,
        bool isActive,
        int balance,
        Profile userProfile,
        Leaderboard leaderboard,
        Address address,
        Rank rank,
        CompletenessLevel completenessLevel,
        Subscription subscription)
    {
        Email = email;
        Name = name;
        Password = password;
        RegistrationDate = registrationDate;
        BirthDate = birthDate;
        IsActive = isActive;
        Balance = balance;
        UserProfile = userProfile;
        Leaderboard = leaderboard;
        AssignAddress(address);
        Rank = rank;
        CompletenessLevel = completenessLevel;
        Subscription = subscription;

        _extent.Add(this);
    }

    public static Person AddPerson(
        string email, string name, string password, DateTime registrationDate, DateTime birthDate, bool isActive,
        int balance, Profile profile, Leaderboard leaderboard, Address address, Rank rank, CompletenessLevel completenessLevel,
        Subscription subscription)
    {
        var person = new Person(email, name, password, registrationDate, birthDate, isActive, balance, profile, leaderboard, address, rank, completenessLevel, subscription);
        return person;
    }

    public static void DeletePerson(Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (!_extent.Contains(person)) throw new InvalidOperationException("Person not found in the extent.");
        person.RemoveAddress(); // Удаляем связь с Address перед удалением
        _extent.Remove(person);
    }

    public void UpdatePerson(
        string? email = null,
        string? name = null,
        string? password = null,
        DateTime? birthDate = null,
        bool? isActive = null,
        int? balance = null)
    {
        if (!string.IsNullOrEmpty(email)) Email = email;
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (!string.IsNullOrEmpty(password)) Password = HashPassword(password);
        if (birthDate.HasValue)
        {
            BirthDate = birthDate.Value;
            Age = CalculateAge(BirthDate);
        }
        if (isActive.HasValue) IsActive = isActive.Value;
        if (balance.HasValue) Balance = balance.Value;
    }

    public void AssignAddress(Address address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address), "Address cannot be null.");

        if (ReferenceEquals(Address, address)) // Если новый адрес совпадает с текущим, ничего не делаем
            return;

        if (Address != null)
        {
            var oldAddress = Address;
            Address = null; // Удаляем текущий адрес перед назначением нового
            oldAddress.RemovePerson(this); // Убираем связь с объектом Person в старом Address
        }

        Address = address; // Назначаем новый адрес

        if (!address.Persons.Contains(this))
            address.AddPerson(this); // Устанавливаем связь с новым Address
    }







    public void RemoveAddress()
    {
        if (Address == null)
            return; // Если адреса нет, просто выходим из метода

        var oldAddress = Address;
        Address = null; // Сбрасываем адрес у текущего объекта Person
        oldAddress.RemovePerson(this); // Убираем связь с объектом Person в Address
    }


    public static void SaveExtent(string filename = "person_extent.json")
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        try
        {
            var json = JsonSerializer.Serialize(_extent, options);
            File.WriteAllText(filename, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the extent: {ex.Message}");
        }
    }

    public static void LoadExtent(string filename = "person_extent.json")
    {
        if (File.Exists(filename))
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            try
            {
                var json = File.ReadAllText(filename);
                _extent = JsonSerializer.Deserialize<List<Person>>(json, options) ?? new List<Person>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the extent: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"File '{filename}' not found. Initializing an empty extent.");
            _extent = new List<Person>();
        }
    }


    public static void ClearExtent()
    {
        _extent.Clear();
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
    public void AddCourse(Course course)
    {
        if (course != null && !_courses.Contains(course))
        {
            _courses.Add(course);
            Console.WriteLine($"{Name} enrolled in course: {course.CourseName}");
        }
    }
}
