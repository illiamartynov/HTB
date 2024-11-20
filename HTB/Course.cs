﻿using System;
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
        private IContentType _contentType;
        private IAccessType _accessType;

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

        [Required(ErrorMessage = "Content type is required.")]
        public IContentType ContentType
        {
            get => _contentType;
            set => _contentType = value;
        }

        [Required(ErrorMessage = "Access type is required.")]
        public IAccessType AccessType
        {
            get => _accessType;
            set => _accessType = value;
        }

        public Course(string courseName, string difficultyLevel, IContentType contentType, IAccessType accessType)
        {
            _courseName = courseName;
            _difficultyLevel = difficultyLevel;
            _contentType = contentType;
            _accessType = accessType;
            _extent.Add(this);
        }

        public void RegisterCourse()
        {
            Console.WriteLine($"Course {_courseName} registered with difficulty level {_difficultyLevel}.");
            Console.WriteLine($"Content Type: {_contentType.GetTypeDescription()}");
            Console.WriteLine($"Access Type: {_accessType.GetAccessDescription()}");
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



    public interface IContentType
    {
        string GetTypeDescription();
    }

    public interface IAccessType
    {
        string Type { get; }
        string GetAccessDescription();
    }

    public class OSINT : IContentType
    {
        public string Type => "OSINT";
        private string _techniqueFocus;

        public string TechniqueFocus
        {
            get => _techniqueFocus;
            set => _techniqueFocus = value;
        }

        public OSINT(string techniqueFocus)
        {
            _techniqueFocus = techniqueFocus;
        }

        public string GetTypeDescription()
        {
            return $"OSINT with focus on {_techniqueFocus}";
        }
    }

    public class PenetrationTesting : IContentType
    {
        public string Type => "PenetrationTesting";
        private string _testingEnvironment;

        public string TestingEnvironment
        {
            get => _testingEnvironment;
            set => _testingEnvironment = value;
        }

        public PenetrationTesting(string testingEnvironment)
        {
            _testingEnvironment = testingEnvironment;
        }

        public string GetTypeDescription()
        {
            return $"Penetration Testing in environment: {_testingEnvironment}";
        }
    }

    public class Free : IAccessType
    {
        public string Type => "Free";
        private int _accessDuration;

        public int AccessDuration
        {
            get => _accessDuration;
            set => _accessDuration = value;
        }

        public Free(int accessDuration)
        {
            _accessDuration = accessDuration;
        }

        public string GetAccessDescription()
        {
            return $"Free access for {_accessDuration} days";
        }
    }

    public class Paid : IAccessType
    {
        public string Type => "Paid";
        private int _price;

        public int Price
        {
            get => _price;
            set => _price = value;
        }

        public Paid(int price)
        {
            _price = price;
        }

        public string GetAccessDescription()
        {
            return $"Paid access with price ${_price}";
        }
    }

}
