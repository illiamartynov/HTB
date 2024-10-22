using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
}

namespace ProjectTests
{
    public class PaymentTests
    {
        private Payment payment;

        [SetUp]
        public void Setup()
        {
            
            payment = new Payment(1, 50.0f, DateTime.Now, "Credit Card");
        }

        [Test]
        public void TestPaymentCreation()
        {
            
            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.Amount, Is.EqualTo(50.0f));
            Assert.That(payment.PaymentMethod, Is.EqualTo("Credit Card"));
        }

        [Test]
        public void TestProcessPayment()
        {
            
            Assert.DoesNotThrow(() => payment.ProcessPayment());
        }

        [Test]
        public void TestViewPaymentDetails()
        {
            
            Assert.DoesNotThrow(() => payment.ViewPaymentDetails());
        }
    }
}
