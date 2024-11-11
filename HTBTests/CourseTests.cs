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
            person = new Person(
                email: "user@example.com", 
                name: "UserTest", 
                password: "password", 
                registrationDate: DateTime.Now, 
                birthDate: DateTime.Now.AddYears(-20), 
                isActive: true, 
                balance: 500, 
                profile: new Profile(500, "intermediate"), 
                leaderboard: new Leaderboard(),
                address: new Address("USA", "New York", "5th Avenue", 10),
                rank: new Rank(1),
                completenessLevel: new CompletenessLevel(80, DateTime.Now),
                subscription: new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30))
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
                Assert.That(result, Does.Contain("UserTest enrolled in course: Nmap"));
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
