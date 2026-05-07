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
Overkill -> sehr lange Task (z.B. 3h)
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
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
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
special_hidden
```

### Rarity

<span style="color:gray"> Common</span><br>
    - primary-color: ... <br>
    - secondary-color: ... <br>
<span style="color:blue"> Rare</span><br>
    - primary-color: ... <br>
    - secondary-color: ... <br>
<span style="color:violet"> Epic</span><br>
    - primary-color: ... <br>
    - secondary-color: ... <br>
<span style="color:yellow"> Legendary </span><br>
    - primary-color: ... <br>
    - secondary-color: ... <br>

---

### Task-basierte Badges

#### First Step
- Beschreibung: 
    - Absolviere deine erste Task ab und beginne deinen Weg für ein Produktiveres Leben.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 1

#### Getting Started
- Beschreibung: 
    - Bewältige fünf Tasks auf deiner Reise auf FocusUp.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 5

#### Task Grinder
- Beschreibung: 
    - Schaffe 50 Tasks. Dies ist erst der Anfang.
- rule_type: 
    - tasks_completed
- rule_value: 
    - 50


#### Machine
- Beschreibung: 
    - Wie? Du hast 100 Tasks abgeschlossen! Wie geht es weiter?
- rule_type: 
    - tasks_completed
- rule_value: 
    - 100

### Streak-Badges

#### 3 Day Streak
- Beschreibung: 
    - Die ersten Schritte zu einem Produktivem Alltag
- rule_type: 
    - streak
- rule_value: 
    - 3

#### 7 Day Warrior
- Beschreibung: 
    - Eine Woche am Stück bist du schon Produktiv. Weiter so!
- rule_type: 
    - streak
- rule_value: 
    - 7

#### 30 Day Discipline
- Beschreibung: 
    - Du bist nicht aufzuhalten! Einen Monat bist du schon kontinuierlich dran.
- rule_type: 
    - streak
- rule_value: 
    - 30


#### 100 Day Legend
- Beschreibung: 
    - Deine Willenskraft ist insane! Wie schafft du es so lange am Ball zu bleiben? Sehr beeindruckend!!!
- rule_type: 
    - streak
- rule_value: 
    - 100

### XP-Badges

#### XP Starter 
- Beschreibung: 
    - Schritt für Schritt. Die ersten 50 XP.
- rule_type: 
    - xp_total
- rule_value: 
    - 50

#### Leveling Up 
- Beschreibung: 
    - Das Rennen beginnt, wie weit wirst du noch kommen? Die 800 XP hast du schon mal.
- rule_type: 
    - xp_total
- rule_value: 
    - 800

#### XP Master
- Beschreibung: 
    - Du bist weit gekommen, der Master Rang kommt dir echt zu gute.
- rule_type: 
    - xp_total
- rule_value: 
    - 8000

### Zeit-basierte Badges


#### Time Tracker
- Beschreibung: 
    - Wie fühlts sich an zu gamen, eh, zu arbeiten meine ich?
- rule_type: 
    - time_logged
- rule_value: 
    - 300


#### Deep Worker
- Beschreibung: 
    - Du bist konzentriert an der Sache, wollen wir mal nicht weiter Stören.
- rule_type: 
    - time_logged
- rule_value: 
    - 1000


#### Focus Beast
- Beschreibung: 
    - Konzentration ist in deinem Sinne, schon fast eine Untertreibung. Kann man dich noch aufhalten?
- rule_type: 
    - time_logged
- rule_value: 
    - 5000

### Schwierigkeit-Badges

#### Easy Rider
- Beschreibung: 
    - Die einfachen Aufgaben müssen auch mal gemacht werden, neben den Schweren. Schaffe 20 einfache Tasks
- rule_type: 
    - difficulty_achived_easy
- rule_value: 
    - 20

#### Challenger
- Beschreibung: 
    - Jetzt kommt's zu den schweren Brocken, da kommt man ins Schwitzen. 
- rule_type: 
    - difficulty_achived_hard
- rule_value: 
    - 10

#### Hardcore
- Beschreibung: 
    - Nur Tasks des Types Hard, du bist wirklich Hardcore. 
- rule_type: 
    - difficulty_achived_hardcore
- rule_value: 
    - 10

### Konsistenz / Verhalten

#### Early Bird
- Beschreibung: 
    - 
- rule_type: 
    - consistency_check_morning
- rule_value: 
    - 8

#### Night Owl
- Beschreibung: 
    - ...
- rule_type: 
    - consistency_check_evening
- rule_value: 
    - 22

#### Deadline Master
- Beschreibung: 
    - ...
- rule_type: 
    - consistency_check_deadline
- rule_value: 
    - 










#### Badge Name
- Beschreibung: 
    - ...
- rule_type: 
    - ...
- rule_value: 
    - ...

