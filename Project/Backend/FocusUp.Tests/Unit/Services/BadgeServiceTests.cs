using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;

namespace FocusUp.Tests.Services;

public class BadgeServiceTests
{
    [Fact]
    public void HasBadge_WhenBadgeExists_ShouldReturnTrue()
    {
        var service = new BadgeService(null!, null!, null!, []);

        var badges = new List<UserBadge>
        {
            new UserBadge(1, 5, DateTime.Now)
        };

        service.HasBadge(badges, 5).Should().BeTrue();
    }

    [Fact]
    public void HasBadge_WhenBadgeDoesNotExist_ShouldReturnFalse()
    {
        var service = new BadgeService(null!, null!, null!, []);

        var badges = new List<UserBadge>
        {
            new UserBadge(1, 3, DateTime.Now)
        };

        service.HasBadge(badges, 5).Should().BeFalse();
    }
}