using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class ChallengeTests
    {
        private Challenge challenge;

        [SetUp]
        public void Setup()
        {
            Challenge.Extent.Clear();
            challenge = new Challenge("Buffer Overflow", "Hard", "Exploit a buffer overflow", 100, ChallengeStatus.NotTried);
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
            var attempt = new Attempt(1, DateTime.Now, "Success");
            
            challenge.AddAttempt(attempt);

            Assert.That(challenge.Attempts.Count, Is.EqualTo(1));
            Assert.That(challenge.Attempts[0].Result, Is.EqualTo("Success"));
        }

        [Test]
        public void TestExtentSerialization()
        {
            var challenge2 = new Challenge("SQL Injection", "Medium", "Exploit a SQL injection", 50, ChallengeStatus.Solved);

            Challenge.SaveExtent("test_challenge_extent.json");

            Challenge.LoadExtent("test_challenge_extent.json");

            Assert.That(Challenge.Extent.Count, Is.EqualTo(2));
        }
    }
}