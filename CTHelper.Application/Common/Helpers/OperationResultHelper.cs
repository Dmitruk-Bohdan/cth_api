using CTHelper.Application.Common.Enums;
using CTHelper.Application.Models;
using System.Net;

namespace CTHelper.Application.Common.Helpers
{
    public static class OperationResultHelper
    {
        public static OperationResult UserNotFoundTemplate(
            long? id = null,
            string? email = null)
        {
            string errorMessage = string.Empty;

            if (id != null)
            {
                errorMessage = $"User with id {id} wasn't found";
            }
            else if (email != null)
            {
                errorMessage = $"User with email {email} wasn't found";
            }
            else
            {
                errorMessage = "Specified user wasn't found";
            }
            return new OperationResult()
            {
                ErrorCode = ErrorCodeConstants.UserNotFound,
                ErrorMessage = errorMessage,
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        public static OperationResult<T> UserNotFoundTemplate<T>(long? id = null, string? email = null)
        {
            var baseResult = UserNotFoundTemplate(id, email); 
            return new OperationResult<T>
            {
                ErrorCode = baseResult.ErrorCode,
                ErrorMessage = baseResult.ErrorMessage,
                HttpStatusCode = baseResult.HttpStatusCode,
                Payload = default
            };
        }
    }
}
