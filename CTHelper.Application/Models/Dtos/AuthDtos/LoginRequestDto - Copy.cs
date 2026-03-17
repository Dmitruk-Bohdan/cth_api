using System.Text.Json.Serialization;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Dtos.AuthDtos;

public class LoginResponseDto
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
}
