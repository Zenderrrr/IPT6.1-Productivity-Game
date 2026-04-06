# Frontend Enwicklungsstruktur

## Überblick
Die Frontend Struktur ist so strukturiert das alle Elemente des Frontends einen spezifischen Platz haben und nur eine Verantwortung übernehmen, so das Modularität und Erweiterbarkeit sichergestellt sind.

- **Trennung von UI, Logik und Daten**
- **Feature-basierter Aufbau**
- **Wiederverwendbarkeit von Komponenten**
- **Skalierbarkeit für zukünftige Erweiterungen**


## Struktur
```
frontend/
│
├── public/
│
├── src/
│   │
│   ├── assets/              # Bilder, Icons, Logos
│   ├── styles/              # globale CSS, Variablen, Theme
│   ├── components/          # wiederverwendbare UI-Komponenten
│   │   ├── ui/              # Button, Input, Modal, Card, Badge
│   │   ├── layout/          # Navbar, Footer, PageContainer
│   │   └── charts/          # Diagramme für Statistik
│   │
│   ├── pages/               # ganze Seiten
│   │   ├── Home/
│   │   ├── Dashboard/
│   │   ├── Tasks/
│   │   ├── Stats/
│   │   ├── Badges/
│   │   ├── Profile/
│   │   └── Auth/
│   │       ├── Login/
│   │       └── Register/
│   │
│   ├── features/            # Logik pro Bereich
│   │   ├── auth/
│   │   ├── tasks/
│   │   ├── xp/
│   │   ├── streak/
│   │   ├── level/
│   │   ├── badges/
│   │   ├── stats/
│   │   ├── categories/
│   │   └── user/
│   │
│   ├── services/            # API-Aufrufe zum Backend
│   │   ├── apiClient.ts
│   │   ├── authService.ts
│   │   ├── taskService.ts
│   │   ├── statsService.ts
│   │   └── badgeService.ts
│   │
│   ├── stores/              # globaler State
│   │   ├── authStore.ts
│   │   ├── taskStore.ts
│   │   ├── userStore.ts
│   │   └── statsStore.ts
│   │
│   ├── router/              # Routing
│   │   └── index.ts
│   │
│   ├── types/               # TypeScript Interfaces / Typen
│   │   ├── task.ts
│   │   ├── user.ts
│   │   ├── badge.ts
│   │   └── stats.ts
│   │
│   ├── utils/               # Hilfsfunktionen
│   │   ├── date.ts
│   │   └── helpers.ts
│   │
│   ├── App.vue
│   └── main.ts
```

## Erklärung der Hauptbereiche

### public/
Enthält statische Dateien, die direkt ausgeliefert werden.

- zb. favicon, index.html
- keine Verarbeitung durch das Framework

--- 

### assets/
Speichert statische Ressourcen innerhalb des Codes.

- Bilder
- Icons
- Logos

---

### styles/
Globale Styles und Design-System.

- Farben (Theme)
- Schriftarten
- globale CSS-Regeln

---

### components/
Wiederverwendbare UI-Komponenten.

- kleine Bausteine (Button, Card)
- Layout-Komponenten (Navbar)
- visuelle Elemente (Charts)

---

### pages/
Komplette Seiten der Website.

- eine Seite der Website
- kombiniert mehrere Components

---

### features/
Fachliche Logik nach Bereichen getrennt.

- enthält Business-Logik
- unabhängig von UI

---

### services/
Kommunikation mit dem Backend.

- API Calls (HTTP Requests)
- Daten holen / senden

---

### stores/
Globaler Zustand der Website.

- zentrale Daten (User, Tasks, Stats)
- wird von mehreren Komponenten genutzt

---

### router/
Navigation und Routing.

- definiert URLs -> Pages
- steuert Zugriff

---

### types/
TypeScript Typen.

- Interfaces für Daten (Task, User, etc.)
- sorgt für Typensicherheit

---

### utils/
Hilfsfunktionen.

- wiederverwendbare Logik
- unabhängig von Features