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

        public List<Course> Courses { get; private set; } 

        public List<Resource> Resources { get; private set; } 

        public Lesson(string lessonTitle, string content)
        {
            _lessonTitle = lessonTitle;
            _content = content;
            _extent.Add(this);
        }
        
        // Course funcs
        public void AddCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            if (Courses.Contains(course)) 
                throw new ArgumentException("The course already exists in the Lesson class", nameof(course));

            Courses.Add(course);
            course.AddLessonReverse(this);
        }

        public void RemoveCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));
            if (!Courses.Contains(course))
                return;
            
            // remove Course from Lesson
            Courses.Remove(course);

            // remove lesson from course
            course.RemoveLessonReverse(this);
        }

        public void UpdateCourse(Course oldCourse, Course newCourse)
        {
            if (oldCourse == null)
                throw new ArgumentNullException(nameof(oldCourse));
            if (newCourse == null)
                throw new ArgumentNullException(nameof(newCourse));
            if (!Courses.Contains(oldCourse))
                throw new ArgumentException("The oldLesson doesn't exist in Course class", nameof(oldCourse));
            if (Courses.Contains(newCourse))   
                throw new ArgumentException("The lesson already exists in the Course class", nameof(newCourse));
            
            Courses.Remove(oldCourse);
            oldCourse.RemoveLessonReverse(this);

            // add new course
            Courses.Add(newCourse);
            newCourse.AddLessonReverse(this);
        }
        
        
        // Course reverse funcs
        public void AddCourseReverse(Course course)
        {
            Courses.Add(course);
        }

        public void RemoveCourseReverse(Course course)
        {
            Courses.Remove(course);
        }
        
        // resource funcs
        public void AddResource(Resource resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));
            if (Resources.Contains(resource)) 
                throw new ArgumentException("The resource already exists in the Lesson class", nameof(resource));

            Resources.Add(resource);
            resource.AddLessonReverse(this);
        }

        public void RemoveResource(Resource resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));
            if (!Resources.Contains(resource))
                return;
            
            // remove Resource from Lesson
            Resources.Remove(resource);

            // remove lesson from resource
            resource.RemoveLessonReverse(this);
        }

        public void UpdateResource(Resource oldResource, Resource newResource)
        {
            if (oldResource == null)
                throw new ArgumentNullException(nameof(oldResource));
            if (newResource == null)
                throw new ArgumentNullException(nameof(newResource));
            if (!Resources.Contains(oldResource))
                throw new ArgumentException("The oldLesson doesn't exist in Resource class", nameof(oldResource));
            if (Resources.Contains(newResource))   
                throw new ArgumentException("The lesson already exists in the Resource class", nameof(newResource));
            
            Resources.Remove(oldResource);
            oldResource.RemoveLessonReverse(this);

            // add new resource
            Resources.Add(newResource);
            newResource.AddLessonReverse(this);
        }
        
        
        // resource reverse funcs
        public void AddResourceReverse(Resource resource)
        {
            Resources.Add(resource);
        }

        public void RemoveResourceReverse(Resource resource)
        {
            Resources.Remove(resource);
        }
        

        // extent funcs
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
