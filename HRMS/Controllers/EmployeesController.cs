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
        public EmployeesController()
        {
            Console.WriteLine();
        }

        // Mock Data
        public static List<Employee> employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "Ahmad", LastName="Nasser", Email="Ahmad@123",
                Position = "Developer", BirthDate = new DateTime(1995,1,25), PhoneNumber = "+96279545135",
                IsActive = true, StartDate = DateTime.Now, Salary = 1000
            },
            new Employee(){ Id = 2, FirstName = "Layla", LastName = "Kareem", Email = "Layla@123.com",
                Position = "HR", BirthDate = new DateTime(2000,1,25), PhoneNumber = "+9625588625",
                IsActive = true, StartDate = new DateTime(2026, 1, 1), Salary = 1000},

            new Employee(){ Id = 3, FirstName = "Yousef", LastName = "Faris", Email = "Yousef@123.com",
                Position = "Manager", BirthDate = new DateTime(1996,1,25), PhoneNumber = "+9625588625",
                IsActive = true, StartDate = new DateTime(2026, 1, 1), Salary = 1200},

            new Employee(){ Id = 4, FirstName = "Nadia", LastName = "Zaid", Email = "Nadia@123.com",
                Position = "Developer", BirthDate = new DateTime(1999,1,25), PhoneNumber = "+9625588625",
                IsActive = true, StartDate = new DateTime(2026, 1, 1), Salary = 800}

        };

        [HttpGet]
        public IActionResult GetByCriteria([FromQuery] SearchEmployeeDto searchEmployeeDto)
        {
            var data = from emp in employees
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
                           Salary = emp.Salary
                       };

            return Ok(data);
        }

        [HttpGet("{id:long}")] // Route Parameter
        public IActionResult GetById(long id)
        {

            var data = employees.Select(x => new EmployeeDto
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName,
                Position = x.Position,
                BirthDate = x.BirthDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Salary = x.Salary
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
                Id = (employees.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate.Value,
                StartDate = employeeDto.StartDate.Value,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary
            };

            employees.Add(employee);
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

            var employee = employees.FirstOrDefault(x => x.Id == employeeDto.Id);
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

            return Ok();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound(new Exception("Employee Does Not Exist"));
            }

            employees.Remove(employee);
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
