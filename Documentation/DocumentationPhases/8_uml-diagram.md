# UML Design / Architektur
Wir beschreiben hier das Design des C# Backend's mit den Architektur entscheidungen und dem OOP Design was wir gewählt haben.

## OOP-Design
- Vererbung
- Polymorphismus
- Abstraktion
- Kapselung

## 1) Überblick Klassen mit Verantwortlichkeiten

### Entities (nahe am DB)
Repräsentieren Tabellen/Datensätze in SQLite: `User`, `Task`, `Category`, `TaskLog`, `XPEvent`, `UserStats`, `Badge`, `UserBadge`.

### Repositories (Datenzugriff)
Bei Entities mit CRUD Features werden Repos gebraucht um diese zu Kapseln (z.B. `TaskRepository`, `XPEventRepository`, `UserStatsRepository`).

### Services (Business-Logik)
- `TaskService` (CRUD)
- `TaskCompletionService`
- `XPService`, `StreakService`, `LevelService`, `BadgeService`

### Strategies (Regeln)
- XP-Regeln, Streak-Regeln, Level-Kurven, Badge-Regeln (austauschbare Strategien).

### Infrastruktur
- `DatabaseConnection` (SQLite Verbindung)

| Ebene | Aufgabe|
|-------|--------|
|Repository| Datenbankzugriff|
|Service| Business-Logik|
|Entity| Datenmodell |

## Entities

### User

#### Attribute
| Sicherbarkeit | Name | Typ | Beschreibung |
| --- | --- | --- |
| + | Id | Int | Primäreschlüssel |
| + | Username | String | Benutzername |
| + | Email | String | E-Mail |
| + | PasswordHash | String | Passwort-Hash |
| + | CreatedAt | DateTime | Erstellungsdatum |
| + | UpdatedAt | DateTime | Letzte Änderung |
| ---- | ---- | ---- | ---- |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | User() | Standardkonstruktor |
| + | User(string username, string email, string passwordHash) | Neuen Benutzer erstellen |

#### Methoden
| Sichtbarkeit | Definition                                     | Rückgabetyp | Beschreibung               |
| ------------ | -------------------------------------------- | ----------- | -------------------------- |
| +            | UpdateProfile(string username, string email) | void        | Profildaten ändern         |
| +            | ChangePassword(string newPasswordHash)       | void        | Passwort ändern            |
| +            | ValidateData()                                   | bool        | Prüft Gültigkeit der Daten |
| +            | UpdateDate()                                      | void        | Setzt UpdatedAt neu        |

#### Beziehungen
- User 1 -- N Task
- User 1 -- N Category
- User 1 -- N TaskLog
- User 1 -- N XPEvent
- User 1 -- 1 UserStats
- User 1 -- N UserBadge

---

### Task

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | ----------- | --------- | ------------------------ |
| +            | Id          | int       | Primärschlüssel          |
| +            | UserId      | int       | FK zu User               |
| +            | CategoryId  | int?      | FK zu Category, optional |
| +            | Title       | string    | Titel                    |
| +            | Description | string    | Beschreibung             |
| +            | Difficulty  | int       | Schwierigkeit            |
| +            | DurationMin | int       | Dauer in Minuten         |
| +            | DueDate     | DateTime? | Fälligkeitsdatum         |
| +            | Status      | string    | open / done              |
| +            | CompletedAt | DateTime? | Abschlusszeitpunkt       |
| +            | CreatedAt   | DateTime  | Erstellungsdatum         |
| +            | UpdatedAt   | DateTime  | Letzte Änderung          |

#### Konstruktoren
| Sichtbarkeit | Definition                                                                                                                              | Beschreibung        |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------- | ------------------- |
| +            | Task()                                                                                                                                | Standardkonstruktor |
| +            | Task(int userId, string title, string description, int difficulty, int durationMin, int? categoryId = null, DateTime? dueDate = null) | Erstellt neue Task  |

