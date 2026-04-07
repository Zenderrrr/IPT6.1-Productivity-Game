PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

-- =========================
-- USERS (5)
-- =========================
INSERT INTO User (username, email, password_hash, created_at, updated_at) VALUES
('alice', 'alice@example.com', 'hash_alice', '2026-03-01 08:00:00', '2026-03-01 08:00:00'),
('ben', 'ben@example.com', 'hash_ben', '2026-03-01 08:10:00', '2026-03-01 08:10:00'),
('clara', 'clara@example.com', 'hash_clara', '2026-03-01 08:20:00', '2026-03-01 08:20:00'),
('david', 'david@example.com', 'hash_david', '2026-03-01 08:30:00', '2026-03-01 08:30:00'),
('emma', 'emma@example.com', 'hash_emma', '2026-03-01 08:40:00', '2026-03-01 08:40:00');

-- =========================
-- CATEGORIES (25)
-- =========================
INSERT INTO Category (user_id, name, color, created_at) VALUES
(1,'Study','#FF6B6B','2026-03-02 09:00:00'),
(1,'Work','#4ECDC4','2026-03-02 09:01:00'),
(1,'Fitness','#45B7D1','2026-03-02 09:02:00'),
(1,'Health','#2A9D8F','2026-03-02 09:03:00'),
(1,'Personal','#F4A261','2026-03-02 09:04:00'),

(2,'Study','#FF6B6B','2026-03-02 09:05:00'),
(2,'Work','#4ECDC4','2026-03-02 09:06:00'),
(2,'Fitness','#45B7D1','2026-03-02 09:07:00'),
(2,'Health','#2A9D8F','2026-03-02 09:08:00'),
(2,'Personal','#F4A261','2026-03-02 09:09:00'),

(3,'Study','#FF6B6B','2026-03-02 09:10:00'),
(3,'Work','#4ECDC4','2026-03-02 09:11:00'),
(3,'Fitness','#45B7D1','2026-03-02 09:12:00'),
(3,'Health','#2A9D8F','2026-03-02 09:13:00'),
(3,'Personal','#F4A261','2026-03-02 09:14:00'),

(4,'Study','#FF6B6B','2026-03-02 09:15:00'),
(4,'Work','#4ECDC4','2026-03-02 09:16:00'),
(4,'Fitness','#45B7D1','2026-03-02 09:17:00'),
(4,'Health','#2A9D8F','2026-03-02 09:18:00'),
(4,'Personal','#F4A261','2026-03-02 09:19:00'),

(5,'Study','#FF6B6B','2026-03-02 09:20:00'),
(5,'Work','#4ECDC4','2026-03-02 09:21:00'),
(5,'Fitness','#45B7D1','2026-03-02 09:22:00'),
(5,'Health','#2A9D8F','2026-03-02 09:23:00'),
(5,'Personal','#F4A261','2026-03-02 09:24:00');

-- =========================
-- BADGES (5)
-- =========================
INSERT INTO Badge (name, description, rule_type, rule_value, created_at) VALUES
('First Task','Complete first task','tasks_completed',1,'2026-03-02 11:00:00'),
('XP Starter','Earn 50 XP','xp_total',50,'2026-03-02 11:01:00'),
('Task Grinder','Complete 10 tasks','tasks_completed',10,'2026-03-02 11:02:00'),
('3 Day Streak','3-day streak','streak',3,'2026-03-02 11:03:00'),
('Time Tracker','300 minutes','time_logged',300,'2026-03-02 11:04:00');

-- =========================
-- TASKS (100)
-- =========================
WITH RECURSIVE nums(n) AS (
  SELECT 1 UNION ALL SELECT n+1 FROM nums WHERE n < 20
)
INSERT INTO Task (
  user_id, category_id, title, description, difficulty, duration_min,
  due_date, status, completed_at, created_at, updated_at
)
SELECT
  u.id,
  ((u.id-1)*5)+((n-1)%5)+1,
  'Task '||n||' User '||u.id,
  'Task description '||n,
  CASE WHEN n%3=1 THEN 'Easy' WHEN n%3=2 THEN 'Medium' ELSE 'Hard' END,
  15+(n*5),
  datetime('2026-03-10','+'||(u.id*n)||' hours'),
  CASE WHEN n%3=1 THEN 'Completed' WHEN n%3=2 THEN 'In Progress' ELSE 'Pending' END,
  CASE WHEN n%3=1 THEN datetime('2026-03-03','+'||(u.id*n)||' hours','+45 minutes') END,
  datetime('2026-03-03','+'||(u.id*n)||' hours'),
  datetime('2026-03-03','+'||(u.id*n)||' hours')
FROM User u, nums;

-- =========================
-- TASKLOG
-- =========================
INSERT INTO TaskLog (task_id, action, xp_awarded, created_at)
SELECT id,'Task created',0,created_at FROM Task;

INSERT INTO TaskLog (task_id, action, xp_awarded, created_at)
SELECT id,
CASE WHEN status='Completed' THEN 'Task completed' ELSE 'Task started' END,
CASE WHEN status='Completed' THEN 20 ELSE 5 END,
updated_at
FROM Task WHERE status!='Pending';

-- =========================
-- XPEVENT
-- =========================
INSERT INTO XPEvent (user_id, task_id, reason, amount, created_at)
SELECT user_id,id,'TaskComplete',20,completed_at FROM Task WHERE status='Completed';

INSERT INTO XPEvent (user_id, task_id, reason, amount, created_at)
SELECT user_id,id,'TaskStart',5,updated_at FROM Task WHERE status='In Progress';

-- =========================
-- USERSTATS (5)
-- =========================
INSERT INTO UserStats (
  user_id,total_xp,tasks_done,tasks_open,total_time_min,
  streak_count,best_streak,streak_last_date,last_active_at,created_at,updated_at
)
SELECT
  u.id,
  COALESCE(SUM(x.amount),0),
  SUM(CASE WHEN t.status='Completed' THEN 1 ELSE 0 END),
  SUM(CASE WHEN t.status!='Completed' THEN 1 ELSE 0 END),
  SUM(t.duration_min),
  u.id+1,u.id+2,
  MAX(t.updated_at),
  MAX(t.updated_at),
  CURRENT_TIMESTAMP,
  CURRENT_TIMESTAMP
FROM User u
LEFT JOIN Task t ON t.user_id=u.id
LEFT JOIN XPEvent x ON x.user_id=u.id
GROUP BY u.id;

-- =========================
-- USERBADGE (25)
-- =========================
INSERT INTO UserBadge (user_id, badge_id, awarded_at)
SELECT u.id,b.id,datetime('2026-03-08','+'||(u.id+b.id)||' hours')
FROM User u, Badge b;

COMMIT;