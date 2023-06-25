using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace LabWork3.Models
{
    public class DataProvider
    {
        private static List<Student>? _students;
        private static List<Rating>? _ratings;
        private static Dictionary<int, Student>? _rateStudents;
        private static List<Student>? _passed;
        private static Dictionary<Student, int>? _countLessons;

        private const string DefaultDataDir = "./App_Data";
        private const string StudentsFileName = "Students.csv";
        private const string RatingsFileName = "Rating.csv";
        private static object _lock = new();

        public static List<Student> Students
        {
            get
            {
                if (_students is null)
                {
                    lock (_lock)
                    {
                        _students ??= ReadData<Student>(StudentsFileName);
                    }
                }

                return _students;
            }
        }
        public static List<Rating> Ratings
        {
            get
            {
                if (_ratings is null)
                {
                    lock (_lock)
                    {
                        _ratings ??= ReadData<Rating>(RatingsFileName);
                    }
                }

                return _ratings;
            }
        }

        public static Dictionary<int, Student> RateStudents
        {
            get
            {
                if (_rateStudents is null)
                {
                    Dictionary<int, int> result = new Dictionary<int, int>();
                    _rateStudents = new Dictionary<int, Student>();
                    foreach (var el in Ratings)
                    {
                        int index = el.Id;
                        if (!result.ContainsKey(index))
                        {
                            result.Add(index, el.Points);
                        }
                        else
                        {
                            result[index] += el.Points;
                        }
                    }
                    foreach (var el in result.Keys)
                    {
                        _rateStudents.Add(result[el], Students.Find(s => s.Id == el));
                    }
                }
                return _rateStudents;
            }
        }

        public static Dictionary<Student, int> CountLesson
        {
            get
            {
                if(_countLessons is null)
                {
                    Dictionary<int, int> result = new Dictionary<int, int>();
                    _countLessons = new Dictionary<Student, int>();
                    foreach (var el in Ratings)
                    {
                        int index = el.Id;
                        if (!result.ContainsKey(index))
                        {
                            result.Add(index, 1);
                        }
                        else
                        {
                            result[index] += 1;
                        }
                    }
                    foreach (var el in result.Keys)
                    {
                        _countLessons.Add(Students.Find(s => s.Id == el), result[el]);
                    }
                    
                }
                return _countLessons;
            }
        }
        public static List<Student> Passed
        {
            get
            {
                Dictionary<int, List<(int, int)>> exams = new Dictionary<int, List<(int, int)>>();
                _passed = new List<Student>();
                foreach (var el in Ratings)
                {
                    if (exams.ContainsKey(el.Id))
                    {
                        exams[el.Id].Add(el.Type == "Залік" ? (1, el.Points) :  (2, el.Points));
                    }
                    else
                    {
                        exams.Add(el.Id, new List<(int, int)>() {el.Type == "Залік" ? (1, el.Points) : (2, el.Points)});
                    }
                }
                foreach(var el in exams)
                {
                    bool passed1 = false;
                    bool passed2 = true;
                    foreach (var item in el.Value)
                    {
                        if (item.Item1 == 1)
                        {
                            if(item.Item2 < 60)
                            {
                                passed2 = false;
                            }
                        } 
                        else
                        {
                            if(item.Item2 < 35)
                            {
                                passed1 = true;
                            }
                        }
                    }
                    if (passed1 && passed2)
                    {
                        _passed.Add(Students.Find(s => s.Id == el.Key));
                    }
                }
                return _passed;
            }
        }

        public static List<T> ReadData<T>(string fileName, string? dataDir = null, string separator = ";")
            where T : ICSVParser<T>
        {
            var items = new List<T>();
            dataDir ??= DefaultDataDir;

            int lineNumber = 0;
            var fullName = Path.Combine(dataDir, fileName);
            Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {fullName} load started");
            try
            {
                foreach (var line in File.ReadAllLines(fullName))
                {
                    lineNumber++;
                    try
                    {
                        var item = T.Parse(line, separator: separator);
                        items.Add(item);
                    }
                    catch (System.Exception)
                    {
                        Trace.WriteLine($"{fullName}: inconsistent data in line #{lineNumber}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Trace.WriteLine($"{fullName}: exception {e.Message}");
            }
            finally
            {
                Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {fullName} load finished");
            }

            return items;
        }
    }
}