#### Methoden
| Sichtbarkeit | Definition                                                                                                             | Rückgabetyp | Beschreibung                |
| ------------ | -------------------------------------------------------------------------------------------------------------------- | ----------- | --------------------------- |
| +            | MarkAsCompleted()                                                                                                    | void        | Markiert Task als erledigt  |
| +            | Reopen()                                                                                                             | void        | Setzt Task wieder auf offen |
| +            | UpdateDetails(string title, string description, int difficulty, int durationMin, int? categoryId, DateTime? dueDate) | void        | Bearbeitet Task             |
| +            | IsCompleted()                                                                                                        | bool        | Prüft, ob Task erledigt ist |
| +            | ValidateData()                                                                                                           | bool        | Validiert Taskdaten         |
| +            | UpdateDate()                                                                                                              | void        | Setzt UpdatedAt neu         |

#### Beziehungen
- Task N -- 1 User
- Task N -- C Category
- Task 1 -- N TaskLog
- Task 1 -- N XPEvent

---

### Category

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | --------- | -------- | ---------------- |
| +            | Id        | int      | Primärschlüssel  |
| +            | UserId    | int      | FK zu User       |
| +            | Name      | string   | Kategoriename    |
| +            | Color     | string   | Farbe            |
| +            | CreatedAt | DateTime | Erstellungsdatum |

#### Konstruktoren
| Sichtbarkeit | Definition                                        | Beschreibung            |
| ------------ | ----------------------------------------------- | ----------------------- |
| +            | Category()                                      | Standardkonstruktor     |
| +            | Category(int userId, string name, string color) | Erstellt neue Kategorie |

#### Methoden
| Sichtbarkeit | Definition                     | Rückgabetyp | Beschreibung         |
| ------------ | ---------------------------- | ----------- | -------------------- |
| +            | Rename(string newName)       | void        | Kategorie umbenennen |
| +            | ChangeColor(string newColor) | void        | Farbe ändern         |
| +            | ValidateData()                   | bool        | Kategorie prüfen     |

#### Beziehungen
- Category N -- 1 User
- Category 1 -- N Task

---

### TaskLog

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | --------- | -------- | ---------------------- |
| +            | Id        | int      | Primärschlüssel        |
| +            | UserId    | int      | FK zu User             |
| +            | TaskId    | int      | FK zu Task             |
| +            | Action    | string   | Aktion, z.B. completed |
| +            | XpAwarded | int      | Vergebene XP           |
| +            | CreatedAt | DateTime | Zeitpunkt              |

#### Konstruktoren
| Sichtbarkeit | Definition                                                      | Beschreibung        |
| ------------ | ------------------------------------------------------------- | ------------------- |
| +            | TaskLog()                                                     | Standardkonstruktor |
| +            | TaskLog(int userId, int taskId, string action, int xpAwarded) | Erstellt Logeintrag |

#### Methoden
| Sichtbarkeit | Definition   | Rückgabetyp | Beschreibung   |
| ------------ | ---------- | ----------- | -------------- |
| +            | ValidateData() | bool        | Prüft Logdaten |

#### Beziehungen
- TaskLog N -- 1 User
- TaskLog N -- 1 Task

---

### XpEvent

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | --------- | -------- | -------------------- |
| +            | Id        | int      | Primärschlüssel      |
| +            | UserId    | int      | FK zu User           |
| +            | TaskId    | int?     | FK zu Task, optional |
| +            | Amount    | int      | XP-Menge             |
| +            | Reason    | string   | Grund                |
| +            | CreatedAt | DateTime | Zeitpunkt            |


#### Konstruktoren
| Sichtbarkeit | Definition                                                           | Beschreibung        |
| ------------ | ------------------------------------------------------------------ | ------------------- |
| +            | XPEvent()                                                          | Standardkonstruktor |
| +            | XPEvent(int userId, int amount, string reason, int? taskId = null) | Erstellt XP-Event   |

#### Methoden
| Sichtbarkeit | Definition   | Rückgabetyp | Beschreibung     |
| ------------ | ---------- | ----------- | ---------------- |
| +            | Validate() | bool        | Prüft Eventdaten |

#### Beziehungen
- XPEvent N -- 1 User
- XPEvent N -- C Task

---

