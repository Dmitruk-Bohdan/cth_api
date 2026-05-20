namespace CTHelper.UnitTests.Services.PasswordHasher;

public class PasswordHasherAdapterTests
{
    [Fact]
    public void Hash_ProducesHashNotEqualToPassword()
    {
        var hasher = new PasswordHasherAdapter();
        var password = "MySecret123!";

        var hash = hasher.Hash(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        var hasher = new PasswordHasherAdapter();
        var password = "MySecret123!";

        var hash = hasher.Hash(password);
        var result = hasher.Verify(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var hasher = new PasswordHasherAdapter();
        var password = "MySecret123!";

        var hash = hasher.Hash(password);
        var result = hasher.Verify("WrongPassword", hash);

        Assert.False(result);
    }

    [Fact]
    public void Hash_SamePassword_DifferentHashEachTime()
    {
        var hasher = new PasswordHasherAdapter();
        var password = "MySecret123!";

        var hash1 = hasher.Hash(password);
        var hash2 = hasher.Hash(password);

        Assert.NotEqual(hash1, hash2);
        Assert.True(hasher.Verify(password, hash1));
        Assert.True(hasher.Verify(password, hash2));
    }
}