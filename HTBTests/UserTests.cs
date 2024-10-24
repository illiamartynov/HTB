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

        [SetUp]
        public void Setup()
        {
            user = new User("user@example.com", "UserTest", "password", DateTime.Now, DateTime.Now.AddYears(-20), true, 500, "user123", new Profile(500, "Intermediate"), new Leaderboard());
            course = new Course("Pentesting 101", "Medium", false);
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
        public void TestViewCourses()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 

                user.AddCourse(course);  
                Assert.DoesNotThrow(() => user.ViewCourses());

                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain(course.CourseName)); 
            }
        }
    }
}   