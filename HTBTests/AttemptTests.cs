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
            Challenge.Extent.Clear();
            Person.ClearExtent();

            challenge = new Challenge("Buffer Overflow", "Hard", "Exploit a buffer overflow", 100, ChallengeStatus.NotTried);

            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var leaderboard = new Leaderboard();
            var profile = new Profile(0, "Novice");
            var rank = new Rank(1);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            person = Person.AddPerson(
                email: "person@example.com",
                name: "TestPerson",
                password: "password",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 0,
                profile: profile,
                leaderboard: leaderboard,
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: subscription
            );
        }

        [Test]
        public void TestAttemptCreation()
        {
            var timestamp = DateTime.Now;
            var attempt = new Attempt(person, challenge, timestamp, "Success");

            Assert.Multiple(() =>
            {
                Assert.That(attempt.Person, Is.EqualTo(person));
                Assert.That(attempt.Challenge, Is.EqualTo(challenge));
                Assert.That(attempt.Timestamp, Is.EqualTo(timestamp));
                Assert.That(attempt.Result, Is.EqualTo("Success"));
            });
        }

        [Test]
        public void TestAttemptAddedToExtent()
        {
            var attempt = new Attempt(person, challenge, DateTime.Now, "Success");

            Assert.That(Attempt.Extent.Count, Is.EqualTo(1));
            Assert.That(Attempt.Extent.Contains(attempt), Is.True);
        }

        [Test]
        public void TestRecordAttempt()
        {
            var timestamp = DateTime.Now;
            var attempt = new Attempt(person, challenge, timestamp, "Success");

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                attempt.RecordAttempt();

                var output = sw.ToString().Trim();
                Assert.That(output, Is.EqualTo($"attempt recorded at {timestamp} with result: Success for person: {person.Name} on challenge: {challenge.ChallengeName}"));
            }
        }

        [Test]
        public void TestSaveExtent()
        {
            var filename = "test_attempt_extent.json";
            var attempt = new Attempt(person, challenge, DateTime.Now, "Success");

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
            var attempt = new Attempt(person, challenge, timestamp, "Success");

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
