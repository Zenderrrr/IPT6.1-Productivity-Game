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

## Services (Business-Logik)

### TaskService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _taskRepository | TaskRepository | Repository für Task-Datenzugriffe |
| - | _userStatsRepository | UserStatsRepository | Repository für Statistikdaten, z.B. offene/erledigte Tasks |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | TaskService(TaskRepository taskRepository, UserStatsRepository userStatsRepository) | Initialisiert den Service mit den benötigten Repositories |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | GetTaskById(int id) | Task? | Lädt eine Task anhand ihrer ID |
| + | GetTasksByUserId(int userId) | List<Task> | Lädt alle Tasks eines Benutzers |
| + | CreateTask(Task task) | int | Validiert und erstellt eine neue Task |
| + | UpdateTask(Task task) | void | Validiert und aktualisiert eine bestehende Task |
| + | DeleteTask(int id) | void | Löscht eine Task |
| + | GetOpenTasksByUserId(int userId) | List<Task> | Lädt alle offenen Tasks eines Benutzers |
| + | GetCompletedTasksByUserId(int userId) | List<Task> | Lädt alle erledigten Tasks eines Benutzers |

#### Beziehungen
- TaskService 1 -- 1 TaskRepository
- TaskService 1 -- 1 UserStatsRepository
- TaskService 1 -- N Task

---

### TaskCompletionService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _taskRepository | TaskRepository | Zugriff auf Task-Daten |
| - | _xpService | XPService | Berechnet und vergibt XP |
| - | _streakService | StreakService | Aktualisiert Streaks |
| - | _levelService | LevelService | Berechnet das aktuelle Level |
| - | _badgeService | BadgeService | Prüft und vergibt Badges |
| - | _userStatsRepository | UserStatsRepository | Aktualisiert UserStats |
| - | _xpEventRepository | XPEventRepository | Speichert XPEvents |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | TaskCompletionService(TaskRepository taskRepository, XPService xpService, StreakService streakService, LevelService levelService, BadgeService badgeService, UserStatsRepository userStatsRepository, XPEventRepository xpEventRepository) | Initialisiert den Orchestrierungs-Service |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CompleteTask(int taskId, int userId) | void | Markiert eine Task als erledigt und führt alle Folgeaktionen aus |
| + | CanCompleteTask(int taskId, int userId) | bool | Prüft, ob die Task gültig abgeschlossen werden kann |
| + | IsAlreadyCompleted(int taskId) | bool | Prüft, ob eine Task bereits erledigt ist |

#### Beziehungen
- TaskCompletionService 1 -- 1 TaskRepository
- TaskCompletionService 1 -- 1 XPService
- TaskCompletionService 1 -- 1 StreakService
- TaskCompletionService 1 -- 1 LevelService
- TaskCompletionService 1 -- 1 BadgeService
- TaskCompletionService 1 -- 1 UserStatsRepository
- TaskCompletionService 1 -- 1 XPEventRepository

---

### XPService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _xpEventRepository | XPEventRepository | Speichert XP-Verlauf |
| - | _userStatsRepository | UserStatsRepository | Aktualisiert Gesamt-XP |
| - | _xpStrategy | IXpCalculationStrategy | Berechnet XP anhand der aktuellen Regel |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | XPService(XPEventRepository xpEventRepository, UserStatsRepository userStatsRepository, IXpCalculationStrategy xpStrategy) | Initialisiert den XP-Service mit Repository und Strategy |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateXP(Task task, int streakCount) | int | Berechnet XP für eine Task |
| + | AwardXP(int userId, Task task, int streakCount, string reason) | int | Berechnet und vergibt XP an einen Benutzer |
| + | GetTotalXP(int userId) | int | Gibt die Gesamt-XP eines Benutzers zurück |
| + | HasXPEventForTask(int taskId, string reason) | bool | Prüft, ob für diese Task bereits XP vergeben wurden |

#### Beziehungen
- XPService 1 -- 1 XPEventRepository
- XPService 1 -- 1 UserStatsRepository
- XPService 1 -- 1 IXpCalculationStrategy
- XPService 1 -- N XPEvent

---

### StreakService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _userStatsRepository | UserStatsRepository | Zugriff auf UserStats |
| - | _streakStrategy | IStreakRuleStrategy | Enthält die fachliche Streak-Regel |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | StreakService(UserStatsRepository userStatsRepository, IStreakRuleStrategy streakStrategy) | Initialisiert den Streak-Service |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | UpdateStreak(int userId, DateTime completedAt) | void | Aktualisiert die Streak nach einem Task-Abschluss |
| + | ResetStreakIfNeeded(int userId, DateTime today) | void | Setzt die Streak zurück, falls eine Lücke entstanden ist |
| + | GetCurrentStreak(int userId) | int | Gibt die aktuelle Streak zurück |
| + | GetBestStreak(int userId) | int | Gibt die beste bisherige Streak zurück |

