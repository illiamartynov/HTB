using System;
using System.ComponentModel.DataAnnotations;
using HTB;

public class CompletenessLevel
{
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

    public CompletenessLevel(int completenessPercentage, DateTime startDate, Person person, Course course)
    {
        if (completenessPercentage < 0 || completenessPercentage > 100)
            throw new ArgumentException("Completeness percentage must be between 0 and 100.");

        CompletenessPercentage = completenessPercentage;
        StartDate = startDate;
        Person = person ?? throw new ArgumentNullException(nameof(person));
        Course = course ?? throw new ArgumentNullException(nameof(course));
    }
    

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
}