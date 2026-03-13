namespace CTHelper.Domain.Common.Extensions
{
    public static class EnumExtension
    {
        public static TEnum ToEnum<TEnum>(this short value) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), value);
            }

            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Value {value} is not defined in enum {typeof(TEnum).Name}");
        }

        public static int ToInt<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            return (int)(object)value;
        }
    }
}
