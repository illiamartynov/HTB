namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leaderboard
    {
        private readonly List<Rank> _ranks = new();
        public IReadOnlyList<Rank> Ranks => _ranks.AsReadOnly();
        private List<(Person Person, int Rank, int TotalPoints)> _rankedPeople = new List<(Person, int, int)>();

        public IReadOnlyList<(Person Person, int Rank, int TotalPoints)> RankedPeople => _rankedPeople.AsReadOnly();

        public void AddPersonToLeaderboard(
            [Required(ErrorMessage = "Person is required.")] Person person,
            [Range(1, int.MaxValue, ErrorMessage = "Rank must be a positive integer.")] int rank,
            [Range(0, int.MaxValue, ErrorMessage = "Total points cannot be negative.")] int totalPoints)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            _rankedPeople.Add((person, rank, totalPoints));
        }

        public void UpdateRankings(
            [Required(ErrorMessage = "Person is required.")] Person person,
            [Range(1, int.MaxValue, ErrorMessage = "New rank must be a positive integer.")] int newRank,
            [Range(0, int.MaxValue, ErrorMessage = "New total points cannot be negative.")] int newTotalPoints)
        {
            var existingEntry = _rankedPeople.Find(entry => entry.Person == person);

            if (existingEntry.Person != null)
            {
                _rankedPeople.Remove(existingEntry);
                _rankedPeople.Add((person, newRank, newTotalPoints));
            }
            else
            {
                AddPersonToLeaderboard(person, newRank, newTotalPoints);
            }
        }

        public void RemovePersonFromLeaderboard(Person person)
        {
            var entry = _rankedPeople.Find(p => p.Person == person);
            if (entry.Person != null)
            {
                _rankedPeople.Remove(entry);
                Console.WriteLine($"Removed {person.Name} from the leaderboard.");
            }
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

        
        public void ViewRankings()
        {
            foreach (var entry in _rankedPeople)
            {
                Console.WriteLine($"Person: {entry.Person.Name}, Rank: {entry.Rank}, Total Points: {entry.TotalPoints}");
            }
        }
    }
}
