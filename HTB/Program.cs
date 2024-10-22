using HTB;

class Program
{
    static void Main(string[] args)
    {
        var leaderboard = new Leaderboard(1, 500);
        var profile1 = new Profile(200, "Intermediate");
        var profile2 = new Profile(100, "Beginner");

        var person1 = new Person("test1@example.com", "Test User 1", "password1", DateTime.Now, new DateTime(1990, 1, 1), true, 100, profile1, leaderboard);
        var person2 = new Person("test2@example.com", "Test User 2", "password2", DateTime.Now, new DateTime(1992, 2, 2), false, 50, profile2, leaderboard);

        var certificate = new Certificate(1, DateTime.Now);
        person1.AddCertificate(certificate);

        var payment = new Payment(1, 100.0f, DateTime.Now, "Credit Card");
        person1.AddPayment(payment);

        var notification = new Notification(1, "Welcome to the platform!", false);
        person1.AddNotification(notification);

        Console.WriteLine("Before serialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }

        Person.SaveExtent("person_extent.json");
        Console.WriteLine("\nObjects have been serialized and saved to person_extent.json");

        Person.Extent.Clear();

        Console.WriteLine("\nAfter clearing the list:");
        Console.WriteLine($"Person count: {Person.Extent.Count}");

        Person.LoadExtent("person_extent.json");

        Console.WriteLine("\nAfter deserialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }
    }
}