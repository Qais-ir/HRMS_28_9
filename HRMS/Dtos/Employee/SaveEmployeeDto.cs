using System.ComponentModel.DataAnnotations;

namespace HRMS.Dtos.Employee
{
    public class SaveEmployeeDto
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; } // Optinal / Nullable (null)

        //[Required]
        //[StringLength(100, MinimumLength = 2)]
        //public string Position { get; set; }

        public long? PositionId { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public string PhoneNumber { get; set; } // 07, +962

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(0, 9999)]
        public decimal? Salary { get; set; }

        public long? DepartmentId { get; set; }
        public long? ManagerId { get; set; }
    }
}
