namespace HTB;

using System;
using System.Collections.Generic;

public class Resource
{
    public static List<Resource> Resources = new List<Resource>();

    private string _resourceName;
    private string _resourceType;
    private string _url;

    public string ResourceName
    {
        get => _resourceName;
        set => _resourceName = value;
    }

    public string ResourceType
    {
        get => _resourceType;
        set => _resourceType = value;
    }

    public string Url
    {
        get => _url;
        set => _url = value;
    }

    public Resource(string resourceName, string resourceType, string url)
    {
        _resourceName = resourceName;
        _resourceType = resourceType;
        _url = url;
        Resources.Add(this);
    }

    public void ViewResource()
    {
        Console.WriteLine($"Viewing resource: {_resourceName}");
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