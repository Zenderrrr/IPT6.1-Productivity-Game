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
| ---- | ---- | ---- | ---- |
| + | Id | Int | Primäreschlüssel |
| + | Username | String | Benutzername |
| + | Email | String | E-Mail |
| + | PasswordHash | String | Passwort-Hash |
| + | CreatedAt | DateTime | Erstellungsdatum |
| + | UpdatedAt | DateTime | Letzte Änderung |

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

## Repositories

### IRepository<T>

#### Typ
`<<interface>>`

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | GetById(int id) | T? | Lädt einen Datensatz anhand seiner ID |
| + | Insert(T entity) | int | Speichert einen neuen Datensatz und gibt die erzeugte ID zurück |
| + | Update(T entity) | void | Aktualisiert einen bestehenden Datensatz |
| + | Delete(int id) | void | Löscht einen Datensatz anhand seiner ID |

#### Beziehungen
- TaskRepository ---> IRepository
- XPEventRepository ---> IRepository
- UserStatsRepository ---> IRepository

---

### TaskRepository

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _dbConnection | DatabaseConnection | Verwaltet den Zugriff auf die SQLite-Datenbank |
| - | _tableName | string | Speichert den Namen der Tabelle (`Task`) |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | TaskRepository(DatabaseConnection dbConnection) | Initialisiert das Repository mit einer Datenbankverbindung |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | -------------------------------------------- | ----------- | -------------------------- |
| + | GetById(int id) | Task? | Lädt eine Task anhand ihrer ID |
| + | GetAllByUserId(int userId) | List<Task> | Lädt alle Tasks eines Benutzers |
| + | GetOpenByUserId(int userId) | List<Task> | Lädt alle offenen Tasks eines Benutzers |
| + | GetCompletedByUserId(int userId) | List<Task> | Lädt alle erledigten Tasks eines Benutzers |
| + | Insert(Task task) | int | Speichert eine neue Task und gibt die erzeugte ID zurück |
| + | Update(Task task) | void | Aktualisiert eine bestehende Task |
| + | Delete(int id) | void | Löscht eine Task anhand ihrer ID |
| + | Exists(int id) | bool | Prüft, ob eine Task mit dieser ID existiert |
| + | UpdateStatus(int taskId, string status, DateTime? completedAt) | void | Aktualisiert Status und Abschlusszeitpunkt einer Task |
| + | CountOpenTasks(int userId) | int | Zählt alle offenen Tasks eines Benutzers |
| + | CountCompletedTasks(int userId) | int | Zählt alle erledigten Tasks eines Benutzers |

#### Beziehungen
- TaskRepository 1 -- 1 DatabaseConnection
- TaskRepository 1 -- N Task

---

### XPEventRepository

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _dbConnection | DatabaseConnection | Verwaltet den Zugriff auf die SQLite-Datenbank |
| - | _tableName | string | Speichert den Namen der Tabelle (`XPEvent`) |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | XPEventRepository(DatabaseConnection dbConnection) | Initialisiert das Repository mit einer Datenbankverbindung |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | -------------------------------------------- | ----------- | -------------------------- |
| + | GetById(int id) | XPEvent? | Lädt ein XPEvent anhand seiner ID |
| + | GetAllByUserId(int userId) | List<XPEvent> | Lädt alle XPEvents eines Benutzers |
| + | GetAllByTaskId(int taskId) | List<XPEvent> | Lädt alle XPEvents einer bestimmten Task |
| + | Insert(XPEvent xpEvent) | int | Speichert ein neues XPEvent und gibt die erzeugte ID zurück |
| + | Delete(int id) | void | Löscht ein XPEvent anhand seiner ID |
| + | GetTotalXpByUserId(int userId) | int | Berechnet die Gesamt-XP eines Benutzers |
| + | ExistsForTask(int taskId, string reason) | bool | Prüft, ob für eine Task bereits ein XPEvent mit gleichem Grund existiert |
| + | GetRecentByUserId(int userId, int limit) | List<XPEvent> | Lädt die letzten XPEvents eines Benutzers |

#### Beziehungen
- XPEventRepository 1 -- 1 DatabaseConnection
- XPEventRepository 1 -- N XPEvent

---

### UserStatsRepository

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _dbConnection | DatabaseConnection | Verwaltet den Zugriff auf die SQLite-Datenbank |
| - | _tableName | string | Speichert den Namen der Tabelle (`UserStats`) |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | UserStatsRepository(DatabaseConnection dbConnection) | Initialisiert das Repository mit einer Datenbankverbindung |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | -------------------------------------------- | ----------- | -------------------------- |
| + | GetById(int id) | UserStats? | Lädt UserStats anhand der ID |
| + | GetByUserId(int userId) | UserStats? | Lädt die Statistiken eines bestimmten Benutzers |
| + | Insert(UserStats stats) | int | Speichert neue UserStats und gibt die erzeugte ID zurück |
| + | Update(UserStats stats) | void | Aktualisiert bestehende UserStats |
| + | ExistsByUserId(int userId) | bool | Prüft, ob für einen Benutzer bereits UserStats existieren |
| + | UpdateTotalXp(int userId, int totalXp) | void | Aktualisiert die Gesamt-XP eines Benutzers |
| + | UpdateStreak(int userId, int streakCount, int bestStreak, DateTime? streakLastDate) | void | Aktualisiert die Streak-Daten eines Benutzers |
| + | UpdateTaskCounters(int userId, int tasksDone, int tasksOpen) | void | Aktualisiert die Anzahl offener und erledigter Tasks |
| + | UpdateLastActive(int userId, DateTime lastActiveAt) | void | Aktualisiert den Zeitpunkt der letzten Aktivität |
| + | DeleteByUserId(int userId) | void | Löscht die UserStats eines Benutzers |

#### Beziehungen
- UserStatsRepository 1 -- 1 DatabaseConnection
- UserStatsRepository 1 -- 1 UserStats 

---
