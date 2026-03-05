# USE-Cases

## Use-Case Unterteilung

- 1. Core Use-Cases
- 2. Gamification Use-Cases
- 3. Analyse- und Statistik Use-Cases
- 4. System- und Technische Use-Cases

## Core Use-Cases

### Benutzer

#### UC-01: Benutzer registrieren

Akteuer: Benutzer

Beschreibung: Benutzer erstellt ein neues Konto, um die App nutzen zu können.

Vorbedingenen: Benutzer ist nicht eingeloggt.

Hauptablauf:

1. Benutzer öffnet die Registrierungsseite.
2. Benutzer gibt Benutzername / E-Mail und Passwort ein.
3. Benutzer bestätigt die Registrierung.
4. System validiert die Eingaben.
5. System erstellt den Benutzeraccount.
6. System leitet den Benutzer zur Login-Seite oder direkt ins Dashboard weiter.

Alternativablauf (nur falls notwendig):

- A1: E-Mail/Benutzername existiert bereits -> System zeigt Fehlermeldung, Benutzer korrigiert Eingaben.
- A2: Passwort erfüllt Regeln nicht -> System zeigt Fehlermeldung.

Nachbedingungen: Benutzerkonto existiert im System.

#### UC-02: Benutzer einloggen

Akteuer: Benutzer

Beschreibung: Benutzer meldet sich mit seinen Zugangsdaten an.

Vorbedingenen: Benutzerkonto existiert. Benutzer ist nicht eingeloggt.

Hauptablauf:

1. Benutzer öffnet die Login-Seite.
2. Benutzer gibt Login-Daten ein.
3. Benutzer klickt auf „Login“.
4. System prüft die Zugangsdaten.
5. System erstellt eine Session / JWT und loggt den Benutzer ein.
6. System öffnet das Dashboard.

Alternativablauf (nur falls notwendig):

- A1: Falsche Zugangsdaten -> System zeigt Fehlermeldung.
- A2: Account gesperrt / nicht gefunden -> System zeigt Fehlermeldung.

Nachbedingungen: Benutzer ist eingeloggt und kann Tasks verwalten.

### Task

#### UC-03: Task erstellen

Akteuer: Benutzer

Beschreibung: Benutzer erstellt einen neuen Task mit Metadaten (Schwierigkeit, Dauer, Kategorie, Fälligkeitsdatum).

Vorbedingenen: Benutzer ist eingeloggt.

Hauptablauf:

1. Benutzer öffnet „Task erstellen“.
2. Benutzer füllt Pflichtfelder aus und optional weitere Felder.
3. Benutzer speichert den Task.
4. System validiert Eingaben.
5. System speichert Task als „offen“.
6. System zeigt den Task in der Task-Liste an.

Alternativablauf (nur falls notwendig):

- A1: Pflichtfeld fehlt/ungültig -> System zeigt Fehlermeldung, Task wird nicht gespeichert.

Nachbedingungen: Neuer Task existiert und ist in der Liste sichtbar.

#### UC-04: Task bearbeiten

Akteuer: Benutzer

Beschreibung: Benutzer passt einen bestehenden Task an (Titel, Beschreibung, Schwierigkeit, Dauer, Kategorie, Fälligkeitsdatum)

Vorbedingenen: Benutzer ist eingeloggt. Task existiert und gehört dem Benutzer.

Hauptablauf:

1. Benutzer öffnet einen Task.
2. Benutzer klickt „Bearbeiten“.
3. Benutzer ändert Felder.
4. Benutzer speichert.
5. System validiert Eingaben.
6. System speichert Änderungen.
7. System zeigt aktualisierte Task-Daten an.

Alternativablauf (nur falls notwendig):

- A1: Eingaben ungültig -> System zeigt Fehlermeldung.

Nachbedingungen: Task ist aktualisiert gespeichert.

#### UC-05: Task löschen

Akteuer: Benutzer

Beschreibung: Benutzer löscht einen bestehenden Task.

