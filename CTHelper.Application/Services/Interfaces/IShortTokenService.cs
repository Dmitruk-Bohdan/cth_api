namespace CTHelper.Application.Services.Interfaces
{
    public interface IShortTokenService
    {
        string ComputeHash(string token);
        string Get6NumbersToken();
        string Get9SymbolsBindingCode();
        string Format9SymbolsBindingCode(string rawCode);
        bool Verify(string token, string storedHash);
    }
}