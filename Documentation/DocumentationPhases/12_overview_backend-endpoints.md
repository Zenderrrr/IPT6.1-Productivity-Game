# Controller-Übersicht Backend

In diesem Dokument werden die Controller des Backends von **FocusUp** beschrieben. Für jeden Controller wird festgehalten, welche Endpoints er enthält, welchen Input sie benötigen, welchen Output sie zurückgeben und welche Meldungen bzw. HTTP-Responses möglich sind.

Die Struktur orientiert sich an den Kernbereichen des Projekts: Authentifizierung, Tasks, Kategorien, Dashboard, Statistiken und Badges.

---

```md
AuthController
TaskController
CategoryController
DashboardController
StatsController
BadgeController
```

---

## 1. AuthController

Der `AuthController` ist für Registrierung, Login, Logout und das Laden des aktuell eingeloggten Benutzers zuständig. Diese Endpoints decken die Use-Cases **Benutzer registrieren** und **Benutzer einloggen** ab.

### POST `/api/auth/register`

**Zweck:**  
Erstellt einen neuen Benutzeraccount.

**Input:**  
Request Body:
- `username: string`
- `email: string`
- `password: string`

**Output:**  
- Benutzer wurde erfolgreich erstellt
- optional Benutzerdaten oder Erfolgsmeldung

**Mögliche Meldungen / Responses:**  
- `201 Created` -> Registrierung erfolgreich
- `400 Bad Request` -> Eingaben ungültig
- `409 Conflict` -> Benutzername oder E-Mail existiert bereits
- `500 Internal Server Error` -> unerwarteter Fehler

---

### POST `/api/auth/login`

**Zweck:**  
Authentifiziert einen Benutzer und erstellt eine Session bzw. ein JWT.

**Input:**  
Request Body:
- `usernameOrEmail: string`
- `password: string`

**Output:**  
- Access Token / Session-Information
- grundlegende Benutzerdaten

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Login erfolgreich
- `400 Bad Request` -> Eingaben fehlen oder sind ungültig
- `401 Unauthorized` -> Login-Daten falsch
- `404 Not Found` -> Benutzer nicht gefunden
- `500 Internal Server Error` -> unerwarteter Fehler

---

### POST `/api/auth/logout`

**Zweck:**  
Beendet die Session bzw. macht den Benutzer im System abgemeldet.

**Input:**  
- kein Body nötig
- Authentifizierung erforderlich

**Output:**  
- Erfolgsmeldung

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Logout erfolgreich
- `401 Unauthorized` -> Benutzer ist nicht eingeloggt
- `500 Internal Server Error` -> unerwarteter Fehler

---

### GET `/api/auth/me`

**Zweck:**  
Gibt den aktuell eingeloggten Benutzer zurück.

**Input:**  
- kein Body
- gültiger JWT / gültige Session

**Output:**  
- `id`
- `username`
- `email`

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Benutzer erfolgreich geladen
- `401 Unauthorized` -> kein gültiger Login
- `500 Internal Server Error` -> unerwarteter Fehler

---

## 2. TaskController

Der `TaskController` ist für die Kernfunktion der Anwendung zuständig: Tasks erstellen, laden, bearbeiten, löschen und abschliessen. Diese Endpoints decken die Core Use-Cases UC-03 bis UC-06 ab.

### GET `/api/tasks`

**Zweck:**  
Lädt alle Tasks des eingeloggten Benutzers.

**Input:**  
Optional Query Parameter:
- `status`
- `categoryId`
- `difficulty`
- `from`
- `to`

**Output:**  
Liste von Tasks mit:
- `id`
- `title`
- `description`
- `difficulty`
- `durationMin`
- `categoryId`
- `dueDate`
- `status`
- `completedAt`

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Tasks erfolgreich geladen
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `500 Internal Server Error` -> unerwarteter Fehler

---

### GET `/api/tasks/{id}`

**Zweck:**  
Lädt die Details einer bestimmten Task.

**Input:**  
Route Parameter:
- `id: int`

**Output:**  
Eine einzelne Task mit allen Task-Daten.

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Task erfolgreich geladen
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `403 Forbidden` -> Task gehört nicht dem Benutzer
- `404 Not Found` -> Task nicht gefunden
- `500 Internal Server Error` -> unerwarteter Fehler

---

### POST `/api/tasks`

**Zweck:**  
Erstellt eine neue Task. Entspricht UC-03.

**Input:**  
Request Body:
- `title: string`
- `description: string`
- `difficulty: string`
- `durationMin: int`
- `categoryId: int?`
- `dueDate: DateTime?`

**Output:**  
- erzeugte `taskId`
- optional die neu erstellte Task

**Mögliche Meldungen / Responses:**  
- `201 Created` -> Task erfolgreich erstellt
- `400 Bad Request` -> Eingaben ungültig
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `500 Internal Server Error` -> unerwarteter Fehler

