using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class PaymentTests
    {
        private Payment payment;
        private Person owner;

        [SetUp]
        public void Setup()
        {
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var profile = new Profile(500, "Intermediate", null);
            var rank = new Rank(1, null, new Leaderboard());

            owner = Person.AddPerson(
                email: "owner@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 500,
                profile: profile,
                address: address,
                rank: rank,
                completenessLevel: null,
                subscription: null
            );

            payment = Payment.Create(1, 50.0f, DateTime.Now, "credit card", owner, "EUR");
        }

        [Test]
        public void TestPaymentCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(payment.PaymentID, Is.EqualTo(1));
                Assert.That(payment.Amount, Is.EqualTo(50.0f));
                Assert.That(payment.PaymentMethod, Is.EqualTo("credit card"));
                Assert.That(Payment.Currency, Is.EqualTo("EUR"));
                Assert.That(payment.Owner, Is.EqualTo(owner));
            });
        }

        [Test]
        public void TestUpdatePaymentDetails()
        {
            payment.Update(newAmount: 75.0f, newDate: DateTime.Now.AddDays(1), newMethod: "debit card", newCurrency: "USD");

            Assert.Multiple(() =>
            {
                Assert.That(payment.Amount, Is.EqualTo(75.0f));
                Assert.That(payment.PaymentMethod, Is.EqualTo("debit card"));
                Assert.That(Payment.Currency, Is.EqualTo("USD"));
            });
        }

        [Test]
        public void TestUnassignOwner()
        {
            payment.UnassignOwner();

            Assert.That(payment.Owner, Is.Null);
        }

        [Test]
        public void TestDeletePayment()
        {
            Payment.Delete(payment);

            Assert.That(payment.Owner, Is.Null);
        }

        [Test]
        public void TestViewPaymentDetails()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                payment.ViewPaymentDetails();

                var output = sw.ToString().Trim();
                Assert.That(output, Does.Contain($"PaymentID: {payment.PaymentID}, Amount: {payment.Amount} in EUR, Method: {payment.PaymentMethod}"));
                Assert.That(output, Does.Contain($"Owner: {owner.Name}"));
            }
        }
    }
}
