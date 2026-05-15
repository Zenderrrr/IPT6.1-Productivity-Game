# Badge Auswahl

## Übersicht Badges
```
1. Task-basierte Badges

First Step -> 1 Task erledigt
Getting Started -> 5 Tasks
Task Grinder -> 50 Tasks
Machine -> 100+ Tasks

2. Streak-Badges

3 Day Streak
7 Day Warrior
30 Day Discipline
100 Day Legend

3. XP-Badges

XP Starter -> 50 XP
Leveling Up -> 500 XP
XP Master -> 5000 XP

4. Zeit-basierte Badges

Time Tracker -> 300 min
Deep Worker -> 1000 min
Focus Beast -> 5000 min

5. Schwierigkeit-Badges

Easy Rider -> 20 easy tasks
Challenger -> 10 hard tasks
Hardcore -> nur Hard Tasks abgeschlossen

6. Konsistenz / Verhalten

Early Bird -> Task vor 8 Uhr
Night Owl -> Task nach 22 Uhr
Deadline Master -> Task vor DueDate erledigt
Clutch Player -> Task kurz vor Deadline

7. Special / Hidden Badges

Comeback -> nach 7 Tagen Pause wieder aktiv
Perfect Day -> 5 Tasks an einem Tag
Overkill -> sehr lange Task
```

## Badge Beschreibung

```SQL
CREATE TABLE Badge (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    rule_type VARCHAR(50) NOT NULL,
    rule_value INTEGER NOT NULL,
    rarity VARCHAR(20) NOT NULL DEFAULT 'Common',
    primary_color VARCHAR(7) NOT NULL DEFAULT '#FFFFFF',
    secondary_color VARCHAR(7) NOT NULL DEFAULT '#000000',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    svg TEXT,
    isVisible INTEGER NOT NULL DEFAULT 1;

);
```

### Rule Typen:
```
tasks_completed
streak
xp_total
time_logged
difficulty_achived_easy
difficulty_achived_hard
difficulty_achived_hardcore
consistency_check_morning
consistency_check_evening
consistency_check_deadline
comeback_after_break
tasks_completed_single_day
long_task_duration
```

### Rarity

<span style="color:gray"> Common</span><br>
    - primary-color: #374151 <br>
    - secondary-color: #E5E7EB <br>
<span style="color:blue"> Rare</span><br>
    - primary-color: #1D4ED8 <br>
    - secondary-color: #BFDBFE <br>
<span style="color:violet"> Epic</span><br>
    - primary-color: #7C3AED <br>
    - secondary-color: #DDD6FE <br>
<span style="color:yellow"> Legendary </span><br>
    - primary-color: #CA8A04 <br>
    - secondary-color: #FEF08A <br>

---

### Task-basierte Badges

#### First Step - Common
- Beschreibung: 
    - Absolviere deine erste Task ab und beginne deinen Weg für ein Produktiveres Leben.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 1

#### Getting Started - Rare
- Beschreibung: 
    - Bewältige fünf Tasks auf deiner Reise auf FocusUp.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 5

#### Task Grinder - Epic
- Beschreibung: 
    - Schaffe 50 Tasks. Dies ist erst der Anfang.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 50


#### Machine - Legendary
- Beschreibung: 
    - Wie? Du hast 100 Tasks abgeschlossen! Wie geht es weiter?
- rule_type: 
    - tasks_completed
- rule_value: 
    - 100

### Streak-Badges

#### 3 Day Streak - Common
- Beschreibung: 
    - Die ersten Schritte zu einem Produktivem Alltag
- rule_type: 
    - streak
- rule_value: 
    - 3

#### 7 Day Warrior - Rare
- Beschreibung: 
    - Eine Woche am Stück bist du schon Produktiv. Weiter so!
- rule_type: 
    - streak
- rule_value: 
    - 7

#### 30 Day Discipline - Epic
- Beschreibung: 
    - Du bist nicht aufzuhalten! Einen Monat bist du schon kontinuierlich dran.
- rule_type: 
    - streak
- rule_value: 
    - 30


