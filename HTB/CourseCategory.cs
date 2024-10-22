namespace HTB;

using System;


public class CourseCategory
{
    public string CategoryName { get; set; }
    public string Description { get; set; }

    
    public CourseCategory(string categoryName, string description)
    {
        CategoryName = categoryName;
        Description = description;
    }

    
    public void ViewCoursesByCategory()
    {
        Console.WriteLine($"Category: {CategoryName}, Description: {Description}");
    }
}
