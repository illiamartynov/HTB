using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTB
{
    public enum SubscriptionType
    {
        Free,
        Premium
    }

    public class Subscription
{
    public static List<Subscription> Extent = new List<Subscription>();

    private int _subscriptionID;
    private DateTime _startDate;
    private DateTime _endDate;
    private SubscriptionType _type;

    [JsonConverter(typeof(AccessTypeConverter))] 
    public IAccessType AccessType { get; set; }

    public int SubscriptionID
    {
        get => _subscriptionID;
        set => _subscriptionID = value;
    }

    public DateTime StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public DateTime EndDate
    {
        get => _endDate;
        set => _endDate = value;
    }

    public SubscriptionType Type
    {
        get => _type;
        set => _type = value;
    }

    public Subscription(int subscriptionID, DateTime startDate, DateTime endDate, SubscriptionType type, IAccessType accessType)
    {
        _subscriptionID = subscriptionID;
        _startDate = startDate;
        _endDate = endDate;
        _type = type;
        AccessType = accessType;
        Extent.Add(this);
    }

    public void ShowSubscriptionInfo()
    {
        Console.WriteLine($"Subscription ID: {_subscriptionID}");
        Console.WriteLine($"Type: {_type}");
        Console.WriteLine($"Access Details: {AccessType.GetAccessDescription()}");
        Console.WriteLine($"Duration: {_startDate} - {_endDate}");
    }

    public static void SaveExtent(string filename = "subscription_extent.json")
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Extent, options);
        File.WriteAllText(filename, json);
    }

    public static void LoadExtent(string filename = "subscription_extent.json")
    {
        if (File.Exists(filename))
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AccessTypeConverter());
            Extent = JsonSerializer.Deserialize<List<Subscription>>(File.ReadAllText(filename), options);
        }
    }
}

}
