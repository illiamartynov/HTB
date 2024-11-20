using System;
using System.ComponentModel.DataAnnotations;

public class CompletenessLevel
{
    private int _completenessPercentage;
    private DateTime _startDate;

    [Required(ErrorMessage = "Completeness percentage is required.")]
    [Range(0, 100, ErrorMessage = "Completeness percentage must be between 0 and 100.")]
    public int CompletenessPercentage
    {
        get => _completenessPercentage;
        set
        {
            if (value >= 0 && value <= 100)
                _completenessPercentage = value;
            else
                Console.WriteLine("Invalid percentage value. Must be between 0 and 100.");
        }
    }

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Start date must be a valid date.")]
    public DateTime StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public CompletenessLevel(int completenessPercentage, DateTime startDate)
    {
        _completenessPercentage = completenessPercentage >= 0 && completenessPercentage <= 100
            ? completenessPercentage
            : throw new ArgumentException("Completeness percentage must be between 0 and 100.");
        _startDate = startDate;
    }

    public void UpdateCompleteness(int newPercentage)
    {
        if (newPercentage >= 0 && newPercentage <= 100)
        {
            _completenessPercentage = newPercentage;
            Console.WriteLine($"Completeness updated to {newPercentage}%.");
        }
        else
        {
            Console.WriteLine("Invalid percentage value. Must be between 0 and 100.");
        }
    }
}