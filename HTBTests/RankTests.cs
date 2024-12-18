using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class RankTests
    {
        private Rank rank;
        private Person person;
        private Leaderboard leaderboard;

        [SetUp]
        public void Setup()
        {
            leaderboard = new Leaderboard();

            person = Person.AddPerson(
                email: "test@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 1000,
                profile: null,
                address: null,
                rank: null,
                completenessLevel: null,
                subscription: null
            );

            rank = new Rank(1, person, leaderboard);
        }

        [Test]
        public void TestRankCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(rank.RankLevel, Is.EqualTo(1));
                Assert.That(rank.Person, Is.EqualTo(person));
                Assert.That(rank.Leaderboard, Is.EqualTo(leaderboard));
            });
        }

        [Test]
        public void TestInvalidRankCreation()
        {
            Assert.Throws<ArgumentException>(() => new Rank(0, person, leaderboard));
            Assert.Throws<ArgumentException>(() => new Rank(-5, person, leaderboard));
        }

        [Test]
        public void TestUpdateRank_ValidValue()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                rank.UpdateRank(2);
                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(rank.RankLevel, Is.EqualTo(2));
                    Assert.That(output, Does.Contain("Rank updated to level 2."));
                });
            }
        }

        [Test]
        public void TestUpdateRank_InvalidValue()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                rank.UpdateRank(-1);
                rank.UpdateRank(0);

                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(rank.RankLevel, Is.EqualTo(1)); // значение не изменилось
                    Assert.That(output, Does.Contain("Invalid rank level. Must be greater than 0."));
                });
            }
        }

        [Test]
        public void TestRankAssociatedWithPersonAndLeaderboard()
        {
            Assert.Multiple(() =>
            {
                Assert.That(rank.Person, Is.EqualTo(person));
                Assert.That(rank.Leaderboard, Is.EqualTo(leaderboard));
            });
        }
    }
}
