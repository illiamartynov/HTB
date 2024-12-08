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

    // Рефлексивная ассоциация
    private List<Person> _referrals = new List<Person>();
    public IReadOnlyList<Person> Referrals => _referrals.AsReadOnly();

    public Person ReferredBy { get; private set; }


    private int _totalPoints; // Очки пользователя

    public int TotalPoints => _totalPoints;
    
    [Required(ErrorMessage = "Profile is required.")]
    public Profile Profile { get; private set; } // Ссылка на Profile
    
    private List<Attempt> _attempts = new List<Attempt>(); // Связь с Attempt

    public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();

    
    
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
        Leaderboard leaderboard,
        Address address,
        Rank rank,
        CompletenessLevel completenessLevel,
        Subscription subscription,
        Profile profile,
        List<Certificate>? certificates = null,
        List<Payment>? payments = null
    )
    {
        Email = email;
        Name = name;
        Password = password;
        RegistrationDate = registrationDate;
        BirthDate = birthDate;
        IsActive = isActive;
        Balance = balance;

        AssignProfile(profile);
        Leaderboard = leaderboard;
        AssignAddress(address);
        Rank = rank;
        CompletenessLevel = completenessLevel;
        Subscription = subscription;

        // Добавление сертификатов
        if (certificates != null)
        {
            foreach (var certificate in certificates)
            {
                AddCertificate(certificate);
            }
        }

        // Добавление платежей
        if (payments != null)
        {
            foreach (var payment in payments)
            {
                AddPayment(payment);
            }
        }

        _extent.Add(this);
    }


    // Метод для добавления попытки
    public void AddAttempt(Attempt attempt)
    {
        if (attempt == null) throw new ArgumentNullException(nameof(attempt));
        if (!_attempts.Contains(attempt))
        {
            _attempts.Add(attempt);
        }
    }

    // Метод для удаления попытки
    public void RemoveAttempt(Attempt attempt)
    {
        if (attempt == null) throw new ArgumentNullException(nameof(attempt));
        _attempts.Remove(attempt);
    }
    

    public void AddChallenge(Challenge challenge)
    {
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));
        if (!_challenges.Contains(challenge))
        {
            _challenges.Add(challenge);
            challenge.AddParticipant(this); // Устанавливаем связь
        }
    }

    public void RemoveChallenge(Challenge challenge)
    {
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));
        if (_challenges.Remove(challenge))
        {
            challenge.RemoveParticipant(this); // Удаляем связь
        }
    }
    
    public Payment AssignPayment(int paymentID, float amount, DateTime paymentDate, string paymentMethod, string currency = "USD")
    {
        var payment = Payment.Create(paymentID, amount, paymentDate, paymentMethod, this, currency);
        _payments.Add(payment);
        return payment;
    }

    public void UnassignPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (_payments.Remove(payment))
        {
            payment.UnassignOwner();
        }
    }

    // Добавление платежа
    public Payment AddPayment(int paymentID, float amount, DateTime paymentDate, string paymentMethod, string currency = "USD")
    {
        var payment = Payment.Create(paymentID, amount, paymentDate, paymentMethod, this, currency);
        _payments.Add(payment);
        return payment;
    }

    // Обновление платежа
    public void UpdatePayment(int paymentID, float? newAmount = null, DateTime? newDate = null, string? newMethod = null, string? newCurrency = null)
    {
        var payment = _payments.Find(p => p.PaymentID == paymentID);
        if (payment == null)
            throw new ArgumentException($"Payment with ID {paymentID} not found.");

        payment.Update(newAmount, newDate, newMethod, newCurrency);
    }

    // Удаление платежа
    public void DeletePayment(int paymentID)
    {
        var payment = _payments.Find(p => p.PaymentID == paymentID);
        if (payment == null)
            throw new ArgumentException($"Payment with ID {paymentID} not found.");

        _payments.Remove(payment);
        payment.UnassignOwner();
    }
    
    public void AddCertificate(Certificate certificate)
    {
        if (certificate == null)
            throw new ArgumentNullException(nameof(certificate));

        if (!_certificates.Contains(certificate))
        {
            _certificates.Add(certificate);
            certificate.AssignOwner(this);
        }
    }
    

// Метод добавления платежа
    public void AddPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (!_payments.Contains(payment))
        {
            _payments.Add(payment);
            payment.AssignOwner(this);
        }
    }

