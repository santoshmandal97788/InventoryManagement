using Microsoft.EntityFrameworkCore;
using UI.Data;
using UI.Models;

namespace UI.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _appDbContext;

        public AccountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Employee AuthenticateUser(string email, string password)
        {
            var employee = (from e in _appDbContext.Employees where e.Email == email && e.Password == Helper.HashPassword(password)
                     join p in _appDbContext.Persons on e.PersonId equals p.PersonId
                     join r in _appDbContext.Roles on e.RoleId equals r.RoleId
                     select new Employee
                     {
                         EmployeeId = e.EmployeeId,
                         Email=e.Email,
                         Role=r,
                         Person=p
                     }).FirstOrDefault();
            //Employee employee =  _appDbContext.Employees.Where(e=>e.Email == email && e.Password == password).Include(e => e.Person).Include(e => e.Role).FirstOrDefault();
           return employee;
        }

        public Employee ChangePassword(Employee employee)
        {
            Employee employeePasswordUpdate = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (employeePasswordUpdate != null)
            {
                employeePasswordUpdate.Password = employee.Password;
                _appDbContext.Employees.Update(employeePasswordUpdate);
                _appDbContext.SaveChanges();
            }
            return employeePasswordUpdate;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee employee = (from emp in _appDbContext.Employees
                                 where emp.Email == email
                                 select emp).FirstOrDefault();
            return employee;
        }
    }
}
