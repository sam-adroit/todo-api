using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8.Model;

namespace Week8.Dto
{
    public class AddTodoDto
    {
        public string Task { get; set; } = string.Empty;
        public Categories Category { get; set; }
        public string Description {get; set;}
        public bool Completed { get; set; }
        public DateTime EndDate { get; set; }
    }
}