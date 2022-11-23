using System.ComponentModel.DataAnnotations;

namespace Week8.Model
{
    public class Todo
    {
        public Guid Id { get; set; }
        [Required]
        public string Task { get; set; } = string.Empty;
        public Categories Category { get; set; }
        public string Description {get; set;}
        public bool Completed { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User? User { get; set; }

    }
}