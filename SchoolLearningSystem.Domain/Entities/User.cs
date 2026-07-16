using SchoolLearningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Entities
{
    public  class User:BaseEntity
    {
        public string Username { get; set; }= string.Empty;

        public string Email { get; set; }= string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public int ? StudentId { get; set; }
        public int ? TeacherId { get; set; }
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
