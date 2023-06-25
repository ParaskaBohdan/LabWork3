using System.ComponentModel.DataAnnotations;

namespace LabWork3.Models
{
    public class Rating : ICSVParser<Rating>
    {
        [Required] public int Id { get; set; }
        [Required] public string Lesson { get; set; } = string.Empty;

        [Required] public string Type { get; set; } = string.Empty;
        [Required] public int Points { get; set; }
        public static Rating Parse(string s, string separator = ";")
        {
            var words = s.Split(new[] { separator }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != 4 || !int.TryParse(words[0], out var id) || !int.TryParse(words[3], out var points))
                throw new FormatException("String cannot be parsed to the instance of Doctor type");
            return new Rating { Id = id, Lesson = words[1], Type = words[2], Points = points };
        }
    }
}
