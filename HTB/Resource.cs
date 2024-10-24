namespace HTB;

using System;
using System.Collections.Generic;

public class Resource
{
    public static List<Resource> Resources = new List<Resource>();  

    public string ResourceName { get; set; }
    public string ResourceType { get; set; }
    public string Url { get; set; }

    public Resource(string resourceName, string resourceType, string url)
    {
        ResourceName = resourceName;
        ResourceType = resourceType;
        Url = url;
        Resources.Add(this); 
    }

    public void ViewResource()
    {
        Console.WriteLine($"Viewing resource: {ResourceName}");
    }

    public static void ViewAllResources()
    {
        Console.WriteLine("All resources:");
        foreach (var resource in Resources)
        {
            resource.ViewResource();
        }
    }
}