using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsForAgentAsync(Guid agentId);
        Task<List<Appointment>> GetAppointmentsInRangeAsync(DateTime start, DateTime end);
        Task<Appointment?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Appointment appointment);
    }
}
