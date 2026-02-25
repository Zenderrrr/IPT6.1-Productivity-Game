# CRUD Features
Wichtig um die Kernfunktionen der Verwaltung und Bearbeitung von Daten zu visualisieren und die vier grundlegenden Aktionen darzulegen.

## CRUD Features fuer verschiedene Elemente auf der Website
- User
- Task
- Kategorien
- XP & Level
- Streaks
- Badges
- Statistiken

## 1. **User (Benutzerinteraktionen)**

### CREATE
- Benutzer registrieren
- Default-User beim ersten Start erstellen
### READ
- Benutzerprofil anzeigen
- XP, Level, Streak anzeigen
- Badges anzeigen
### Update 
- Benutzername ändern
- Passwort ändern
- Theme / Einstellungen ändern
### Delete
- Benutzer löschen
- Account zurücksetzen

## 2. **Tasks (Grund- / Kernfunktion)**

### CREATE
- Task erstellen
    - Titel
    - Beschreibung
    - Schwierigkeit
    - Dauer
    - Kategorie
    - Fälligkeitsdatum
### READ
- Task-Liste anzeigen
- Filter:
    - offen / erledigt
    - Kategorie
    - Zeitraum
- Detailansicht eines Tasks
### Update 
- Task bearbeiten
- Schwierigkeit anpassen
- Kategorie ändern
### Delete
- Task löschen
- Erledigten Task archivieren

## 3. **TaskLog**

### CREATE
- Eintrag erstellen, wenn Task abgeschlossen wird
### READ
- Verlauf anzeigen
- Produktivität pro Tag abrufen
### Update 
- keine
### Delete
- Einzelne Logs löschen (im Admin)

## 4. **XP-System**

### CREATE
- XP Element erstellen bei:
    - Task erledigt
    - Streak getroffen
    - Level Up
### READ
- Gesammte XP berechnen
- XP verlauf anzeigen
### Update 
- XP neu berechnen (bei Updates)
### Delete
- XP Elemente entfernen (als Admin)

## 5. **Level-System**

### CREATE
- Levelgrenzen definieren
### READ
- Aktuelles Level berechnen
- Fortschrittsbalken anzeigen
### Update 
- Level neu rechnen, bei XP Update
### Delete
- keine

## 6. **Streak-System**

### CREATE
- Streak starten / überschreiben wenn Streak regel erfüllt (zb. 1 Task pro Tag)
### READ
- Aktuelle Streak anzeigen
- Beste Streak anzeigen
### Update 
- Streak erhöhen 
- Streak zurücksetzen
### Delete
Streak Daten löschen

## 7. **Kategorien**

### CREATE
- Kategorie erstellen (Arbeit, Schule usw.)
### READ
- Kategorien anzeigen
- Anzahl Task pro Kategorie
### Update 
- Kategorie umbenennen
### Delete
- Kategorie löschen

## 8. **Badges**

### CREATE
- Badge definieren
- Badge erteilen bei Regel erfüllung (zb. 30 Tage Streak)
### READ
- erreichte Badges anzeigen
- gesperrte Badges anzeigen
### Update 
- Badge von gesperrt auf freigeschaltet ändern
### Delete
- Badge entfernen