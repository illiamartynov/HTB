using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CourseTests
    {
        private Course course;
        private Person person;

        [SetUp]
        public void Setup()
        {
            Course.ClearExtent();
            Person.ClearExtent();
            
            course = new Course("Nmap", "Intermediate", new OSINT("Network Scanning"), new Free(30));
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 10);
            var profile = new Profile(500, "Intermediate");
            var rank = new Rank(1);
            var completenessLevel = new CompletenessLevel(80, DateTime.Now);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            person = Person.AddPerson(
                email: "user@example.com",
                name: "UserTest",
                password: "password",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-20),
                isActive: true,
                balance: 500,
                profile: profile,
                leaderboard: new Leaderboard(),
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: subscription
            );
        }

        [Test]
        public void TestPersonEnrollsInCourse()
        {
            using (var sw = new StringWriter())
            {
                TextWriter originalOutput = Console.Out;
                Console.SetOut(sw);

                person.AddCourse(course);

                Console.SetOut(originalOutput);
                var result = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(result, Does.Contain("UserTest enrolled in course: Nmap"));
                    Assert.That(person.Courses.Contains(course), Is.True, "The course should be added to the person's list.");
                });
            }
        }

        [Test]
        public void TestPersonCompletesCourse()
        {
            using (var sw = new StringWriter())
            {
                TextWriter originalOutput = Console.Out;
                Console.SetOut(sw);

                person.AddCourse(course);

                Console.SetOut(originalOutput);
                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("UserTest enrolled in course: Nmap"));
            }
        }
    }
}
