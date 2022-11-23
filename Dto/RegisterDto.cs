using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week8.Model;

namespace Week8.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}