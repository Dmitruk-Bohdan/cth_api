using CTHelper.Application.Models.Notification;
using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Persistence.Context;
using CTHelper.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CTHelper.UnitTests.Services.NotificationService;

public class NotificationServiceTests
{
    private AppDbContext CreateContext() => new(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task GetMyNotificationList_Success()
    {
        var ctx = CreateContext();
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "user"));
        ctx.Notifications.Add(new Notification { Id = 1, RecipientId = 10, IsDeleted = false, Payload = "Hello", PriorityLevel = NotificationPriorityLevelTypeEnum.Low, IsSeen = false, CreatedAt = DateTimeOffset.UtcNow });
        ctx.Notifications.Add(new Notification { Id = 2, RecipientId = 10, IsDeleted = false, Payload = "World", PriorityLevel = NotificationPriorityLevelTypeEnum.Important, IsSeen = false, CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.NotificationService(ctx);
        var result = await svc.GetMyNotificationList(10, 10, 1);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Payload!.Items.Count());
    }

    [Fact]
    public async Task GetMyNotificationList_Empty()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.NotificationService(ctx);
        var result = await svc.GetMyNotificationList(10, pageSize: 10, pageNumber: 1);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Payload!.Items);
    }

    [Fact]
    public async Task MarkAsRead_Success()
    {
        var ctx = CreateContext();
        ctx.Users.Add(TestEntityFactory.CreateUser(10, "user"));
        ctx.Notifications.Add(new Notification { Id = 1, RecipientId = 10, IsDeleted = false, IsSeen = false, Payload = "Hello", PriorityLevel = NotificationPriorityLevelTypeEnum.Low, CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.NotificationService(ctx);
        var result = await svc.MarkAsRead(new ReadNotificationRequestModel { UserId = 10, NotificationIds = new List<long> { 1 } });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetNotificationDetails_Success()
    {
        var ctx = CreateContext();
        ctx.Notifications.Add(new Notification { Id = 1, RecipientId = 10, IsDeleted = false, Payload = "Full notification payload text", PriorityLevel = NotificationPriorityLevelTypeEnum.Low, IsSeen = false, CreatedAt = DateTimeOffset.UtcNow });
        await ctx.SaveChangesAsync();

        var svc = new CTHelper.Infrastructure.Services.Implementations.NotificationService(ctx);
        var result = await svc.GetNotificationDetails(new NotificationDetailsRequestModel { NotificationId = 1, UserId = 10 });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task MarkAsRead_EmptyIds()
    {
        var ctx = CreateContext();
        var svc = new CTHelper.Infrastructure.Services.Implementations.NotificationService(ctx);
        var result = await svc.MarkAsRead(new ReadNotificationRequestModel { UserId = 10, NotificationIds = new List<long>() });
        Assert.False(result.IsSuccess);
    }
}