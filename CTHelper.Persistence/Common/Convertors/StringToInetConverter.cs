using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

public class StringToInetConverter : ValueConverter<string?, NpgsqlInet?>
{
    public StringToInetConverter()
        : base(
            v => string.IsNullOrEmpty(v) ? null : new NpgsqlInet(v), 
            v => v == null ? null : v.ToString()                     
        )
    { }
}
