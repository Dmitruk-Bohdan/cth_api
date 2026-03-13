using CTHelper.Application.Models;

namespace CTHelper.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public OperationResult ValidationResult { get; }

        public CustomValidationException(OperationResult result)
        {
            ValidationResult = result;
        }
    }
}
