# FocusUp – Unit- und Integrationstests

Die Testsuite deckt die 13 registrierten Frontend-Routen ab.

## Testaufbau

**Unit-Tests** prüfen Hilfsfunktionen und einzelne Vue-Komponenten. Dazu gehören Datumsformatierung, Fehlermeldungen, Farbmodus, Eingabevalidierung, Task-Filter, die Reihenfolge der Dialoge beim Task-Abschluss und die Kalenderberechnung der Heatmap.

**Frontend-Integrationstests** verbinden gerenderte Seiten, Vue Router, Pinia, Local Storage und den API-Client. Vitest führt die Tests mit Vue Test Utils und jsdom aus. Die HTTP-Antworten, ausgewählte Browser-APIs und die grafische Ausgabe von ECharts werden dabei ersetzt. Die Diagrammtests prüfen die berechneten Daten, die FocusUp an ECharts übergibt.

**Browser-E2E-Tests** öffnen die Statistikseite in Chromium. Playwright prüft die echte ECharts-Darstellung und die Bedienung der Heatmap mit simulierten API-Antworten.

## Abdeckung nach Seite

| Route oder Ansicht | Geprüftes Verhalten |
| --- | --- |
| `/` | Landingpage mit Links zur Anmeldung, Registrierung und zu den Rechtsseiten. Navigation zur Registrierung. Weiterleitung zum Dashboard bei aktiver Sitzung. |
| `/login` | Übermittlung der Zugangsdaten, Speicherung des Tokens und Navigation zum Dashboard. Fehlermeldung bei abgelehnter Anmeldung. Ein- und Ausblenden des Passworts ohne Formularanfrage. |
| `/register` | Abweichende Passwortbestätigung blockiert die Anfrage. Nach erfolgreicher Registrierung folgt die Anmeldung. Ein Konflikt bei Benutzername oder E-Mail wird angezeigt. |
| `/forgot-password` | Übermittlung der E-Mail, Bestätigung und API-Fehler. Ein fehlgeschlagener erneuter Versuch ersetzt eine frühere Erfolgsmeldung. |
| `/reset-password` | Übermittlung des decodierten Tokens aus der URL und des neuen Passworts. Schutz bei fehlendem oder mehrfachem Token sowie bei abweichenden Passwörtern. Anzeige der API-Meldung bei abgelaufenem Link. |
| `/dashboard` | Ladezustand, Benutzername, Gesamt-XP, Level, erledigte Tasks und Links zu den Task-Ansichten. Erfolgsquote mit fristgerechten, verspäteten und undatierten Tasks. Diagramm für 14, 30 und 90 Tage. Task-Schnellerfassung. |
| `/tasks` | Offene und erledigte Tasks, Suche, Kategorien, Sortierung nach Fälligkeit, Schnellfilter und leere Trefferliste. Anlegen, Bearbeiten, Löschen und Abschliessen von Tasks. Kategorieanlage, fehlgeschlagene Schreibvorgänge und nutzerbezogene Auswahl im Local Storage. |
| `/stats` | Abfrage der Vergleichszeiträume, Trends, Balkendiagramm, Kategorienverteilung, kumulierte XP und Heatmap. Lade-, Leer- und Fehlerzustand der Heatmap. Verlauf mit fehlenden Task-Details. |
| `/profile` | Accountdaten, Level, Streak und Badge-Status. Badge-Liste erweitern und reduzieren. Farbmodus, Logout und Accountlöschung inklusive Abbruch und API-Fehler. |
| `/allCompletedTasks` | Titel, Kategorie, Dauer und XP pro Tabellenzeile. Kategorie-Ersatztext, leere Liste und Anzeige eines Eintrags mit fehlenden Task-Details. |
| `/legal-notice` | Impressum mit Kontaktlink und Rückweg zur Startseite. |
| `/privacy-policy` | Inhaltsübersicht und gültige Anker zu den einzelnen Abschnitten. |
| Unbekannte URL | 404-Ansicht mit Links zur Startseite und zum Dashboard. |

Der Router leitet Gäste von allen fünf geschützten Routen zur Anmeldung weiter. Eine gespeicherte Sitzung lädt das Benutzerprofil. Der API-Client erneuert einen abgelaufenen Token, wiederholt die ursprüngliche Anfrage und beendet die Sitzung, wenn die Erneuerung abgelehnt wird.

## Testdateien

Die Vitest-Dateien liegen unter `src/__tests__`. Der Browser-Test liegt unter `e2e`.

| Datei | Art | Testfälle |
| --- | --- | ---: |
| `Utilities.spec.ts` | Unit · Datum, Badge-Slug, Fehlermeldungen und Farbmodus | 19 |
| `ActivityHeatmap.spec.ts` | Unit · Heatmap-Zeitraum, Wochenraster und XP-Skala | 20 |
| `TaskComponents.spec.ts` | Unit · Task-Formular, Filtergrenzen und Abschlussdialog | 11 |
| `ApiSession.spec.ts` | Integration · API-Client, Token-Erneuerung und HTTP-Antworten | 5 |
| `App.spec.ts` | Integration · App, Routing und öffentliche Seiten | 12 |
| `AuthPages.spec.ts` | Integration · Anmeldung, Registrierung und Passwort-Reset | 14 |
| `TasksPage.spec.ts` | Integration · Task-Übersicht und Task-Aktionen | 14 |
| `ReportingPages.spec.ts` | Integration · Dashboard, Statistik und Task-Verlauf | 15 |
| `ProfilePage.spec.ts` | Integration · Profil, Badges, Einstellungen und Account | 6 |
| `e2e/activity-heatmap.spec.ts` | Browser-E2E · ECharts-Rendering, Desktop, Touchscreen und Zustände | 4 |
| **Total** | **50 Unit-Tests, 66 Frontend-Integrationstests und 4 Browser-E2E-Tests** | **120** |