// Метод удаления платежа
    public void RemovePayment(Payment payment)
    {
        if (_payments.Remove(payment))
        {
            payment.UnassignOwner();
        }
    }
    public Certificate AssignCertificate(int certificateId, DateTime issueDate)
    {
        var certificate = Certificate.Create(certificateId, issueDate, this);
        _certificates.Add(certificate);
        return certificate;
    }

    public void UnassignCertificate(Certificate certificate)
    {
        if (certificate == null)
            throw new ArgumentNullException(nameof(certificate));

        if (_certificates.Remove(certificate))
        {
            certificate.UnassignOwner();
        }
    }

    
    // Создаем и добавляем сертификат через фабрику Certificate
    public Certificate CreateCertificate(int certificateId, DateTime issueDate)
    {
        var certificate = Certificate.Create(certificateId, issueDate, this);
        _certificates.Add(certificate);
        return certificate;
    }

    // Удаляем сертификат
    public void RemoveCertificate(Certificate certificate)
    {
        if (_certificates.Contains(certificate))
        {
            _certificates.Remove(certificate);
            certificate.UnassignOwner();
        }
    }

    // Метод для отображения всех сертификатов
    public void ViewCertificates()
    {
        foreach (var certificate in _certificates)
        {
            certificate.ViewCertificate();
        }
    }
    
    public void AddReferral(Person referral)
    {
        if (referral == null)
            throw new ArgumentNullException(nameof(referral), "Referral cannot be null.");
        if (referral == this)
            throw new InvalidOperationException("A person cannot refer themselves.");
        if (_referrals.Contains(referral))
            throw new InvalidOperationException("This referral is already added.");
        if (referral.ReferredBy != null)
            throw new InvalidOperationException("This person is already referred by someone else.");

        _referrals.Add(referral);
        referral.ReferredBy = this;
    }

    public void RemoveReferral(Person referral)
    {
        if (referral == null)
            throw new ArgumentNullException(nameof(referral), "Referral cannot be null.");
        if (_referrals.Remove(referral))
        {
            referral.ReferredBy = null;
        }
    }

    public void RemoveReferredBy()
    {
        if (ReferredBy != null)
        {
            ReferredBy._referrals.Remove(this);
            ReferredBy = null;
        }
    }
    
    public void UpdateProfile(Profile newProfile)
    {
        if (newProfile == null)
            throw new ArgumentNullException(nameof(newProfile), "Profile cannot be null.");

        if (ReferenceEquals(Profile, newProfile))
            return; // Если профиль уже назначен, ничего не делаем

        // Удаляем старую связь, если она есть
        if (Profile != null)
        {
            var oldProfile = Profile;
            Profile = null;
            oldProfile.UnassignPerson();
        }

        // Назначаем новый профиль
        Profile = newProfile;
        newProfile.AssignPerson(this);
    }
    
    public static Person AddPerson(
        string email,
        string name,
        string password,
        DateTime registrationDate,
        DateTime birthDate,
        bool isActive,
        int balance,
        Profile profile,
        Leaderboard leaderboard,
        Address address,
        Rank rank,
        CompletenessLevel completenessLevel,
        Subscription subscription,
        List<Certificate>? certificates = null, // Дополнительные сертификаты
        List<Payment>? payments = null          // Дополнительные платежи
    )
    {
        // Создаем объект Person с базовыми параметрами
        var person = new Person(
            email,
            name,
            password,
            registrationDate,
            birthDate,
            isActive,
            balance,
            leaderboard,
            address,
            rank,
            completenessLevel,
            subscription,
            profile
        );

        // Добавляем сертификаты, если они переданы
        if (certificates != null)
        {
            foreach (var certificate in certificates)
            {
                person.AddCertificate(certificate); // Используем AddCertificate вместо AssignCertificate
            }
        }

        // Добавляем платежи, если они переданы
        if (payments != null)
        {
            foreach (var payment in payments)
            {
                person.AddPayment(payment);
            }
        }

        return person;
    }



    public static void DeletePerson(Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (!_extent.Contains(person)) throw new InvalidOperationException("Person not found in the extent.");

        // Удаляем профиль вместе с `Person`
        person.Profile = null;
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
    
    
    // Метод для обновления ранга
    public void UpdateRank(Rank newRank)
    {
        Rank = newRank;
        Console.WriteLine($"{Name} has been updated to rank level {newRank.RankLevel}.");

        // Синхронизация с Leaderboard
        Leaderboard?.UpdateRankings(this, newRank.RankLevel, _totalPoints);
    }

    // Метод для добавления очков
    public void AddPoints(int points)
    {
        if (points < 0)
            throw new ArgumentException("Points cannot be negative.");

        _totalPoints += points;
        Console.WriteLine($"{Name} gained {points} points. Total: {_totalPoints}.");

        // Синхронизация с Leaderboard
        Leaderboard?.UpdateRankings(this, Rank.RankLevel, _totalPoints);
    }

    // Метод для удаления из Leaderboard
    public void RemoveFromLeaderboard()
    {
        Leaderboard?.RemovePersonFromLeaderboard(this);
        Leaderboard = null;
    }
    
    // Метод для присвоения профиля
    public void AssignProfile(Profile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile), "Profile cannot be null.");

        if (Profile != null)
        {
            var oldProfile = Profile;
            Profile = null;
            oldProfile.UnassignPerson();
        }

        Profile = profile;
        profile.AssignPerson(this);
    }

    // Удаление связи с профилем
    public void RemoveProfile()
    {
        if (Profile != null)
        {
            var oldProfile = Profile;
            Profile = null;
            oldProfile.UnassignPerson();
        }
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
