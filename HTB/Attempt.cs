namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

public class Attempt
{
    private Person _person;
    private Challenge _challenge;
    private DateTime _timestamp;
    private string _result;

    private static List<Attempt> _extent = new List<Attempt>();

    [Required(ErrorMessage = "Person is required.")]
    public Person Person
    {
        get => _person;
        private set
        {
            _person?.RemoveAttempt(this); // Убираем старую связь
            _person = value;
            _person?.AddAttempt(this);   // Устанавливаем новую связь
        }
    }

    [Required(ErrorMessage = "Challenge is required.")]
    public Challenge Challenge
    {
        get => _challenge;
        private set
        {
            _challenge?.RemoveAttempt(this); // Убираем старую связь
            _challenge = value;
            _challenge?.AddAttempt(this);   // Устанавливаем новую связь
        }
    }

    [Required(ErrorMessage = "Timestamp is required.")]
    public DateTime Timestamp
    {
        get => _timestamp;
        private set => _timestamp = value;
    }

    [Required(ErrorMessage = "Result is required.")]
    [StringLength(100, ErrorMessage = "Result cannot exceed 100 characters.")]
    public string Result
    {
        get => _result;
        private set => _result = value;
    }

    public static IReadOnlyList<Attempt> Extent => _extent.AsReadOnly();

    public Attempt(Person person, Challenge challenge, DateTime timestamp, string result)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));

        Person = person;
        Challenge = challenge;
        _timestamp = timestamp;
        _result = result;

        _extent.Add(this);
    }

    public void UpdateAttempt(DateTime? timestamp = null, string? result = null)
    {
        if (timestamp != null) _timestamp = timestamp.Value;
        if (result != null) _result = result;
    }

    public static void DeleteAttempt(Attempt attempt)
    {
        if (attempt == null) throw new ArgumentNullException(nameof(attempt));

        attempt.Person = null; // Убираем связь с Person
        attempt.Challenge = null; // Убираем связь с Challenge
        _extent.Remove(attempt);
    }

    public static void SaveExtent(string filename = "attempt_extent.json")
    {
        try
        {
            var json = JsonSerializer.Serialize(_extent);
            File.WriteAllText(filename, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static void LoadExtent(string filename = "attempt_extent.json")
    {
        if (File.Exists(filename))
        {
            try
            {
                var json = File.ReadAllText(filename);
                var loadedExtent = JsonSerializer.Deserialize<List<Attempt>>(json);

                _extent.Clear();
                if (loadedExtent != null)
                {
                    _extent.AddRange(loadedExtent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static void ClearExtent()
    {
        _extent.Clear();
    }
}

