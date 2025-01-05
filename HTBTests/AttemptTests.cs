using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class AttemptTests
    {
        private Person person;
        private Challenge challenge;

        [SetUp]
        public void Setup()
        {
            Attempt.ClearExtent();
            Person.ClearExtent();

            challenge = new Challenge("Buffer Overflow", "Hard", "Exploit a buffer overflow", 100, ChallengeStatus.NotTried);

            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var leaderboard = new Leaderboard();
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            // Создаем объект Person с упрощением Profile, Rank, CompletenessLevel
            person = Person.AddPerson(
                email: "person@example.com",
                name: "TestPerson",
                password: "password",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 0,
                profile: new Profile(0, "Novice", null), // Profile создан без Person, связь добавляется позже
                address: address,
                rank: null, // Rank будет назначен после создания Person
                completenessLevel: null, // CompletenessLevel будет назначен после создания Person
                subscription: subscription
            );

            // Назначение связи для Profile, Rank, CompletenessLevel
            var profile = new Profile(0, "Novice", person);
            var rank = new Rank(1, person, leaderboard);

            person.AssignProfile(profile);
            person.UpdateRank(rank);
        }

        [Test]
        public void TestAttemptAddedToExtent()
        {
            var attempt = new Attempt(person, challenge, "Success");

            Assert.That(Attempt.Extent.Count, Is.EqualTo(1));
            Assert.That(Attempt.Extent.Contains(attempt), Is.True);
        }

        [Test]
        public void TestSaveExtent()
        {
            var filename = "test_attempt_extent.json";
            var attempt = new Attempt(person, challenge, "Success");

            Attempt.SaveExtent(filename);

            Assert.That(System.IO.File.Exists(filename), Is.True);

            var content = System.IO.File.ReadAllText(filename);
            Assert.That(content, Does.Contain(person.Name));
            Assert.That(content, Does.Contain(challenge.ChallengeName));

            System.IO.File.Delete(filename);
        }

        [Test]
        public void TestLoadExtent()
        {
            var filename = "test_attempt_extent.json";
            var timestamp = DateTime.Now;
            var attempt = new Attempt(person, challenge, "Success");

            Attempt.SaveExtent(filename);

            Attempt.ClearExtent();
            Attempt.LoadExtent(filename);

            Assert.Multiple(() =>
            {
                Assert.That(Attempt.Extent.Count, Is.EqualTo(1));
                var loadedAttempt = Attempt.Extent[0];
                Assert.That(loadedAttempt.Person.Name, Is.EqualTo(person.Name));
                Assert.That(loadedAttempt.Challenge.ChallengeName, Is.EqualTo(challenge.ChallengeName));
                Assert.That(loadedAttempt.Result, Is.EqualTo("Success"));
            });

            System.IO.File.Delete(filename);
        }
    }
}
