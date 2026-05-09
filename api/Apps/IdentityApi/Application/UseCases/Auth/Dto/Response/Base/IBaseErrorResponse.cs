namespace Application.UseCases.Auth.Dto.Response.Base;

public interface IBaseErrorResponse
{
    string? Message { get; set; }
    string? Code { get; set; }
}