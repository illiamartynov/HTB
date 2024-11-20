namespace HTB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Resource
{
    public static List<Resource> Resources = new List<Resource>();

    private string _resourceName;
    private string _resourceType;
    private string _url;

    [Required(ErrorMessage = "Resource name is required.")]
    [StringLength(100, ErrorMessage = "Resource name cannot exceed 100 characters.")]
    public string ResourceName
    {
        get => _resourceName;
        set => _resourceName = value;
    }

    [Required(ErrorMessage = "Resource type is required.")]
    [StringLength(50, ErrorMessage = "Resource type cannot exceed 50 characters.")]
    public string ResourceType
    {
        get => _resourceType;
        set => _resourceType = value;
    }

    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Invalid URL format.")]
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