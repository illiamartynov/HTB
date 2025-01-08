using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CourseTests
    {
        private Challenge _challenge;
        private Person _person;

        [SetUp]
        public void Setup()
        {
            typeof(Challenge).GetField("_extent",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Challenge>());

            typeof(Attempt).GetField("_extent",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Attempt>());

            typeof(Person).GetField("_extent",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Person>());

            var address = Address.AddAddress("USA", "New York", "Broadway", 1);
            var profile = new Profile(11, "", null);
            _person = new Person("john.doe@example.com", "John Doe", "SecurePass123", DateTime.UtcNow,
                new DateTime(1990, 1, 1), true, 100, address, profile);
        }

        [Test]
        public void AddAttempt_ShouldAddAttemptToChallengeAndPerson()
        {
            var attempt = _challenge.AddAttempt("successful", _person);

            Assert.Contains(attempt, _challenge.Attempts.ToList());
            Assert.Contains(attempt, _person.Attempts.ToList());
            Assert.AreEqual(_challenge, attempt.Challenge);
            Assert.AreEqual(_person, attempt.Person);
        }

        [Test]
        public void RemoveAttempt_ShouldRemoveAttemptFromChallengeAndPerson()
        {
            var attempt = _challenge.AddAttempt("successful", _person);

            _challenge.RemoveAttempt(attempt);

            Assert.IsFalse(_challenge.Attempts.Contains(attempt));
            Assert.IsFalse(_person.Attempts.Contains(attempt));
            Assert.IsNull(attempt.Person);
            Assert.IsNull(attempt.Challenge);
        }

        [Test]
        public void DeleteChallenge_ShouldRemoveAllAttemptsAndChallenge()
        {
            var attempt1 = _challenge.AddAttempt("successful", _person);
            var attempt2 = _challenge.AddAttempt("successful", _person);

            Challenge.DeleteChallenge(_challenge);

            Assert.IsEmpty(Challenge.Extent);
            Assert.IsFalse(_person.Attempts.Contains(attempt1));
            Assert.IsFalse(_person.Attempts.Contains(attempt2));
            Assert.IsEmpty(Attempt.Extent);
        }

        [Test]
        public void AttemptExtent_ShouldContainAddedAttempts()
        {
            var attempt = _challenge.AddAttempt("successful", _person);

            Assert.Contains(attempt, Attempt.Extent.ToList());
        }

        [Test]
        public void InvalidScore_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _challenge.AddAttempt("successful", _person));
        }

        [Test]
        public void AddAttempt_WithNullPerson_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _challenge.AddAttempt("successful", _person));
        }

        [Test]
        public void RemoveAttempt_WithInvalidAttempt_ShouldThrowException()
        {
            var invalidAttempt = new Attempt(_person, _challenge, "75");

            Assert.Throws<ArgumentException>(() => _challenge.RemoveAttempt(invalidAttempt));
        }
    }
}