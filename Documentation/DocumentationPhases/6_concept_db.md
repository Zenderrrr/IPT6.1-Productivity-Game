# Entity-Relationsship Diagram

## Entitäten mit Attributen & PK/FK

### User

| Attribute     | Datentyp     | Schlüsseltyp |
| ------------- | ------------ | ------------ |
| id            | UUID / INT   | PK           |
| username      | VARCHAR(50)  | UQ           |
| email         | VARCHAR(50)  | UQ           |
| password_hash | VARCHAR(255) | -            |
| created_at    | TIMESTAMP    | -            |
| updated_at    | TIMESTAMP    | -            |

---

### Task

| Attribute    | Datentyp     | Schlüsseltyp     |
| ------------ | ------------ | ---------------- |
| id           | UUID / INT   | PK               |
| user_id      | UUID / INT   | FK → User.id     |
| category_id  | INT          | FK → Category.id |
| title        | VARCHAR(50)  | -                |
| description  | VARCHAR(255) | -                |
| difficulty   | VARCHAR(10)  | -                |
| duration_min | INT          | -                |
| due_date     | DATE         | -                |
| status       | VARCHAR(20)  | -                |
| completed_at | DATE         | -                |
| created_at   | DATE         | -                |
| updated_at   | DATE         | -                |

---

### TaskLog

| Attribute  | Datentyp     | Schlüsseltyp |
| ---------- | ------------ | ------------ |
| id         | UUID / INT   | PK           |
| task_id    | UUID / INT   | FK → Task.id |
| action     | VARCHAR(255) | -            |
| xp_awarded | INT          | -            |
| created_at | DATE         | -            |

---

### XPEvent

| Attribute  | Datentyp    | Schlüsseltyp            |
| ---------- | ----------- | ----------------------- |
| id         | UUID / INT  | PK                      |
| user_id    | UUID / INT  | FK → User.id            |
| task_id    | UUID / INT  | FK → Task.id (nullable) |
| reason     | VARCHAR(20) | -                       |
| amount     | INT         | -                       |
| created_at | DATE        | -                       |

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
| last_active_at   | DATE       | -                 |
| created_at       | DATE       | -                 |
| updated_at       | DATE       | -                 |

---

### Category

| Attribute  | Datentyp    | Schlüsseltyp |
| ---------- | ----------- | ------------ |
| id         | UUID / INT  | PK           |
| user_id    | UUID / INT  | FK → User.id |
| name       | VARCHAR(50) | -            |
| color      | VARCHAR(7)  | -            |
| created_at | DATE        | -            |

---

### UserBadge

| Attribute  | Datentyp   | Schlüsseltyp  |
| ---------- | ---------- | ------------- |
| id         | UUID / INT | PK            |
| user_id    | UUID / INT | FK → User.id  |
| badge_id   | UUID / INT | FK → Badge.id |
| awarded_at | DATE       | -             |

---

### Badge

| Attribute   | Datentyp     | Schlüsseltyp |
| ----------- | ------------ | ------------ |
| id          | UUID / INT   | PK           |
| name        | VARCHAR(50)  | UQ           |
| description | VARCHAR(255) | -            |
| rule_type   | VARCHAR(50)  | -            |
| rule_value  | INT          | -            |
| created_at  | DATE         | -            |

---
