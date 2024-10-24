using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

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
            Assert.DoesNotThrow(() => certificate.GenerateCertificate());
        }

        [Test]
        public void TestViewCertificate()
        {
            Assert.DoesNotThrow(() => certificate.ViewCertificate());
        }
    }
}