# Entity-Relationsship Diagram

## Entitäten mit Attributen & PK/FK

### User

| Attribute     | Datentyp   | Schlüsseltyp      |
| ------------- | ---------- | ----------------- |
| id            | UUID / INT | PK                |
| userBadge_id  | UUID / INT | FK → UserBadge.id |
| username      | VARCHAR    | UQ                |
| email         | VARCHAR    | UQ                |
| password_hash | VARCHAR    | -                 |
| created_at    | TIMESTAMP  | -                 |
| updated_at    | TIMESTAMP  | -                 |

---

### Task

| Attribute    | Datentyp   | Schlüsseltyp                |
| ------------ | ---------- | --------------------------- |
| id           | UUID / INT | PK                          |
| user_id      | UUID / INT | FK → User.id                |
| title        | VARCHAR    | -                           |
| description  | TEXT       | -                           |
| difficulty   | SMALLINT   | -                           |
| duration_min | INT        | -                           |
| due_date     | DATE       | -                           |
| status       | VARCHAR    | -                           |
| completed_at | TIMESTAMP  | -                           |
| created_at   | TIMESTAMP  | -                           |
| updated_at   | TIMESTAMP  | -                           |

---

### TaskLog

| Attribute  | Datentyp   | Schlüsseltyp |
| ---------- | ---------- | ------------ |
| id         | UUID / INT | PK           |
| user_id    | UUID / INT | FK → User.id |
| task_id    | UUID / INT | FK → Task.id |
| action     | VARCHAR    | -            |
| xp_awarded | INT        | -            |
| created_at | TIMESTAMP  | -            |

---

### XPEvent

| Attribute  | Datentyp   | Schlüsseltyp            |
| ---------- | ---------- | ----------------------- |
| id         | UUID / INT | PK                      |
| user_id    | UUID / INT | FK → User.id            |
| task_id    | UUID / INT | FK → Task.id (nullable) |
| reason     | VARCHAR    | -                       |
| amount     | INT        | -                       |
| created_at | TIMESTAMP  | -                       |

---

### UserStats

| Attribute        | Datentyp   | Schlüsseltyp      |
| ---------------- | ---------- | ----------------- |
| id               | UUID / INT | PK                |
| user_id          | UUID / INT | FK → User.id (UQ) |
| total_xp         | INT        | -                 |
| tasks_done       | INT        | -                 |
| tasks_open       | INT        | -                 |
| total_time_min   | INT        | -                 |
| streak_count     | INT        | -                 |
| best_streak      | INT        | -                 |
| streak_last_date | DATE       | -                 |
| last_active_at   | TIMESTAMP  | -                 |
| created_at       | TIMESTAMP  | -                 |
| updated_at       | TIMESTAMP  | -                 |

---

### Category

| Attribute  | Datentyp   | Schlüsseltyp |
| ---------- | ---------- | ------------ |
| id         | UUID / INT | PK           |
| user_id    | UUID / INT | FK → User.id |
| name       | VARCHAR    | -            |
| color      | VARCHAR    | -            |
| created_at | TIMESTAMP  | -            |

---

### UserBadge

| Attribute  | Datentyp   | Schlüsseltyp  |
| ---------- | ---------- | ------------- |
| id         | UUID / INT | PK            |
| user_id    | UUID / INT | FK → User.id  |
| badge_id   | UUID / INT | FK → Badge.id |
| awarded_at | TIMESTAMP  | -             |

---

### Badge

| Attribute    | Datentyp   | Schlüsseltyp      |
| ------------ | ---------- | ----------------- |
| id           | UUID / INT | PK                |
| name         | VARCHAR    | UQ                |
| description  | TEXT       | -                 |
| rule_type    | VARCHAR    | -                 |
| rule_value   | INT        | -                 |
| created_at   | TIMESTAMP  | -                 |

---
