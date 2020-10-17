using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _listEmployees;
        public MockEmployeeRepository()
        {
            _listEmployees = new List<Employee>() { 
                new Employee () { ID = 1, Name = "Sam", Email = "Same@s.com", Department = Dept.IT },
                new Employee () { ID = 2, Name = "John", Email = "John@s.com", Department = Dept.HR },
                new Employee () { ID = 3, Name = "Sara", Email = "Sara@s.com", Department = Dept.IT }};
        }

        public Employee GetEmployeeById(int id)
        {
           return _listEmployees.Where(x => x.ID == id).FirstOrDefault();
        }

        Employee IEmployeeRepository.Add(Employee employee)
        {
            employee.ID = _listEmployees.Max(x => x.ID) + 1;
             _listEmployees.Add(employee);
            return employee;
        }

        Employee IEmployeeRepository.Delete(int id)
        {
           return _listEmployees.FirstOrDefault(x => x.ID == id);
        }

        IEnumerable<Employee> IEmployeeRepository.GetAllEmployees()
        {
            return _listEmployees;
        }

        Employee IEmployeeRepository.Update(Employee newEmployeechange)
        {
            var employee = _listEmployees.FirstOrDefault(z => z.ID == newEmployeechange.ID);
            if(employee!=null)
            {
                employee.Name = newEmployeechange.Name;
                employee.Email = newEmployeechange.Email;
                employee.Department = newEmployeechange.Department;
            }
            return employee;
        }
    }
}
