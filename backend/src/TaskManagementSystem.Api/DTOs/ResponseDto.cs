using System.Text.Json.Serialization;

namespace TaskManagamentSystem.Api.DTOs;

public class ResponseDto
{
    public int StatusCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Payload { get; set; } = null;
}