using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class User
    {
        public long Id { get; set; }
        [MaxLength(100)]
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public bool IsAdmin { get; set; }

    }
}
