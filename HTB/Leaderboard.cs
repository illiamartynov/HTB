namespace HTB;

using System;
using System.Collections.Generic;

public class Leaderboard
{
    public int Rank { get; set; }
    public int TotalPoints { get; set; }
    public List<Person> RankedPeople { get; set; } = new List<Person>();

    public Leaderboard(int rank, int totalPoints)
    {
        Rank = rank;
        TotalPoints = totalPoints;
    }

    public void AddPersonToLeaderboard(Person person)
    {
        RankedPeople.Add(person);
    }

    public void ViewRankings()
    {
        foreach (var person in RankedPeople)
        {
            Console.WriteLine($"Person: {person.Name}, Rank: {Rank}, Total Points: {TotalPoints}");
        }
    }

    public void UpdateRankings(Person person, int newRank, int newTotalPoints)
    {
        Rank = newRank;
        TotalPoints = newTotalPoints;
        if (!RankedPeople.Contains(person))
        {
            RankedPeople.Add(person);
        }
    }
}