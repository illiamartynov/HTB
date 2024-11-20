namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leaderboard
    {
        private List<(Person Person, int Rank, int TotalPoints)> _rankedPeople = new List<(Person, int, int)>();

        public List<(Person Person, int Rank, int TotalPoints)> RankedPeople
        {
            get => _rankedPeople;
            private set => _rankedPeople = value;
        }

        public void AddPersonToLeaderboard(
            [Required(ErrorMessage = "Person is required.")] Person person,
            [Range(1, int.MaxValue, ErrorMessage = "Rank must be a positive integer.")] int rank,
            [Range(0, int.MaxValue, ErrorMessage = "Total points cannot be negative.")] int totalPoints)
        {
            _rankedPeople.Add((person, rank, totalPoints));
        }

        public void ViewRankings()
        {
            foreach (var entry in _rankedPeople)
            {
                Console.WriteLine($"Person: {entry.Person.Name}, Rank: {entry.Rank}, Total Points: {entry.TotalPoints}");
            }
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
    }
}