#### Beziehungen
- StreakService 1 -- 1 UserStatsRepository
- StreakService 1 -- 1 IStreakRuleStrategy
- StreakService 1 -- 1 UserStats

---

### LevelService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _levelStrategy | ILevelStrategy | Enthält die Formel für Level und Progress |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | LevelService(ILevelStrategy levelStrategy) | Initialisiert den Level-Service mit einer Level-Strategie |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | GetLevel(int totalXP) | int | Berechnet das aktuelle Level |
| + | GetProgressToNextLevel(int totalXP) | double | Berechnet den Fortschritt zum nächsten Level |
| + | GetXpForNextLevel(int totalXP) | int | Berechnet die noch fehlenden XP bis zum nächsten Level |

#### Beziehungen
- LevelService 1 -- 1 ILevelStrategy

---

### BadgeService

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _userStatsRepository | UserStatsRepository | Liefert Statistikdaten für Badge-Prüfungen |
| - | _badgeRepository | BadgeRepository | Lädt definierte Badges |
| - | _userBadgeRepository | UserBadgeRepository | Speichert vergebene Badges |
| - | _badgeRules | List<IBadgeRule> | Liste aller Badge-Regeln |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | BadgeService(UserStatsRepository userStatsRepository, BadgeRepository badgeRepository, UserBadgeRepository userBadgeRepository, List<IBadgeRule> badgeRules) | Initialisiert den Badge-Service |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CheckAndAwardBadges(int userId) | List<Badge> | Prüft alle Regeln und vergibt neue Badges |
| + | HasBadge(int userId, int badgeId) | bool | Prüft, ob ein Benutzer ein Badge bereits besitzt |
| + | GetUnlockedBadges(int userId) | List<Badge> | Lädt alle bereits freigeschalteten Badges |
| + | GetAvailableBadges() | List<Badge> | Lädt alle definierten Badges |

#### Beziehungen
- BadgeService 1 -- 1 UserStatsRepository
- BadgeService 1 -- 1 BadgeRepository
- BadgeService 1 -- 1 UserBadgeRepository
- BadgeService 1 -- N IBadgeRule
- BadgeService 1 -- N Badge

---

## Strategies

### IXpCalculationStrategy

#### Typ
`<<interface>>`

#### Zweck
Definiert die Berechnungsregel für XP. Dadurch kann die XP-Formel ausgetauscht werden, ohne den `XPService` zu ändern.

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateXP(Task task, int streakCount) | int | Berechnet die XP für eine abgeschlossene Task anhand von Schwierigkeit, Dauer und Streak |
| + | GetBonusMultiplier(int streakCount) | double | Gibt den Bonus-Multiplikator für die aktuelle Streak zurück |

#### Beziehungen
- XPService 1 -- 1 IXpCalculationStrategy

---

### IStreakRuleStrategy

#### Typ
`<<interface>>`

#### Zweck
Definiert die Regel, wie eine Streak aktualisiert oder zurückgesetzt wird. Dadurch können verschiedene Streak-Systeme umgesetzt werden.

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateNewStreak(UserStats stats, DateTime completedAt) | int | Berechnet den neuen Streak-Wert nach einem Task-Abschluss |
| + | ShouldResetStreak(UserStats stats, DateTime today) | bool | Prüft, ob die Streak wegen einer Unterbrechung zurückgesetzt werden muss |

#### Beziehungen
- StreakService 1 -- 1 IStreakRuleStrategy

---

### ILevelStrategy

#### Typ
`<<interface>>`

#### Zweck
Definiert die Formel zur Berechnung von Level und Fortschritt. Dadurch kann die Level-Kurve flexibel ausgetauscht werden.

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateLevel(int totalXP) | int | Berechnet das aktuelle Level anhand der Gesamt-XP |
| + | CalculateProgressToNextLevel(int totalXP) | double | Berechnet den Fortschritt zum nächsten Level als Wert zwischen 0 und 1 |

#### Beziehungen
- LevelService 1 -- 1 ILevelStrategy

---

### IBadgeRule

#### Typ
`<<interface>>`

#### Zweck
Definiert eine einzelne Badge-Regel. Dadurch kann jede Badge-Bedingung als eigene Regel implementiert werden.

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | IsUnlocked(UserStats stats, Badge badge) | bool | Prüft, ob ein Badge anhand der UserStats freigeschaltet werden soll |
| + | GetRuleType() | string | Gibt den Typ der Regel zurück, z.B. `streak`, `xp`, `tasks_done` |

