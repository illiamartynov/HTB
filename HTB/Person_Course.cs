namespace HTB;

using System;

public class Person_Course
{
    public Person Person { get; set; }
    public Course Course { get; set; }
    public bool IsCompleted { get; set; }
    public string Level { get; set; }

    public Person_Course(Person person, Course course, string level, bool isCompleted = false)
    {
        Person = person;
        Course = course;
        Level = level;
        IsCompleted = isCompleted;
    }

    public void CompleteCourse()
    {
        IsCompleted = true;
        Console.WriteLine($"Course {Course.CourseName} completed by {Person.Name}.");
    }
}