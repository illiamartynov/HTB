using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

namespace HTB
{
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
    private List<Person> _participants = new List<Person>(); // Ассоциация с Person

    public static List<Challenge> Extent => _extent;

    [Required(ErrorMessage = "Challenge name is required.")]
    [StringLength(100, ErrorMessage = "Challenge name cannot exceed 100 characters.")]
    public string ChallengeName
    {
        get => _challengeName;
        set => _challengeName = value;
    }

    [Required(ErrorMessage = "Difficulty is required.")]
    [StringLength(50, ErrorMessage = "Difficulty cannot exceed 50 characters.")]
    public string Difficulty
    {
        get => _difficulty;
        set => _difficulty = value;
    }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description
    {
        get => _description;
        set => _description = value;
    }

    [Required(ErrorMessage = "Points are required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Points must be greater than 0.")]
    public int Points
    {
        get => _points;
        set => _points = value;
    }

    [Required(ErrorMessage = "Status is required.")]
    public ChallengeStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();
    public IReadOnlyList<Person> Participants => _participants.AsReadOnly();

    public Challenge(string challengeName, string difficulty, string description, int points, ChallengeStatus status)
    {
        _challengeName = challengeName;
        _difficulty = difficulty;
        _description = description;
        _points = points;
        _status = status;
        _extent.Add(this);
    }

    public void AddAttempt(Attempt attempt)
    {
        if (attempt == null) throw new ArgumentNullException(nameof(attempt));
        if (!_attempts.Contains(attempt))
        {
            _attempts.Add(attempt);
        }
    }

    public void RemoveAttempt(Attempt attempt)
    {
        if (attempt == null) throw new ArgumentNullException(nameof(attempt));
        _attempts.Remove(attempt);
    }

    public void AddParticipant(Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (!_participants.Contains(person))
        {
            _participants.Add(person);
            person.AddChallenge(this);
        }
    }

    public void RemoveParticipant(Person person)
    {
        if (person == null) throw new ArgumentNullException(nameof(person));
        if (_participants.Remove(person))
        {
            person.RemoveChallenge(this);
        }
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

    public static void DeleteChallenge(Challenge challenge)
    {
        if (challenge == null) throw new ArgumentNullException(nameof(challenge));
        foreach (var attempt in challenge._attempts)
        {
            Attempt.DeleteAttempt(attempt);
        }
        foreach (var participant in challenge._participants)
        {
            participant.RemoveChallenge(challenge);
        }
        _extent.Remove(challenge);
    }
}

}
