using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class ChallengeTests
    {
        private Challenge challenge;
        private Person person;

        [SetUp]
        public void Setup()
        {
            Challenge.Extent.Clear();
            challenge = new Challenge("Buffer Overflow", "Hard", "Exploit a buffer overflow", 100, ChallengeStatus.NotTried);

            
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
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
                leaderboard: new Leaderboard(),
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: subscription
            );
        }

        [Test]
        public void TestChallengeCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(challenge.ChallengeName, Is.EqualTo("Buffer Overflow"));
                Assert.That(challenge.Difficulty, Is.EqualTo("Hard"));
                Assert.That(challenge.Points, Is.EqualTo(100));
                Assert.That(challenge.Status, Is.EqualTo(ChallengeStatus.NotTried));
            });
        }

        [Test]
        public void TestAddAttempt()
        {
            var attempt = new Attempt(person, challenge, DateTime.Now, "Success");

            Assert.Multiple(() =>
            {
                Assert.That(attempt.Result, Is.EqualTo("Success"));
                Assert.That(attempt.Challenge, Is.EqualTo(challenge));
                Assert.That(attempt.Person, Is.EqualTo(person));
            });
        }
    }
}
