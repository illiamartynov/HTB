using System;
using System.ComponentModel.DataAnnotations;

public class Rank
{
    private int _rankLevel;

    [Required(ErrorMessage = "Rank level is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Rank level must be greater than 0.")]
    public int RankLevel
    {
        get => _rankLevel;
        set
        {
            if (value > 0)
                _rankLevel = value;
            else
                Console.WriteLine("Invalid rank level. Must be greater than 0.");
        }
    }

    public Rank(int rankLevel)
    {
        _rankLevel = rankLevel > 0 ? rankLevel : throw new ArgumentException("Rank level must be greater than 0.");
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