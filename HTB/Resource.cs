namespace HTB;

using System;


public class Resource
{
    public string ResourceName { get; set; }
    public string ResourceType { get; set; }
    public string Url { get; set; }

    
    public Resource(string resourceName, string resourceType, string url)
    {
        ResourceName = resourceName;
        ResourceType = resourceType;
        Url = url;
    }

    
    public void ViewResource()
    {
        Console.WriteLine($"Viewing resource: {ResourceName}");
    }
}
