namespace PracticeProject.Dto.TicketEmployees;

public static class TicketEmployeesMapper
{
    public static TicketEmployeesDto ToDto(this Entity.TicketEmployees ticketEmployees)
    {
        return new TicketEmployeesDto
        {
            EmployeeId = ticketEmployees.EmployeeId,
            TicketId = ticketEmployees.TicketId,
        };
    }
}