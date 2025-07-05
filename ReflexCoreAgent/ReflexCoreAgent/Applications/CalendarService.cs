using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces.Repositories;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class CalendarService : ICalendarService
    {
        private readonly ILogger<CalendarService> _logger;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly ITimeParser _timeParser; // สำหรับแปลง userInput เป็นเวลา

        public CalendarService(
            ILogger<CalendarService> logger,
            IAppointmentRepository appointmentRepo,
            ITimeParser timeParser)
        {
            _logger = logger;
            _appointmentRepo = appointmentRepo;
            _timeParser = timeParser;
        }
        public async Task<List<Appointment>> GetAppointmentsAsync(Guid agentId)
        {
            var list = await _appointmentRepo.GetAppointmentsForAgentAsync(agentId);

            foreach (var appt in list)
            {
                appt.StartTime = appt.StartTime.AddYears(-543);
                appt.EndTime = appt.EndTime.AddYears(-543);
            }

            return list;
        }

        public async Task<bool> TryAddAppointmentAsync(string userInput, Guid agentId)
        {
            try
            {
                var parsed = _timeParser.Parse(userInput);
                if (parsed is null)
                {
                    _logger.LogWarning("ไม่สามารถแปลงเวลาได้จาก: {Input}", userInput);
                    return false;
                }

                var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var startUtc = TimeZoneInfo.ConvertTimeToUtc(parsed.Start, tz);
                var endUtc = TimeZoneInfo.ConvertTimeToUtc(parsed.End, tz);

                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    AgentId = agentId,
                    Title = $"นัดหมายจาก Agent {agentId}",
                    Description = userInput,
                    StartTime = startUtc,
                    EndTime = endUtc,
                    CreatedAt = DateTime.UtcNow
                };

                await _appointmentRepo.AddAsync(appointment);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "เพิ่มนัดหมายล้มเหลว");
                return false;
            }
        }
    }
}
