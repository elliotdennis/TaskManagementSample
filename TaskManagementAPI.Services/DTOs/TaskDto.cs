namespace TaskManagementAPI.Application.DTOs
{
    public record TaskDto(
        Guid Id,
        string Title,
        string Assignee,
        string Status,
        DateTime LastModified,
        byte[] Version
    );
}
