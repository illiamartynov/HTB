namespace HTB
{
    using System;
    using System.Collections.Generic;

    public class Leaderboard
    {
        public List<(Person Person, int Rank, int TotalPoints)> RankedPeople { get; set; } = new List<(Person, int, int)>();

        public void AddPersonToLeaderboard(Person person, int rank, int totalPoints)
        {
            RankedPeople.Add((person, rank, totalPoints));
        }

        public void ViewRankings()
        {
            foreach (var entry in RankedPeople)
            {
                Console.WriteLine($"Person: {entry.Person.Name}, Rank: {entry.Rank}, Total Points: {entry.TotalPoints}");
            }
        }

        public void UpdateRankings(Person person, int newRank, int newTotalPoints)
        {
            var existingEntry = RankedPeople.Find(entry => entry.Person == person);

            if (existingEntry.Person != null)
            {
                // Обновляем существующую запись
                RankedPeople.Remove(existingEntry);
                RankedPeople.Add((person, newRank, newTotalPoints));
            }
            else
            {
                // Добавляем нового человека
                AddPersonToLeaderboard(person, newRank, newTotalPoints);
            }
        }
    }
}