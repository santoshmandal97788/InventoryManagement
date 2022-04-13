using UI.Data;
using UI.Services.IService;
using UI.ViewModel;

namespace UI.Services.MockService
{
    public class MockDashboard : IDashboardService
    {
        private readonly AppDbContext _appDbContext;

        public MockDashboard(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public DashboardViewModel TotalEmployeeCount()
        {

            var result = (from emp in _appDbContext.Employees select emp.EmployeeId).Count();
            DashboardViewModel dvm = new DashboardViewModel();
            dvm.TotalEmployee = result;
            return dvm;
        }
    }
}
