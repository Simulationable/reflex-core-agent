using Google;
using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;

        public AppointmentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Appointment appointment)
        {
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsForAgentAsync(Guid agentId)
        {
            return await _dbContext.Appointments
                .Where(a => a.AgentId == agentId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsInRangeAsync(DateTime start, DateTime end)
        {
            return await _dbContext.Appointments
                .Where(a => a.StartTime >= start && a.StartTime <= end)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Appointments.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Appointments.FindAsync(id);
            if (entity != null)
            {
                _dbContext.Appointments.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
