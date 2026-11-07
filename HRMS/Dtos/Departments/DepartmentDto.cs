namespace HRMS.Dtos.Departments
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? FloorNumber { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
    }
}
