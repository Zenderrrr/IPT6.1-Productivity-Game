PRAGMA foreign_keys = ON;

-- =========================
-- BASIC CHECKS
-- =========================

-- all users
SELECT * FROM User;

-- all categories
SELECT * FROM Category;

-- all tasks
SELECT * FROM Task;

-- all task logs
SELECT * FROM TaskLog;

-- all xp events
SELECT * FROM XPEvent;

-- all user stats
SELECT * FROM UserStats;

-- all badges
SELECT * FROM Badge;

-- all user badges
SELECT * FROM UserBadge;


-- =========================
-- COUNT CHECKS
-- =========================

SELECT COUNT(*) AS user_count FROM User;
SELECT COUNT(*) AS category_count FROM Category;
SELECT COUNT(*) AS task_count FROM Task;
SELECT COUNT(*) AS tasklog_count FROM TaskLog;
SELECT COUNT(*) AS xpevent_count FROM XPEvent;
SELECT COUNT(*) AS userstats_count FROM UserStats;
SELECT COUNT(*) AS badge_count FROM Badge;
SELECT COUNT(*) AS userbadge_count FROM UserBadge;


-- =========================
-- FILTER TASKS
-- =========================

-- all completed tasks
SELECT * 
FROM Task
WHERE status = 'Completed';

-- all pending tasks
SELECT * 
FROM Task
WHERE status = 'Pending';

-- all tasks in progress
SELECT * 
FROM Task
WHERE status = 'In Progress';

-- all hard tasks
SELECT * 
FROM Task
WHERE difficulty = 'Hard';

-- tasks longer than 60 minutes
SELECT * 
FROM Task
WHERE duration_min > 60;

-- tasks for one user
SELECT * 
FROM Task
WHERE user_id = 1;

-- tasks for one category
SELECT * 
FROM Task
WHERE category_id = 1;


-- =========================
-- ORDERING
-- =========================

-- tasks ordered by due date
SELECT * 
FROM Task
ORDER BY due_date;

-- tasks ordered by newest created
SELECT * 
FROM Task
ORDER BY created_at DESC;

-- users ordered alphabetically
SELECT * 
FROM User
ORDER BY username ASC;


-- =========================
-- JOINS
-- =========================

-- tasks with username
SELECT 
    Task.id,
    Task.title,
    Task.status,
    User.username
FROM Task
JOIN User ON Task.user_id = User.id;

-- tasks with category name
SELECT 
    Task.id,
    Task.title,
    Category.name AS category_name
FROM Task
LEFT JOIN Category ON Task.category_id = Category.id;

-- tasks with user and category
SELECT 
    Task.id,
    Task.title,
    Task.status,
    User.username,
    Category.name AS category_name
FROM Task
JOIN User ON Task.user_id = User.id
LEFT JOIN Category ON Task.category_id = Category.id;

-- user badges with badge names
SELECT
    User.username,
    Badge.name AS badge_name,
    UserBadge.awarded_at
FROM UserBadge
JOIN User ON UserBadge.user_id = User.id
JOIN Badge ON UserBadge.badge_id = Badge.id;

-- xp events with username and task title
SELECT
    XPEvent.id,
    User.username,
    Task.title,
    XPEvent.reason,
    XPEvent.amount,
    XPEvent.created_at
FROM XPEvent
JOIN User ON XPEvent.user_id = User.id
LEFT JOIN Task ON XPEvent.task_id = Task.id;


-- =========================
-- AGGREGATES
-- =========================

-- number of tasks per user
SELECT
    user_id,
    COUNT(*) AS total_tasks
FROM Task
GROUP BY user_id;

-- completed tasks per user
SELECT
    user_id,
    COUNT(*) AS completed_tasks
FROM Task
WHERE status = 'Completed'
GROUP BY user_id;

-- average task duration
SELECT
    AVG(duration_min) AS avg_duration
FROM Task;

-- total xp per user from XPEvent
SELECT
    user_id,
    SUM(amount) AS total_xp
FROM XPEvent
GROUP BY user_id;

-- max task duration
SELECT
    MAX(duration_min) AS longest_task
FROM Task;

-- min task duration
SELECT
    MIN(duration_min) AS shortest_task
FROM Task;


-- =========================
-- GROUP BY WITH JOIN
-- =========================

-- task count per username
SELECT
    User.username,
    COUNT(Task.id) AS task_count
FROM User
LEFT JOIN Task ON User.id = Task.user_id
GROUP BY User.id, User.username;

-- completed task count per username
SELECT
    User.username,
    COUNT(Task.id) AS completed_count
FROM User
LEFT JOIN Task 
    ON User.id = Task.user_id
   AND Task.status = 'Completed'
GROUP BY User.id, User.username;

-- total xp per username
SELECT
    User.username,
    COALESCE(SUM(XPEvent.amount), 0) AS total_xp
FROM User
LEFT JOIN XPEvent ON User.id = XPEvent.user_id
GROUP BY User.id, User.username;


-- =========================
-- TIME / TIMESTAMP TESTS
-- =========================

-- tasks with exact time gap in minutes between created and completed
SELECT
    id,
    title,
    created_at,
    completed_at,
    ROUND((julianday(completed_at) - julianday(created_at)) * 24 * 60, 2) AS minutes_taken
FROM Task
WHERE completed_at IS NOT NULL;

-- tasks created after a certain date
SELECT *
FROM Task
WHERE created_at > '2026-03-04 00:00:00';

-- tasks due before a certain date
SELECT *
FROM Task
WHERE due_date < '2026-03-12 00:00:00';


-- =========================
-- USERSTATS CHECKS
-- =========================

SELECT * 
FROM UserStats
WHERE total_xp > 0;

SELECT * 
FROM UserStats
ORDER BY total_xp DESC;

SELECT * 
FROM UserStats
ORDER BY streak_count DESC;


-- =========================
-- BADGE TESTS
-- =========================

-- all badges earned by user 1
SELECT
    User.username,
    Badge.name
FROM UserBadge
JOIN User ON UserBadge.user_id = User.id
JOIN Badge ON UserBadge.badge_id = Badge.id
WHERE User.id = 1;

-- how many users have each badge
SELECT
    Badge.name,
    COUNT(UserBadge.user_id) AS user_count
FROM Badge
LEFT JOIN UserBadge ON Badge.id = UserBadge.badge_id
GROUP BY Badge.id, Badge.name;


-- =========================
-- TASKLOG TESTS
-- =========================

-- logs for one task
SELECT *
FROM TaskLog
WHERE task_id = 1;

-- logs with task title
SELECT
    TaskLog.id,
    Task.title,
    TaskLog.action,
    TaskLog.xp_awarded,
    TaskLog.created_at
FROM TaskLog
JOIN Task ON TaskLog.task_id = Task.id;


-- =========================
-- MULTI-CONDITION TESTS
-- =========================

-- completed hard tasks
SELECT *
FROM Task
WHERE status = 'Completed'
  AND difficulty = 'Hard';

-- tasks for user 1 that are still open
SELECT *
FROM Task
WHERE user_id = 1
  AND status IN ('Pending', 'In Progress');

-- tasks between 30 and 60 minutes
SELECT *
FROM Task
WHERE duration_min BETWEEN 30 AND 60;


-- =========================
-- SEARCH TESTS
-- =========================

-- tasks with "Task 1" in title
SELECT *
FROM Task
WHERE title LIKE '%Task 1%';

-- users with email from example.com
SELECT *
FROM User
WHERE email LIKE '%@example.com';