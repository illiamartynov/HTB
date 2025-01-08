using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class LeaderboardTests
    {
        private Leaderboard _leaderboard;
        private Person _person;

        [SetUp]
        public void Setup()
        {
            _leaderboard = new Leaderboard("Test Leaderboard");
            var address = Address.AddAddress("USA", "New York", "Broadway", 1);
            var profile = new Profile(200, "Intermediate", null);
            _person = new Person("john.doe@example.com", "John Doe", "SecurePass123", DateTime.UtcNow, new DateTime(1990, 1, 1), true, 100, address, profile);
        }

        [Test]
        public void AddRank_ShouldAddRankToLeaderboardAndPerson()
        {
            var rank = _leaderboard.AddRank(1, _person);

            Assert.Contains(rank, _leaderboard.Ranks.ToList());
            Assert.Contains(rank, _person.Ranks.ToList());
            Assert.AreEqual(_leaderboard, rank.Leaderboard);
            Assert.AreEqual(_person, rank.Person);
        }

        [Test]
        public void RemoveRank_ShouldRemoveRankFromLeaderboardAndPerson()
        {
            var rank = _leaderboard.AddRank(1, _person);

            _leaderboard.RemoveRank(rank);

            Assert.IsFalse(_leaderboard.Ranks.Contains(rank));
            Assert.IsFalse(_person.Ranks.Contains(rank));
            Assert.IsNull(rank.Person);
            Assert.IsNull(rank.Leaderboard);
        }

        [Test]
        public void UpdateRank_ShouldUpdateRankInLeaderboardAndPerson()
        {
            var rank = _leaderboard.AddRank(1, _person);
            var newPerson = new Person("jane.doe@example.com", "Jane Doe", "SecurePass456", DateTime.UtcNow, new DateTime(1995, 6, 15), true, 200, Address.AddAddress("USA", "New York", "Broadway", 1), new Profile(200, "Intermediate", null));

            _leaderboard.UpdateRank(rank, 2, newPerson);

            Assert.AreEqual(2, rank.RankLevel);
            Assert.AreEqual(newPerson, rank.Person);
            Assert.Contains(rank, newPerson.Ranks.ToList());
            Assert.IsFalse(_person.Ranks.Contains(rank));
        }

        [Test]
        public void DeleteLeaderboard_ShouldRemoveAllRanksAndLeaderboard()
        {
            var rank1 = _leaderboard.AddRank(1, _person);
            var rank2 = _leaderboard.AddRank(2, _person);

            Leaderboard.DeleteLeaderboard(_leaderboard);

            Assert.IsEmpty(Leaderboard.Extent);
            Assert.IsFalse(_person.Ranks.Contains(rank1));
            Assert.IsFalse(_person.Ranks.Contains(rank2));
            Assert.IsEmpty(Rank.Extent);
        }

        [Test]
        public void RankExtent_ShouldContainAddedRanks()
        {
            var rank = _leaderboard.AddRank(1, _person);

            Assert.Contains(rank, Rank.Extent.ToList());
        }

        [Test]
        public void InvalidRankLevel_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _leaderboard.AddRank(0, _person));
        }

        [Test]
        public void AddRank_WithNullPerson_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _leaderboard.AddRank(1, null));
        }

        [Test]
        public void UpdateRank_WithInvalidOldRank_ShouldThrowException()
        {
            var invalidRank = new Rank(3, _person, _leaderboard);

            Assert.Throws<ArgumentException>(() => _leaderboard.UpdateRank(invalidRank, 2, _person));
        }

        [Test]
        public void RemoveRank_WithInvalidRank_ShouldThrowException()
        {
            var invalidRank = new Rank(3, _person, _leaderboard);

            Assert.Throws<ArgumentException>(() => _leaderboard.RemoveRank(invalidRank));
        }
    }
}

