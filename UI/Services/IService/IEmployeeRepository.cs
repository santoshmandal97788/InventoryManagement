using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.ViewModel;

namespace UI.Services.IService
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        //IEnumerable<Employee> GetAllEmployees();
        void EmployeeIns(Employee employee, Person person);
        void EmployeeUpd(Employee employee, Person person);
        void EmployeeDel(int id);
        bool IsEmailInUse(string email);
        JsonResult GetListEmployeesForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip);
        EmployeeViewModel GetEmployeeDetails(int Id);
        //For Password Reset Get By Id
        Employee GetEmployeeForPasswordReset(int Id);
        
    }
}
