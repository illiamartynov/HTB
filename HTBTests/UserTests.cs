using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class UserTests
    {
        private User user;
        private Course course;
        private Person_Course personCourse;

        [SetUp]
        public void Setup()
        {
            user = new User("user@example.com", "UserTest", "password", DateTime.Now, DateTime.Now.AddYears(-20), true, 500, "user123", new Profile(500, "Intermediate"), new Leaderboard());
            course = new Course("Pentesting 101");
            personCourse = new Person_Course(user, course, "Medium");
        }

        [Test]
        public void TestAddCourse()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Assert.DoesNotThrow(() => user.AddCourse(course));

                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain($"Added course: {course.CourseName} for user: {user.Username}"));
            }
        }

        [Test]
        public void TestCompleteCourse()
        {
            personCourse.CompleteCourse();
            Assert.That(personCourse.IsCompleted, Is.True);
        }
    }
}