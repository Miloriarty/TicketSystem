namespace PracticeProject.Dto;

public record TicketEmployeeDto(int TicketId, int EmployeeId);

public static class TicketEmployeeMapper
{
    public static TicketEmployeeDto ToDto(this Entity.TicketEmployees ticketEmployees)
    {
        return new TicketEmployeeDto(ticketEmployees.EmployeeId, ticketEmployees.TicketId);
    }
}