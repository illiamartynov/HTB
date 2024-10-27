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
        private Person_Course personCourse;

        [SetUp]
        public void Setup()
        {
            Course.Extent.Clear();  
            course = new Course("Nmap");
            person = new Person("user@example.com", "UserTest", "password", DateTime.Now, DateTime.Now.AddYears(-20), true, 500, new Profile(500, "intermediate"), new Leaderboard());
            personCourse = new Person_Course(person, course, "easy");
        }

        [Test]
        public void TestCourseCreation()
        {
            Assert.That(course.CourseName, Is.EqualTo("Nmap"));
        }

        [Test]
        public void TestEnrollInCourse()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);  
                Assert.DoesNotThrow(() => course.Enroll());

                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain("Enrolled in course"));  
            }
        }

        [Test]
        public void TestCompleteCourseThroughAssociation()
        {
            personCourse.CompleteCourse();
            Assert.That(personCourse.IsCompleted, Is.True);
        }
    }
}