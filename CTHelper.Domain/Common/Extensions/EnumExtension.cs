namespace CTHelper.Domain.Common.Extensions
{
    public static class EnumExtension
    {
        public static TEnum ToEnum<TEnum>(this short value) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), (int)value))
            {
                return (TEnum)(object)value;
            }

            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Value {value} is not defined in enum {typeof(TEnum).Name}");
        }
    }
}
