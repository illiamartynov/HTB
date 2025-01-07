using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using HTB;

public class Rank
{
    private static List<Rank> _extent = new List<Rank>();
    public static IReadOnlyList<Rank> Extent => _extent.AsReadOnly();
    
    private int _rankLevel;

    [Required(ErrorMessage = "Rank level is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Rank level must be greater than 0.")]
    public int RankLevel
    {
        get => _rankLevel; 
        private set {}
    }

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

    public void RemoveRank()
    {
        _extent.Remove(this);
    }

    public void DisassociateRank()
    {
        Person = null;
        Leaderboard = null;
    }
    
    // rank funcs
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
    
    // extent funcs
    public static void SaveExtent(string filename = "rank_extent.json")
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

    public static void LoadExtent(string filename = "rank_extent.json")
    {
        if (File.Exists(filename))
        {
            try
            {
                var json = File.ReadAllText(filename);
                var loadedExtent = JsonSerializer.Deserialize<List<Rank>>(json);

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