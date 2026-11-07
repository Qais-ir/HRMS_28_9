using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Department
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? FloorNumber { get; set; }

        [ForeignKey("Lookup")]
        public long? TypeId { get; set; }
        public Lookup? Type { get; set; }
    }
}
