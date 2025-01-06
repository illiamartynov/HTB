using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace HTB
{
    public enum ChallengeStatus
    {
        NotTried,
        Attempted,
        Solved,
        Failed
    }

    public class Challenge
    {
        private static List<Challenge> _extent = new List<Challenge>();
        public static IReadOnlyCollection<Challenge> Extent => _extent.AsReadOnly();

        private string _challengeName;
        private string _difficulty;
        private string _description;
        private int _points;
        private ChallengeStatus _status;

        private List<Attempt> _attempts = new List<Attempt>();
        public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();

        [Required(ErrorMessage = "Challenge name is required.")]
        [StringLength(100, ErrorMessage = "Challenge name cannot exceed 100 characters.")]
        public string ChallengeName
        {
            get => _challengeName;
            set => _challengeName = value;
        }

        [Required(ErrorMessage = "Difficulty is required.")]
        [StringLength(50, ErrorMessage = "Difficulty cannot exceed 50 characters.")]
        public string Difficulty
        {
            get => _difficulty;
            set => _difficulty = value;
        }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [Required(ErrorMessage = "Points are required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Points must be greater than 0.")]
        public int Points
        {
            get => _points;
            set => _points = value;
        }

        [Required(ErrorMessage = "Status is required.")]
        public ChallengeStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public Challenge(string challengeName, string difficulty, string description, int points,
            ChallengeStatus status)
        {
            _challengeName = challengeName;
            _difficulty = difficulty;
            _description = description;
            _points = points;
            _status = status;
            _extent.Add(this);
        }

        // Attempt funcs
        public Attempt AddAttempt(string result, Person person)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (result.Length == 0)
                throw new ArgumentException("The result is empty.", nameof(result));
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            Attempt attempt = new Attempt(person, this, result);
            person.AddAttemptReverse(attempt);

            _attempts.Add(attempt);

            return attempt;
        }

        public void RemoveAttempt(Attempt attempt)
        {
            if (attempt == null)
                throw new ArgumentNullException(nameof(attempt));
            if (!_attempts.Contains(attempt))
                throw new ArgumentException("The attempt doesn't exist in Challenge class", nameof(attempt));

            // remove attempt from Person
            Person attemptPerson = attempt.Person;
            attemptPerson.RemoveAttemptReverse(attempt);

            // remove attemp from Challenge
            _attempts.Remove(attempt);

            // remove Attempt
            attempt.RemoveAttempt();
        }

        public void UpdateAttempt(Attempt oldAttempt, string result, Person person)
        {
            if (oldAttempt == null)
                throw new ArgumentNullException(nameof(oldAttempt));
            if (!_attempts.Contains(oldAttempt))
                throw new ArgumentException("The attempt doesn't exist in Challenge class", nameof(oldAttempt));

            // remove oldAttempt from Challenge
            _attempts.Remove(oldAttempt);
            // remove oldAttempt from Person
            oldAttempt.Person.RemoveAttemptReverse(oldAttempt);
            // remove associations from Attempt
            oldAttempt.DisassociateAttempt();

            // add new attempt
            AddAttempt(result, person);
        }

        // Challenge funcs
        public static void DeleteChallenge(Challenge challenge)
        {
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge));

            foreach (var attempt in new List<Attempt>(challenge._attempts))
            {
                challenge.RemoveAttempt(attempt);
            }

            _extent.Remove(challenge);
        }

        // funcs for reverse connection
        public void AddAttemptReverse(Attempt attempt)
        {
            _attempts.Add(attempt);
        }

        public void RemoveAttemptReverse(Attempt attempt)
        {
            _attempts.Remove(attempt);
        }


        // extent funcs
        public static void SaveExtent(string filename = "challenge_extent.json")
        {
            var json = JsonSerializer.Serialize(_extent);
            File.WriteAllText(filename, json);
        }

        public static void LoadExtent(string filename = "challenge_extent.json")
        {
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                _extent = JsonSerializer.Deserialize<List<Challenge>>(json) ?? new List<Challenge>();
            }
        }
    }
}
