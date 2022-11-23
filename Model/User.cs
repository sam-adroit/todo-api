using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week8.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Roles Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Todo>? Todos { get; set; }
    }
}