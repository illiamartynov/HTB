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

    public IReadOnlyList<Person> ReferredUsers => _referredUsers.AsReadOnly();

    private List<Person> _referredUsers = new List<Person>();

    private List<Subscription> _subscriptions = new List<Subscription>();
    public IReadOnlyList<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    



    // Рефлексивная ассоциация
    private List<Person> _referrals = new List<Person>();
    public IReadOnlyList<Person> Referrals => _referrals.AsReadOnly();

    public Person ReferredBy { get; private set; }


    private int _totalPoints; // Очки пользователя

    public int TotalPoints => _totalPoints;
    
    [Required(ErrorMessage = "Profile is required.")]
    public Profile Profile { get; private set; } // Ссылка на Profile
    
    private List<Attempt> _attempts = new List<Attempt>(); 

    public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();

    private readonly List<CompletenessLevel> _completenessLevels = new();
    public IReadOnlyList<CompletenessLevel> CompletenessLevels => _completenessLevels.AsReadOnly();
    
    
    
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
        Address address,
        Rank rank,
        CompletenessLevel completenessLevel,
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
        AssignAddress(address);
        Rank = rank;
        CompletenessLevel = completenessLevel;

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
    
    // Attempt funcs
    public Attempt AddAttempt(string result, Challenge challenge)
    {
        if (result == null) 
            throw new ArgumentNullException(nameof(result));
        if (result.Length == 0) 
            throw new ArgumentException("The result is empty.", nameof(result));
        if (challenge == null) 
            throw new ArgumentNullException(nameof(challenge));
        
        Attempt attempt = new Attempt(this, challenge, result);
        challenge.AddAttemptReverse(attempt);
        
        _attempts.Add(attempt);
        
        return attempt;
    }
    
    public void RemoveAttempt(Attempt attempt)
    {
        if (attempt == null) 
            throw new ArgumentNullException(nameof(attempt));
        if (!_attempts.Contains(attempt))
            throw new ArgumentException("The attempt doesn't exist in Person class", nameof(attempt));

        // remove attempt from Challenge
        Challenge attemptChallenge = attempt.Challenge;
        attemptChallenge.RemoveAttemptReverse(attempt);
        
        // remove attemp from Person
        _attempts.Remove(attempt);
        
        // remove Attempt
        attempt.RemoveAttempt();
    }

    public void UpdateAttempt(Attempt oldAttempt, string result, Challenge challenge)
    {
        if (oldAttempt == null)
            throw new ArgumentNullException(nameof(oldAttempt));
        if (!_attempts.Contains(oldAttempt))
            throw new ArgumentException("The attempt doesn't exist in Person class", nameof(oldAttempt));
        
        // remove oldAttempt from Person
        _attempts.Remove(oldAttempt);
        // remove oldAttempt from Challenge
        oldAttempt.Challenge.RemoveAttemptReverse(oldAttempt);
        // remove associations from Attempt
        oldAttempt.DisassociateAttempt();
        
        // add new attempt
        AddAttempt(result, challenge);
    }
    
    // reverse funcs for Attempt
    public void AddAttemptReverse(Attempt attempt)
    {
        _attempts.Add(attempt);
    }

    public void RemoveAttemptReverse(Attempt attempt)
    {
        _attempts.Remove(attempt);
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
    
    // funcs for certificate
    public void AddCertificate(Certificate certificate)
    {
        if (certificate == null)
            throw new ArgumentNullException(nameof(certificate));

        if (!_certificates.Contains(certificate))
        {
            _certificates.Add(certificate);
            certificate.AddOwnerReverse(this);
        }
    }
    
    public void RemoveCertificate(Certificate certificate)
    {
        if (certificate == null) 
            throw new ArgumentNullException(nameof(certificate));
            
        if (_certificates.Contains(certificate))
        {
            _certificates.Remove(certificate);
            certificate.RemoveOwnerReverse();
        }
    }

    public void UpdateCertificate(Certificate oldCert, Certificate newCert)
    {
        if (newCert == null) 
            throw new ArgumentNullException(nameof(newCert), "Null values are not allowed");
        if (oldCert == null)
            throw new ArgumentNullException(nameof(oldCert), "Null values are not allowed");

        if (_certificates.Contains(newCert))
            throw new InvalidOperationException("The newCert already exists in the list.");
        if (!_certificates.Contains(oldCert))
            throw new InvalidOperationException("The oldCert doesn't exists in the list.");
        
        // disassociate oldCert
        _certificates.Remove(oldCert);
        oldCert.RemoveOwnerReverse();
        
        // associate newCert
        AddCertificate(newCert);
    }
    
    // reverse funcs for certificate
    public void AddCertificateReverse(Certificate certificate)
    {
        if (!_certificates.Contains(certificate))
            _certificates.Add(certificate);        
    }

    public void RemoveCertificateReverse(Certificate certificate)
    {
        _certificates.Remove(certificate);
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
    
    public void UpdateReferredBy(Person newReferrer)
    {
        if (newReferrer == null)
            throw new ArgumentNullException(nameof(newReferrer), "Referrer cannot be null.");
    
        if (newReferrer == this)
            throw new InvalidOperationException("A person cannot refer themselves.");
    
        // Удаляем старую связь, если она существует
        if (ReferredBy != null)
        {
            ReferredBy._referrals.Remove(this);
        }

        // Обновляем ссылку
        ReferredBy = newReferrer;

        // Добавляем текущий объект в список рефералов нового реферера
        if (!newReferrer._referrals.Contains(this))
        {
            newReferrer._referrals.Add(this);
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
            address,
            rank,
            completenessLevel,
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
        foreach (var certificate in person._certificates)
        {
            person.RemoveCertificate(certificate);
        }
            
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

    }

    // Метод для добавления очков
    public void AddPoints(int points)
    {
        if (points < 0)
            throw new ArgumentException("Points cannot be negative.");

        _totalPoints += points;
        Console.WriteLine($"{Name} gained {points} points. Total: {_totalPoints}.");

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
    
    public void AddSubscription(Subscription subscription)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription), "Subscription cannot be null.");

        if (!_subscriptions.Contains(subscription))
        {
            _subscriptions.Add(subscription);
            subscription.AddPerson(this); // Двусторонняя связь
        }
    }
    
    public void RemoveSubscription(Subscription subscription)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription), "Subscription cannot be null.");

        if (_subscriptions.Remove(subscription))
        {
            subscription.RemovePerson(this); // Удаляем связь из Subscription
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
    
}
