using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class UserTests
    {
        private User user;
        private Course course;

        [SetUp]
        public void Setup()
        {
            user = new User("user@example.com", "UserTest", "password", DateTime.Now, DateTime.Now.AddYears(-20), true, 500, "user123", new Profile(500, "Intermediate"), new Leaderboard());
            course = new Course("Pentesting 101", "Medium", false);
        }

        [Test]
        public void TestAddCourse()
        {
            Assert.DoesNotThrow(() => user.AddCourse(course));
            Assert.That(user.EnrolledCourses.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestViewCourses()
        {
            user.AddCourse(course);
            Assert.DoesNotThrow(() => user.ViewCourses());
        }
    }
}