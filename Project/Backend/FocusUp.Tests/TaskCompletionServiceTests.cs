using Xunit;

namespace FocusUp.Tests.Services;

public class TaskCompletionServiceTests
{
    [Fact(Skip = "Cannot be unit tested without changing backend code: TaskCompletionService depends on concrete repositories and DatabaseConnection.")]
    public void CompleteTask_ShouldSetTaskToCompleted()
    {
    }

    [Fact(Skip = "Cannot be unit tested without repository interfaces or mockable repositories.")]
    public void CompleteTask_AlreadyCompletedTask_ShouldNotAwardXpAgain()
    {
    }

    [Fact(Skip = "Cannot be unit tested without repository interfaces or mockable repositories.")]
    public void CompleteTask_WrongUser_ShouldThrowUnauthorizedTaskAccessException()
    {
    }

    [Fact(Skip = "Cannot verify XP awarding exactly once because XPEventRepository is a concrete database dependency.")]
    public void CompleteTask_ShouldAwardXpExactlyOnce()
    {
    }

    [Fact(Skip = "Cannot verify streak update because StreakService and repositories are concrete dependencies.")]
    public void CompleteTask_ShouldUpdateStreak()
    {
    }

    [Fact(Skip = "Cannot verify badge check because BadgeService is a concrete dependency.")]
    public void CompleteTask_ShouldTriggerBadgeCheck()
    {
    }

    [Fact(Skip = "Cannot test transaction rollback without integration test database setup.")]
    public void CompleteTask_WhenErrorOccurs_ShouldKeepStateConsistent()
    {
    }
}