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

Akteuer:

Beschreibung:

Vorbedingenen:

Hauptablauf:

Alternativablauf (nur falls notwendig):

Nachbedingungen:

#### UC-05: Task löschen

Akteuer:

Beschreibung:

Vorbedingenen:

Hauptablauf:

Alternativablauf (nur falls notwendig):

Nachbedingungen:

#### UC-06: Task erledigen

Akteuer:

Beschreibung:

Vorbedingenen:

Hauptablauf:

Alternativablauf (nur falls notwendig):

Nachbedingungen:

## Gamification Use-Cases

<<<<<<< HEAD

### XP-System

#### UC-07: XP berechnen

Akteur: System

Beschreibung:
Das System berechnet automatisch die XP was eine Aufgabe enthält, wenn diese als erledigt markiert wird.

Vorbedingenen:

-

Hauptablauf:

Alternativablauf (nur falls notwendig):

Nachbedingungen:

#### UC-08: XP anzeigen
