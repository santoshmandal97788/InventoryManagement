using UI.Models;

namespace UI.Services
{
    public interface IAccountService
    {
        Employee AuthenticateUser(string email, string password);
        Employee ChangePassword(Employee employee);
        Employee GetEmployeeByEmail(string email);
    }
}
