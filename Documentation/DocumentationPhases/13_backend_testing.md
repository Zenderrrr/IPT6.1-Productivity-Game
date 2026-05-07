# Backend Testing Protokoll

Tests werden aufs Backend angewendet um die Sicherstellung aller Funktionalitäten zu gewährleisten, dass bei Erweiterungen nicht Funktionen unendeckt nicht mehr gewollt Funktionieren.

---

# Unit Tests

## Kerntests

LevelService
StreakService
XPService
BadgeService
TaskModel

### LevelService

- 0 XP -> Level 1
- 100 XP -> Level 2
- 400 XP -> Level 3
- Fortschritt zum nächsten Level korrekt
- Grenzwerte direkt vor und nach Level-Up

### StreakService

- erster Abschluss -> Streak = 1
- Abschluss am selben Tag -> keine Erhöhung
- Abschluss am Folgetag -> +1
- Lücke von mehr als 1 Tag -> Reset bzw. Neustart
- best_streak wird korrekt aktualisiert

### XPService

- CalculateXP() mit Easy / Medium / Hard
- CalculateXP() mit verschiedenen Dauern
- CalculateXP() mit Streak-Bonus

### BadgeService

- HasBadge() gibt true zurück wenn Badge vorhanden ist
- HasBadge() gibt false zurück wenn Badge nicht vorhanden ist

### TaskModel

- gültige Task wird akzeptiert
- ungültige Task mit leerem Titel wird abgelehnt
- MarkAsCompleted() setzt Status korrekt auf Completed

---

# Integration Tests

## Bereiche mit Datenbank- oder Repository-Abhängigkeiten

TaskCompletionService
TaskService
Repository-Tests
Controller-Tests

### TaskCompletionService

- Task wird auf Completed gesetzt
- bereits erledigte Task gibt keine doppelte Vergabe
- falscher User darf Task nicht erledigen
- XP werden genau einmal vergeben
- Streak wird korrekt aktualisiert
- Badge-Prüfung wird ausgelöst
- bei Fehlern bleibt der Zustand konsistent

### TaskService

- gültige Task erstellen
- ungültige Task ablehnen
- Update funktioniert
- Delete funktioniert
- nur Tasks des richtigen Users laden

### Repository-Tests

- Insert funktioniert
- GetById liefert korrektes Objekt
- Update speichert korrekt
- Delete löscht korrekt
- Fremdschlüssel / Cascade-Verhalten wie erwartet

### Controller-Tests

- korrekte HTTP-Statuscodes
- Bad Request bei ungültigem Input
- Unauthorized / Forbidden / NotFound korrekt
- erwartete Response-Struktur

# Ergebnisse

Die Tests wurden mit folgendem Befehl ausgeführt:

```bash
dotnet test FocusUp.Tests
```
