using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class ResourceTests
    {
        private Lesson lesson;
        private Resource resource;

        [SetUp]
        public void Setup()
        {
            Resource.Resources.Clear();
            Lesson.Extent.Clear();

            lesson = new Lesson("Introduction to Security", "Basic concepts of security.");
            resource = new Resource("Pentesting Guide", "Document", "https://example.com/pentest-guide", lesson);
        }

        [Test]
        public void TestResourceCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(resource.ResourceName, Is.EqualTo("Pentesting Guide"));
                Assert.That(resource.ResourceType, Is.EqualTo("Document"));
                Assert.That(resource.Url, Is.EqualTo("https://example.com/pentest-guide"));
                Assert.That(resource.Lesson, Is.EqualTo(lesson));
            });
        }

        [Test]
        public void TestResourceAddedToResourcesList()
        {
            var anotherResource = new Resource("CTF Platform", "Website", "https://example.com/ctf", lesson);

            Assert.That(Resource.Resources.Count, Is.EqualTo(2));
            Assert.That(Resource.Resources, Contains.Item(resource));
            Assert.That(Resource.Resources, Contains.Item(anotherResource));
        }

        [Test]
        public void TestAssignLessonToResource()
        {
            var newLesson = new Lesson("Advanced Security", "In-depth topics on security.");
            resource.AssignLesson(newLesson);

            Assert.Multiple(() =>
            {
                Assert.That(resource.Lesson, Is.EqualTo(newLesson));
                Assert.That(newLesson.Resources, Contains.Item(resource));
                Assert.That(lesson.Resources, Does.Not.Contain(resource));
            });
        }

        [Test]
        public void TestUnassignLessonFromResource()
        {
            resource.UnassignLesson();

            Assert.Multiple(() =>
            {
                Assert.That(resource.Lesson, Is.Null);
                Assert.That(lesson.Resources, Does.Not.Contain(resource));
            });
        }

        [Test]
        public void TestDeleteResource()
        {
            Resource.DeleteResource(resource);

            Assert.Multiple(() =>
            {
                Assert.That(Resource.Resources, Does.Not.Contain(resource));
                Assert.That(lesson.Resources, Does.Not.Contain(resource));
                Assert.That(resource.Lesson, Is.Null);
            });
        }
    }
}
