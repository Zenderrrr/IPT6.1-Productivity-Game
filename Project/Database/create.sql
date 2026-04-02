-- SQLite

PRAGMA foreign_keys = ON;

CREATE TABLE User (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Category (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    name VARCHAR(50) NOT NULL,
    color VARCHAR(7),
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE
);

CREATE TABLE Task (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    category_id INTEGER,
    title VARCHAR(255) NOT NULL,
    description VARCHAR(255),
    difficulty VARCHAR(20),
    duration_min INTEGER,
    due_date DATETIME,
    status VARCHAR(20) NOT NULL,
    completed_at DATETIME,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES Category(id) ON DELETE SET NULL,

    CHECK (difficulty IN ('Easy', 'Medium', 'Hard') OR difficulty IS NULL),
    CHECK (status IN ('Pending', 'In Progress', 'Completed')),
    CHECK (duration_min IS NULL OR duration_min >= 0)
);

CREATE TABLE TaskLog (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    task_id INTEGER NOT NULL,
    action VARCHAR(255),
    xp_awarded INTEGER DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (task_id) REFERENCES Task(id) ON DELETE CASCADE
);

CREATE TABLE XPEvent (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    task_id INTEGER,
    reason VARCHAR(20),
    amount INTEGER NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE,
    FOREIGN KEY (task_id) REFERENCES Task(id) ON DELETE SET NULL
);

CREATE TABLE UserStats (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL UNIQUE,
    total_xp INTEGER DEFAULT 0,
    tasks_done INTEGER DEFAULT 0,
    tasks_open INTEGER DEFAULT 0,
    total_time_min INTEGER DEFAULT 0,
    streak_count INTEGER DEFAULT 0,
    best_streak INTEGER DEFAULT 0,
    streak_last_date DATETIME,
    last_active_at DATETIME,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE
);

CREATE TABLE Badge (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    rule_type VARCHAR(50) NOT NULL,
    rule_value INTEGER NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE UserBadge (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    badge_id INTEGER NOT NULL,
    awarded_at DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE,
    FOREIGN KEY (badge_id) REFERENCES Badge(id) ON DELETE CASCADE,

    UNIQUE (user_id, badge_id)
);

-- INDEXES
CREATE INDEX idx_user_email ON User(email);

CREATE INDEX idx_category_user_id ON Category(user_id);

CREATE INDEX idx_task_user_id ON Task(user_id);
CREATE INDEX idx_task_category_id ON Task(category_id);
CREATE INDEX idx_task_status ON Task(status);
CREATE INDEX idx_task_due_date ON Task(due_date);
CREATE INDEX idx_task_created_at ON Task(created_at);
CREATE INDEX idx_task_completed_at ON Task(completed_at);
CREATE INDEX idx_task_user_status ON Task(user_id, status);

CREATE INDEX idx_tasklog_task_id ON TaskLog(task_id);
CREATE INDEX idx_tasklog_created_at ON TaskLog(created_at);

CREATE INDEX idx_xpevent_user_id ON XPEvent(user_id);
CREATE INDEX idx_xpevent_task_id ON XPEvent(task_id);
CREATE INDEX idx_xpevent_created_at ON XPEvent(created_at);

CREATE INDEX idx_userbadge_user_id ON UserBadge(user_id);
CREATE INDEX idx_userbadge_badge_id ON UserBadge(badge_id);