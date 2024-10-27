using HTB;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Инициализация Leaderboard
        var leaderboard = new Leaderboard();
        var profile1 = new Profile(200, "intermediate");
        var profile2 = new Profile(100, "Beginner");

        // Инициализация Person
        var person1 = new Person("test1@example.com", "Test User 1", "password1", DateTime.Now, new DateTime(1990, 1, 1), true, 100, profile1, leaderboard);
        var person2 = new Person("test2@example.com", "Test User 2", "password2", DateTime.Now, new DateTime(1992, 2, 2), false, 50, profile2, leaderboard);

        // Добавление людей в Leaderboard
        leaderboard.AddPersonToLeaderboard(person1, 1, 500);
        leaderboard.AddPersonToLeaderboard(person2, 2, 300);

        // Генерация сертификата и привязка к человеку
        var certificate = new Certificate(1, DateTime.Now);
        person1.AddCertificate(certificate);

        // Добавление платежа к человеку
        var payment = new Payment(1, 100.0f, DateTime.Now, "Credit Card");
        person1.AddPayment(payment);

        // Добавление уведомления к человеку
        var notification = new Notification(1, "Welcome to the platform!", false);
        person1.AddNotification(notification);

        // Вывод информации до сериализации
        Console.WriteLine("Before serialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }

        // Сохранение данных Person в файл
        Person.SaveExtent("person_extent.json");
        Console.WriteLine("\nObjects have been serialized and saved to person_extent.json");

        // Очистка списка Person
        Person.Extent.Clear();

        Console.WriteLine("\nAfter clearing the list:");
        Console.WriteLine($"Person count: {Person.Extent.Count}");

        // Загрузка данных из файла
        Person.LoadExtent("person_extent.json");

        // Вывод информации после десериализации
        Console.WriteLine("\nAfter deserialization:");
        foreach (var person in Person.Extent)
        {
            Console.WriteLine($"Name: {person.Name}, Email: {person.Email}, Balance: {person.Balance}");
        }

        // Вывод рангов
        Console.WriteLine("\nLeaderboard Rankings:");
        leaderboard.ViewRankings();
    }
}
