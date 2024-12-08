namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Leaderboard
    {
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


        public void ViewRankings()
        {
            foreach (var entry in _rankedPeople)
            {
                Console.WriteLine($"Person: {entry.Person.Name}, Rank: {entry.Rank}, Total Points: {entry.TotalPoints}");
            }
        }
    }
}
