using System.ComponentModel.DataAnnotations;

namespace AdminDashCore.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Position { get; set; }

        public string? Office { get; set; }

        public int Age { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public decimal Salary { get; set; }
    }
}
