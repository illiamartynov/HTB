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

        private int _subscriptionID;
        private DateTime _startDate;
        private DateTime _endDate;
        private SubscriptionType _type;
        private IAccessType _accessType;

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

        public IAccessType AccessType
        {
            get => _accessType;
            set => _accessType = value;
        }

        public Subscription(int subscriptionID, DateTime startDate, DateTime endDate, SubscriptionType type, IAccessType accessType)
        {
            _subscriptionID = subscriptionID;
            _startDate = startDate;
            _endDate = endDate;
            _type = type;
            _accessType = accessType;
            Extent.Add(this);
        }

        public void ShowSubscriptionInfo()
        {
            Console.WriteLine($"Subscription ID: {_subscriptionID}");
            Console.WriteLine($"Type: {_type}");
            Console.WriteLine($"Access Details: {_accessType.GetAccessDescription()}");
            Console.WriteLine($"Duration: {_startDate} - {_endDate}");
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
