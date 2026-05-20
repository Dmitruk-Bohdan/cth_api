using CTHelper.Application.Models.Assignment;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.AssignmentService;

public class AssignmentServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static Test CreateTest(long id, bool isPublished) => new()
    {
        Id = id, Title = $"Test {id}", IsPublished = isPublished, IsDeleted = false,
        AuthorId = 0, SubjectId = 0, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow,
        Type = Domain.Common.Enums.TestTypeEnum.Custom
    };

    [Fact]
    public async Task AssignTestToStudent_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToStudent(new AssignTestToStudentRequestModel
        { TestId = 1, TeacherId = 10, StudentId = 20, Deadline = DateTimeOffset.UtcNow.AddDays(7), AttemptsAllowed = 3 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToStudent_TestNotFound()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToStudent(new AssignTestToStudentRequestModel { TestId = 999, TeacherId = 10, StudentId = 20 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToStudent_TestNotPublished()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, false));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToStudent(new AssignTestToStudentRequestModel { TestId = 1, TeacherId = 10, StudentId = 20 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToStudent_NoBinding()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToStudent(new AssignTestToStudentRequestModel { TestId = 1, TeacherId = 10, StudentId = 20 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToStudent_AlreadyAssigned()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        ctx.StudentAssignments.Add(new StudentAssignment { StudentId = 20, TeacherId = 10, TestId = 1, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToStudent(new AssignTestToStudentRequestModel { TestId = 1, TeacherId = 10, StudentId = 20 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToGroup_Success()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        ctx.Groups.Add(new Group { Id = 1, TeacherId = 10, SubjectId = 1, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToGroup(new AssignTestToGroupRequestModel
        { TestId = 1, TeacherId = 10, GroupId = 1, Deadline = DateTimeOffset.UtcNow.AddDays(7), AttemptsAllowed = 3 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToGroup_GroupNotFound()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToGroup(new AssignTestToGroupRequestModel { TestId = 1, TeacherId = 10, GroupId = 999 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task AssignTestToGroup_AlreadyAssigned()
    {
        var ctx = CreateContext();
        ctx.Tests.Add(CreateTest(1, true));
        ctx.Groups.Add(new Group { Id = 1, TeacherId = 10, SubjectId = 1, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        ctx.GroupAssignments.Add(new GroupAssignment { GroupId = 1, TeacherId = 10, TestId = 1, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.AssignTestToGroup(new AssignTestToGroupRequestModel { TestId = 1, TeacherId = 10, GroupId = 1 });
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task PatchAssignment_StudentDeadline()
    {
        var ctx = CreateContext();
        ctx.StudentAssignments.Add(new StudentAssignment { Id = 1, StudentId = 20, TeacherId = 10, TestId = 1, ExpiredAt = DateTimeOffset.MaxValue, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var newDeadline = DateTimeOffset.UtcNow.AddDays(14);
        var result = await svc.PatchAssignment(new PatchAssignmentRequestModel { AssignmentId = 1, TeacherId = 10, Deadline = newDeadline });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task PatchAssignment_GroupDeadline()
    {
        var ctx = CreateContext();
        ctx.GroupAssignments.Add(new GroupAssignment { Id = 1, GroupId = 1, TeacherId = 10, TestId = 1, ExpiredAt = DateTimeOffset.MaxValue, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        ctx.StudentAssignments.Add(new StudentAssignment { Id = 1, GroupAssignmentId = 1, StudentId = 20, TeacherId = 10, TestId = 1, ExpiredAt = DateTimeOffset.MaxValue, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.PatchAssignment(new PatchAssignmentRequestModel { AssignmentId = 1, TeacherId = 10, Deadline = DateTimeOffset.UtcNow.AddDays(14) });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RevokeAssignment_Student()
    {
        var ctx = CreateContext();
        ctx.StudentAssignments.Add(new StudentAssignment { Id = 1, StudentId = 20, TeacherId = 10, TestId = 1, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.RevokeAssignment(new RevokeAssignmentRequestModel { AssignmentId = 1, TeacherId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RevokeAssignment_Group()
    {
        var ctx = CreateContext();
        ctx.GroupAssignments.Add(new GroupAssignment { Id = 1, GroupId = 1, TeacherId = 10, TestId = 1, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        ctx.StudentAssignments.Add(new StudentAssignment { Id = 1, GroupAssignmentId = 1, StudentId = 20, TeacherId = 10, TestId = 1, IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = new CTHelper.Infrastructure.Services.Implementations.AssignmentService(ctx);
        var result = await svc.RevokeAssignment(new RevokeAssignmentRequestModel { AssignmentId = 1, TeacherId = 10 });
        Assert.True(result.IsSuccess);
    }
}