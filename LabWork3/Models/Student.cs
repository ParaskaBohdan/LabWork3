using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace LabWork3.Models
{

    public class Student : ICSVParser<Student>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required] public string Group { get; set; } = string.Empty;
        public static Student Parse(string s, string separator = ";")
        {
            var words = s.Split(new[] { separator }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != 3 || !int.TryParse(words[0], out var id))
                throw new FormatException("String cannot be parsed to the instance of Doctor type");
            return new Student { Id = id, Name = words[1], Group = words[2] };
        }
        public override string ToString() => $"Doctor(Id = {Id}, Name = {Name}, Group = {Group})";
    }
}
