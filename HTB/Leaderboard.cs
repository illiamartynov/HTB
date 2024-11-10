namespace HTB
{
    using System;
    using System.Collections.Generic;

    public class Leaderboard
    {
        private List<(Person Person, int Rank, int TotalPoints)> _rankedPeople = new List<(Person, int, int)>();

        public List<(Person Person, int Rank, int TotalPoints)> RankedPeople
        {
            get => _rankedPeople;
            private set => _rankedPeople = value;
        }

        public void AddPersonToLeaderboard(Person person, int rank, int totalPoints)
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

        public void UpdateRankings(Person person, int newRank, int newTotalPoints)
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