namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leaderboard
    {
        private readonly List<Rank> _ranks = new();
        public IReadOnlyList<Rank> Ranks => _ranks.AsReadOnly();
        private List<(int Rank, int TotalPoints)> _rankedPeople = new List<(int, int)>();

        public IReadOnlyList<(int Rank, int TotalPoints)> RankedPeople => _rankedPeople.AsReadOnly();

        public void AddToLeaderboard(
            [Range(1, int.MaxValue, ErrorMessage = "Rank must be a positive integer.")] int rank,
            [Range(0, int.MaxValue, ErrorMessage = "Total points cannot be negative.")] int totalPoints)
        {
            _rankedPeople.Add((rank, totalPoints));
        }


       

        

        public void AddPerson(Person person, int rankLevel)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (_ranks.Any(r => r.Person == person))
                throw new InvalidOperationException("Person is already on the leaderboard.");

            var rank = new Rank(rankLevel, person, this);
            _ranks.Add(rank);
        }
        
        public void RemovePerson(Person person)
        {
            var rank = _ranks.FirstOrDefault(r => r.Person == person);
            if (rank != null)
            {
                _ranks.Remove(rank);
            }
        }
        
        public void Clear()
        {
            _ranks.Clear();
        }

        public void DeleteLeaderboard()
        {
            _ranks.Clear();
            Console.WriteLine("Leaderboard deleted along with all associated ranks.");
        }

        
        
    }
}
