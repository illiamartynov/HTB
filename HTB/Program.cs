using HTB;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Создаем Leaderboard
        var leaderboard = new Leaderboard();

        // Создаем адреса через фабричный метод AddAddress
        var address1 = Address.AddAddress("USA", "New York", "5th Avenue", 10);
        var address2 = Address.AddAddress("Canada", "Toronto", "Queen St", 20);

        // Создаем ранги
        var rank1 = new Rank(1);
        var rank2 = new Rank(2);

        // Создаем уровни завершенности
        var completenessLevel1 = new CompletenessLevel(80, DateTime.Now.AddMonths(-1));
        var completenessLevel2 = new CompletenessLevel(50, DateTime.Now.AddMonths(-2));

        // Создаем подписки
        var subscription1 = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Paid(100));
        var subscription2 = new Subscription(2, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

        // Создаем объекты Person через фабричный метод AddPerson, автоматически создавая профили внутри
        var person1 = Person.AddPerson(
            "test1@example.com", "Test User 1", "password1", DateTime.Now, new DateTime(1990, 1, 1),
            true, 100, null, leaderboard, address1, rank1, completenessLevel1, subscription1
        );

        var person2 = Person.AddPerson(
            "test2@example.com", "Test User 2", "password2", DateTime.Now, new DateTime(1992, 2, 2),
            false, 50, null, leaderboard, address2, rank2, completenessLevel2, subscription2
        );

        // Привязываем профили к людям
        person1.AssignProfile(new Profile(200, "Intermediate", person1));
        person2.AssignProfile(new Profile(100, "Beginner", person2));

        // Добавляем людей в Leaderboard
        leaderboard.AddPersonToLeaderboard(person1, 1, 500);
        leaderboard.AddPersonToLeaderboard(person2, 2, 300);

        // Вывод данных перед сериализацией
        Console.WriteLine("Before serialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}, Address: {person.Address.Street}");
        }

        // Сериализация списка объектов Person
        Person.SaveExtent("person_extent.json");
        Console.WriteLine("\nObjects have been serialized and saved to person_extent.json");

        // Очистка списка Person
        Person.ClearExtent();
        Console.WriteLine("\nAfter clearing the list:");
        Console.WriteLine($"Person count: {Person.Extent.Count}");

        // Десериализация объектов Person
        Person.LoadExtent("person_extent.json");
        Console.WriteLine("\nAfter deserialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}, Address: {person.Address.Street}");
        }

        // Вывод информации о подписках
        Console.WriteLine("\nSubscription Details:");
        subscription1.ShowSubscriptionInfo();
        subscription2.ShowSubscriptionInfo();

        // Вывод рейтинга Leaderboard
        Console.WriteLine("\nLeaderboard Rankings:");
        leaderboard.ViewRankings();
    }
}
