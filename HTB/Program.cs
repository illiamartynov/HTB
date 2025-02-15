﻿using HTB;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Создаем Leaderboard
        var leaderboard = new Leaderboard("Main");

        // Создаем адреса через фабричный метод AddAddress
        var address1 = Address.AddAddress("USA", "New York", "5th Avenue", 10);
        var address2 = Address.AddAddress("Canada", "Toronto", "Queen St", 20);

        // Создаем подписки
        var subscription1 = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Paid(100));
        var subscription2 = new Subscription(2, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

        var profile1 = new Profile(200, "Intermediate", null);
        var profile2 = new Profile(100, "Beginner", null);
        
        // Создаем пользователей
        var person1 = Person.AddPerson(
            "test1@example.com", "Test User 1", "password1", DateTime.Now, new DateTime(1990, 1, 1),
            true, 100, profile1, address1
        );

        var person2 = Person.AddPerson(
            "test2@example.com", "Test User 2", "password2", DateTime.Now, new DateTime(1992, 2, 2),
            false, 50, profile2, address2
        );

        // Создаем курс (требуется для CompletenessLevel)
        var course1 = new Course("C# Basics", "Beginner", new Free(30));
        var course2 = new Course("Advanced C#", "Advanced", new Paid(150));
        
        person1.AddCompletenessLevel(10, course1);
        person2.AddCompletenessLevel(10, course2);
        
        person1.AddSubscription(subscription1);
        person2.AddSubscription(subscription2);


        // // Создаем уровни завершенности
        // var completenessLevel1 = new CompletenessLevel(80, DateTime.Now.AddMonths(-1), person1, course1);
        // var completenessLevel2 = new CompletenessLevel(50, DateTime.Now.AddMonths(-2), person2, course2);

        // // Добавляем ранги и завершенность пользователям
        // person1.UpdateRank(rank1);
        // person2.UpdateRank(rank2);
        //
        // Console.WriteLine("\nCompleteness Levels:");
        // Console.WriteLine($"Person: {person1.Name}, Course: {course1.CourseName}, Completeness: {completenessLevel1.CompletenessPercentage}%");
        // Console.WriteLine($"Person: {person2.Name}, Course: {course2.CourseName}, Completeness: {completenessLevel2.CompletenessPercentage}%");

    
    }
}