---

### PUT `/api/tasks/{id}`

**Zweck:**  
Bearbeitet eine bestehende Task. Entspricht UC-04. 

**Input:**  
Route Parameter:
- `id: int`

Request Body:
- `title: string`
- `description: string`
- `difficulty: string`
- `durationMin: int`
- `categoryId: int?`
- `dueDate: DateTime?`
- optional `status: string`

**Output:**  
- Erfolgsmeldung
- optional aktualisierte Task

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Task erfolgreich aktualisiert
- `400 Bad Request` -> Daten ungültig
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `403 Forbidden` -> Task gehört nicht dem Benutzer
- `404 Not Found` -> Task nicht gefunden
- `500 Internal Server Error` -> unerwarteter Fehler

---

### DELETE `/api/tasks/{id}`

**Zweck:**  
Löscht eine Task. Entspricht UC-05. 

**Input:**  
Route Parameter:
- `id: int`

**Output:**  
- Erfolgsmeldung

**Mögliche Meldungen / Responses:**  
- `200 OK` oder `204 No Content` -> Task erfolgreich gelöscht
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `403 Forbidden` -> Task gehört nicht dem Benutzer
- `404 Not Found` -> Task nicht gefunden
- `500 Internal Server Error` -> unerwarteter Fehler

---

### POST `/api/tasks/{id}/complete`

**Zweck:**  
Markiert eine Task als erledigt und löst die Gamification-Folgeaktionen aus. Entspricht UC-06. Dabei werden Task-Status, TaskLog, XP, Streak, Level und Badge-Prüfung verarbeitet. 

**Input:**  
Route Parameter:
- `id: int`

Kein Request Body nötig.

**Output:**  
Aktualisierte Abschlussdaten, z. B.:
- `taskId`
- `status`
- `xpAwarded`
- `totalXp`
- `level`
- `progressToNextLevel`
- `streakCount`
- `newBadges`

**Mögliche Meldungen / Responses:**  
- `200 OK` -> Task erfolgreich erledigt
- `400 Bad Request` -> Task kann nicht erledigt werden
- `401 Unauthorized` -> Benutzer nicht eingeloggt
- `403 Forbidden` -> Task gehört nicht dem Benutzer
- `404 Not Found` -> Task nicht gefunden
- `409 Conflict` -> Task ist bereits erledigt
- `500 Internal Server Error` -> unerwarteter Fehler

---

## 3. CategoryController

Der `CategoryController` verwaltet Kategorien, damit Tasks strukturiert erfasst und gefiltert werden können. Kategorien sind im Datenmodell und in den CRUD-Features enthalten.

### GET `/api/categories`

**Zweck:**  
Lädt alle Kategorien des eingeloggten Benutzers.

**Input:**  
- kein Body
- Authentifizierung erforderlich

**Output:**  
Liste von Kategorien:
- `id`
- `name`
- `color`
- `createdAt`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `500 Internal Server Error`

---

### GET `/api/categories/{id}`

**Zweck:**  
Lädt eine bestimmte Kategorie.

**Input:**  
Route Parameter:
- `id: int`

**Output:**  
Eine Kategorie mit allen relevanten Daten.

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`
- `500 Internal Server Error`

---

### POST `/api/categories`

**Zweck:**  
Erstellt eine neue Kategorie.

**Input:**  
Request Body:
- `name: string`
- `color: string`

**Output:**  
- erzeugte `categoryId`
- optional Kategorieobjekt

**Mögliche Meldungen / Responses:**  
- `201 Created`
- `400 Bad Request`
- `401 Unauthorized`
- `409 Conflict` -> Kategorie existiert bereits
- `500 Internal Server Error`

---

### PUT `/api/categories/{id}`

**Zweck:**  
Aktualisiert eine bestehende Kategorie.

**Input:**  
Route Parameter:
- `id: int`

Request Body:
- `name: string`
- `color: string`

**Output:**  
- Erfolgsmeldung
- optional aktualisierte Kategorie

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `400 Bad Request`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`
- `500 Internal Server Error`

---

### DELETE `/api/categories/{id}`

**Zweck:**  
Löscht eine Kategorie.

**Input:**  
Route Parameter:
- `id: int`

**Output:**  
- Erfolgsmeldung

