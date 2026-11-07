using HRMS.DbContexts;
using HRMS.Dtos.Departments;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {

        private readonly HRMSContext _dbContext;
        public DepartmentsController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetByCriteria([FromQuery] SearchDepartmentDto departmentDto) // name = Human Resources, floorNumber = 2
        {

            try
            {
                var result = from department in _dbContext.Departments
                             from type in _dbContext.Lookups.Where(x => x.Id == department.TypeId).DefaultIfEmpty()
                             where (departmentDto.Name == null || department.Name.ToUpper().Contains(departmentDto.Name.ToUpper())) &&
                             (departmentDto.FloorNumber == null || department.FloorNumber == departmentDto.FloorNumber)
                             orderby department.Id descending
                             select new DepartmentDto
                             {
                                 Id = department.Id,
                                 Name = department.Name,
                                 Description = department.Description,
                                 FloorNumber = department.FloorNumber,
                                 TypeId = department.TypeId,
                                 TypeName = type.Name
                             };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception("An unexpected error occurred"));
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var department = _dbContext.Departments.Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FloorNumber = x.FloorNumber,
                    TypeId = x.TypeId,
                    TypeName = x.Type.Name
                }).FirstOrDefault(x => x.Id == id);

                if (department == null)
                {
                    return NotFound("Department Not Found"); // 404
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception("An unexpected error occurred"));
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] SaveDepartmentDto departmentDto)
        {
            try
            {
                var department = new Department
                {
                    Id = 0, // departments.LastOrDefault()?.Id == null ? 0 : departments.LastOrDefault()?.Id
                    Name = departmentDto.Name,
                    Description = departmentDto.Description,
                    FloorNumber = departmentDto.FloorNumber,
                    TypeId = departmentDto.TypeId
                };

                _dbContext.Departments.Add(department);
                _dbContext.SaveChanges();

                return Ok(department.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception("An unexpected error occurred"));
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] SaveDepartmentDto departmentDto)
        {
            try
            {
                if (id != departmentDto.Id)
                {
                    return BadRequest("Id Mismatch");//400
                }

                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == departmentDto.Id);


                if (department == null)
                {
                    return NotFound("Department Does Not Exisit");
                }

                department.Name = departmentDto.Name;
                department.Description = departmentDto.Description;
                department.FloorNumber = departmentDto.FloorNumber;
                department.TypeId = departmentDto.TypeId;

                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception("An unexpected error occurred"));
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);

                if (department == null)
                {
                    return NotFound("Department Does Not Exisit");
                }

                var isEmployee = _dbContext.Employees.Any(x => x.DepartmentId == id);
                if (isEmployee)
                {
                    return BadRequest("Department with assigned employees can not be deleted");
                }

                _dbContext.Departments.Remove(department);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Exception("An unexpected error occurred"));
            }
        }
    }
}
