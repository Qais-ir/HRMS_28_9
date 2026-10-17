using System.ComponentModel.DataAnnotations;

namespace HRMS.Dtos.Departments
{
    public class SaveDepartmentDto
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }
        public int? FloorNumber { get; set; }
    }
}
