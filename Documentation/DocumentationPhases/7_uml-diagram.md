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

### Services (Logik)
- `TaskService` (CRUD)
- `TaskCompletionService`
- `XPService`, `StreakService`, `LevelService`, `BadgeService`

### Strategies (Regeln)
- XP-Regeln, Streak-Regeln, Level-Kurven, Badge-Regeln (austauschbare Strategien).

### Infrastruktur
- `DatabaseConnection` (SQLite Verbindung)


