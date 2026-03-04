# Streak aktualisieren

## Ziel

- Motivation durch tägliche Aktivität
- Streak darf nur einmal pro Tag steigen
- Streak wird zurückgesetzt, wenn ein Tag ausgelassen wird

---

## Definition

**Streak = Anzahl Tage in Folge**, an denen mindestens 1 Task abgeschlossen wurde.

---

## Datenbasis

Benötigte User-Felder:

- `streakCount` (aktuelle Streak)
- `streakLastDate` (Datum der letzten abgeschlossenen Task, Format: `YYYY-MM-DD`)
- optional: `bestStreak` (höchste erreichte Streak)

---

## Update-Zeitpunkt

Streak wird aktualisiert, wenn:

- Eine Task auf **„erledigt“** gesetzt wird.

---

## Streak-Logik

Vergleich von `today` mit `streakLastDate`.

### 1) Neuer Streak (kein Eintrag vorhanden)

if streakLastDate == null:
streakCount = 1
streakLastDate = today

---

### 2) Bereits heute erledigt

if streakLastDate == today:
keine Änderung

---

### 3) Gestern erledigt (Streak läuft weiter)

if streakLastDate == yesterday:
streakCount += 1
streakLastDate = today

---

### 4) Lücke im Verlauf (Streak gebrochen)

if streakLastDate < yesterday:
streakCount = 1
streakLastDate = today

---

## Best Streak aktualisieren (optional)

if streakCount > bestStreak:
bestStreak = streakCount

---

## Wichtiger Hinweis: Zeitzone

Da Streak tagebasiert ist:

- `streakLastDate` als DATE speichern (ohne Uhrzeit)
- `today` in der User-Zeitzone berechnen
- Nicht direkt UTC-Timestamps vergleichen
