namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Attempt
{
    public Person Person { get; set; }
    public Challenge Challenge { get; set; }
    public DateTime Timestamp { get; private set; }
    public string Result { get; private set; }

    public static List<Attempt> Extent { get; set; } = new List<Attempt>();

    public Attempt(Person person, Challenge challenge, DateTime timestamp, string result)
    {
        Person = person;
        Challenge = challenge;
        Timestamp = timestamp;
        Result = result;

        Extent.Add(this);
    }

    public void RecordAttempt()
    {
        Console.WriteLine($"attempt recorded at {Timestamp} with result: {Result} for person: {Person.Name} on challenge: {Challenge.ChallengeName}");
    }

    public static void SaveExtent(string filename = "attempt_extent.json")
    {
        try
        {
            var json = JsonSerializer.Serialize(Extent);
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

                Extent.Clear();
                if (loadedExtent != null)
                {
                    Extent.AddRange(loadedExtent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
    }
}
