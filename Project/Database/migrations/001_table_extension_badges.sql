--SQLite

PRAGMA foreign_keys = ON;

ALTER TABLE Badge
ADD COLUMN svg TEXT;

ALTER TABLE Badge
ADD COLUMN primary_color VARCHAR(7) NOT NULL DEFAULT '#FFFFFF';

ALTER TABLE Badge
ADD COLUMN secondary_color VARCHAR(7) NOT NULL DEFAULT '#000000';

ALTER TABLE Badge
ADD COLUMN rarity VARCHAR(20) NOT NULL DEFAULT 'Common'
CHECK (rarity IN ('Common', 'Rare', 'Epic', 'Legendary'));