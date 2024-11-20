namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Attempt
{
    private Person _person;
    private Challenge _challenge;
    private DateTime _timestamp;
    private string _result;

    private static List<Attempt> _extent = new List<Attempt>();

    public Person Person
    {
        get => _person;
        set => _person = value;
    }

    public Challenge Challenge
    {
        get => _challenge;
        set => _challenge = value;
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        private set => _timestamp = value;
    }

    public string Result
    {
        get => _result;
        private set => _result = value;
    }

    public static IReadOnlyList<Attempt> Extent => _extent.AsReadOnly();

    public Attempt(Person person, Challenge challenge, DateTime timestamp, string result)
    {
        _person = person;
        _challenge = challenge;
        _timestamp = timestamp;
        _result = result;

        _extent.Add(this);
    }

    public void RecordAttempt()
    {
        Console.WriteLine($"attempt recorded at {_timestamp} with result: {_result} for person: {_person.Name} on challenge: {_challenge.ChallengeName}");
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
            Console.WriteLine($"error: {ex.Message}");
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
                Console.WriteLine($"error: {ex.Message}");
            }
        }
    }

    public static void ClearExtent()
    {
        _extent.Clear();
    }
}
