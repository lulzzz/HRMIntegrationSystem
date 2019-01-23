using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Unimicro.HttpClients
{
    public interface IUnimicroClient
    {
        Task<Login> SignIn();
        Task<Company> GetAndSetCompanyAuthorizationInfo(string organizationNumber);
        Task<IEnumerable<SubEntity>> GetSubEntities();
        Task<IEnumerable<User>> GetUsers(IEnumerable<int> userIds);
        Task<IEnumerable<Employee>> GetEmployees(string organizationNumber);
        Task<IEnumerable<Employment>> GetEmployments(IEnumerable<int> employeeIds);
        Task<IEnumerable<EmploymentLeave>> GetEmploymentLeaves(IEnumerable<int> employmentIds);
        Task<IEnumerable<Worker>> GetWorkers(IEnumerable<int> employeeIds);
        Task<IEnumerable<WorkRelation>> GetWorkRelations(IEnumerable<int> workerIds);
        Task<IEnumerable<WorkType>> GetWorkTypes();

        Task<HourBalance> GetHourBalance(int workRelationId);
        
        Task<WorkItem> GetWorkItem(int workItemId);
        Task<int> PostWorkItem(WorkItem workItem);
        Task DeleteWorkItem(string workItemId);
        Task<WorkItem> PutWorkItem(WorkItem workItem);

        Task<int> PostEmploymentLeave(EmploymentLeave employmentLeave);
        Task DeleteEmploymentLeave(string employmentLeaveId);
        Task<EmploymentLeave> PutEmploymentLeave(EmploymentLeave employmentLeave);
    }
}