#### 100 Day Legend - Legendary
- Beschreibung: 
    - Deine Willenskraft ist insane! Wie schafft du es so lange am Ball zu bleiben? Sehr beeindruckend!!!
- rule_type: 
    - streak
- rule_value: 
    - 100

### XP-Badges

#### XP Starter - Common
- Beschreibung: 
    - Schritt für Schritt. Die ersten 50 XP.
- rule_type: 
    - xp_total
- rule_value: 
    - 50

#### Leveling Up - Rare
- Beschreibung: 
    - Das Rennen beginnt, wie weit wirst du noch kommen? Die 800 XP hast du schon mal.
- rule_type: 
    - xp_total
- rule_value: 
    - 800

#### XP Master - Epic
- Beschreibung: 
    - Du bist weit gekommen, der Master Rang kommt dir echt zu gute.
- rule_type: 
    - xp_total
- rule_value: 
    - 8000

### Zeit-basierte Badges


#### Time Tracker - Rare
- Beschreibung: 
    - Wie fühlts sich an zu gamen, eh, zu arbeiten meine ich?
- rule_type: 
    - time_logged
- rule_value: 
    - 300


#### Deep Worker - Epic
- Beschreibung: 
    - Du bist konzentriert an der Sache, wollen wir mal nicht weiter Stören.
- rule_type: 
    - time_logged
- rule_value: 
    - 1000


#### Focus Beast - Legendary
- Beschreibung: 
    - Konzentration ist in deinem Sinne, schon fast eine Untertreibung. Kann man dich noch aufhalten?
- rule_type: 
    - time_logged
- rule_value: 
    - 5000

### Schwierigkeit-Badges

#### Easy Rider - Common
- Beschreibung: 
    - Die einfachen Aufgaben müssen auch mal gemacht werden, neben den Schweren. Schaffe 20 einfache Tasks
- rule_type: 
    - difficulty_achived_easy
- rule_value: 
    - 20

#### Challenger - Rare
- Beschreibung: 
    - Jetzt kommt's zu den schweren Brocken, da kommt man ins Schwitzen. 
- rule_type: 
    - difficulty_achived_hard
- rule_value: 
    - 10

#### Hardcore - Epic
- Beschreibung: 
    - Nur Tasks des Types Hard, du bist wirklich Hardcore. 
- rule_type: 
    - difficulty_achived_hardcore
- rule_value: 
    - 10

### Konsistenz / Verhalten

#### Early Bird - Rare
- Beschreibung: 
    - Der frühe Vogel fängt den Wurm. Erledige Tasks vor 8 Uhr und starte Produktiv in den Tag.
- rule_type: 
    - consistency_check_morning
- rule_value: 
    - 8

#### Night Owl - Rare
- Beschreibung: 
    - Während andere schlafen, bist du noch am Hustlen. Schaffe Tasks nach 22 Uhr.
- rule_type: 
    - consistency_check_evening
- rule_value: 
    - 22

#### Deadline Master - Epic
- Beschreibung: 
    - Deadlines sind für dich keine Gefahr. Erledige Tasks bevor die DueDate erreicht wird.
- rule_type: 
    - consistency_check_deadline
- rule_value: 
    - 10

### Special / Hidden Badges

#### Comeback - Epic
- Beschreibung: 
    - Du warst lange weg, doch jetzt bist du zurück. Ein echter Comeback Moment.
- rule_type: 
    - comeback_after_break
- rule_value: 
    - 7

#### Perfect Day - Epic
- Beschreibung: 
    - Ein perfekter Tag voller Produktivität. Schaffe 20 Tasks an nur einem Tag.
- rule_type: 
    - tasks_completed_single_day
- rule_value: 
    - 20


#### Overkill - Epic
- Beschreibung: 
    - Das war nicht einfach nur Fokus, das war Overkill. Arbeite 180 Minuten an einer einzigen Task.
- rule_type: 
    - long_task_duration
- rule_value: 
    - 180








#### Badge Name
- Beschreibung: 
    - ...
- rule_type: 
    - ...
- rule_value: 
    - ...

