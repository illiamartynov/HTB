using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class CompletenessLevelTests
    {
        private CompletenessLevel completenessLevel;
        private Person person;
        private Course course;

        [SetUp]
        public void Setup()
        {
            var address = Address.AddAddress("USA", "Los Angeles", "Sunset Blvd", 202);
            var leaderboard = new Leaderboard(); // Replace with valid Leaderboard instance as needed
            var accessType = new Free(30);
            course = new Course("OSINT Basics", "Beginner", accessType);
            var profile = new Profile(0, "Beginner", null); // Adjust Person assignment later
            var rank = new Rank(1, null, leaderboard);

            person = Person.AddPerson(
                email: "test@example.com",
                name: "Test User",
                password: "securePass123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 0,
                profile: profile,
                address: address,
                rank: rank,
                completenessLevel: null,
                subscription: null
            );

            completenessLevel = new CompletenessLevel(50, DateTime.Now, person, course);
        }

        [Test]
        public void TestCompletenessLevelCreation()
        {
            var startDate = DateTime.Now;
            var completeness = new CompletenessLevel(80, startDate, person, course);

            Assert.Multiple(() =>
            {
                Assert.That(completeness.CompletenessPercentage, Is.EqualTo(80));
                Assert.That(completeness.StartDate, Is.EqualTo(startDate));
                Assert.That(completeness.Person, Is.EqualTo(person));
                Assert.That(completeness.Course, Is.EqualTo(course));
            });
        }

        [Test]
        public void TestInvalidCompletenessLevelCreation()
        {
            Assert.Throws<ArgumentException>(() => new CompletenessLevel(-10, DateTime.Now, person, course));
            Assert.Throws<ArgumentException>(() => new CompletenessLevel(110, DateTime.Now, person, course));
        }

        [Test]
        public void TestUpdateCompleteness_ValidValue()
        {
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                completenessLevel.UpdateCompleteness(70);
                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(completenessLevel.CompletenessPercentage, Is.EqualTo(70));
                    Assert.That(output, Does.Contain("Completeness updated to 70%"));
                });
            }
        }

        [Test]
        public void TestUpdateCompleteness_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() => completenessLevel.UpdateCompleteness(150));
            Assert.Throws<ArgumentException>(() => completenessLevel.UpdateCompleteness(-20));
        }

        [Test]
        public void TestPersonAssignment()
        {
            Assert.That(completenessLevel.Person, Is.EqualTo(person));
        }

        [Test]
        public void TestCourseAssignment()
        {
            Assert.That(completenessLevel.Course, Is.EqualTo(course));
        }
    }
}
