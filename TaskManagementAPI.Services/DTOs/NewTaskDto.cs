namespace TaskManagementAPI.Application.DTOs
{
    public record NewTaskDto
    (
        string Title,
        string Assignee,
        string Status
    );
}
