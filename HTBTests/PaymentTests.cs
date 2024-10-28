using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class PaymentTests
    {
        private Payment payment;

        [SetUp]
        public void Setup()
        {
            payment = new Payment(1, 50.0f, DateTime.Now, "credit card", "EUR");
        }

        [Test]
        public void TestPaymentCreation()
        {
            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.Amount, Is.EqualTo(50.0f));
            Assert.That(payment.PaymentMethod, Is.EqualTo("credit card"));
            Assert.That(Payment.Currency, Is.EqualTo("USD")); 
        }

        [Test]
        public void TestProcessPayment()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Assert.DoesNotThrow(() => payment.ProcessPayment());

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Processed payment of {payment.Amount} in EUR (converted to USD).")); 
            }
        }

    }
}