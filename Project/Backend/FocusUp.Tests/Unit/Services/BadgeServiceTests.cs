using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;

namespace FocusUp.Tests.Services;

/// <summary>
/// Tests for the BadgeService.
/// </summary>
public class BadgeServiceTests
{
    /// <summary>
    /// Ensures HasBadge returns true when the badge exists.
    /// </summary>
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

    /// <summary>
    /// Ensures HasBadge returns false when the badge does not exist.
    /// </summary>
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

    /// <summary>
    /// Ensures badges are unlocked when the rule is fulfilled.
    /// </summary>
    [Fact]
    public void CheckAndAwardBadges_WhenRuleFulfilled_ShouldUnlockBadge()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

        var stats = new UserStats(userId);
        stats.AddXp(100);
        userStatsRepo.Insert(stats);
        userStatsRepo.UpdateTotalXp(userId, 100);

        var badgeId = badgeRepo.Insert(new Badge("XP Badge", "Earn XP", BadgeRuleType.xp_total, 50));

        var service = new BadgeService(
            userStatsRepo,
            badgeRepo,
            userBadgeRepo,
            [new TotalXpBadgeRule()]
        );

        var result = service.CheckAndAwardBadges(userId);

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(badgeId);
        userBadgeRepo.Exists(userId, badgeId).Should().BeTrue();
    }

    /// <summary>
    /// Ensures already unlocked badges are not awarded twice.
    /// </summary>
    [Fact]
    public void CheckAndAwardBadges_WhenBadgeAlreadyUnlocked_ShouldNotAwardAgain()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

        var stats = new UserStats(userId);
        stats.AddXp(100);
        userStatsRepo.Insert(stats);
        userStatsRepo.UpdateTotalXp(userId, 100);

        var badgeId = badgeRepo.Insert(new Badge("XP Badge", "Earn XP", BadgeRuleType.xp_total, 50));
        userBadgeRepo.Insert(new UserBadge(userId, badgeId, DateTime.Now));

        var service = new BadgeService(
            userStatsRepo,
            badgeRepo,
            userBadgeRepo,
            [new TotalXpBadgeRule()]
        );

        var result = service.CheckAndAwardBadges(userId);

        result.Should().BeEmpty();
        userBadgeRepo.GetByUserId(userId).Should().HaveCount(1);
    }

    /// <summary>
    /// Ensures badges are not unlocked when the rule is not fulfilled.
    /// </summary>
    [Fact]
    public void CheckAndAwardBadges_WhenRuleNotFulfilled_ShouldNotUnlockBadge()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

        userStatsRepo.Insert(new UserStats(userId));

        badgeRepo.Insert(new Badge("XP Badge", "Earn XP", BadgeRuleType.xp_total, 500));

        var service = new BadgeService(
            userStatsRepo,
            badgeRepo,
            userBadgeRepo,
            [new TotalXpBadgeRule()]
        );

        var result = service.CheckAndAwardBadges(userId);

        result.Should().BeEmpty();
        userBadgeRepo.GetByUserId(userId).Should().BeEmpty();
    }

    /// <summary>
    /// Ensures only matching badge rules are applied.
    /// </summary>
    [Fact]
    public void CheckAndAwardBadges_ShouldOnlyApplyMatchingBadgeRules()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

        var stats = new UserStats(userId);
        stats.AddXp(100);
        userStatsRepo.Insert(stats);
        userStatsRepo.UpdateTotalXp(userId, 100);

        badgeRepo.Insert(new Badge("XP Badge", "Earn XP", BadgeRuleType.xp_total, 50));
        badgeRepo.Insert(new Badge("Streak Badge", "Streak", BadgeRuleType.streak, 5));

        var service = new BadgeService(
            userStatsRepo,
            badgeRepo,
            userBadgeRepo,
            [new TotalXpBadgeRule(), new StreakBadgeRule()]
        );

        var result = service.CheckAndAwardBadges(userId);

        result.Should().HaveCount(1);
        result[0].RuleType.Should().Be(BadgeRuleType.xp_total);
    }
}