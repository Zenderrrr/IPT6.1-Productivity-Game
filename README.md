# IPT6.1 – FocusUp

![ASP.NET](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4)
![Vue](https://img.shields.io/badge/Vue-3-42B883)
![TypeScript](https://img.shields.io/badge/TypeScript-Latest-3178C6)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED)
![Railway](https://img.shields.io/badge/Hosted_on-Railway-0B0D0E)
![Status](https://img.shields.io/badge/Status-In_Development-yellow)

## Über das Projekt

**FocusUp** ist eine webbasierte Produktivitätsplattform, die klassische Aufgabenverwaltung mit Gamification kombiniert.

Viele To-Do-Anwendungen helfen zwar beim Organisieren von Aufgaben, bieten jedoch wenig Motivation, diese tatsächlich zu erledigen. FocusUp löst dieses Problem durch spielerische Elemente wie XP, Level, Streaks und Badges, welche den Fortschritt sichtbar machen und Nutzer langfristig motivieren.

Die Anwendung läuft vollständig im Browser und ermöglicht es Benutzern, ihre Aufgaben zu verwalten, Fortschritte zu verfolgen und produktive Gewohnheiten aufzubauen.

---

## Projektziel

Ziel des Projekts ist die Entwicklung einer modernen Full-Stack-Webanwendung, welche Produktivität und Motivation miteinander verbindet.

Durch die Integration von Gamification-Mechaniken soll die tägliche Aufgabenbearbeitung interessanter und motivierender gestaltet werden.

Benutzer werden für ihre Fortschritte belohnt und können ihre Produktivität anhand von Statistiken und Erfolgen nachvollziehen.

---

## Funktionen

### Aufgabenverwaltung

* Aufgaben erstellen, bearbeiten und löschen
* Aufgaben als erledigt markieren
* Prioritäten festlegen
* Schwierigkeitsgrade definieren
* Übersichtliche Aufgabenlisten

### XP- und Levelsystem

* Erfahrungspunkte (XP) für erledigte Aufgaben erhalten
* Automatische Levelaufstiege
* Sichtbarer Fortschritt durch Fortschrittsbalken

### Streak-System

* Tägliche Aktivitätsstreaks
* Motivation zur kontinuierlichen Nutzung
* Belohnung für Regelmässigkeit

### Badge-System

* Freischaltbare Erfolge
* Verschiedene Seltenheitsstufen
* Versteckte Achievements

### Statistiken

* Erledigte Aufgaben
* Gesammelte XP
* Aktuelle Streaks
* Persönliche Produktivitätsübersicht

### Benutzerverwaltung

* Registrierung
* Login
* JWT-basierte Authentifizierung
* Persönliches Benutzerprofil

---

## Systemarchitektur

```text
┌─────────────────────┐
│      Frontend       │
│     Vue 3 + Vite    │
└──────────┬──────────┘
           │ REST API
           ▼
┌─────────────────────┐
│      Backend        │
│ ASP.NET Core WebAPI │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│      SQLite DB      │
└─────────────────────┘
```

---

## Verwendete Technologien

### Frontend

* Vue 3
* TypeScript
* Vite
* Pinia
* Tailwind CSS
* ECharts

Weitere Informationen:

[Frontend Dokumentation](Project/Frontend/focusUp/README.md)

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* JWT Authentication
* SQLite

### Infrastruktur

* Docker
* Docker Compose

---

## Verfügbarkeit

Die Anwendung ist öffentlich über Railway erreichbar.

### Live Demo

[FocusUp](https://focusup.up.railway.app)

### Deployment

Das Projekt wird automatisch über Railway deployed. Änderungen am Hauptbranch werden nach erfolgreichem Build automatisch veröffentlicht.

---

## Entwickler

Projektarbeit im Rahmen des Moduls IPT 6.1.

Entwickelt von:

* Sanjivan
* Egor

---

## Lizenz

Dieses Projekt wurde zu Ausbildungszwecken entwickelt und dient ausschliesslich Lern-, Demonstrations- und Bewertungszwecken.
