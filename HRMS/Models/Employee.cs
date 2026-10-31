using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employee // Model
    {
        public long Id { get; set; } // Required
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; } // Optinal / Nullable (null)

        //[MaxLength(50)]
        //public string Position { get; set; } 

        public DateTime BirthDate { get; set; }

        [MaxLength(50)]
        public string PhoneNumber { get; set; } // 07, +962
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Salary { get; set; }

        [ForeignKey("Department")]
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation Property

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        [ForeignKey("Lookups")]
        public long? PositionId { get; set; }
        public Lookup? Lookups { get; set; }

    }
}
