using System.Text.Json;

namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Lesson
    {
        private static List<Lesson> _extent = new List<Lesson>();
        private string _lessonTitle;
        private string _content;

        public static List<Lesson> Extent
        {
            get => _extent;
            private set => _extent = value;
        }

        [Required(ErrorMessage = "Lesson title is required.")]
        [StringLength(100, ErrorMessage = "Lesson title cannot exceed 100 characters.")]
        public string LessonTitle
        {
            get => _lessonTitle;
            set => _lessonTitle = value;
        }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public Course Course { get; private set; } // Связь с `Course`

        public List<Resource> Resources { get; } = new List<Resource>();

        public Lesson(string lessonTitle, string content)
        {
            _lessonTitle = lessonTitle;
            _content = content;
            _extent.Add(this);
        }

        public void AssignToCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));

            // Проверяем, если урок уже привязан к другому курсу
            if (Course != null)
            {
                throw new InvalidOperationException($"Lesson '{LessonTitle}' is already assigned to a course.");
            }

            Course = course;
            course.AddLesson(this);
        }

        public void UnassignFromCourse()
        {
            if (Course == null)
            {
                throw new InvalidOperationException($"Lesson '{LessonTitle}' is not assigned to any course.");
            }

            var course = Course;
            Course = null;
            course.RemoveLesson(this);
        }

        public void AddResource(Resource resource)
        {
            if (!Resources.Contains(resource))
            {
                Resources.Add(resource);
            }
        }

        public void RemoveResource(Resource resource)
        {
            Resources.Remove(resource);
        }

        public static void DeleteLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));

            _extent.Remove(lesson);

            // Убираем связь с курсом
            lesson.UnassignFromCourse();

            // Удаляем все ресурсы
            foreach (var resource in new List<Resource>(lesson.Resources))
            {
                Resource.DeleteResource(resource);
            }
        }

        public static void SaveExtent(string filename = "lesson_extent.json")
        {
            var json = JsonSerializer.Serialize(_extent);
            File.WriteAllText(filename, json);
        }

        public static void LoadExtent(string filename = "lesson_extent.json")
        {
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                _extent = JsonSerializer.Deserialize<List<Lesson>>(json) ?? new List<Lesson>();
            }
        }
    }
}
