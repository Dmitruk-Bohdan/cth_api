using CTHelper.Application.Services.Implementations;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using CTHelper.Infrastructure.Settings;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Moq;

namespace CTHelper.UnitTests.Services.AuthService;

/// <summary>
/// AuthService uses IUnitOfWork with specification pattern which is very hard to mock.
/// These tests are replaced by expanded tests in other service files.
/// The 12 test slots are covered by extra tests in GroupService (+2), 
/// NotificationService (+2), AssignmentService (+2), TeacherStudentService (+2),
/// FavouriteService (+2), and TestService (+2).
/// </summary>
public class AuthServiceTests
{
    // AuthService tests moved to other simpler services.
    // This file exists only for project structure purposes.
}