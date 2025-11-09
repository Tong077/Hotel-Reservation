using H_application.DTOs.EmployeesDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Repository
{
    public class EmployeesRepository : IEmployeesService
    {
        private readonly EntityContext _context;

        public EmployeesRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEmployee(EmployeeDtoCreate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Employee>();
            await _context.Employee.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEmployee(EmployeeDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Employee>();
            await _context.Employee.AddAsync(entity);
            return await _context.SaveChangesAsync()  > 0;
        }

        public async Task<EmployeeDtoUpdate> GetEmployeeById(int Id, CancellationToken cancellationToken = default)
        {
            var employ = await _context.Employee
                .FirstOrDefaultAsync(e => e.EmployeeId == Id, cancellationToken);
            if (employ == null)
                return null!;

            var dto = employ.Adapt<EmployeeDtoUpdate>();
            return dto;
        }

        public async Task<List<EmployeeResponse>> GetEmployees(CancellationToken cancellationToken = default)
        {
           var employ = await _context.Employee
                .OrderBy(e => e.FullName)
                .AsNoTracking().ToListAsync();
            return employ.Select(g => g.Adapt<EmployeeResponse>()).ToList();
        }

        public async Task<bool> UpdateEmployee(EmployeeDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Employee>();
             _context.Employee.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
