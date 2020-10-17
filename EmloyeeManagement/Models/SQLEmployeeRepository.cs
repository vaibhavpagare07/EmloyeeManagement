using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext context;

        public SQLEmployeeRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            context.employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var emp = context.employees.Find(id);
            if(emp!=null)
            {
                context.employees.Remove(emp);
                context.SaveChanges();
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.employees;
        }

        public Employee GetEmployeeById(int id)
        {
            return context.employees.Find(id);
        }

        public Employee Update(Employee newEmployeechange)
        {
            var emp = context.employees.Attach(newEmployeechange);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return newEmployeechange;

        }
    }
}
