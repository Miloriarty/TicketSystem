namespace PracticeProject.Dto.TicketCategories;

public static class TicketCategoryMapper
{
    public static TicketCategoriesDto ToDto(this Entity.TicketCategories ticketCategories)
    {
        return new TicketCategoriesDto
        {
            TicketId = ticketCategories.TicketId,
            CategoryId = ticketCategories.CategoryId,
        };
    }
}