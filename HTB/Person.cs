using System.ComponentModel;
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
    
    // Rank
    private List<Rank> _ranks = new List<Rank>(); 
    public IReadOnlyList<Rank> Ranks => _ranks.AsReadOnly();

    // Рефлексивная ассоциация
    private List<Person> _referrals = new List<Person>();
    public IReadOnlyList<Person> Referrals => _referrals.AsReadOnly();
    public Person ReferredBy { get; private set; }
    
    
    private int _totalPoints; // Очки пользователя
    public int TotalPoints => _totalPoints;
    
    
    [Required(ErrorMessage = "Profile is required.")]
    public Profile _profile { get; private set; } 
    
    // Attempt
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
        Profile profile
    )
    {
        Email = email;
        Name = name;
        Password = password;
        RegistrationDate = registrationDate;
        BirthDate = birthDate;
        IsActive = isActive;
        Balance = balance;

        AddProfile(profile);
        AddAddress(address);

        _extent.Add(this);
    }
    
    
    // CompletenessLevel funcs
    public CompletenessLevel AddCompletenessLevel(int percentage, Course course)
    {
        if (percentage < 0) 
            throw new ArgumentException("Percentage can't be negative", nameof(percentage));
        if (course == null) 
            throw new ArgumentNullException(nameof(course));
        
        CompletenessLevel completenessLevel = new CompletenessLevel(percentage, this, course);
        course.AddCompletenessLevelReverse(completenessLevel);
        
        _completenessLevels.Add(completenessLevel);
        
        return completenessLevel;
    }
    
    public void RemoveCompletenessLevel(CompletenessLevel completenessLevel)
    {
        if (completenessLevel == null) 
            throw new ArgumentNullException(nameof(completenessLevel));
        if (!_completenessLevels.Contains(completenessLevel))
            throw new ArgumentException("The CompletenessLevel doesn't exist in Person class", nameof(completenessLevel));

        // remove CompletenessLevel from Course
        Course completenessLevelCourse = CompletenessLevel.Course;
        completenessLevelCourse.RemoveCompletenessLevelReverse(completenessLevel);
        
        // remove CompletenessLevel from Person
        _completenessLevels.Remove(completenessLevel);
        
        // remove CompletenessLevel
        completenessLevel.RemoveCompletenessLevel();
    }

    public void UpdateCompletenessLevel(CompletenessLevel oldCompletenessLevel, int percentage, Course course)
    {
        if (oldCompletenessLevel == null)
            throw new ArgumentNullException(nameof(oldCompletenessLevel));
        if (!_completenessLevels.Contains(oldCompletenessLevel))
            throw new ArgumentException("The CompletenessLevel doesn't exist in Person class", nameof(oldCompletenessLevel));
        
        // remove oldAttempt from Person
        _completenessLevels.Remove(oldCompletenessLevel);
        // remove oldAttempt from Challenge
        oldCompletenessLevel.Course.RemoveCompletenessLevelReverse(oldCompletenessLevel);
        // remove associations from CompletenessLevel
        oldCompletenessLevel.DisassociateCompletenessLevel();
        
        // add new CompletenessLevel
        AddCompletenessLevel(percentage, course);
    }
    
    // reverse funcs for CompletenessLevel
    public void AddCompletenessLevelReverse(CompletenessLevel completenessLevel)
    {
        _completenessLevels.Add(completenessLevel);
    }

    public void RemoveCompletenessLevelReverse(CompletenessLevel completenessLevel)
    {
        _completenessLevels.Remove(completenessLevel);
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
    
    
    // Rank funcs
    public Rank AddRank(int rankLevel, Leaderboard leaderboard)
    {
        if (rankLevel >= 0) 
            throw new ArgumentException("The rankLevel should be >= to 0", nameof(rankLevel));
        if (leaderboard == null) 
            throw new ArgumentNullException(nameof(leaderboard));
        
        Rank rank = new Rank(rankLevel, this, leaderboard);
        leaderboard.AddRankReverse(rank);
        
        _ranks.Add(rank);
        
        return rank;
    }
    
    public void RemoveRank(Rank rank)
    {
        if (rank == null) 
            throw new ArgumentNullException(nameof(rank));
        if (!_ranks.Contains(rank))
            throw new ArgumentException("The rank doesn't exist in Person class", nameof(rank));

        // remove rank from Leaderboard
        Leaderboard rankLeaderboard = rank.Leaderboard;
        rankLeaderboard.RemoveRankReverse(rank);
        
        // remove rank from Person
        _ranks.Remove(rank);
        
        // remove Rank
        rank.RemoveRank();
    }

    public void UpdateRank(Rank oldRank, int rankLevel, Leaderboard leaderboard)
    {
        if (oldRank == null)
            throw new ArgumentNullException(nameof(oldRank));
        if (!_ranks.Contains(oldRank))
            throw new ArgumentException("The rank doesn't exist in Person class", nameof(oldRank));
        
        // remove oldRank from Person
        _ranks.Remove(oldRank);
        // remove oldRank from Challenge
        oldRank.Leaderboard.RemoveRankReverse(oldRank);
        // remove associations from Rank
        oldRank.DisassociateRank();
        
        // add new rank
        AddRank(rankLevel, leaderboard);
    }
    
    // reverse funcs for Attempt
    public void AddRankReverse(Rank rank)
    {
        _ranks.Add(rank);
    }

    public void RemoveRankReverse(Rank rank)
    {
        _ranks.Remove(rank);
    }
    
    
    // funcs for subscription connection
    public void AddSubscription(Subscription subscription)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        if (Subscriptions.Contains(subscription)) 
            throw new ArgumentException("The subscription already exists in the Person class", nameof(subscription));

        _subscriptions.Add(subscription);
        subscription.AddSubscriptionReverse(this);
    }

    public void RemoveSubscription(Subscription subscription)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        if (!Subscriptions.Contains(subscription))
            return;
        
        // remove Subscription from person
        _subscriptions.Remove(subscription);

        // remove person from subscription
        subscription.RemoveSubscriptionReverse(this);
    }

    public void UpdateSubscription(Subscription oldSubscription, Subscription newSubscription)
    {
        if (oldSubscription == null)
            throw new ArgumentNullException(nameof(oldSubscription));
        if (newSubscription == null)
            throw new ArgumentNullException(nameof(newSubscription));
        if (!Subscriptions.Contains(oldSubscription))
            throw new ArgumentException("The oldSubscription doesn't exist in Person class", nameof(oldSubscription));
        if (Subscriptions.Contains(newSubscription))   
            throw new ArgumentException("The subscription already exists in the Person class", nameof(newSubscription));
        
        _subscriptions.Remove(oldSubscription);
        oldSubscription.RemoveSubscriptionReverse(this);

        // add new subscription
        _subscriptions.Add(newSubscription);
        newSubscription.AddSubscriptionReverse(this);
    }
    
    // Subscription reverse funcs
    public void AddSubscriptionReverse(Subscription subscription)
    {
        _subscriptions.Add(subscription);
    }

    public void RemoveSubscriptionReverse(Subscription subscription)
    {
        _subscriptions.Remove(subscription);
    }
    
    
    // funcs for address connection
    public void AddAddress(Address address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));
        if (Address == address) 
            throw new ArgumentException("The address already exists in the Person class", nameof(address));

        Address = null;
        address.AddAddressReverse(this);
    }

    public void RemoveAddress(Address address)
    {
        if (address == null)
            throw new ArgumentNullException(nameof(address));
        if (Address != address)
            throw new ArgumentException("Isn't the right address", nameof(address));
        
        // remove Address from Address
        Address = null;

        // remove Address from address
        address.RemoveAddressReverse(this);
    }

    public void UpdateAddress(Address oldAddress, Address newAddress)
    {
        if (oldAddress == null)
            throw new ArgumentNullException(nameof(oldAddress));
        if (newAddress == null)
            throw new ArgumentNullException(nameof(newAddress));
        if (Address != oldAddress)
            throw new ArgumentException("The oldAddress doesn't exist in Address class", nameof(oldAddress));
        if (Address == newAddress)  
            throw new ArgumentException("The address already exists in the Address class", nameof(newAddress));
        
        Address = null;
        oldAddress.RemoveAddressReverse(this);

        // add new address
        Address = newAddress;
        newAddress.AddAddressReverse(this);
    }
    
    // Address reverse funcs
    public void AddAddressReverse(Address address)
    {
        Address = address;
    }

    public void RemoveAddressReverse(Address address)
    {
        Address = address;
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
    

    // Метод для отображения всех сертификатов
    public void ViewCertificates()
    {
        foreach (var certificate in _certificates)
        {
            certificate.ViewCertificate();
        }
    }
    
    
    // funcs for payment connection
    public void AddPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));
        if (_payments.Contains(payment)) 
            throw new ArgumentException("The payment already exists in the Person class", nameof(payment));

        _payments.Add(payment);
        payment.AddPaymentReverse(this);
    }

    public void RemovePayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));
        if (!_payments.Contains(payment))
            return;
        
        // remove payment from person
        _payments.Remove(payment);

        // remove person from payment
        payment.RemovePaymentReverse(this);
    }

    public void UpdatePayment(Payment oldPayment, Payment newPayment)
    {
        if (oldPayment == null)
            throw new ArgumentNullException(nameof(oldPayment));
        if (newPayment == null)
            throw new ArgumentNullException(nameof(newPayment));
        if (!_payments.Contains(oldPayment))
            throw new ArgumentException("The oldPayment doesn't exist in Person class", nameof(oldPayment));
        if (_payments.Contains(newPayment))   
            throw new ArgumentException("The payment already exists in the Person class", nameof(newPayment));
        
        _payments.Remove(oldPayment);
        oldPayment.RemovePaymentReverse(this);

        // add new payment
        _payments.Add(newPayment);
        newPayment.AddPaymentReverse(this);
    }
    
    // Payment reverse funcs
    public void AddPaymentReverse(Payment payment)
    {
        _payments.Add(payment);
    }

    public void RemovePaymentReverse(Payment payment)
    {
        _payments.Remove(payment);
    }
    
    
    // funcs for profile connection
    public void AddProfile(Profile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));
        if (_profile == profile) 
            throw new ArgumentException("The profile already exists in the Profile class", nameof(profile));

        _profile = null;
        profile.AddProfileReverse(this);
    }

    public void RemoveProfile(Profile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));
        if (_profile != profile)
            throw new ArgumentException("Isn't the right profile", nameof(profile));
        
        // remove Profile from Profile
        _profile = null;

        // remove Profile from profile
        profile.RemoveProfileReverse(this);
    }

    public void UpdateProfile(Profile oldProfile, Profile newProfile)
    {
        if (oldProfile == null)
            throw new ArgumentNullException(nameof(oldProfile));
        if (newProfile == null)
            throw new ArgumentNullException(nameof(newProfile));
        if (_profile != oldProfile)
            throw new ArgumentException("The oldProfile doesn't exist in Profile class", nameof(oldProfile));
        if (_profile == newProfile)  
            throw new ArgumentException("The profile already exists in the Profile class", nameof(newProfile));
        
        _profile = null;
        oldProfile.RemoveProfileReverse(this);

        // add new profile
        _profile = newProfile;
        newProfile.AddProfileReverse(this);
    }
    
    // Profile reverse funcs
    public void AddProfileReverse(Profile profile)
    {
        _profile = profile;
    }

    public void RemoveProfileReverse(Profile profile)
    {
        _profile = profile;
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
    
    public static Person AddPerson(
        string email,
        string name,
        string password,
        DateTime registrationDate,
        DateTime birthDate,
        bool isActive,
        int balance,
        Profile profile,
        Address address
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
            profile
        );

        return person;
    }


    public static void DeletePerson(Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (!_extent.Contains(person)) throw new InvalidOperationException("Person not found in the extent.");

        // Удаляем профиль вместе с `Person`
        person._profile = null;
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
    

    // Метод для добавления очков
    public void AddPoints(int points)
    {
        if (points < 0)
            throw new ArgumentException("Points cannot be negative.");

        _totalPoints += points;
        Console.WriteLine($"{Name} gained {points} points. Total: {_totalPoints}.");

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
