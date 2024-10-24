using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class ResourceTests
    {
        [SetUp]
        public void Setup()
        {
            Resource.Resources.Clear();  
        }

        [Test]
        public void TestResourceCreation()
        {
            var resource = new Resource("Pentesting Guide", "Document", "https://example.com/pentest-guide");

            Assert.That(resource.ResourceName, Is.EqualTo("Pentesting Guide"));
            Assert.That(resource.ResourceType, Is.EqualTo("Document"));
            Assert.That(resource.Url, Is.EqualTo("https://example.com/pentest-guide"));
        }

        [Test]
        public void TestResourceAddedToList()
        {
            var resource1 = new Resource("Pentesting Guide", "Document", "https://example.com/pentest-guide");
            var resource2 = new Resource("CTF Platform", "Website", "https://example.com/ctf");

            Assert.That(Resource.Resources.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestViewAllResources()
        {
            var resource1 = new Resource("Pentesting Guide", "Document", "https://example.com/pentest-guide");
            var resource2 = new Resource("CTF Platform", "Website", "https://example.com/ctf");

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                Resource.ViewAllResources();
                var result = sw.ToString().Trim();

                Assert.That(result, Does.Contain("Viewing resource: Pentesting Guide"));
                Assert.That(result, Does.Contain("Viewing resource: CTF Platform"));
            }
        }
    }
}