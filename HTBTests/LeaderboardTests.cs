using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class LeaderboardTests
    {
        private Leaderboard leaderboard;
        private Person person1;
        private Person person2;

        [SetUp]
        public void Setup()
        {
            leaderboard = new Leaderboard();

            var address1 = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var address2 = Address.AddAddress("USA", "Los Angeles", "Sunset Blvd", 202);

            var profile1 = new Profile(0, "Novice", null);
            var profile2 = new Profile(0, "Intermediate", null);

            var rank1 = new Rank(1, null, leaderboard);
            var rank2 = new Rank(2, null, leaderboard);

            var completenessLevel1 = new CompletenessLevel(50, DateTime.Now, null, new Course("Course 1", "Beginner", new Free(30)));
            var completenessLevel2 = new CompletenessLevel(75, DateTime.Now, null, new Course("Course 2", "Intermediate", new Paid(100)));

            person1 = Person.AddPerson(
                email: "person1@example.com",
                name: "Alice",
                password: "password1",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 100,
                profile: profile1,
                address: address1,
                rank: rank1,
                completenessLevel: completenessLevel1,
                subscription: null
            );

            person2 = Person.AddPerson(
                email: "person2@example.com",
                name: "Bob",
                password: "password2",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 200,
                profile: profile2,
                address: address2,
                rank: rank2,
                completenessLevel: completenessLevel2,
                subscription: null
            );
        }

        [Test]
        public void TestAddPersonToLeaderboard()
        {
            leaderboard.AddToLeaderboard(1, 500);

            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.RankedPeople.Count, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].Rank, Is.EqualTo(1));
                Assert.That(leaderboard.RankedPeople[0].TotalPoints, Is.EqualTo(500));
            });
        }

        [Test]
        public void TestAddPerson()
        {
            leaderboard.AddPerson(person1, 1);

            Assert.That(leaderboard.Ranks.Count, Is.EqualTo(1));
            Assert.That(leaderboard.Ranks[0].Person, Is.EqualTo(person1));
            Assert.That(leaderboard.Ranks[0].RankLevel, Is.EqualTo(1));
        }

        [Test]
        public void TestRemovePerson()
        {
            leaderboard.AddPerson(person1, 1);
            leaderboard.RemovePerson(person1);

            Assert.That(leaderboard.Ranks, Is.Empty);
        }

        [Test]
        public void TestDeleteLeaderboard()
        {
            leaderboard.AddPerson(person1, 1);
            leaderboard.DeleteLeaderboard();

            Assert.That(leaderboard.Ranks, Is.Empty);
        }

        [Test]
        public void TestClearLeaderboard()
        {
            leaderboard.AddPerson(person1, 1);
            leaderboard.AddPerson(person2, 2);

            leaderboard.Clear();

            Assert.That(leaderboard.Ranks, Is.Empty);
        }

        [Test]
        public void TestPreventDuplicatePersons()
        {
            leaderboard.AddPerson(person1, 1);

            Assert.Throws<InvalidOperationException>(() => leaderboard.AddPerson(person1, 2));
        }
    }
}

