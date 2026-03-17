using CTHelper.Domain.Common.Enums;

namespace CTHelper.Application.UseCases.Identity.Command.ResponseModels
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public Guid SessionJti { get; set; }
    }
}