### UserStats

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | -------------- | --------- | ------------------ |
| +            | Id             | int       | Primärschlüssel    |
| +            | UserId         | int       | FK zu User         |
| +            | TotalXP        | int       | Gesamt-XP          |
| +            | TasksDone      | int       | Erledigte Tasks    |
| +            | TasksOpen      | int       | Offene Tasks       |
| +            | TotalTimeMin   | int       | Gesamte Zeit       |
| +            | StreakCount    | int       | Aktuelle Streak    |
| +            | BestStreak     | int       | Beste Streak       |
| +            | StreakLastDate | DateTime? | Letzter Streak-Tag |
| +            | LastActiveAt   | DateTime? | Letzte Aktivität   |
| +            | CreatedAt      | DateTime  | Erstellungsdatum   |
| +            | UpdatedAt      | DateTime  | Letzte Änderung    |

#### Konstruktoren
| Sichtbarkeit | Definition              | Beschreibung            |
| ------------ | --------------------- | ----------------------- |
| +            | UserStats()           | Standardkonstruktor     |
| +            | UserStats(int userId) | Erstellt Stats für User |

#### Methoden
| Sichtbarkeit | Definition                             | Rückgabetyp | Beschreibung             |
| ------------ | ------------------------------------ | ----------- | ------------------------ |
| +            | AddXP(int amount)                    | void        | Addiert XP               |
| +            | IncrementTasksDone()                 | void        | Erhöht erledigte Tasks   |
| +            | DecrementTasksOpen()                 | void        | Verringert offene Tasks  |
| +            | AddDuration(int durationMin)         | void        | Addiert bearbeitete Zeit |
| +            | UpdateStreak(DateTime today)         | void        | Aktualisiert Streak      |
| +            | ResetStreak()                        | void        | Setzt Streak zurück      |
| +            | UpdateLastActive(DateTime timestamp) | void        | Aktualisiert Aktivität   |
| +            | ValidateData()                           | bool        | Prüft Stats              |
| +            | UpdateDate()                              | void        | Setzt UpdatedAt          |

#### Beziehungen
- UserStats 1 -- 1 User

---

### Badge

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | ----------- | -------- | ---------------- |
| +            | Id          | int      | Primärschlüssel  |
| +            | Name        | string   | Name             |
| +            | Description | string   | Beschreibung     |
| +            | RuleType    | string   | Regeltyp         |
| +            | RuleValue   | int      | Regelwert        |
| +            | CreatedAt   | DateTime | Erstellungsdatum |

#### Konstruktoren
| Sichtbarkeit | Definition                                                               | Beschreibung        |
| ------------ | ---------------------------------------------------------------------- | ------------------- |
| +            | Badge()                                                                | Standardkonstruktor |
| +            | Badge(string name, string description, string ruleType, int ruleValue) | Erstellt Badge      |

#### Methoden
| Sichtbarkeit | Signatur                                   | Rückgabetyp | Beschreibung       |
| ------------ | ------------------------------------------ | ----------- | ------------------ |
| +            | UpdateRule(string ruleType, int ruleValue) | void        | Badge-Regel ändern |
| +            | ValidateData()                                 | bool        | Prüft Badge-Daten  |

#### Beziehungen
- Badge 1 -- N UserBadge

---

### UserBadge

#### Attribute
| Sichtbarkeit | Name        | Typ  | Beschreibung             |
| ------------ | --------- | -------- | ---------------- |
| +            | Id        | int      | Primärschlüssel  |
| +            | UserId    | int      | FK zu User       |
| +            | BadgeId   | int      | FK zu Badge      |
| +            | AwardedAt | DateTime | Übergabe Datum |

#### Konstruktoren
| Sichtbarkeit | Definition                                               | Beschreibung        |
| ------------ | ------------------------------------------------------ | ------------------- |
| +            | UserBadge()                                            | Standardkonstruktor |
| +            | UserBadge(int userId, int badgeId, DateTime awardedAt) | Erstellt UserBadge  |

#### Methoden
| Sichtbarkeit | Definition   | Rückgabetyp | Beschreibung          |
| ------------ | ---------- | ----------- | --------------------- |
| +            | ValidateData() | bool        | Prüft UserBadge-Daten |

#### Beziehungen
- UserBadge N -- 1 User
- UserBadge N -- 1 Badge