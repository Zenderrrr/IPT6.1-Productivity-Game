-- SQLite


PRAGMA foreign_keys = ON;

CREATE TABLE PasswordResetToken (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    token_hash VARCHAR(255) NOT NULL UNIQUE,
    expires_at DATETIME NOT NULL,
    used_at DATETIME,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE
);

-- INDEXES
CREATE INDEX idx_passwordresettoken_user_id ON PasswordResetToken(user_id);
