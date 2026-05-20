using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using CTHelper.Domain.Abstractions;
using CTHelper.Application.Services.Interfaces;
using CTHelper.UnitTests.Helpers;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.UnitTests.Services.TeacherStudentService;

public class TeacherStudentServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private CTHelper.Infrastructure.Services.Implementations.TeacherStudentService CreateService(AppDbContext ctx)
    {
        var uowMock = new Mock<IUnitOfWork>();
        var tokenMock = new Mock<IShortTokenService>();
        var userMgmtMock = new Mock<IUserManagmentService>();
        var fileStorageMock = new Mock<IFileStorageService>();
        tokenMock.Setup(t => t.Get9SymbolsBindingCode()).Returns("ABC-123-DEF");
        tokenMock.Setup(t => t.Format9SymbolsBindingCode(It.IsAny<string>())).Returns("ABC 123 DEF");
        return new CTHelper.Infrastructure.Services.Implementations.TeacherStudentService(
            uowMock.Object, tokenMock.Object, ctx, userMgmtMock.Object, fileStorageMock.Object);
    }

    [Fact]
    public async Task RequestBindingWithTeacherByCode_Success()
    {
        var ctx = CreateContext();
        ctx.InvitationCodes.Add(new InvitationCode { Id = 1, Code = "CODE123", TeacherId = 10, IsRevoked = false });
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var result = await svc.RequestBindingWithTeacherByCode(20, "CODE123");
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RequestBindingWithTeacherByCode_CodeNotFound()
    {
        var ctx = CreateContext();
        var svc = CreateService(ctx);
        var result = await svc.RequestBindingWithTeacherByCode(20, "NONEXISTENT");
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task RequestBindingWithTeacherByCode_CodeRevoked()
    {
        var ctx = CreateContext();
        ctx.InvitationCodes.Add(new InvitationCode { Id = 1, Code = "REVOKED", TeacherId = 10, IsRevoked = true });
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var result = await svc.RequestBindingWithTeacherByCode(20, "REVOKED");
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AcceptStudentByInvitationCode_Success()
    {
        var ctx = CreateContext();
        ctx.InvitationCodes.Add(new InvitationCode { Id = 1, Code = "CODE", TeacherId = 10 });
        ctx.BindingRequests.Add(new BindingRequest { Id = 1, CodeId = 1, StudentId = 20, IsAccepted = false, CreatedAt = DateTimeOffset.UtcNow });
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var result = await svc.AcceptStudentByInvitationCode(10, 1);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AcceptStudentByInvitationCode_RequestNotFound()
    {
        var ctx = CreateContext();
        var svc = CreateService(ctx);
        var result = await svc.AcceptStudentByInvitationCode(10, 999);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task RemoveBindingWithStudent_Success()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false, Status = TeacherStudentStatusEnum.Active });
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var result = await svc.RemoveBindingWithStudent(10, 20);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task BlockStudent_Success()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false, Status = TeacherStudentStatusEnum.Active });
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var result = await svc.BlockStudent(10, 20);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RemoveBindingWithStudent_NotFound()
    {
        var ctx = CreateContext();
        var svc = CreateService(ctx);
        var result = await svc.RemoveBindingWithStudent(10, 20);
        Assert.False(result.IsSuccess);
    }
}