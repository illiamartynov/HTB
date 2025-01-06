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
    public static IReadOnlyList<Attempt> Extent => _extent.AsReadOnly();


    [Required(ErrorMessage = "Person is required.")]
    public Person Person
    {
        get => _person;
        private set {}
    }

    [Required(ErrorMessage = "Challenge is required.")]
    public Challenge Challenge
    {
        get => _challenge;
        private set {}
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
    
    public Attempt(Person person, Challenge challenge, string result)
    {
        Person = person ?? throw new ArgumentNullException(nameof(person));
        Challenge = challenge ?? throw new ArgumentNullException(nameof(challenge));
        Timestamp = DateTime.Now;
        Result = result ?? throw new ArgumentNullException(nameof(result));

        _extent.Add(this);
    }

    public void RemoveAttempt()
    {
        _extent.Remove(this);
    }

    public void DisassociateAttempt()
    {
        _person = null;
        _challenge = null;
    }

    
    // extent funcs
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

