using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

namespace HTB
{
    public class Course
    {
        private static List<Course> _extent = new List<Course>();
        private string _courseName;
        private string _difficultyLevel;
        private readonly IAccessType _accessType;

        // Двусторонняя связь с Lesson
        private List<Lesson> _lessons = new List<Lesson>();

        public static IReadOnlyList<Course> Extent => _extent.AsReadOnly();

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        public string CourseName
        {
            get => _courseName;
            set => _courseName = value;
        }

        [Required(ErrorMessage = "Difficulty level is required.")]
        [StringLength(50, ErrorMessage = "Difficulty level cannot exceed 50 characters.")]
        public string DifficultyLevel
        {
            get => _difficultyLevel;
            set => _difficultyLevel = value;
        }

        [Required(ErrorMessage = "Access type is required.")]
        public IAccessType AccessType
        {
            get => _accessType;
        }

        public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();
        private readonly List<CompletenessLevel> _completenessLevels = new();
        public IReadOnlyList<CompletenessLevel> CompletenessLevels => _completenessLevels.AsReadOnly();

        public Course(string courseName, string difficultyLevel, IAccessType accessType)
        {
            _courseName = courseName;
            _difficultyLevel = difficultyLevel;
            _accessType = accessType;
            _extent.Add(this);
        }

        
        // CompletenessLevel funcs
        public CompletenessLevel AddCompletenessLevel(int percentage, Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            CompletenessLevel completenessLevel = new CompletenessLevel(percentage, person, this);
            person.AddCompletenessLevelReverse(completenessLevel);

            _completenessLevels.Add(completenessLevel);

            return completenessLevel;
        }

        public void RemoveCompletenessLevel(CompletenessLevel completenessLevel)
        {
            if (!_completenessLevels.Contains(completenessLevel))
                throw new ArgumentException("The CompletenessLevel doesn't exist in Course class", nameof(completenessLevel));

            // remove CompletenessLevel from Person
            Person rankPerson = completenessLevel.Person;
            rankPerson.RemoveCompletenessLevelReverse(completenessLevel);

            // remove CompletenessLevel from Course
            _completenessLevels.Remove(completenessLevel);

            // remove CompletenessLevel
            completenessLevel.RemoveCompletenessLevel();
        }

        public void UpdateRank(CompletenessLevel oldCompletenessLevel, int percentage, Person person)
        {
            if (oldCompletenessLevel == null)
                throw new ArgumentNullException(nameof(oldCompletenessLevel));
            if (!_completenessLevels.Contains(oldCompletenessLevel))
                throw new ArgumentException("The CompletenessLevel doesn't exist in Course class", nameof(oldCompletenessLevel));

            // remove oldCompletenessLevel from Course
            _completenessLevels.Remove(oldCompletenessLevel);
            // remove oldCompletenessLevel from Person
            oldCompletenessLevel.Person.RemoveCompletenessLevelReverse(oldCompletenessLevel);
            // remove associations from oldCompletenessLevel
            oldCompletenessLevel.DisassociateCompletenessLevel();

            // add new CompletenessLevel
            AddCompletenessLevel(percentage, person);
        }

        // Course funcs
        public static void DeleteCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            foreach (var com in new List<CompletenessLevel>(course._completenessLevels))
            {
                course.RemoveCompletenessLevel(com);
            }

            _extent.Remove(course);
        }

        // funcs for person reverse connection
        public void AddCompletenessLevelReverse(CompletenessLevel completenessLevel)
        {
            _completenessLevels.Add(completenessLevel);
        }

        public void RemoveCompletenessLevelReverse(CompletenessLevel completenessLevel)
        {
            _completenessLevels.Remove(completenessLevel);
        }
        
        // funcs for lesson connection
        public void AddLesson(Lesson lesson)
        {
            if (lesson == null)
                throw new ArgumentNullException(nameof(lesson));
            if (_lessons.Contains(lesson)) 
                throw new ArgumentException("The lesson already exists in the Course class", nameof(lesson));

            _lessons.Add(lesson);
            lesson.AddCourseReverse(this);
        }

