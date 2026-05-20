@echo off
chcp 65001 >nul
cd /d "C:\custom\uni\CTHelper\App\CTHelper\CTHelper.UnitTests"

echo Creating folders and files...

mkdir Services\AssignmentService 2>nul
mkdir Services\AuthService 2>nul
mkdir Services\TestService 2>nul
mkdir Services\TestAttemptService 2>nul
mkdir Services\StatisticsService 2>nul
mkdir Services\TeacherStudentService 2>nul
mkdir Services\ProblemService 2>nul
mkdir Services\GroupService 2>nul
mkdir Services\FavouriteService 2>nul
mkdir Services\NotificationService 2>nul
mkdir Services\PasswordHasher 2>nul
mkdir Services\ShortTokenService 2>nul
mkdir Helpers 2>nul

type nul > Services\AssignmentService\AssignmentServiceAssignTestToStudentTests.cs
type nul > Services\AssignmentService\AssignmentServiceAssignTestToGroupTests.cs
type nul > Services\AssignmentService\AssignmentServicePatchAssignmentTests.cs
type nul > Services\AssignmentService\AssignmentServiceRevokeAssignmentTests.cs

type nul > Services\AuthService\AuthServiceRegisterTests.cs
type nul > Services\AuthService\AuthServiceLoginTests.cs
type nul > Services\AuthService\AuthServiceConfirmEmailTests.cs
type nul > Services\AuthService\AuthServicePasswordResetTests.cs
type nul > Services\AuthService\AuthServiceRefreshTokenTests.cs

type nul > Services\TestService\TestServiceCreateMixedTests.cs
type nul > Services\TestService\TestServiceCreateTests.cs
type nul > Services\TestService\TestServiceGetDetailsTests.cs
type nul > Services\TestService\TestServiceUpdateDeleteTests.cs

type nul > Services\TestAttemptService\TestAttemptServiceStartTests.cs
type nul > Services\TestAttemptService\TestAttemptServiceCompleteTests.cs
type nul > Services\TestAttemptService\TestAttemptServicePauseTests.cs
type nul > Services\TestAttemptService\TestAttemptServiceResumeGetTests.cs

type nul > Services\StatisticsService\StatisticsServiceTests.cs

type nul > Services\TeacherStudentService\TeacherStudentServiceTests.cs

type nul > Services\ProblemService\ProblemServiceTests.cs

type nul > Services\GroupService\GroupServiceTests.cs

type nul > Services\FavouriteService\FavouriteServiceTests.cs

type nul > Services\NotificationService\NotificationServiceTests.cs

type nul > Services\PasswordHasher\PasswordHasherAdapterTests.cs

type nul > Services\ShortTokenService\ShortTokenServiceTests.cs

type nul > Helpers\TestHelper.cs

echo Done!