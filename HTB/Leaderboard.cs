namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leaderboard
    {
        private static List<Leaderboard> _extent = new();
        public static IReadOnlyList<Leaderboard> Extent => _extent.AsReadOnly();
        
        private readonly List<Rank> _ranks = new();
        public IReadOnlyList<Rank> Ranks => _ranks.AsReadOnly();
        
        private string _name;
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public Leaderboard(string name)
        {
            _name = name;
        }
        
        // Rank funcs
        public Rank AddRank(int rankLevel, Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            Rank rank = new Rank(rankLevel, person, this);
            person.AddRankReverse(rank);

            _ranks.Add(rank);

            return rank;
        }

        public void RemoveRank(Rank rank)
        {
            if (!_ranks.Contains(rank))
                throw new ArgumentException("The rank doesn't exist in Leaderboard class", nameof(rank));

            // remove rank from Person
            Person rankPerson = rank.Person;
            rankPerson.RemoveRankReverse(rank);

            // remove rank from Leaderboard
            _ranks.Remove(rank);

            // remove Rank
            rank.RemoveRank();
        }

        public void UpdateRank(Rank oldRank, int rankLevel, Person person)
        {
            if (oldRank == null)
                throw new ArgumentNullException(nameof(oldRank));
            if (!_ranks.Contains(oldRank))
                throw new ArgumentException("The rank doesn't exist in Leaderboard class", nameof(oldRank));

            // remove oldRank from Challenge
            _ranks.Remove(oldRank);
            // remove oldRank from Person
            oldRank.Person.RemoveRankReverse(oldRank);
            // remove associations from Rank
            oldRank.DisassociateRank();

            // add new rank
            AddRank(rankLevel, person);
        }

        // Leaderboard funcs
        public static void DeleteLeaderboard(Leaderboard leaderboard)
        {
            if (leaderboard == null)
                throw new ArgumentNullException(nameof(leaderboard));

            foreach (var rank in new List<Rank>(leaderboard._ranks))
            {
                leaderboard.RemoveRank(rank);
            }

            _extent.Remove(leaderboard);
        }

        // funcs for reverse connection
        public void AddRankReverse(Rank rank)
        {
            _ranks.Add(rank);
        }

        public void RemoveRankReverse(Rank rank)
        {
            _ranks.Remove(rank);
        }

        // Leaderboard funcs
        public void DeleteLeaderboard()
        {
            _ranks.Clear();
            Console.WriteLine("Leaderboard deleted along with all associated ranks.");
        }
    }
}
