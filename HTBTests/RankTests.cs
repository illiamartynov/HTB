using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class RankTests
    {
        private Rank rank;

        [SetUp]
        public void Setup()
        {
            rank = new Rank(1);
        }

        [Test]
        public void TestRankCreation()
        {
            Assert.That(rank.RankLevel, Is.EqualTo(1));
        }

        [Test]
        public void TestInvalidRankCreation()
        {
            Assert.Throws<ArgumentException>(() => new Rank(0));
            Assert.Throws<ArgumentException>(() => new Rank(-5));
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

                Assert.That(rank.RankLevel, Is.EqualTo(1)); // значение не изменилось
                Assert.That(output, Does.Contain("Invalid rank level. Must be greater than 0."));
            }
        }

        [Test]
        public void TestRankLevel_SetValidValue()
        {
            rank.RankLevel = 3;

            Assert.That(rank.RankLevel, Is.EqualTo(3));
        }

        [Test]
        public void TestRankLevel_SetInvalidValue()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                rank.RankLevel = -10;
                var output = sw.ToString().Trim();

                Assert.That(rank.RankLevel, Is.EqualTo(1)); // значение не изменилось
                Assert.That(output, Does.Contain("Invalid rank level. Must be greater than 0."));
            }
        }
    }
}