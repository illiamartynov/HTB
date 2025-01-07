using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using HTB;

public class CompletenessLevel
{
    private static List<CompletenessLevel> _extent = new List<CompletenessLevel>();
    public static IReadOnlyList<CompletenessLevel> Extent => _extent.AsReadOnly();
    
    private int _completenessPercentage;
    private DateTime _startDate;

    [Required(ErrorMessage = "Completeness percentage is required.")]
    [Range(0, 100, ErrorMessage = "Completeness percentage must be between 0 and 100.")]
    public int CompletenessPercentage { get; private set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; private set; }
    
    [Required]
    public Person Person { get; private set; }

    [Required]
    public Course Course { get; private set; }

    public CompletenessLevel(int completenessPercentage, Person person, Course course)
    {
        if (completenessPercentage < 0 || completenessPercentage > 100)
            throw new ArgumentException("Completeness percentage must be between 0 and 100.");

        CompletenessPercentage = completenessPercentage;
        StartDate = DateTime.Now;
        Person = person ?? throw new ArgumentNullException(nameof(person));
        Course = course ?? throw new ArgumentNullException(nameof(course));
    }
    

    public void RemoveCompletenessLevel()
    {
        _extent.Remove(this);
    }

    public void DisassociateCompletenessLevel()
    {
        Person = null;
        Course = null;
    }
    
    // rank funcs
    public void UpdateCompleteness(int newPercentage)
    {
        if (newPercentage >= 0 && newPercentage <= 100)
        {
            CompletenessPercentage = newPercentage;
            Console.WriteLine($"Completeness updated to {newPercentage}%.");
        }
        else
        {
            throw new ArgumentException("Invalid percentage value. Must be between 0 and 100.");
        }
    }
    
    // extent funcs
    public static void SaveExtent(string filename = "completenesslevel_extent.json")
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

    public static void LoadExtent(string filename = "completenesslevel_extent.json")
    {
        if (File.Exists(filename))
        {
            try
            {
                var json = File.ReadAllText(filename);
                var loadedExtent = JsonSerializer.Deserialize<List<CompletenessLevel>>(json);

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