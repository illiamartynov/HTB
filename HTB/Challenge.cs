namespace HTB;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public enum ChallengeStatus
{
    NotTried,
    Attempted,
    Solved,
    Failed
}

public class Challenge
{
    public static List<Challenge> Extent = new List<Challenge>();  

    public string ChallengeName { get; set; }
    public string Difficulty { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
    public ChallengeStatus Status { get; set; }  
    public List<Attempt> Attempts { get; set; } = new List<Attempt>();  

    
    public Challenge(string challengeName, string difficulty, string description, int points, ChallengeStatus status)
    {
        ChallengeName = challengeName;
        Difficulty = difficulty;
        Description = description;
        Points = points;
        Status = status;
        Extent.Add(this);  
    }

    
    public static void SaveExtent(string filename = "challenge_extent.json")
    {
        var json = JsonSerializer.Serialize(Extent);
        File.WriteAllText(filename, json);
    }

    
    public static void LoadExtent(string filename = "challenge_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            Extent = JsonSerializer.Deserialize<List<Challenge>>(json);
        }
    }

    
    public void AddAttempt(Attempt attempt)
    {
        Attempts.Add(attempt);
    }
}
