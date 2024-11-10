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
    private static List<Challenge> _extent = new List<Challenge>();
    private string _challengeName;
    private string _difficulty;
    private string _description;
    private int _points;
    private ChallengeStatus _status;
    private List<Attempt> _attempts = new List<Attempt>();

    public static List<Challenge> Extent => _extent;

    public string ChallengeName
    {
        get => _challengeName;
        set => _challengeName = value;
    }

    public string Difficulty
    {
        get => _difficulty;
        set => _difficulty = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public int Points
    {
        get => _points;
        set => _points = value;
    }

    public ChallengeStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public List<Attempt> Attempts => _attempts;

    public Challenge(string challengeName, string difficulty, string description, int points, ChallengeStatus status)
    {
        _challengeName = challengeName;
        _difficulty = difficulty;
        _description = description;
        _points = points;
        _status = status;
        _extent.Add(this);
    }

    public static void SaveExtent(string filename = "challenge_extent.json")
    {
        var json = JsonSerializer.Serialize(_extent);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "challenge_extent.json")
    {
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            _extent = JsonSerializer.Deserialize<List<Challenge>>(json) ?? new List<Challenge>();
        }
    }

    public void AddAttempt(Attempt attempt)
    {
        _attempts.Add(attempt);
    }
}