**Mögliche Meldungen / Responses:**  
- `200 OK` oder `204 No Content`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`
- `500 Internal Server Error`

---

## 4. DashboardController

Der `DashboardController` liefert eine kompakte Übersicht für die Dashboard-Seite. Dort sollen XP, Level, Streak, letzte Aktivität und wichtige Kennzahlen gezeigt werden.

### GET `/api/dashboard`

**Zweck:**  
Lädt alle wichtigen Dashboard-Daten in einem Request.

**Input:**  
- kein Body
- gültiger Login

**Output:**  
Kombiniertes Dashboard-Objekt, z. B.:
- `totalXp`
- `level`
- `xpCurrent`
- `xpNext`
- `progressToNextLevel`
- `streakCount`
- `bestStreak`
- `tasksOpen`
- `tasksDone`
- `lastCompletedTasks`
- `recentXpEvents`
- `newUnlockedBadges`

Die Level- und Progress-Berechnung basieren auf `totalXP` und der quadratischen Formel.

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `404 Not Found` -> falls UserStats fehlen
- `500 Internal Server Error`

---

## 5. StatsController

Der `StatsController` ist für Analyse- und Statistikdaten zuständig. Er greift auf `TaskLog`, `XPEvent` und `UserStats` zu, damit Produktivität und Verlauf dargestellt werden können. 

### GET `/api/stats`

**Zweck:**  
Lädt die wichtigsten Kennzahlen für die Statistik-Seite.

**Input:**  
Optional Query Parameter:
- `range=week|month|year`

**Output:**  
Statistik-Objekt, z. B.:
- `tasksDone`
- `tasksOpen`
- `totalTimeMin`
- `totalXp`
- `streakCount`
- `bestStreak`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `404 Not Found`
- `500 Internal Server Error`

---

### GET `/api/stats/productivity`

**Zweck:**  
Lädt Produktivitätsdaten über einen Zeitraum.

**Input:**  
Query Parameter:
- `range`
- optional `from`
- optional `to`

**Output:**  
Liste / Zeitreihe mit:
- `date`
- `completedTasks`
- `xpGained`
- `timeSpent`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `400 Bad Request` -> Zeitraum ungültig
- `401 Unauthorized`
- `500 Internal Server Error`

---

### GET `/api/stats/tasklogs`

**Zweck:**  
Lädt den Task-Verlauf des Benutzers.

**Input:**  
Optional Query Parameter:
- `limit`
- `taskId`

**Output:**  
Liste von TaskLogs:
- `id`
- `taskId`
- `action`
- `xpAwarded`
- `createdAt`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `500 Internal Server Error`

---

### GET `/api/stats/xp-events`

**Zweck:**  
Lädt den XP-Verlauf des Benutzers.

**Input:**  
Optional Query Parameter:
- `limit`
- `taskId`

**Output:**  
Liste von XPEvents:
- `id`
- `userId`
- `taskId`
- `reason`
- `amount`
- `createdAt`

Die Speicherung einzelner XP-Events dient genau der Nachvollziehbarkeit des XP-Verlaufs. 

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `500 Internal Server Error`

---

## 6. BadgeController

Der `BadgeController` stellt definierte Badges sowie bereits freigeschaltete Badges bereit. Badges dienen als Motivationssystem und haben eine eigene Seite im Frontend.

### GET `/api/badges`

**Zweck:**  
Lädt alle verfügbaren Badges.

**Input:**  
- kein Body
- Login erforderlich

**Output:**  
Liste aller Badge-Definitionen:
- `id`
- `name`
- `description`
- `ruleType`
- `ruleValue`
- `createdAt`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `500 Internal Server Error`

---

### GET `/api/badges/unlocked`

**Zweck:**  
Lädt alle bereits freigeschalteten Badges des Benutzers.

**Input:**  
- kein Body
- Login erforderlich

**Output:**  
Liste der freigeschalteten Badges:
- `id`
- `name`
- `description`
- `awardedAt`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `404 Not Found` -> falls User oder UserBadge-Daten fehlen
- `500 Internal Server Error`

---

### GET `/api/badges/locked`

**Zweck:**  
Lädt alle noch nicht freigeschalteten Badges.

**Input:**  
- kein Body
- Login erforderlich

**Output:**  
Liste aller gesperrten Badges:
- `id`
- `name`
- `description`
- `ruleType`
- `ruleValue`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `500 Internal Server Error`

---

### GET `/api/badges/{id}`

**Zweck:**  
Lädt die Details eines bestimmten Badges.

**Input:**  
Route Parameter:
- `id: int`

**Output:**  
Ein Badge-Objekt mit:
- `id`
- `name`
- `description`
- `ruleType`
- `ruleValue`
- optional `isUnlocked`

**Mögliche Meldungen / Responses:**  
- `200 OK`
- `401 Unauthorized`
- `404 Not Found`
- `500 Internal Server Error`

---

## Zusammenfassung

Die sechs Controller des Backends sind:

- `AuthController`
- `TaskController`
- `CategoryController`
- `DashboardController`
- `StatsController`
- `BadgeController`

Damit werden die wichtigsten Projektbereiche von **FocusUp** abgedeckt: Authentifizierung, Task-Verwaltung, Kategorien, Dashboard, Statistiken und Badges. Diese Struktur passt zu euren Use-Cases, zum Seitenaufbau sowie zu eurem Datenmodell mit `User`, `Task`, `TaskLog`, `XPEvent`, `UserStats`, `Category`, `Badge` und `UserBadge`.