Vorbedingenen: Benutzer ist eingeloggt. Task existiert und gehört dem Benutzer.

Hauptablauf:

1. Benutzer öffnet Task-Detail oder Task-Liste.
2. Benutzer wählt „Löschen“.
3. System fragt optional nach Bestätigung.
4. Benutzer bestätigt.
5. System löscht den Task (oder markiert ihn als gelöscht/archiviert je nach Konzept).
6. System aktualisiert die Task-Liste.

Alternativablauf (nur falls notwendig):

- A1: Benutzer bricht Bestätigung ab -> keine Änderungen.

Nachbedingungen: Task ist entfernt (oder als gelöscht markiert) und nicht mehr normal sichtbar.

#### UC-06: Task erledigen

Akteuer: Benutzer

Beschreibung: Benutzer markiert eine Task als „erledigt“. System vergibt XP, aktualisiert Streak und berechnet Level neu.

Vorbedingenen: Benutzer ist eingeloggt. Task existiert, gehört dem Benutzer und ist „offen“.

Hauptablauf:

1. Benutzer klickt bei einer offenen Task auf „Erledigt“.
2. System setzt Task-Status auf „erledigt“ und speichert Abschlussdatum/-zeit.
3. System erstellt einen TaskLog-Eintrag (Completion Event).
4. System berechnet XP für die Task und addiert diese zu `totalXP`.
5. System aktualisiert die Streak (siehe Streak-Regeln: heute/gestern/Lücke).
6. System berechnet Level aus `totalXP` neu.
7. System zeigt aktualisierte Werte (XP, Level, Streak) im UI an.

Alternativablauf (nur falls notwendig):

- A1: Task war bereits erledigt -> System macht keine doppelte Vergabe (idempotent), zeigt Status an.
- A2: Speichern/DB Fehler -> System zeigt Fehlermeldung, Task bleibt unverändert.

Nachbedingungen: Task ist erledigt, XP/Level/Streak sind konsistent aktualisiert und der Abschluss ist im Verlauf sichtbar.

## Gamification Use-Cases

### XP-System

## UC-07: XP berechnen

**Akteur:** System

**Trigger:** Task wird abgeschlossen (zb. durch „UC-06: Task als erledigt markieren“)

### Beschreibung:

Das System berechnet automatisch XP für den Benutzer basierend auf den Eigenschaften der erledigten Task und schreibt diese gut.

### Vorbedingungen:

- Benutzer existiert und ist identifizierbar (User-ID vorhanden).
- Eine Task wurde erfolgreich als „erledigt“ gespeichert (TaskLog vorhanden oder wird erstellt).
- Task hat gültige Parameter (zb. Schwierigkeit, Dauer).
- XP-Regeln/Formel sind im System definiert.

### Hauptablauf:

1. System erkennt den Task-Abschluss (Event/Trigger).
2. System lädt Task-Daten (Schwierigkeit, Dauer, ggf. Kategorie).
3. System berechnet XP anhand der XP-Regel.
4. System speichert ein **XPEvent** (UserId, TaskId, XP, Timestamp, Reason).
5. System aktualisiert **Gesamt-XP** des Benutzers (zb. in User-Profil oder als berechneter Wert).
6. System speichert Änderungen in der Datenbank.

### Alternativablauf :

* **A1: Ungültige Task-Daten** (Schwierigkeit/Dauer fehlt oder ausserhalb Bereich)
  -> System bricht ab, protokolliert Fehler, keine XP-Gutschrift.
* **A2: Datenbankfehler beim Speichern**
  -> System geht zurück (Transaction), meldet Fehler an aufrufenden Prozess.

### Nachbedingungen:

- XP wurden korrekt berechnet und gutgeschrieben.
- Ein XPEvent ist persistent gespeichert.
- Gesamt-XP ist konsistent aktualisiert.

---

## UC-08: XP anzeigen

**Akteur:** Benutzer

### Beschreibung:

