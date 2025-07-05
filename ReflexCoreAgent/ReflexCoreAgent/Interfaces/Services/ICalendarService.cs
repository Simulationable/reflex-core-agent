using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface ICalendarService
    {
        Task<List<Appointment>> GetAppointmentsAsync(Guid agentId);
        Task<bool> TryAddAppointmentAsync(string userInput, Guid agentId);
    }
}
