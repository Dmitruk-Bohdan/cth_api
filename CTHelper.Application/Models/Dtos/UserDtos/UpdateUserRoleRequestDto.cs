using System.Text.Json.Serialization;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.Models.Dtos.UserDtos;

public class UpdateUserRoleRequestDto
{
    [JsonPropertyName("role")]
    public Role Role { get; set; }
}
