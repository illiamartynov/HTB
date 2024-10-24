using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CertificateTests
    {
        private Certificate certificate;

        [SetUp]
        public void Setup()
        {
            certificate = new Certificate(1, DateTime.Now);
        }

        [Test]
        public void TestGenerateCertificate()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 
                Assert.DoesNotThrow(() => certificate.GenerateCertificate());

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Certificate {certificate.CertificateId} generated."));
            }
        }

        [Test]
        public void TestViewCertificate()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Assert.DoesNotThrow(() => certificate.ViewCertificate());

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Certificate ID: {certificate.CertificateId}, Issue Date: {certificate.IssueDate}"));
            }
        }
    }
}