        public void RemoveLesson(Lesson lesson)
        {
            if (lesson == null)
                throw new ArgumentNullException(nameof(lesson));
            if (!_lessons.Contains(lesson))
                return;
            
            // remove Lesson from Course
            _lessons.Remove(lesson);

            // remove course from Lesson
            lesson.RemoveCourseReverse(this);
        }

        public void UpdateLesson(Lesson oldLesson, Lesson newLesson)
        {
            if (oldLesson == null)
                throw new ArgumentNullException(nameof(oldLesson));
            if (newLesson == null)
                throw new ArgumentNullException(nameof(newLesson));
            if (!_lessons.Contains(oldLesson))
                throw new ArgumentException("The oldLesson doesn't exist in Course class", nameof(oldLesson));
            if (_lessons.Contains(newLesson))   
                throw new ArgumentException("The lesson already exists in the Course class", nameof(newLesson));
            
            // remove oldCompletenessLevel from Course
            _lessons.Remove(oldLesson);
            // remove oldCompletenessLevel from Person
            oldLesson.RemoveCourseReverse(this);

            // add new CompletenessLevel
            _lessons.Add(newLesson);
            newLesson.AddCourseReverse(this);
        }
        
        // Lesson reverse funcs
        public void AddLessonReverse(Lesson lesson)
        {
            _lessons.Add(lesson);
        }

        public void RemoveLessonReverse(Lesson lesson)
        {
            _lessons.Remove(lesson);
        }

        // Динамическая регистрация курса
        public virtual void RegisterCourse()
        {
            Console.WriteLine($"Course '{CourseName}' registered with difficulty '{DifficultyLevel}'.");
            Console.WriteLine($"Access Type: {_accessType.GetAccessDescription()}");
        }
        
        
        // extent funcs
        public static void SaveExtent(string filename = "course_extent.json")
        {
            var json = JsonSerializer.Serialize(_extent);
            File.WriteAllText(filename, json);
        }

        public static void LoadExtent(string filename = "course_extent.json")
        {
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                _extent = JsonSerializer.Deserialize<List<Course>>(json) ?? new List<Course>();
            }
        }

        public static void ClearExtent()
        {
            _extent.Clear();
        }
    }

    // Inheritance part
    
    // Наследование вместо интерфейса для ContentType
    public class OSINT : Course
    {
        public string TechniqueFocus { get; set; }

        public OSINT(string courseName, string difficultyLevel, IAccessType accessType, string techniqueFocus)
            : base(courseName, difficultyLevel, accessType)
        {
            TechniqueFocus = techniqueFocus;
        }

        public override void RegisterCourse()
        {
            base.RegisterCourse();
            Console.WriteLine($"OSINT Course with focus on: {TechniqueFocus}");
        }
    }

    public class PenetrationTesting : Course
    {
        public string TestingEnvironment { get; set; }

        public PenetrationTesting(string courseName, string difficultyLevel, IAccessType accessType, string testingEnvironment)
            : base(courseName, difficultyLevel, accessType)
        {
            TestingEnvironment = testingEnvironment;
        }

        public override void RegisterCourse()
        {
            base.RegisterCourse();
            Console.WriteLine($"Penetration Testing Course in environment: {TestingEnvironment}");
        }
    }

    // AccessType остается динамическим
    public interface IAccessType
    {
        string Type { get; }
        string GetAccessDescription();
    }

    public class Free : IAccessType
    {
        public string Type => "Free";
        public int AccessDuration { get; set; }

        public Free(int accessDuration)
        {
            AccessDuration = accessDuration;
        }

        public string GetAccessDescription()
        {
            return $"Free access for {AccessDuration} days.";
        }
    }

    public class Paid : IAccessType
    {
        public string Type => "Paid";
        public int Price { get; set; }

        public Paid(int price)
        {
            Price = price;
        }

        public string GetAccessDescription()
        {
            return $"Paid access with price ${Price}.";
        }
    }
}

