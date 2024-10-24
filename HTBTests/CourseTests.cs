using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CourseTests
    {
        private Course course;

        [SetUp]
        public void Setup()
        {
            Course.Extent.Clear();  
            course = new Course("Nmap", "Easy", false);
        }

        [Test]
        public void TestCourseCreation()
        {
            Assert.That(course.CourseName, Is.EqualTo("Nmap"));
            Assert.That(course.Level, Is.EqualTo("Easy"));
            Assert.That(course.IsCompleted, Is.False);
        }

        [Test]
        public void TestEnrollInCourse()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);  
                Assert.DoesNotThrow(() => course.Enroll());

                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain(""));  
            }
        }

        [Test]
        public void TestCompleteCourse()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                course.CompleteCourse();

               
                Assert.That(course.IsCompleted, Is.True);

                var result = sw.ToString().Trim();
                Assert.That(result, Does.Contain(""));  
            }
        }

        [Test]
        public void TestExtentSerialization()
        {
            var course2 = new Course("Nmap", "Easy", false);

            Course.SaveExtent("test_course_extent.json");

            Course.LoadExtent("test_course_extent.json");

            Assert.That(Course.Extent.Count, Is.EqualTo(2));
        }
    }
}
