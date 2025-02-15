﻿using HTB;

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
            challenge = new Challenge("Buffer Overflow", "Hard", "Exploit a buffer overflow", 100, ChallengeStatus.NotTried);

            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var profile = new Profile(0, "Novice", null); // Adjust Person assignment later

            person = Person.AddPerson(
                email: "person@example.com",
                name: "TestPerson",
                password: "password",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 0,
                profile: profile,
                address: address
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
            Attempt attempt = challenge.AddAttempt("Success", person);

            Assert.Multiple(() =>
            {
                Assert.That(challenge.Attempts, Contains.Item(attempt));
                Assert.That(attempt.Result, Is.EqualTo("Success"));
                Assert.That(attempt.Challenge, Is.EqualTo(challenge));
                Assert.That(attempt.Person, Is.EqualTo(person));
            });
        }

        [Test]
        public void TestRemoveAttempt()
        {
            Attempt attempt = challenge.AddAttempt("Failed", person);

            challenge.RemoveAttempt(attempt);

            Assert.That(challenge.Attempts, Does.Not.Contain(attempt));
        }

        [Test]
        public void TestDeleteChallenge()
        {
            challenge.AddAttempt("Success", person);

            Challenge.DeleteChallenge(challenge);

            Assert.That(Challenge.Extent, Does.Not.Contain(challenge));
            Assert.That(challenge.Attempts, Is.Empty);
        }
    }
}