`testSupport.ts` stellt isolierte Testdaten, HTTP-Antworten und Hilfen zum Starten der Seiten bereit. Eine unerwartete HTTP-Anfrage lässt den jeweiligen Test fehlschlagen.

## Heatmap

- Das Raster umfasst die letzten 91 Kalendertage inklusive heute. Je nach Startwochentag berührt der Zeitraum 13 oder 14 Kalenderwochen. Die Zeilen laufen von Montag bis Sonntag. Daten ausserhalb des Zeitraums werden ausgeblendet, fehlende Tage verschieben keine Zellen.
- Unit-Tests prüfen alle sieben Startwochentage, Wochen-, Jahres- und Zeitwechsel, den Schalttag, unsortierte Daten, leere Antworten, Lücken und Farbskalen ab 0 XP.
- Integrationstests prüfen die 91-Tage-Anfrage, das Kalenderdatum aus dem API-Format, erste und letzte Zelle, Wochentagsreihenfolge, Tooltip-Inhalt sowie Lade-, Leer- und Fehlerzustand. Bei einem Fehler erscheinen keine Daten aus dem vorher geladenen 7-Tage-Diagramm in der Heatmap.
- Chromium-Tests prüfen die gerenderten Farben, den Tooltip bei Maus- und Touchbedienung, horizontales Scrollen bei 375 px Breite und die sichtbaren Meldungen bei leeren Daten oder einem API-Fehler.

## Regressionen

- Fehlgeschlagene Task- und Kategorieanlagen behalten die Formulareingaben. Eine bereits gespeicherte Task gilt auch dann als angelegt, wenn das Nachladen der Liste scheitert. Eine abgelehnte Löschung entfernt die Task nicht aus der Liste.
- Der Task-Abschluss lädt die Liste nach dem letzten Dialog neu. Der Ablauf funktioniert ohne neues Badge und endet bei mehreren Badges nach der letzten Bestätigung.
- Passwort-Sichtbarkeitsschalter lösen keine Formularanfrage aus. Beim Passwort-Reset wird ein fehlender oder mehrdeutiger Token vor dem Senden abgefangen.
- Nach erfolgreicher Accountlöschung wird die lokale Sitzung beendet. Bei einem Fehler bleibt das Profil geöffnet und zeigt eine Meldung.
- Das Dashboard hält seinen Ladezustand während des initialen Ladens. Verlaufstabellen zeigen einen Ersatztext, wenn Task-Details fehlen.
- Datumsfilter berücksichtigen Kalendertage, die lokale Woche von Montag bis Sonntag und den gesamten letzten Tag eines gewählten Zeitraums. Relative Zeitangaben verwenden die korrekte Einheit und das korrekte Kalenderdatum.
- Die Heatmap wechselt am Montag die Spalte. Die letzte Spalte ist auf schmalen Bildschirmen erreichbar. Ein Fehler beim Laden der 91 Tage wird als Fehler angezeigt.

## Ausführung

Die Befehle werden im Frontend-Ordner `focusUp` ausgeführt. Der Prüflauf verwendet Node.js 24 und Vitest 4.1.2.

```powershell
# Abhängigkeiten inklusive Vitest installieren
npm ci --include=dev

# Gesamte Suite einmal ausführen
npm run test:unit -- --run

# Einzelne Datei ausführen
npm run test:unit -- --run src/__tests__/TasksPage.spec.ts

# Typprüfung
npm run type-check

# Browser einmalig installieren und Heatmap-Tests ausführen
npx playwright install chromium
npm run test:e2e
```

Der Prüflauf vom **9. November 2026** hat **116 bestandene Vitest-Fälle in 9 Dateien und 4 bestandene Playwright-Fälle** ergeben. Die Heatmap- und Statistiktests wurden zusätzlich in der Zeitzone America/Los_Angeles geprüft. Der Browserlauf verwendet Chromium 153. Der Produktionsbuild inklusive Typprüfung war erfolgreich. Die bereits im Ausgangsstand auftretenden Warnungen zur CSS-Optimierung und Bundle-Grösse erscheinen weiterhin im Build.

Das Paket betrifft das Vue-Frontend. Die Browsertests decken die Heatmap in Chromium ab. Die ASP.NET-Core-API, die SQLite-Datenbank und der tatsächliche E-Mail-Versand liegen ausserhalb dieser Frontend-Testsuite. Für einen lokalen Start der Anwendung mit API kann `.env` aus `.env.example` erstellt werden. Der Testlauf benötigt diese Datei nicht.
