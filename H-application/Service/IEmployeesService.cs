using H_application.DTOs.EmployeesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IEmployeesService
    {
        Task<bool> CreateEmployee(EmployeeDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployee(EmployeeDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployee(EmployeeDtoUpdate dto,CancellationToken cancellationToken = default);
        Task<EmployeeDtoUpdate> GetEmployeeById(int Id,CancellationToken cancellationToken = default);
        Task<List<EmployeeResponse>> GetEmployees(CancellationToken cancellationToken = default);
    }
}
