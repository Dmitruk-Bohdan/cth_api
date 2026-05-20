using CTHelper.Application.Models.Group;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using CTHelper.Application.Services.Interfaces;
using CTHelper.UnitTests.Helpers;

namespace CTHelper.UnitTests.Services.GroupService;

public class GroupServiceTests
{
    private AppDbContext CreateContext() => new(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);

    private CTHelper.Infrastructure.Services.Implementations.GroupService CreateService(AppDbContext ctx)
    {
        var fileStorageMock = new Mock<IFileStorageService>();
        fileStorageMock.Setup(f => f.GetDownloadUrl(It.IsAny<long>())).ReturnsAsync("http://example.com/img.jpg");
        return new CTHelper.Infrastructure.Services.Implementations.GroupService(ctx, fileStorageMock.Object);
    }

    [Fact]
    public async Task CreateGroup_Success()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var r = await svc.CreateGroup(new CreateGroupModel { SubjectId = 1, TeacherId = 10, GroupName = "Group A" });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task DeleteGroup_Success()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Groups.Add(new Group { Id = 1, SubjectId = 1, TeacherId = 10, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var r = await svc.DeleteGroup(new DeleteGroupModel { GroupId = 1, TeacherId = 10 });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task AddStudentToGroup_Success()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        ctx.Groups.Add(new Group { Id = 1, TeacherId = 10, SubjectId = 1, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var r = await svc.AddStudentToGroup(new AddStudentToGroupModel { GroupId = 1, TeacherId = 10, StudentId = 20 });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task AddStudentToGroup_AlreadyInGroup()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));

        // Create group with student via DB
        var g = new Group { Id = 1, TeacherId = 10, SubjectId = 1, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow };
        ctx.Groups.Add(g);
        await ctx.SaveChangesAsync();

        // Add student to group
        var student = await ctx.Users.FindAsync(20L);
        var group = await ctx.Groups.Include(x => x.Students).FirstAsync(g => g.Id == 1);
        group.Students.Add(student!);
        await ctx.SaveChangesAsync();

        var svc = CreateService(ctx);
        var r = await svc.AddStudentToGroup(new AddStudentToGroupModel { GroupId = 1, TeacherId = 10, StudentId = 20 });
        Assert.False(r.IsSuccess);
    }

    [Fact]
    public async Task RemoveStudentFromGroup_Success()
    {
        var ctx = CreateContext();
        ctx.TeacherStudents.Add(new TeacherStudent { TeacherId = 10, StudentId = 20, IsDeleted = false });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Users.Add(TestEntityFactory.CreateUser(20, "student"));
        var g = new Group { Id = 1, TeacherId = 10, SubjectId = 1, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow };
        ctx.Groups.Add(g);
        await ctx.SaveChangesAsync();

        var student = await ctx.Users.FindAsync(20L);
        var group = await ctx.Groups.Include(x => x.Students).FirstAsync(g => g.Id == 1);
        group.Students.Add(student!);
        await ctx.SaveChangesAsync();

        var svc = CreateService(ctx);
        var r = await svc.RemoveStudentFromGroup(new RemoveStudentFromGroupModel { GroupId = 1, TeacherId = 10, StudentId = 20 });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task GetGroupById_Success()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Groups.Add(new Group { Id = 1, SubjectId = 1, TeacherId = 10, Name = "Group A", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var r = await svc.GetGroupById(new GetGroupByIdModel { GroupId = 1, TeacherId = 10 });
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task GetMyGroupList_Success()
    {
        var ctx = CreateContext();
        ctx.Subjects.Add(new Subject { Id = 1, Name = "Math" });
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "teacher"));
        ctx.Groups.Add(new Group { Id = 1, SubjectId = 1, TeacherId = 10, Name = "G1", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        ctx.Groups.Add(new Group { Id = 2, SubjectId = 1, TeacherId = 10, Name = "G2", IsDeleted = false, CreatedAt = DateTimeOffset.UtcNow, LastUpdateAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();
        var svc = CreateService(ctx);
        var r = await svc.GetMyGroupList(new MyGroupListRequestModel { TeacherId = 10, SubjectId = 1, PageNumber = 1, PageSize = 10 });
        Assert.True(r.IsSuccess);
        Assert.Equal(2, r.Payload!.Items.Count());
    }

    [Fact]
    public async Task DeleteGroup_NotFound()
    {
        var ctx = CreateContext();
        var svc = CreateService(ctx);
        var r = await svc.DeleteGroup(new DeleteGroupModel { GroupId = 999, TeacherId = 10 });
        Assert.False(r.IsSuccess);
    }

    [Fact]
    public async Task GetGroupById_NotFound()
    {
        var ctx = CreateContext();
        var svc = CreateService(ctx);
        var r = await svc.GetGroupById(new GetGroupByIdModel { GroupId = 999, TeacherId = 10 });
        Assert.False(r.IsSuccess);
    }
}