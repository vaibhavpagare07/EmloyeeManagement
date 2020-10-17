using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
   public interface IEmployeeRepository
    {
        public Employee GetEmployeeById(int id);

        public IEnumerable<Employee> GetAllEmployees();

        public Employee Add(Employee employee);

        public Employee Update(Employee newEmployeechange);

        public Employee Delete(int id);
    }
}