Der Benutzer sieht seine aktuelle XP sowie die XP bis zum nächsten Level.

### Vorbedingungen:

- Benutzer ist eingeloggt.
- XP-Daten sind vorhanden (Gesamt-XP oder XPEvents).
- Level-Regeln sind definiert.

### Hauptablauf:

1. Benutzer öffnet Profil/Dashboard/Statusbereich.
2. System lädt Gesamt-XP des Benutzers.
3. System berechnet (oder lädt) aktuelles Level.
4. System berechnet XP bis zum nächsten Level.
5. System zeigt an:

   - Aktuelle XP (Gesamt-XP)
   - XP bis zum nächsten Level (Differenz)

### Alternativablauf :

- **A1: Keine XP vorhanden (Neuer User)**
  -> System zeigt 0 XP und volle XP bis zum nächsten Level.
- **A2: Datenbankfehler**
  -> System zeigt Fehlermeldung/Placeholder („Daten konnten nicht geladen werden“).

### Nachbedingungen:

- Benutzer sieht aktuelle XP und die Distanz zum nächsten Level.

---

## UC-09: Streak aktualisieren

**Akteur:** System

**Trigger:** Erste Task wird abgeschlossen in einem bestimmten Zeitraum (TaskLog wird erstellt)

### Beschreibung:

Das System erhöht die Streak um 1, wenn der Benutzer an einem **neuen Tag** mindestens eine Task erledigt.

### Vorbedingungen:

- Benutzer existiert.
- Task-Abschluss wurde gespeichert (Zeitpunkt bekannt).
- Streak-Status des Benutzers ist gespeichert (CurrentStreak, LastStreakDate o.ä.).

### Hauptablauf:

1. System erhält das Abschlussdatum der Task (zb. `CompletedAt`).
2. System lädt Streak-Daten des Benutzers (letztes Streak-Datum, aktuelle Streak).
3. System prüft:

   - Wenn `CompletedAt` **am selben Datum** wie `LastStreakDate` -> keine Änderung.
   - Wenn `CompletedAt` **ein neuer Kalendertag** ist -> Streak +1.
4. System setzt `LastStreakDate` auf das Abschlussdatum.
5. System speichert Streak-Daten in der Datenbank.

### Alternativablauf :

- **A1: Kein Streak-Datensatz vorhanden (Erstmalig)**
  -> System erstellt Streak-Daten, setzt Streak = 1, LastStreakDate = heute.
- **A2: Datenbankfehler**
  -> System bricht ab und protokolliert Fehler.

### Nachbedingungen:

- Streak ist korrekt aktualisiert (oder unverändert, wenn schon für heute gezählt).
- LastStreakDate ist konsistent gespeichert.

---

## UC-10: Streak zurücksetzen

**Akteur:** System

**Trigger:** Tageswechsel / App-Start / Dashboard-Aufruf

### Beschreibung:

Wenn der Benutzer an einem Tag **keine** Task abschliesst, setzt das System die Streak gemäss Regel zurück (auf 0).

### Vorbedingungen:

- Streak-Daten existieren (CurrentStreak, LastStreakDate).
- Das System kann „heute“ bestimmen (Datum).

### Hauptablauf:

1. System lädt Streak-Daten des Benutzers.
2. System vergleicht `LastStreakDate` mit dem heutigen Datum.
3. System prüft: Wenn seit `LastStreakDate` **mindestens 1 Tag ohne Abschluss** vergangen ist (Regel: „gestern keine Task“) -> Reset.
4. System setzt `CurrentStreak = 0`.
5. System speichert die aktualisierten Streak-Daten.

### Alternativablauf :

- **A1: Streak ist bereits 0**
  -> System macht nichts.
- **A2: LastStreakDate ist leer (Neuer User)**
  -> System setzt Streak auf 0.

### Nachbedingungen:

- Streak ist zurückgesetzt, wenn die Bedingungen erfüllt sind.
- Streak-Daten bleiben konsistent.
