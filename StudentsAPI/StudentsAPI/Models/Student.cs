using System.ComponentModel.DataAnnotations;

namespace StudentsAPI.Models
{
    public class Student
    {
        [Key]
        public int id { get; set; }

        public string? name { get; set; }

        public string? email { get; set; }

        public int age { get; set; }

        public string? city { get; set; }
    }
}
