using HTB;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var leaderboard = new Leaderboard();
        var profile1 = new Profile(200, "Intermediate");
        var profile2 = new Profile(100, "Beginner");

        var address1 = new Address("USA", "New York", "5th Avenue", 10);
        var rank1 = new Rank(1);
        var completenessLevel1 = new CompletenessLevel(80, DateTime.Now.AddMonths(-1));

        var address2 = new Address("Canada", "Toronto", "Queen St", 20);
        var rank2 = new Rank(2);
        var completenessLevel2 = new CompletenessLevel(50, DateTime.Now.AddMonths(-2));

        var subscription1 = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Paid(100)); 
        var subscription2 = new Subscription(2, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30)); 

        var person1 = new Person(
            "test1@example.com", "Test User 1", "password1", DateTime.Now, new DateTime(1990, 1, 1), 
            true, 100, profile1, leaderboard, address1, rank1, completenessLevel1, subscription1);
        
        var person2 = new Person(
            "test2@example.com", "Test User 2", "password2", DateTime.Now, new DateTime(1992, 2, 2), 
            false, 50, profile2, leaderboard, address2, rank2, completenessLevel2, subscription2);

        leaderboard.AddPersonToLeaderboard(person1, 1, 500);
        leaderboard.AddPersonToLeaderboard(person2, 2, 300);

        var certificate = new Certificate(1, DateTime.Now);
        person1.AddCertificate(certificate);

        var payment = new Payment(1, 100.0f, DateTime.Now, "Credit Card", "USD");
        person1.AddPayment(payment);

        Console.WriteLine("Before serialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }

        Person.SaveExtent("person_extent.json");
        Console.WriteLine("\nObjects have been serialized and saved to person_extent.json");


        Console.WriteLine("\nAfter clearing the list:");
        Console.WriteLine($"Person count: {Person.Extent.Count}");

        Person.LoadExtent("person_extent.json");

        Console.WriteLine("\nAfter deserialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }

        Console.WriteLine("\nSubscription Details:");
        subscription1.ShowSubscriptionInfo();
        subscription2.ShowSubscriptionInfo();

        Console.WriteLine("\nLeaderboard Rankings:");
        leaderboard.ViewRankings();
    }
}
