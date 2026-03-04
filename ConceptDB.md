# Entity-Relationsship Diagram

## Entitäten mit Attributen & PK/FK

### User

| Attribute        | Datentyp   | Schlüsseltyp |
| ---------------- | ---------- | ------------ |
| id               | UUID / INT | PK           |
| username         | VARCHAR    | UQ           |
| email            | VARCHAR    | UQ           |
| password_hash    | VARCHAR    | -            |
| total_xp         | INT        | -            |
| streak_count     | INT        | -            |
| streak_last_date | DATE       | -            |
| best_streak      | INT        | -            |
| created_at       | TIMESTAMP  | -            |
| updated_at       | TIMESTAMP  | -            |

---

### Task

| Attribute    | Datentyp   | Schlüsseltyp                |
| ------------ | ---------- | --------------------------- |
| id           | UUID / INT | PK                          |
| user_id      | UUID / INT | FK → User.id                |
| category_id  | UUID / INT | FK → Category.id (nullable) |
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

| Attribute      | Datentyp   | Schlüsseltyp           |
| -------------- | ---------- | ---------------------- |
| id             | UUID / INT | PK                     |
| user_id        | UUID / INT | FK → User.id (UQ, 1:1) |
| tasks_done     | INT        | -                      |
| tasks_open     | INT        | -                      |
| total_time_min | INT        | -                      |
| last_active_at | TIMESTAMP  | -                      |
| updated_at     | TIMESTAMP  | -                      |

---

### Streak

| Attribute  | Datentyp   | Schlüsseltyp |
| ---------- | ---------- | ------------ |
| id         | UUID / INT | PK           |
| user_id    | UUID / INT | FK → User.id |
| count      | INT        | -            |
| best_count | INT        | -            |
| last_date  | DATE       | -            |
| created_at | TIMESTAMP  | -            |
| updated_at | TIMESTAMP  | -            |

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

### Badge

| Attribute   | Datentyp   | Schlüsseltyp |
| ----------- | ---------- | ------------ |
| id          | UUID / INT | PK           |
| name        | VARCHAR    | UQ           |
| description | TEXT       | -            |
| rule_type   | VARCHAR    | -            |
| rule_value  | INT        | -            |
| created_at  | TIMESTAMP  | -            |

---