#### Beziehungen
- BadgeService 1 -- N IBadgeRule

---

## Strategy-Implementierungen

### DefaultXpCalculationStrategy

#### Typ
`implements IXpCalculationStrategy`

#### Zweck
Standard-Implementierung für die XP-Berechnung basierend auf Schwierigkeit, Dauer und Streak-Bonus.

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _streakBonusFactor | double | Prozentualer Bonus pro Streak-Tag |
| - | _temporaryBonusFactor | double | Zusätzlicher Bonusfaktor, z.B. für spezielle Belohnungen |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | DefaultXpCalculationStrategy() | Initialisiert die Standardwerte für die XP-Berechnung |
| + | DefaultXpCalculationStrategy(double streakBonusFactor, double temporaryBonusFactor) | Initialisiert benutzerdefinierte Bonusfaktoren |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateXP(Task task, int streakCount) | int | Berechnet die XP einer Task anhand der Standardformel |
| + | GetBonusMultiplier(int streakCount) | double | Berechnet den Bonus-Multiplikator für die aktuelle Streak |

#### Beziehungen
- DefaultXpCalculationStrategy 1 -- 1 IXpCalculationStrategy
- XPService 1 -- 1 DefaultXpCalculationStrategy

---

### DefaultStreakRuleStrategy

#### Typ
`implements IStreakRuleStrategy`

#### Zweck
Standard-Implementierung für das Streak-System: mindestens 1 erledigte Task pro Tag erhöht die Streak.

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _maxGapDays | int | Anzahl erlaubter Lückentage, bevor die Streak zurückgesetzt wird |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | DefaultStreakRuleStrategy() | Initialisiert die Standardregel mit täglicher Aktivität |
| + | DefaultStreakRuleStrategy(int maxGapDays) | Initialisiert eine benutzerdefinierte Lückenregel |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateNewStreak(UserStats stats, DateTime completedAt) | int | Berechnet die neue Streak anhand des letzten Aktivitätstags |
| + | ShouldResetStreak(UserStats stats, DateTime today) | bool | Prüft, ob die Streak wegen einer Lücke zurückgesetzt werden muss |

#### Beziehungen
- DefaultStreakRuleStrategy 1 -- 1 IStreakRuleStrategy
- StreakService 1 -- 1 DefaultStreakRuleStrategy

---

### QuadraticLevelStrategy

#### Typ
`implements ILevelStrategy`

#### Zweck
Standard-Implementierung für das Level-System mit quadratisch steigender XP-Kurve.

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _baseXp | int | Basiswert für die quadratische Level-Berechnung |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | QuadraticLevelStrategy() | Initialisiert die Standard-Levelkurve |
| + | QuadraticLevelStrategy(int baseXp) | Initialisiert die Levelkurve mit benutzerdefiniertem Basiswert |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | CalculateLevel(int totalXP) | int | Berechnet das Level aus den Gesamt-XP |
| + | CalculateProgressToNextLevel(int totalXP) | double | Berechnet den Fortschritt zum nächsten Level |
| + | GetXpThresholdForLevel(int level) | int | Berechnet die XP-Grenze für ein bestimmtes Level |

#### Beziehungen
- QuadraticLevelStrategy 1 -- 1 ILevelStrategy
- LevelService 1 -- 1 QuadraticLevelStrategy

---

### StreakBadgeRule

#### Typ
`implements IBadgeRule`

#### Zweck
Konkrete Badge-Regel, die ein Badge freischaltet, wenn eine bestimmte Streak erreicht wurde.

#### Attribute
| Sichtbarkeit | Name | Typ | Beschreibung |
| ---- | ---- | ---- | ---- |
| - | _requiredStreak | int | Benötigte Streak für die Badge-Freischaltung |
| - | _ruleType | string | Typ der Regel, standardmässig `streak` |

#### Konstruktoren
| Sichtbarkeit | Definition | Beschreibung |
| --- | --- | --- |
| + | StreakBadgeRule() | Initialisiert die Standard-Streak-Regel |
| + | StreakBadgeRule(int requiredStreak) | Initialisiert die Regel mit benutzerdefiniertem Grenzwert |

#### Methoden
| Sichtbarkeit | Definition | Rückgabetyp | Beschreibung |
| ------------ | ---------- | ----------- | ------------ |
| + | IsUnlocked(UserStats stats, Badge badge) | bool | Prüft, ob die aktuelle Streak des Users für das Badge ausreicht |
| + | GetRuleType() | string | Gibt den Regeltyp `streak` zurück |

#### Beziehungen
- StreakBadgeRule 1 -- 1 IBadgeRule
- BadgeService 1 -- N StreakBadgeRule

---
