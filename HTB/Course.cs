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
        private IAccessType _accessType;

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
            set => _accessType = value;
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

        // Добавление уроков с двусторонней связью
        public void AddLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));
            if (!_lessons.Contains(lesson))
            {
                _lessons.Add(lesson);
                lesson.AssignToCourse(this);
            }
        }

        // Удаление уроков с двусторонней связью
        public void RemoveLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));
            if (_lessons.Remove(lesson))
            {
                lesson.UnassignFromCourse();
            }
        }

        // Удаление всех уроков
        public void ClearLessons()
        {
            foreach (var lesson in new List<Lesson>(_lessons))
            {
                RemoveLesson(lesson);
            }
        }

        // Динамическая регистрация курса
        public virtual void RegisterCourse()
        {
            Console.WriteLine($"Course '{CourseName}' registered with difficulty '{DifficultyLevel}'.");
            Console.WriteLine($"Access Type: {_accessType.GetAccessDescription()}");
        }

        // Удаление курса с обработкой двусторонних связей
        public static void DeleteCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            course._completenessLevels.Clear();
            _extent.Remove(course);
        }
        
        

       

        

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
