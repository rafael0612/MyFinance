using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Shared.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public string? NameUser { get; set; }
        public string? LastName { get; set; }
        public int UserType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
