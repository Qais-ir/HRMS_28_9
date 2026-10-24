using HRMS.DbContexts;
using HRMS.Dtos.Employee;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    // Data Annotations => Class, Methods, Properties, Parameters
    [Route("api/[controller]")] // api/Employees
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // CRUD Operations
        // C : Create
        // R : Read
        // U : Update
        // D : Delete

        // Dependency Injection
        private readonly HRMSContext _dbContext;//= new HRMSContext();
        public EmployeesController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetByCriteria([FromQuery] SearchEmployeeDto searchEmployeeDto)
        {
            var data = from emp in _dbContext.Employees
                       from dep in _dbContext.Departments.Where(x => x.Id == emp.DepartmentId).DefaultIfEmpty() // -> Left Join
                       from man in _dbContext.Employees.Where(x => x.Id == emp.ManagerId).DefaultIfEmpty()

                       where (searchEmployeeDto.Position == null || emp.Position.ToUpper().Contains(searchEmployeeDto.Position.ToUpper())) &&
                             (searchEmployeeDto.Name == null || emp.FirstName.ToUpper().Contains(searchEmployeeDto.Name.ToUpper())) &&
                             (searchEmployeeDto.IsActive == null || emp.IsActive == searchEmployeeDto.IsActive)
                       orderby emp.Id descending
                       select new EmployeeDto
                       {
                           Id = emp.Id,
                           FullName = emp.FirstName + " " + emp.LastName,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate,
                           Salary = emp.Salary,
                           DepartmentId = dep.Id,
                           DepartmentName = dep.Name,
                           ManagerId = man.Id,
                           ManagerName = man.FirstName + " " + man.LastName
                       };

            return Ok(data);
        }

        [HttpGet("{id:long}")] // Route Parameter
        public IActionResult GetById(long id)
        {

            //  var data = _dbContext.Employees.Join(
            //    _dbContext.Departments,
            //    employee => employee.DepartmentId,
            //    department => department.Id,
            //    (employee, department) => new EmployeeDto
            //    {
            //        Id = employee.Id,
            //        FullName = employee.FirstName + " " + employee.LastName,
            //        Position = employee.Position,
            //        BirthDate = employee.BirthDate,
            //        StartDate = employee.StartDate,
            //        EndDate = employee.EndDate,
            //        DepartmentId = employee.DepartmentId,
            //        DepartmentName = department.Name,
            //    }
            //).FirstOrDefault(x => x.Id == id);

            var data = _dbContext.Employees.Select(x => new EmployeeDto
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName,
                Position = x.Position,
                BirthDate = x.BirthDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Salary = x.Salary,
                DepartmentId = x.DepartmentId,
                DepartmentName = "",
                ManagerId = x.ManagerId,
                ManagerName = ""
            }).FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound(new Exception("Employee Not Found")); // 404
            }

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SaveEmployeeDto employeeDto)
        {
            if (employeeDto.EndDate != null && employeeDto.EndDate < employeeDto.StartDate)
            {
                return BadRequest(new Exception("End Date cannot be earlier than Start Date"));
            }

            var employee = new Employee
            {
                Id = 0, //(employees.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate.Value,
                StartDate = employeeDto.StartDate.Value,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.ManagerId
            };

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            return Ok(employee.Id);
        }


        [HttpPut("{id:long}")]
        //[HttpPatch]
        public IActionResult Update(long id, SaveEmployeeDto employeeDto)
        {
            if(id != employeeDto.Id)
            {
                return BadRequest(new Exception("Id Mismatch"));
            }

            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);
            if(employee == null)
            {
                return NotFound(new Exception("Employee Does Not Exist"));
            }

            if (employeeDto.EndDate != null && employeeDto.EndDate < employeeDto.StartDate)
            {
                return BadRequest(new Exception("End Date cannot be earlier than Start Date"));
            }

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Position = employeeDto.Position;
            employee.BirthDate = employeeDto.BirthDate.Value;
            employee.StartDate = employeeDto.StartDate.Value;
            employee.EndDate = employeeDto.EndDate;
            employee.Email = employeeDto.Email;
            employee.IsActive = employeeDto.IsActive;
            employee.Salary = employeeDto.Salary;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.ManagerId = employeeDto.ManagerId;

            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound(new Exception("Employee Does Not Exist"));
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();

            return Ok();
        }
    }

   

    // Request Body | Query Parameter

    // Simple Type => string, int, double... // Query Parameter
    // Complex Type => Dto, Object... // Body

    // Can have multiple Query Parameters
    // Can only have one Request body

    // Get endpoints cannot have body
}
