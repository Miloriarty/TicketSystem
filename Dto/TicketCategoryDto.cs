namespace PracticeProject.Dto;

public record TicketCategoryDto(int TicketId, int CategoryId);

public static class TicketCategoryMapper
{
    public static TicketCategoryDto ToDto(this Entity.TicketCategories ticketCategories)
    {
        return new TicketCategoryDto(ticketCategories.TicketId, ticketCategories.CategoryId);
    }
}