using Microsoft.AspNetCore.Mvc;
using UI.Data;
using UI.Models;
using UI.Services.IService;
using UI.ViewModel;

namespace UI.Services.MockService
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMailService _mailService;

        public MockEmployeeRepository(AppDbContext appDbContext, IMailService mailService)
        {
            _appDbContext = appDbContext;
            _mailService = mailService;
        }
        public void EmployeeDel(int id)
        {
            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
               
                try
                {
                    Employee emp = _appDbContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
                    Person person = _appDbContext.Persons.SingleOrDefault(e => e.PersonId == emp.PersonId);

                    if (emp != null && person != null)
                    {
                        _appDbContext.Employees.Remove(emp);
                        _appDbContext.Persons.Remove(person);
                        _appDbContext.SaveChanges();
                        transaction.Commit();
                    }
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public void EmployeeIns(Employee employee, Person person)
        {
            using (_appDbContext)
            {
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var pwd = Helper.CreateRandomPassword(5); ;
                        //employee.Password = Helper.HashPassword(pwd);
                        _appDbContext.Persons.Add(person);
                        _appDbContext.SaveChanges();
                        employee.PersonId = person.PersonId;
                        //employee.RoleId = 13;
                        _appDbContext.Employees.Add(employee);

                        _appDbContext.SaveChanges();

                        transaction.Commit();
                        //MailRequest mail = new MailRequest
                        //{
                        //    ToEmail= employee.Email,
                        //    Subject= "Login Credentials for Inventory Management",                            
                        //};
                        //EmployeeViewModel emp = new EmployeeViewModel   
                        //{
                        //    FullName = string.Concat(person.FirstName, " ", person.MiddleName, " ", person.LastName),
                        //    Email = employee.Email,
                        //    Password= pwd
                        //};

                        //_mailService.SendEmailAsync(mail, emp);
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }


                }
            }
        }

        public void EmployeeUpd(Employee employee, Person person)
        {
            Employee emp = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            Person pperson = _appDbContext.Persons.FirstOrDefault(e => e.PersonId == person.PersonId);
          
           
           
            if (emp != null && pperson != null)
            {
                //using (_appDbContext)
                //{
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                      
                        pperson.FirstName = person.FirstName;
                        pperson.MiddleName = person.MiddleName;
                        pperson.LastName = person.LastName;
                        pperson.GenderListItemId = person.GenderListItemId;
                        _appDbContext.Persons.Update(pperson);
                        
                        emp.PersonId = employee.PersonId;
                        emp.Email = employee.Email;
                        emp.RoleId = employee.RoleId;
                        _appDbContext.Employees.Update(emp);

                        _appDbContext.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }


                }
                //}
            }
        }

        public Employee GetEmployee(int Id)
        {
            Employee employee = (from emp in _appDbContext.Employees
                                 where emp.EmployeeId == Id
                                 join person in _appDbContext.Persons on emp.PersonId equals person.PersonId
                                 join role in _appDbContext.Roles on emp.RoleId equals role.RoleId
                                 select new Employee
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     PersonId = emp.PersonId,
                                     Email = emp.Email,
                                     Person = person,
                                     Role= role
                                 }).FirstOrDefault();

           
            return employee;
            //return _appDbContext.ListItems.Where(l => l.ListItemId == Id).FirstOrDefault();
        }

     

        public JsonResult GetListEmployeesForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip)
        {
            int recordsTotal = 0;
            List<EmployeeViewModel> listEmployees = new List<EmployeeViewModel>();
            listEmployees = (from emp in _appDbContext.Employees
                         join person in _appDbContext.Persons on emp.PersonId equals person.PersonId  
                         join Li in _appDbContext.ListItems on person.GenderListItemId equals Li.ListItemId
                         join role in _appDbContext.Roles on emp.RoleId equals role.RoleId
                         select new EmployeeViewModel
                         {
                             EmployeeId = emp.EmployeeId,
                             PersonId = emp.PersonId,
                             Email = emp.Email,
                             RoleName = role.RoleName,
                             FirstName = person.FirstName,
                             LastName= person.LastName,
                             FullName= string.Concat(person.FirstName," ", person.MiddleName," ", person.LastName),
                             Gender = Li.ListItemName
                         }).Skip(skip).Take(pageSize).ToList();
            //roleList = _appDbContext.Roles.Skip(skip).Take(pageSize).ToList();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                listEmployees = listEmployees.OrderByDescending(s => sortColumn + " " + sortColumnDirection).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                listEmployees = listEmployees.Where(e => e.Email.ToLower().Contains(searchValue.ToLower()) || e.FirstName.ToLower().Contains(searchValue.ToLower()) ||  e.LastName.ToLower().Contains(searchValue.ToLower()) || e.RoleName.ToLower().Contains(searchValue.ToLower()) || e.Gender.ToLower().Contains(searchValue.ToLower())).ToList();

            }
            //recordsTotal = _appDbContext.Roles.Count();
            recordsTotal = (from emp in _appDbContext.Employees select emp).Count();
            var data = listEmployees;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        public bool IsEmailInUse(string email)
        {
            bool isEmailExists = _appDbContext.Employees.Any(e => e.Email == email);
            if (isEmailExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public EmployeeViewModel GetEmployeeDetails(int Id)
        {
            var employees = (from emp in _appDbContext.Employees
                             where emp.EmployeeId == Id
                             join person in _appDbContext.Persons on emp.PersonId equals person.PersonId
                             join Li in _appDbContext.ListItems on person.GenderListItemId equals Li.ListItemId
                             join role in _appDbContext.Roles on emp.RoleId equals role.RoleId
                             select new EmployeeViewModel
                             {
                                 EmployeeId = emp.EmployeeId,
                                 PersonId = emp.PersonId,
                                 Email = emp.Email,
                                 RoleName = role.RoleName,
                                 FullName = string.Concat(person.FirstName, " ", person.MiddleName, " ", person.LastName),
                                 Gender = Li.ListItemName
                             }).FirstOrDefault();

            return employees;


        }

        public Employee GetEmployeeForPasswordReset(int Id)
        {
            Employee employee = (from emp in _appDbContext.Employees
                                 where emp.EmployeeId == Id select emp).FirstOrDefault();
            return employee;
        }
    }
}
