# Backend Testing Protokoll

**Testframework:** xUnit  
**Zusätzliche Libraries:** FluentAssertions, Moq  
**Datenbank:** SQLite  
**Test-Ausführung:** `dotnet test FocusUp.Tests`  
**Datum:** 20.05.2026  
**Tester:** Egor

---

# Zusammenfassung

Das Backend wurde mit automatisierten Unit- und Integrationstests getestet.

Getestet wurden:

- Services
- Models
- Repositories
- Controller
- SQLite-Datenbankintegration
- Fehlerbehandlung
- Berechtigungen
- XP-, Badge- und Streak-System

Die wichtigsten Business-Logiken sowie Fehlerfälle wurden überprüft.

---

# Unit Tests

## Getestete Bereiche

- LevelService
- StreakService
- XPService
- BadgeService
- TaskModel

---

# Integration Tests

## Getestete Bereiche

- TaskCompletionService
- TaskService
- XPService
- Repository-Tests
- TasksController

---

# Testergebnisse

| Nr. | Bereich               | Testfall                    | Erwartet               | Ergebnis | Status |
| --- | --------------------- | --------------------------- | ---------------------- | -------- | ------ |
| 1   | LevelService          | 0 XP -> Level 1             | Level 1                | korrekt  | OK     |
| 2   | LevelService          | 100 XP -> Level 2           | Level 2                | korrekt  | OK     |
| 3   | LevelService          | 400 XP -> Level 3           | Level 3                | korrekt  | OK     |
| 4   | LevelService          | Progress zum nächsten Level | korrekter Wert         | korrekt  | OK     |
| 5   | StreakService         | erster Abschluss            | Streak = 1             | korrekt  | OK     |
| 6   | StreakService         | gleicher Tag                | keine Erhöhung         | korrekt  | OK     |
| 7   | StreakService         | Folgetag                    | +1 Streak              | korrekt  | OK     |
| 8   | StreakService         | Lücke > 1 Tag               | Reset                  | korrekt  | OK     |
| 9   | XPService             | Easy Task XP                | korrekte XP            | korrekt  | OK     |
| 10  | XPService             | Medium Task XP              | korrekte XP            | korrekt  | OK     |
| 11  | XPService             | Hard Task XP                | korrekte XP            | korrekt  | OK     |
| 12  | XPService             | Dauer beeinflusst XP        | höhere XP              | korrekt  | OK     |
| 13  | XPService             | Streak Bonus                | Bonus angewendet       | korrekt  | OK     |
| 14  | BadgeService          | Badge vorhanden             | true                   | korrekt  | OK     |
| 15  | BadgeService          | Badge nicht vorhanden       | false                  | korrekt  | OK     |
| 16  | TaskModel             | gültige Task                | akzeptiert             | korrekt  | OK     |
| 17  | TaskModel             | leerer Titel                | abgelehnt              | korrekt  | OK     |
| 18  | TaskModel             | MarkAsCompleted()           | Status Completed       | korrekt  | OK     |
| 19  | Repository            | Task Insert                 | gespeichert            | korrekt  | OK     |
| 20  | Repository            | GetById()                   | korrektes Objekt       | korrekt  | OK     |
| 21  | Repository            | Update()                    | Änderungen gespeichert | korrekt  | OK     |
| 22  | Repository            | Delete()                    | Objekt gelöscht        | korrekt  | OK     |
| 23  | Repository            | Cascade Verhalten           | korrekt                | korrekt  | OK     |
| 24  | TaskService           | gültige Task erstellen      | gespeichert            | korrekt  | OK     |
| 25  | TaskService           | Task löschen                | gelöscht               | korrekt  | OK     |
| 26  | TaskService           | Tasks nach User laden       | nur eigene Tasks       | korrekt  | OK     |
| 27  | XPService             | AwardXP() erstellt XPEvent  | Event vorhanden        | korrekt  | OK     |
| 28  | XPService             | AwardXP() erhöht total_xp   | XP erhöht              | korrekt  | OK     |
| 29  | TaskCompletionService | Task abschließen            | Status Completed       | korrekt  | OK     |
| 30  | TaskCompletionService | doppelte Completion         | Exception              | korrekt  | OK     |
| 31  | TaskCompletionService | falscher User               | Forbidden/Exception    | korrekt  | OK     |
| 32  | TaskCompletionService | XP nur einmal vergeben      | 1 XPEvent              | korrekt  | OK     |
| 33  | TaskCompletionService | Streak Update               | korrekt erhöht         | korrekt  | OK     |
| 34  | TaskCompletionService | Badge Check                 | Badge ausgelöst        | korrekt  | OK     |
| 35  | TaskCompletionService | TaskLog erstellt            | vorhanden              | korrekt  | OK     |
| 36  | TasksController       | ohne User Claim             | Unauthorized           | korrekt  | OK     |
| 37  | TasksController       | unbekannte Task             | NotFound               | korrekt  | OK     |
| 38  | TasksController       | fremde Task                 | Forbidden              | korrekt  | OK     |
| 39  | TasksController       | gültige Task laden          | OK + DTO               | korrekt  | OK     |
| 40  | TasksController       | ungültige Difficulty        | BadRequest             | korrekt  | OK     |
| 41  | TasksController       | gültige Task erstellen      | 201 Created            | korrekt  | OK     |
| 42  | TasksController       | Delete unbekannte Task      | NotFound               | korrekt  | OK     |

---

# Testergebnis

Die Tests wurden mit folgendem Befehl ausgeführt:

```bash
dotnet test FocusUp.Tests
```

## Ergebnis

```txt
Test summary:
total: 52, failed: 0, succeeded: 52, skipped: 0
```

Alle Tests wurden erfolgreich ausgeführt.

Die Unit Tests überprüfen einzelne Klassen und Business-Logik isoliert.

Die Integration Tests überprüfen das Zusammenspiel zwischen:

- Services
- Repositories
- Controllern
- SQLite-Datenbank

Dabei wurden Datenbankoperationen, Validierungen, Berechtigungen, Statuscodes und Datenkonsistenz überprüft.

---

# Fazit

Die wichtigsten Backend-Komponenten funktionieren korrekt.

Getestet wurden:

- XP-System
- Badge-System
- Streak-System
- Task-Verwaltung
- Datenbankoperationen
- Controller-Endpunkte
- Fehlerbehandlung

Alle automatisierten Tests wurden erfolgreich bestanden.
