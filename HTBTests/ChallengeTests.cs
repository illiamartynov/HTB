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
            person = new Person("person@example.com", "TestPerson", "password", DateTime.Now, DateTime.Now.AddYears(-25), true, 0, new Profile(0, "Novice"), new Leaderboard());
        }

        [Test]
        public void TestChallengeCreation()
        {
            Assert.That(challenge.ChallengeName, Is.EqualTo("Buffer Overflow"));
            Assert.That(challenge.Difficulty, Is.EqualTo("Hard"));
            Assert.That(challenge.Points, Is.EqualTo(100));
            Assert.That(challenge.Status, Is.EqualTo(ChallengeStatus.NotTried));
        }

        [Test]
        public void TestAddAttempt()
        {
            var attempt = new Attempt(person, challenge, DateTime.Now, "Success");
            
            Assert.That(attempt.Result, Is.EqualTo("Success"));
            Assert.That(attempt.Challenge, Is.EqualTo(challenge));
            Assert.That(attempt.Person, Is.EqualTo(person));
        }
    }
}