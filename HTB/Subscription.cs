using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

        public int SubscriptionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionType Type { get; set; }  
        public IAccessType AccessType { get; set; }  

        public Subscription(int subscriptionID, DateTime startDate, DateTime endDate, SubscriptionType type, IAccessType accessType)
        {
            SubscriptionID = subscriptionID;
            StartDate = startDate;
            EndDate = endDate;
            Type = type;
            AccessType = accessType;
            Extent.Add(this);  
        }

        public void ShowSubscriptionInfo()
        {
            Console.WriteLine($"Subscription ID: {SubscriptionID}");
            Console.WriteLine($"Type: {Type}");
            Console.WriteLine($"Access Details: {AccessType.GetAccessDescription()}");
            Console.WriteLine($"Duration: {StartDate} - {EndDate}");
        }

        public static void SaveExtent(string filename = "subscription_extent.json")
        {
            var json = JsonSerializer.Serialize(Extent);
            File.WriteAllText(filename, json);
        }

        public static void LoadExtent(string filename = "subscription_extent.json")
        {
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                Extent = JsonSerializer.Deserialize<List<Subscription>>(json);
            }
        }
    }
}