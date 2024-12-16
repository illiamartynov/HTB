using System;
using System.ComponentModel.DataAnnotations;
using HTB;

public class Rank
{
    private int _rankLevel;

    [Required(ErrorMessage = "Rank level is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Rank level must be greater than 0.")]
    public int RankLevel { get; private set; }

    [Required]
    public Person Person { get; private set; }

    [Required]
    public Leaderboard Leaderboard { get; private set; }

    public Rank(int rankLevel, Person person, Leaderboard leaderboard)
    {
        if (rankLevel <= 0)
            throw new ArgumentException("Rank level must be greater than 0.");

        RankLevel = rankLevel;
        Person = person ?? throw new ArgumentNullException(nameof(person));
        Leaderboard = leaderboard ?? throw new ArgumentNullException(nameof(leaderboard));
    }

    public void UpdateRank(int newRankLevel)
    {
        if (newRankLevel > 0)
        {
            _rankLevel = newRankLevel;
            Console.WriteLine($"Rank updated to level {newRankLevel}.");
        }
        else
        {
            Console.WriteLine("Invalid rank level. Must be greater than 0.");
        }
    }
}