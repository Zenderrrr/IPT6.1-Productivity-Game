-- SQLite

PRAGMA foreign_keys = ON;

ALTER TABLE Badge
ADD COLUMN isVisible INTEGER NOT NULL DEFAULT 1
CHECK (isVisible IN (0, 1));