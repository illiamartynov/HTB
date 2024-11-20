using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class LeaderboardTests
    {
        private Leaderboard leaderboard;
        private Person person1;
        private Person person2;

        [SetUp]
        public void Setup()
        {
            leaderboard = new Leaderboard();

            person1 = new Person(
                email: "person1@example.com",
                name: "Alice",
                password: "password1",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 100,
                profile: new Profile(0, "Novice"),
                leaderboard: new Leaderboard(),
                address: new Address("USA", "New York", "5th Avenue", 101),
                rank: new Rank(1),
                completenessLevel: new CompletenessLevel(50, DateTime.Now),
                subscription: new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30))
            );

            person2 = new Person(
                email: "person2@example.com",
                name: "Bob",
                password: "password2",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 200,
                profile: new Profile(0, "Intermediate"),
                leaderboard: new Leaderboard(),
                address: new Address("USA", "Los Angeles", "Sunset Blvd", 202),
                rank: new Rank(2),
                completenessLevel: new CompletenessLevel(75, DateTime.Now),
                subscription: new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Free(30))
            );
        }

        [Test]
        public void TestAddPersonToLeaderboard()
        {
            leaderboard.AddPersonToLeaderboard(person1, 1, 500);

            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.RankedPeople.Count, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].Person, Is.EqualTo(person1));
                Assert.That(leaderboard.RankedPeople[0].Rank, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].TotalPoints, Is.EqualTo(500));
            });
        }

        [Test]
        public void TestViewRankings()
        {
            leaderboard.AddPersonToLeaderboard(person1, 1, 500);
            leaderboard.AddPersonToLeaderboard(person2, 2, 300);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                leaderboard.ViewRankings();

                var output = sw.ToString().Trim();
                Assert.Multiple(() =>
                {
                    Assert.That(output, Does.Contain("Person: Alice, Rank: 1, Total Points: 500"));
                    Assert.That(output, Does.Contain("Person: Bob, Rank: 2, Total Points: 300"));
                });
            }
        }

        [Test]
        public void TestUpdateRankings_ExistingPerson()
        {
            leaderboard.AddPersonToLeaderboard(person1, 1, 500);

            leaderboard.UpdateRankings(person1, 2, 700);

            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.RankedPeople.Count, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].Person, Is.EqualTo(person1));
                Assert.That(leaderboard.RankedPeople[0].Rank, Is.EqualTo(2));
                Assert.That(leaderboard.RankedPeople[0].TotalPoints, Is.EqualTo(700));
            });
        }

        [Test]
        public void TestUpdateRankings_NewPerson()
        {
            leaderboard.UpdateRankings(person2, 3, 400);

            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.RankedPeople.Count, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].Person, Is.EqualTo(person2));
                Assert.That(leaderboard.RankedPeople[0].Rank, Is.EqualTo(3));
                Assert.That(leaderboard.RankedPeople[0].TotalPoints, Is.EqualTo(400));
            });
        }
    }
}
